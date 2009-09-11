using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gablarski.Messages;
using Gablarski;
using System.Threading;
using ValkyrieNetwork.Messages.Valkyrie;
using ValkyrieLibrary.Core.Messages;
using System.Collections;
using System.Diagnostics;
using ValkyrieServerLibrary.Entities;
using NHibernate.Criterion;
using ValkyrieLibrary.Network;
using System.Drawing;
using ValkyrieServerLibrary.Network.Messages.Valkyrie;
using ValkyrieLibrary.Characters;

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
					Debugger.Break();

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
		

			character.Location = new Point(character.MapX * 32, character.MapY * 32);
			character.Animation = Enum.GetName (typeof(Directions), character.Direction);

			// Create player
			NetworkPlayer player = new NetworkPlayer();
			player.Character = character;
			player.AccountID = account.ID;
			player.NetworkID = ++this.LastNetworkID;
			player.Connection = ev.Connection;

			// Add to the list of players in the servers memory
			this.players.AddToCache("Default", player);

			// Send them authentication ID
			var msgsuccess = new LoginSuccessMessage();
			msgsuccess.NetworkIDAssigned = player.NetworkID;
			player.Connection.Send(msgsuccess);

			// Update other players
			var updatemsg = new PlayerUpdateMessage();
			updatemsg.CharacterName = player.Character.Name;
			updatemsg.NetworkID = player.NetworkID;
			updatemsg.Action = PlayerUpdateAction.Add;

			foreach (var tmp in this.players["Default"])
				tmp.Connection.Send(updatemsg);
		}
		#endregion

		#region Player Updates
		private void PlayerRequestReceived(MessageReceivedEventArgs ev)
		{
			var msg = (PlayerRequestMessage)ev.Message;

			NetworkPlayer player = this.players.GetFromCache("Default", msg.RequestedPlayerNetworkID);

			PlayerInfoMessage infomsg = new PlayerInfoMessage();
			infomsg.NetworkID = player.NetworkID;
			infomsg.Name = player.Character.Name;
			infomsg.Animation = player.Character.Animation;
			infomsg.Moving = player.Character.Moving;
			infomsg.TileSheet = player.Character.TileSheet;
			infomsg.Location = player.Character.Location;

			ev.Connection.Send(infomsg);
		}

		private void PlayersRequestReceived(MessageReceivedEventArgs ev)
		{
			// Get the player, figure out which players are arround, and send send those players
			var playerlist = this.players["Default"];

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
				infomsg.Location = player.Character.Location;

				ev.Connection.Send(infomsg);
			}

		}

		private void LocationDataReceived(MessageReceivedEventArgs ev)
		{
			var msg = (LocationUpdateMessage)ev.Message;

			NetworkPlayer netplayer = this.players.GetFromCache(ev.Connection);
			netplayer.Character.Animation = msg.Animation;
			netplayer.Character.Location = msg.Location;
			netplayer.Character.MapLocation = new Point(msg.Location.X / 32, msg.Location.Y / 32);

			LocationUpdateReceived locmsg = new LocationUpdateReceived();
			locmsg.Animation = msg.Animation;
			locmsg.Location = msg.Location;
			locmsg.NetworkID = msg.NetworkID;

			foreach (var player in this.players["Default"])
			{
				if (player.Connection != ev.Connection)
					player.Connection.Send(locmsg);
			}
		}
		#endregion
	}
}
