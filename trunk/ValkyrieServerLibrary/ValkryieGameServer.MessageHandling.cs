using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Valkyrie.Messages;
using Valkyrie.Messages.Valkyrie;
using Valkyrie.Library.Core.Messages;
using System.Collections;
using System.Diagnostics;
using Valkyrie;
using ValkyrieServerLibrary.Entities;
using NHibernate.Criterion;
using Valkyrie.Library.Network;
using System.Drawing;
using ValkyrieServerLibrary.Network.Messages.Valkyrie;
using Valkyrie.Library.Core;
using Valkyrie.Messages.Valkyrie.Movement;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Characters;
using Valkyrie.Messages.Valkyrie.Authentication;
using NHibernate;
using Valkyrie.Engine.Providers;
using ValkyrieNetwork.Messages.Valkyrie.Movement;

namespace ValkyrieServerLibrary.Core
{
	public partial class ValkyrieGameServer
	{
		protected ValkyrieGameServer ()
		{
			this.Handlers = new Dictionary<ClientMessageType, Action<MessageReceivedEventArgs>>
			{
				{ClientMessageType.Connect, ClientConnected},
				{ClientMessageType.Login, LoginRequestReceived},
				{ClientMessageType.Logout, LogoutRequestReceived},
				{ClientMessageType.PlayerStartMoving, PlayerStartMovingReceived},
				{ClientMessageType.PlayerStopMoving, PlayerStopMovingReceived},
				{ClientMessageType.PlayerInfoRequest, PlayerRequestReceived},
				{ClientMessageType.PlayerRequest, PlayersRequestReceived},
				{ClientMessageType.ClientMovementMessage, ClientMovementMessageReceived}
			};

			Thread miscreader = new Thread((ParameterizedThreadStart)this.GenericMessageRunner);
			miscreader.Name = "Misc Message Runner";
			miscreader.IsBackground = true;
			miscreader.Start(new object[] {this.miscqueue, this.miscresetevent} );

			Thread movereader = new Thread((ParameterizedThreadStart)this.GenericMessageRunner);
			movereader.Name = "Movement Message Runner";
			movereader.IsBackground = true;
			movereader.Start(new object[] {this.movequeue, this.moveresetevent} );

			Thread loginreader = new Thread((ParameterizedThreadStart)this.GenericMessageRunner);
			loginreader.Name = "Login Message Runner";
			loginreader.IsBackground = true;
			loginreader.Start(new object[] {this.loginqueue, this.loginresetevent} );

		}

		private readonly Dictionary<ClientMessageType, Action<MessageReceivedEventArgs>> Handlers;

		private Queue<MessageReceivedEventArgs> movequeue = new Queue<MessageReceivedEventArgs> (200);
		private Queue<MessageReceivedEventArgs> loginqueue = new Queue<MessageReceivedEventArgs>(1000);
		private Queue<MessageReceivedEventArgs> miscqueue = new Queue<MessageReceivedEventArgs> (400);

		private AutoResetEvent moveresetevent = new AutoResetEvent (false);
		private AutoResetEvent loginresetevent = new AutoResetEvent (false);
		private AutoResetEvent miscresetevent = new AutoResetEvent (false);
		
		private List<Thread> readerthreads = new List<Thread>();

		#region Messages
		private void Server_MessageReceived (object sender, MessageReceivedEventArgs ev)
		{
			var msg = (ClientMessage)ev.Message;

			switch(msg.MessageType)
			{
				case ClientMessageType.Connect:
				case ClientMessageType.Disconnect:
				case ClientMessageType.Login:
					lock(this.loginqueue)
					{
						this.loginqueue.Enqueue (ev);
						this.loginresetevent.Set ();
					}
					break;
				case ClientMessageType.PlayerStartMoving:
				case ClientMessageType.PlayerStopMoving:
				case ClientMessageType.LocationData:
					lock(this.movequeue)
					{
						this.movequeue.Enqueue (ev);
						this.moveresetevent.Set ();
					}
					break;
				default:
					lock(this.miscqueue)
					{
						this.miscqueue.Enqueue (ev);
						this.miscresetevent.Set ();
					}
					break;
			}
		}

		private void GenericMessageRunner(object args)
		{
			var arguments = (object[]) args;

			Queue<MessageReceivedEventArgs> que = (Queue<MessageReceivedEventArgs>) arguments[0];
			AutoResetEvent resetevent = (AutoResetEvent)arguments[1];

			while (true)
			{
				MessageReceivedEventArgs e;

				resetevent.WaitOne ();

				while(que.Count > 0)
				{
					lock(que)
					{
						e = que.Dequeue ();
					}

					if(e == null)
						continue;

					var msg = (e.Message as ClientMessage);
					if(msg == null)
					{
						continue;
					}

					Action<MessageReceivedEventArgs> handler;
					if(this.Handlers.TryGetValue (msg.MessageType, out handler))
						handler (e);	
				}
			}
		}
		#endregion

