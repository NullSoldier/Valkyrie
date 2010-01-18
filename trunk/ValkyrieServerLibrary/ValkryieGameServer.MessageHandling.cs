using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Valkyrie;
using Valkyrie.Library.Network;
using Valkyrie.Messages;
using Valkyrie.Messages.Movement;

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
				case ClientMessageType.ClientMovementMessage:
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
	}
}
