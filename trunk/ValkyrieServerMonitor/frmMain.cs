using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Gablarski.Network;
using System.Reflection;
using Gablarski;
using Gablarski.Messages;
using ValkyrieLibrary.Core;
using ValkyrieLibrary.Network;
using ValkyrieLibrary.Core.Messages;
using ValkyrieLibrary;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Events.EngineEvents;
using ValkyrieServerLibrary.Network.Messages.Valkyrie;
using ValkyrieServerLibrary.Network;
using System.Net;

namespace ValkyrieServerMonitor
{
	public partial class frmMain : Form
	{
		private NetworkServerConnectionProvider server;
		private Dictionary<IConnection, NetworkPlayer> players;

		private bool Started = false;
		private uint LastNetworkID = 0;

		private GameServerClient gameserver;

		public frmMain()
		{
			InitializeComponent();

			this.players = new Dictionary<IConnection, NetworkPlayer>();
			this.server = new NetworkServerConnectionProvider();
			
			this.gameserver = new GameServerClient(IPAddress.Parse("127.0.0.1"), "pokemon", "appleseed1", 5432, "pokemon");
			this.gameserver.Start();
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			this.Trace("Server monitor initialized.");
		}

		private void Trace(string text)
		{
			if (this.InvokeRequired)
			{
				this.Invoke((Action<String>)this.Trace, text);
			}
			else
			{
				this.inConsole.Text += string.Format("{0}: {1}{2}", DateTime.Now.ToString("t"), text, Environment.NewLine);

				this.inConsole.SelectionStart = this.inConsole.Text.Length;
				this.inConsole.ScrollToCaret();
			}

		}

		private void toolConnect_Click(object sender, EventArgs e)
		{
			if(this.Started)
			{
				this.server.StopListening();
				this.server.ConnectionMade -= this.Server_ConnectionMade;

				this.toolConnect.Text = "Connect";
				this.Trace("Valkyrie server stopped.");
			}
			else
			{
				this.server.StartListening();
				this.server.ConnectionMade += this.Server_ConnectionMade;

				this.toolConnect.Text = "Disconnect";
				this.Trace("Valkyrie server started.");
			}

			this.Started = !Started;
		}

		private void Server_ConnectionMade(object sender, ConnectionEventArgs ev)
		{
			//this.Trace("Client connected to server.");

			ev.Connection.MessageReceived += this.Connection_MessageReceived;
			ev.Connection.Disconnected += this.Connection_Disconnected;
		}

		private void Connection_MessageReceived(object sender, MessageReceivedEventArgs ev)
		{
			//this.Trace("Message received from connection.");

			if (ev.Message is LoginMessage)
				this.Message_LoginMessage(ev.Connection, (LoginMessage)ev.Message);
			if (ev.Message is LocationUpdateMessage)
				this.Message_UpdateLocation(ev.Connection, (LocationUpdateMessage)ev.Message);
			if (ev.Message is PlayerRequestListMessage)
				this.Message_PlayerRequest(ev.Connection, (PlayerRequestListMessage)ev.Message);
		}

		private void Connection_Disconnected(object sender, ConnectionEventArgs ev)
		{
			this.Trace("Connection disconnected.");
		}

		#region MessageReceived Methods
		private void Message_PlayerRequest(IConnection connection, PlayerRequestListMessage message)
		{
			this.Trace("Sent players to " + this.players[connection].Name);

			foreach (var player in this.players.Values)
			{
				if (player.Connection == connection)
					continue;

				PlayerUpdateMessage msg = new PlayerUpdateMessage();
				msg.CharacterName = player.Name;
				msg.NetworkID = player.NetworkID;
				msg.Action = PlayerUpdateAction.Add;

				connection.Send(msg);
				this.Trace("Sent player " + player.Name + " to " + this.players[connection].Name);
			}
		}

		private void Message_UpdateLocation(IConnection connection, LocationUpdateMessage message)
		{
			//this.Trace("Movement packet received from " + this.players[connection].Name);

			foreach (var player in this.players.Values)
			{
				if (player.Connection != connection)
					player.Connection.Send(message);
			}
		}

		private void Message_LoginMessage(IConnection connection, LoginMessage message)
		{
			bool result = this.gameserver.AuthenticateLogin(message.Username, message.Password);
			if (!result)
			{
				// Login failed, send fail message and do not continue logging them in
				LoginFailedMessage failmsg = new LoginFailedMessage(ConnectionRejectedReason.BadLogin);
				connection.Send(failmsg);
				return;
			}

			// Create player
			NetworkPlayer player = new NetworkPlayer();
			player.NetworkID = ++this.LastNetworkID;
			player.Connection = connection;
			player.Location = new Point(0, 0);
			player.Name = "Player #" + player.NetworkID;
			
			// Send them authentication ID
			var msg = new LoginSuccessMessage();
			msg.NetworkIDAssigned = player.NetworkID;
			player.Connection.Send(msg);

			//this.Trace("Sent network authentication ID to connection.");

			// Update other players
			var updatemsg = new PlayerUpdateMessage();
			updatemsg.CharacterName = "Player #" + this.LastNetworkID;
			updatemsg.NetworkID = this.LastNetworkID;
			updatemsg.Action = PlayerUpdateAction.Add;

			foreach (var tmp in this.players.Values)
				tmp.Connection.Send(updatemsg);

			// Add to the list of players in the servers memory
			this.players.Add(player.Connection, player);

			this.Trace("Player " + player.Name + " has logged into the server.");

			LoginSuccessMessage successmsg = new LoginSuccessMessage();
			successmsg.NetworkIDAssigned = player.NetworkID;
		}
		#endregion

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!this.Started)
				return;

			this.server.StopListening();

			foreach (var player in this.players.Values)
				player.Connection.Disconnect();
		}

	}
}
