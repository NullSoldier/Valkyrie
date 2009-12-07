using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine;
using Gablarski.Network;
using System.Net;
using System.Net.Sockets;
using Gablarski;
using Gablarski.Messages;

namespace Valkyrie.Library.Providers
{
	public class ValkyrieNetworkProvider
		: INetworkProvider
	{
		#region Public Properties

		public event EventHandler Connected
		{
			add { this.connected += value; }
			remove { this.connected -= value; }
		}

		public event EventHandler<ConnectionEventArgs> Disconnected
		{
			add { this.disconnected += value; }
			remove { this.disconnected -= value; }
		}

		public event EventHandler<MessageReceivedEventArgs> MessageReceived
		{
			add { this.messagereceived += value; }
			remove { this.messagereceived -= value; }
		}

		public bool IsConnected
		{
			get { return this.connection.IsConnected; }
		}

		#endregion

		#region Public Methods

		public bool Connect (string server, int port)
		{
			if(this.IsConnected)
				return true;

			try
			{
				this.connection.Connect(new IPEndPoint(IPAddress.Parse(server), Convert.ToInt32(port)));
			}
			catch(SocketException)
			{
				return false;
			}

			return this.connection.IsConnected;
		}

		public void Disconnect ()
		{
			if(this.IsConnected)
				this.connection.Disconnect();
		}

		public void Send (MessageBase message)
		{
			if(this.IsConnected)
				this.connection.Send(message);
		}

		#endregion

		private NetworkClientConnection connection = new NetworkClientConnection();
		private event EventHandler connected;
		private event EventHandler<ConnectionEventArgs> disconnected;
		private event EventHandler<MessageReceivedEventArgs> messagereceived;

		private void Load ()
		{
			this.connection.Connected += this.Connection_Connected;
			this.connection.Disconnected += this.Connection_Disconnected;
			this.connection.MessageReceived += this.Connection_MessageReceived;
		}

		private void Connection_Connected (object sender, EventArgs e)
		{
			var handler = this.connected;
			if(handler != null)
				handler(sender, e);
		}

		private void Connection_Disconnected (object sender, ConnectionEventArgs e)
		{
			var handler = this.disconnected;
			if(handler != null)
				handler(sender, e);
		}

		private void Connection_MessageReceived (object sender, MessageReceivedEventArgs e)
		{
			var handler = this.messagereceived;
			if(handler != null)
				handler(sender, e);
		}

		#region IEngineProvider Members

		public void LoadEngineContext (IEngineContext context)
		{
			this.Load();

			this.isloaded = true;
		}

		public bool IsLoaded
		{
			get { return this.isloaded = true; }
		}

		private bool isloaded = false;

		#endregion
	}
}
