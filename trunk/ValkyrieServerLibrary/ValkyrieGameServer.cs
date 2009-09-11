using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Network;
using Gablarski.Network;
using Gablarski;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Reflection;
using NHibernate;
using ValkyrieServerLibrary.Entities;

namespace ValkyrieServerLibrary.Core
{
	public partial class ValkyrieGameServer
	{
		private readonly NetworkServerConnectionProvider server;
		private ISession session;
		private Version MinimumVersion = new Version(0, 0, 0, 0);

		private readonly NetworkPlayerCache players;
		private readonly GameServerSettings settings;

		bool Started = false;
		private uint LastNetworkID = 0;

		public ValkyrieGameServer(GameServerSettings settings)
			: this()
		{
			this.players = new NetworkPlayerCache();
			this.server = new NetworkServerConnectionProvider();
			this.server.Port = 6112;

			this.settings = settings;
		}

		public void Start()
		{
			this.server.StartListening();
			this.server.ConnectionMade +=new EventHandler<ConnectionEventArgs>(Server_ConnectionMade);

			this.session = Fluently.Configure()
				.Database(PostgreSQLConfiguration.Standard.ConnectionString(s => s.Host(this.settings[ServerSettingName.DatabaseAddress]).Username(this.settings[ServerSettingName.DatabaseUser]).Password(this.settings[ServerSettingName.DatabasePassword]).Database(this.settings[ServerSettingName.DatabaseName]).Port(Convert.ToInt32(this.settings[ServerSettingName.DatabasePort]))))
				.Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
				.BuildSessionFactory().OpenSession();

			this.Started = true;
		}

		private void Server_ConnectionMade(object sender, ConnectionEventArgs ev)
		{
			ev.Connection.MessageReceived += this.Server_MessageReceived;
			ev.Connection.Disconnected += this.Server_Disconnected;
		}

		private void Server_Disconnected(object sender, ConnectionEventArgs ev)
		{
			ev.Connection.MessageReceived -= this.Server_MessageReceived;
			ev.Connection.Disconnected -= this.Server_Disconnected;

			this.Disconnect(ev.Connection);
		}

		public void Stop()
		{
			this.server.StopListening();
			this.session.Disconnect();

			this.Started = false;
		}

		public void Disconnect(IConnection connection)
		{
			if(connection.IsConnected)
				connection.Disconnect();

			NetworkPlayer player = this.players.GetFromCache(connection);

			this.players.RemoveFromCache(connection);

			PlayerUpdateMessage updatemsg = new PlayerUpdateMessage();
			updatemsg.Action = PlayerUpdateAction.Remove;
			updatemsg.NetworkID = player.NetworkID;

			foreach (var nplayer in this.players["Default"])
				nplayer.Connection.Send(updatemsg);

			this.SaveCharacter(player.Character);
		}

		private void SaveCharacter(Character character)
		{
			this.session.SaveOrUpdate(character);
		}



		#region Oldcode

		

		

		//private void Message_UpdateLocation(IConnection connection, LocationUpdateMessage message)
		//{
		//    NetworkPlayer netplayer = this.players[connection];

		//    netplayer.Animation = message.Animation;
		//    netplayer.Location = message.Location;

		//    foreach (var player in this.players.Values)
		//    {
		//        if (player.Connection != connection)
		//            player.Connection.Send(message);
		//    }
		//}
		#endregion
	}
}
