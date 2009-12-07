﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gablarski.Messages;
using Gablarski;
using System.Threading;
using ValkyrieNetwork.Messages.Valkyrie;
using Valkyrie.Library.Core.Messages;
using System.Collections;
using System.Diagnostics;
using ValkyrieServerLibrary.Entities;
using NHibernate.Criterion;
using Valkyrie.Library.Network;
using System.Drawing;
using ValkyrieServerLibrary.Network.Messages.Valkyrie;
using Valkyrie.Library.Core;
using ValkyrieNetwork.Messages.Valkyrie.Movement;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Characters;

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
				{ClientMessageType.LocationData, LocationDataReceived},
				{ClientMessageType.PlayerStartMoving, PlayerStartMovingReceived},
				{ClientMessageType.PlayerStopMoving, PlayerStopMovingReceived},
				{ClientMessageType.PlayerInfoRequest, PlayerRequestReceived},
				{ClientMessageType.PlayerRequest, PlayersRequestReceived}
			};

			Thread miscreader = new Thread((ParameterizedThreadStart)this.GenericMessageRunner);
			miscreader.Name = "Misc Message Runner";
			miscreader.IsBackground = true;
			miscreader.Start(this.miscqueue);

			Thread movereader = new Thread((ParameterizedThreadStart)this.GenericMessageRunner);
			movereader.Name = "Movement Message Runner";
			movereader.IsBackground = true;
			movereader.Start(this.movequeue);

			Thread loginreader = new Thread((ParameterizedThreadStart)this.GenericMessageRunner);
			loginreader.Name = "Login Message Runner";
			loginreader.IsBackground = true;
			loginreader.Start(this.loginqueue);

		}

		private readonly Dictionary<ClientMessageType, Action<MessageReceivedEventArgs>> Handlers;

		private Queue<MessageReceivedEventArgs> movequeue = new Queue<MessageReceivedEventArgs> (200);
		private Queue<MessageReceivedEventArgs> loginqueue = new Queue<MessageReceivedEventArgs>(1000);
		private Queue<MessageReceivedEventArgs> miscqueue = new Queue<MessageReceivedEventArgs>(400);
		
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
					this.loginqueue.Enqueue(ev);
					break;
				case ClientMessageType.LocationData:
					this.movequeue.Enqueue(ev);
					break;
				default:
					this.miscqueue.Enqueue(ev);
					break;
			}
		}

		private void GenericMessageRunner(object queue)
		{
			Queue<MessageReceivedEventArgs> que = (Queue<MessageReceivedEventArgs>)queue;

			while (true)
			{
				MessageReceivedEventArgs e;

				if (que.Count <= 0)
				{
					Thread.Sleep(1);
					continue;
				}
				
				e = que.Dequeue();

				if (e == null)
					continue;

				var msg = (e.Message as ClientMessage);
				if (msg == null)
				{
					Debugger.Break();
				}

				Action<MessageReceivedEventArgs> handler;
				if (this.Handlers.TryGetValue(msg.MessageType, out handler))
					handler(e);
			}
		}
		#endregion

		#region Authentication
		private void ClientConnected (MessageReceivedEventArgs ev)
		{
			var msg = (ConnectMessage)ev.Message;

			if (msg.Version < this.MinimumVersion)
			{
				ConnectionRejectedMessage rejmsg = new ConnectionRejectedMessage ();
				rejmsg.Reason = ConnectionRejectedReason.IncompatableVersion;
				ev.Connection.Send (rejmsg);

				this.Disconnect(ev.Connection);
			}
		}

		private void LoginRequestReceived (MessageReceivedEventArgs ev)
		{
			var msg = (LoginMessage)ev.Message;

			Account account = this.session.CreateCriteria<Account>()
				.Add(Restrictions.Eq("Username", msg.Username ))
				.Add(Restrictions.Eq("Password", msg.Password ))
				.List<Account>().FirstOrDefault();
			
			if (account == null)
			{
				LoginFailedMessage failmsg = new LoginFailedMessage(ConnectionRejectedReason.BadLogin);
				ev.Connection.Send(failmsg);
				return;
			}
			
			Character character = this.session.CreateCriteria<Character> ()
			.Add(Restrictions.Eq("ID", account.ID))
			.List<Character>().FirstOrDefault();

			character.Location = new ScreenPoint(character.MapX * 32, character.MapY * 32);
			character.Animation = Enum.GetName (typeof(Directions), character.Direction);

			// Create player
			NetworkPlayer player = new NetworkPlayer();
			player.Character = character;
			player.AccountID = account.ID;
			player.NetworkID = ++this.LastNetworkID;
			player.Connection = ev.Connection;
			player.Character.MapChunkName = this.GetChunkName(player.Character.WorldName, player.Character.MapLocation);

			// Add to the list of players in the servers memory
			this.players.AddPlayer(player);

			// Send them authentication ID
			var msgsuccess = new LoginSuccessMessage();
			msgsuccess.NetworkIDAssigned = player.NetworkID;
			player.Connection.Send(msgsuccess);

			var handler = this.UserLoggedIn;
			if(handler != null)
				handler(this, new UserEventArgs(player));

			// Update other players
			var updatemsg = new PlayerUpdateMessage();
			updatemsg.CharacterName = player.Character.Name;
			updatemsg.NetworkID = player.NetworkID;
			updatemsg.Action = PlayerUpdateAction.Add;

			foreach (var tmp in this.players.GetPlayers())
				tmp.Connection.Send(updatemsg);
		}
		#endregion

		#region Player Updates
		private void PlayerRequestReceived(MessageReceivedEventArgs ev)
		{
			var msg = (PlayerRequestMessage)ev.Message;

			NetworkPlayer player = this.players.GetPlayers().Where(p => p.NetworkID == msg.RequestedPlayerNetworkID).FirstOrDefault();

			if(player == null)
				return;

			PlayerInfoMessage infomsg = new PlayerInfoMessage();
			infomsg.NetworkID = player.NetworkID;
			infomsg.Name = player.Character.Name;
			infomsg.Animation = player.Character.Animation;
			infomsg.Moving = player.Character.Moving;
			infomsg.TileSheet = player.Character.TileSheet;
			infomsg.Location = new Point(player.Character.Location.X, player.Character.Location.Y);
			infomsg.WorldName = player.Character.WorldName;

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

				PlayerInfoMessage infomsg = new PlayerInfoMessage();
				infomsg.NetworkID = player.NetworkID;
				infomsg.Name = player.Character.Name;
				infomsg.Animation = player.Character.Animation;
				infomsg.Moving = player.Character.Moving;
				infomsg.TileSheet = player.Character.TileSheet;
				infomsg.Location = new Point(player.Character.Location.X, player.Character.Location.Y);
				infomsg.WorldName = player.Character.WorldName;

				ev.Connection.Send(infomsg);
			}

		}
		#endregion

		#region Movement

		private void LocationDataReceived (MessageReceivedEventArgs ev)
		{
			/*var msg = (LocationUpdateMessage)ev.Message;

			lock(this.players)
			{
				NetworkPlayer netplayer = this.players.GetPlayer(ev.Connection);
				netplayer.Character.Animation = msg.Animation;
				netplayer.Character.Location = new ScreenPoint(msg.Location.X, msg.Location.Y);
				netplayer.Character.MapLocation = new MapPoint(msg.Location.X / 32, msg.Location.Y / 32);
				netplayer.Character.Direction = (Directions)msg.Direction;
			}*/
		}

		private void PlayerLoadedReceived (MessageReceivedEventArgs ev)
		{
			PlayerLoadedMessage message = (PlayerLoadedMessage)ev.Message;

			NetworkPlayer player = this.players.GetPlayers().Where(p => p.NetworkID == message.NetworkID).FirstOrDefault();

			if(player == null)
				return;

			PlayerInfoMessage infomsg = new PlayerInfoMessage();
			infomsg.NetworkID = player.NetworkID;
			infomsg.Name = player.Character.Name;
			infomsg.Animation = player.Character.Animation;
			infomsg.Moving = player.Character.Moving;
			infomsg.TileSheet = player.Character.TileSheet;
			infomsg.Location = new Point(player.Character.Location.X, player.Character.Location.Y);
			infomsg.WorldName = player.Character.WorldName;

			foreach(NetworkPlayer destPlayer in this.players.GetPlayers())
			{
				if(destPlayer.Connection != player.Connection)
					destPlayer.Connection.Send(infomsg);
			}
		}

		private void PlayerStartMovingReceived (MessageReceivedEventArgs ev)
		{
			//Thread.Sleep(1000); // Simulate network lag of 1000 MS

			PlayerStartMovingMessage message = (PlayerStartMovingMessage)ev.Message;

			NetworkPlayer player = this.players[ev.Connection];
			player.Character.Speed = message.Speed;
			player.Character.MoveDelay = message.MoveDelay;

			var direction = (Directions)message.Direction;

			// Should throw the movement onto the queue and then process it on each update
			this.movement.BeginMove(player.Character, direction, message.Animation);

			PlayerStartedMovingMessage movmsg = new PlayerStartedMovingMessage();
			movmsg.NetworkID = player.NetworkID;
			movmsg.Direction = message.Direction;
			movmsg.Speed = message.Speed;
			movmsg.Animation = message.Animation;
			movmsg.MoveDelay = player.Character.MoveDelay;
			
			foreach(NetworkPlayer destPlayer in this.players.GetPlayers())
			{
				if(destPlayer.Connection != player.Connection)
					destPlayer.Connection.Send(movmsg);
			}
		}

		private void PlayerStopMovingReceived (MessageReceivedEventArgs ev)
		{
			// Process stop movement
			// Send out stop message to all other players

			PlayerStopMovingMessage message = (PlayerStopMovingMessage)ev.Message;
			NetworkPlayer player = this.players.GetPlayer(message.NetworkID);
			player.Character.Animation = player.Character.Direction.ToString();
			player.Character.Location = new ScreenPoint(message.MapX * 32, message.MapY * 32);

			((ServerMovementProvider)this.movement).EndMoveLocation(player.Character, new MapPoint(message.MapX, message.MapY), player.Character.Animation);

			if(message.Animation == "Any")
				return;

			PlayerStoppedMovingMessage movmsg = new PlayerStoppedMovingMessage();
			movmsg.X = message.MapX * 32;
			movmsg.Y = message.MapY * 32;
			movmsg.NetworkID = player.NetworkID;
			movmsg.Animation = message.Animation;

			foreach(NetworkPlayer netplayer in this.players.GetPlayers())
			{
				// Send out stopped moving message
				if(netplayer.Connection != player.Connection)
					netplayer.Connection.Send(movmsg);
			}
			
		}

		#endregion
	}
}