		#region Authentication
		private void ClientConnected (MessageReceivedEventArgs ev)
		{
			var msg = (ConnectMessage)ev.Message;

			if (msg.Version < this.MinimumVersion)
			{
				ev.Connection.Send (new ConnectionRejectedMessage () { Reason = ConnectionRejectedReason.IncompatableVersion });

				this.Disconnect(ev.Connection);
			}
		}

		private uint networkid = 1;

		private void LoginRequestReceived (MessageReceivedEventArgs ev)
		{
			var msg = (LoginMessage)ev.Message;

			ISession session = this.sessionfactory.OpenSession ();

			Account account = session.CreateCriteria<Account>()
				.Add(Restrictions.InsensitiveLike("Username", msg.Username ))
				.Add(Restrictions.Eq("Password", msg.Password ))
				.List<Account>().FirstOrDefault();
			
			if (account == null || this.players.ContainsPlayer((uint)account.ID))
			{
				ev.Connection.Send (new LoginFailedMessage (ConnectionRejectedReason.BadLogin));
				return;
			}
			
			Character character = session.CreateCriteria<Character> ()
				.Add(Restrictions.Eq("AccountID", account.ID))
				.List<Character>().FirstOrDefault();

			character.Location = new ScreenPoint(character.MapX * 32, character.MapY * 32);
			character.Animation = Enum.GetName (typeof(Directions), character.Direction);
			character.NextMoveActive = true;

			// Create player
			NetworkPlayer player = new NetworkPlayer();
			player.Character = character;
			player.AccountID = account.ID;
			//player.NetworkID = ++networkid;
			player.NetworkID = (uint) account.ID;
			player.Character.NetworkID = player.NetworkID;
			player.Connection = ev.Connection;
			player.Character.MapChunkName = this.GetChunkName(player.Character.WorldName, player.Character.MapLocation);
			player.State = PlayerState.LoggedIn;

			// Add to the list of players in the servers memory
			this.players.AddPlayer(player);

			// Send them authentication ID
			player.Connection.Send(new LoginSuccessMessage () { NetworkIDAssigned = player.NetworkID });

			var handler = this.UserLoggedIn;
			if(handler != null)
				handler(this, new UserEventArgs(player));

			// Update other players
			var updatemsg = new PlayerUpdateMessage ()
			{
				CharacterName = player.Character.Name,
				NetworkID = player.NetworkID,
				Action = PlayerUpdateAction.Add
			};

			foreach (var tmp in this.players.GetPlayers())
				tmp.Connection.Send(updatemsg);

			session.Close ();
		}

		private void LogoutRequestReceived (MessageReceivedEventArgs ev)
		{
			var message = (LogoutMessage)ev.Message;

			this.Disconnect(message.NetworkID);
		}
		#endregion

		#region Player Updates
		private void PlayerRequestReceived(MessageReceivedEventArgs ev)
		{
			var msg = (PlayerRequestMessage)ev.Message;

			NetworkPlayer player = this.players.GetPlayers().Where(p => p.NetworkID == msg.RequestedPlayerNetworkID).FirstOrDefault();

			if(player == null)
				return;

			PlayerInfoMessage infomsg = new PlayerInfoMessage ()
			{
				NetworkID = player.NetworkID,
				Name = player.Character.Name,
				Animation = player.Character.Animation,
				Moving = player.Character.Moving,
				TileSheet = player.Character.TileSheet,
				Location = new Point (player.Character.Location.X, player.Character.Location.Y),
				WorldName = player.Character.WorldName
			};

			ev.Connection.Send(infomsg);
		}

		private void PlayersRequestReceived(MessageReceivedEventArgs ev)
		{
			// Get the player, figure out which players are around, and send send those players
			var playerlist = this.players.GetPlayers();

			foreach (NetworkPlayer player in playerlist)
			{
				if (player.Connection == ev.Connection)
					continue;

				PlayerInfoMessage infomsg = new PlayerInfoMessage ()
				{
					NetworkID = player.NetworkID,
					Name = player.Character.Name,
					Animation = player.Character.Animation,
					Moving = player.Character.Moving,
					TileSheet = player.Character.TileSheet,
					Location = new Point(player.Character.Location.X, player.Character.Location.Y),
					WorldName = player.Character.WorldName
				};

				ev.Connection.Send(infomsg);
			}

		}
		#endregion

		#region Movement

		private void PlayerLoadedReceived (MessageReceivedEventArgs ev)
		{
			PlayerLoadedMessage message = (PlayerLoadedMessage)ev.Message;

			NetworkPlayer player = this.players.GetPlayers().Where(p => p.NetworkID == message.NetworkID).FirstOrDefault();

			if(player == null)
				return;

			PlayerInfoMessage infomsg = new PlayerInfoMessage ()
			{
				NetworkID = player.NetworkID,
				Name = player.Character.Name,
				Animation = player.Character.Animation,
				Moving = player.Character.Moving,
				TileSheet = player.Character.TileSheet,
				Location = new Point (player.Character.Location.X, player.Character.Location.Y),
				WorldName = player.Character.WorldName
			};

			foreach(NetworkPlayer destPlayer in this.players.GetPlayers())
			{
				if(destPlayer.Connection != player.Connection)
					destPlayer.Connection.Send(infomsg);
			}
		}

		Random rand = new Random (DateTime.Now.Millisecond);

		private void PlayerStartMovingReceived (MessageReceivedEventArgs ev)
		{
			//Thread.Sleep (5000);
			PlayerStartMovingMessage message = (PlayerStartMovingMessage)ev.Message;

			NetworkPlayer player = this.players[message.NetworkID];
			if(player == null)
				return;

			var direction = (Directions)message.Direction;

			// Add the movement item to the characters movement queue
			player.Character.MovementQueue.Enqueue (new MovementInfo (new ScreenPoint(message.X, message.Y), direction, MovementStage.StartMovement, MovementType.TileBased, string.Format("Walk{0}", direction.ToString())));

			if(!this.movement.ContainsCharacter(player.Character))
				this.movement.AddToProvider (player.Character);
		}

		private void PlayerStopMovingReceived (MessageReceivedEventArgs ev)
		{
			//Thread.Sleep (5000);

			PlayerStopMovingMessage message = (PlayerStopMovingMessage)ev.Message;
			NetworkPlayer player = this.players.GetPlayer(message.NetworkID);

			// Get animation from last move item
			var direction = Directions.Any;
			string animation = "Any";

			var item = player.Character.MovementQueue.LastOrDefault ();
			if(item != null)
			{
				animation = player.Character.MovementQueue.LastOrDefault ().Direction.ToString ();
				direction = item.Direction;
			}


			player.Character.ClientMovementQueue.Enqueue (new MovementInfo (new ScreenPoint(message.X, message.Y), direction, MovementStage.EndMovement, MovementType.Destination, animation));
			player.Character.MovementQueue.Enqueue (new MovementInfo (ScreenPoint.Zero, direction, MovementStage.EndMovement, MovementType.Destination, animation));		
		}

		private void ClientMovementMessageReceived (MessageReceivedEventArgs ev)
		{
			ClientMovementMessage message = (ClientMovementMessage) ev.Message;
			NetworkPlayer player = this.players.GetPlayer (message.NetworkID);

			if(player == null)
				return; // Must have gotten disconnected

			Directions direction = (Directions)message.Direction;

			player.Character.MovementQueue.Enqueue (new MovementInfo (new ScreenPoint (message.X, message.Y), direction, MovementStage.EndMovement, MovementType.Destination, message.Animation));

			if(!this.movement.ContainsCharacter (player.Character))
				this.movement.AddToProvider (player.Character);
		}

		private void MovementProvider_PlayerStartedMoving (object sender, MovementChangedEventArgs ev)
		{
			PlayerStartedMovingMessage movmsg = new PlayerStartedMovingMessage ()
			{
				NetworkID = ev.Character.NetworkID,
				Direction = (int) ev.Character.Direction,
				Speed = ev.Character.Speed,
				Animation = ev.Character.Animation,
				MoveDelay = ev.Character.MoveDelay
			};

			foreach(NetworkPlayer destPlayer in this.players.GetPlayers())
			{
			    if(destPlayer.NetworkID != ev.Character.NetworkID)
			        destPlayer.Connection.Send(movmsg);
			}
		}

		private void MovementProvider_PlayerStoppedMoving (object sender, MovementChangedEventArgs ev)
		{
			NetworkPlayer player = this.players[(uint) ev.Character.NetworkID];

			PlayerStoppedMovingMessage movmsg = new PlayerStoppedMovingMessage ()
			{
				X = ev.Character.Location.X,
				Y = ev.Character.Location.Y,
				NetworkID = player.NetworkID,
				Animation = ev.Character.Animation,
				Direction = (int) ev.Character.Direction
			};

			foreach(NetworkPlayer netplayer in this.players.GetPlayers ())
			{
				// Send out stopped moving message
				if(netplayer.NetworkID != ev.Character.NetworkID)
					netplayer.Connection.Send (movmsg);
			}
		}

		private void MovementProvider_PlayerMoved (object sender, MovementChangedEventArgs ev)
		{
			NetworkPlayer player = this.players[(uint) ev.Character.NetworkID];

			if(player == null) // They must be disconnected
				return;

			ServerMovementMessage movmsg = new ServerMovementMessage ()
			{
				NetworkID = ev.Character.NetworkID,
				X = ev.Character.Location.X,
				Y = ev.Character.Location.Y,
				Direction = (int)ev.Character.Direction,
				Animation = ev.Character.Animation

			};

			lock(this.players.PlayerLock)
			{
				foreach(NetworkPlayer netplayer in this.players.PlayerRanges[player.NetworkID])
				{
					netplayer.Connection.Send (movmsg);
				}
			}
		}

		private void MovementProvider_FailedVerify (object sender, MovementChangedEventArgs ev)
		{
			var player = this.players.GetPlayer (ev.Character.NetworkID);

			this.Disconnect (player.Connection);
		}
		#endregion
	}
}
