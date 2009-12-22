using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Library.Network;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Reflection;
using NHibernate;
using Valkyrie;
using Valkyrie.Network;
using ValkyrieServerLibrary.Entities;
using System.IO;
using Valkyrie.Library.Core;
using System.Threading;
using Microsoft.Xna.Framework;
using Valkyrie.Engine.Managers;
using Valkyrie.Engine.Providers;
using Valkyrie.Library.Managers;
using Valkyrie.Engine.Core;

namespace ValkyrieServerLibrary.Core
{
	public partial class ValkyrieGameServer
	{
		private readonly NetworkServerConnectionProvider server;
		private ISession session;
		private Version MinimumVersion = new Version(0, 0, 0, 0);

		private NetworkPlayerCache players;
		private readonly GameServerSettings settings;

		private readonly IWorldManager worlds;
		private readonly ServerMovementProvider movement;
		private readonly ICollisionProvider collision;

		private bool Started = false;
		private bool Loaded = false;
		//private uint LastNetworkID = 0;

		public event EventHandler<UserEventArgs> UserLoggedIn;
		public event EventHandler<UserEventArgs> UserLoggedOut;

		Thread MovementUpdateThread;

		public ValkyrieGameServer(GameServerSettings settings)
			: this()
		{
			this.worlds = new ValkyrieWorldManager(new Assembly[] { });
			this.collision = new ServerCollisionProvider(this.worlds);
			this.movement = new ServerMovementProvider(this.collision);

			this.players = new NetworkPlayerCache();

			this.server = new NetworkServerConnectionProvider();
			this.server.Port = 6116;

			this.settings = settings;
		}

		public void Load ()
		{
			if(this.Loaded)
				return;

			this.LoadWorlds();

			this.Loaded = true;
		}

		public void Unload ()
		{
			if(!this.Loaded)
				return;

			this.Loaded = false;
		}

		public void Start()
		{
			if(this.Started)
				return;

			this.Load();

			this.session = Fluently.Configure()
				.Database(PostgreSQLConfiguration.Standard.ConnectionString(s => s.Host(this.settings[ServerSettingName.DatabaseAddress]).Username(this.settings[ServerSettingName.DatabaseUser]).Password(this.settings[ServerSettingName.DatabasePassword]).Database(this.settings[ServerSettingName.DatabaseName]).Port(Convert.ToInt32(this.settings[ServerSettingName.DatabasePort]))))
				.Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
				.BuildSessionFactory().OpenSession();

			this.server.StartListening();
			this.server.ConnectionMade += this.Server_ConnectionMade;

			this.MovementUpdateThread = new Thread(this.UpdateMovementThread);
			this.MovementUpdateThread.Name = "Movement Update";
			this.MovementUpdateThread.IsBackground = false;
			this.MovementUpdateThread.Start();

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

			var player = this.players.GetPlayer (ev.Connection);
			if(player != null && player.State == PlayerState.LoggedIn)
				this.Disconnect (ev.Connection);
		}

		public void Stop()
		{
			if(!this.Started)
				return;

			this.Unload();

			this.server.StopListening();
			this.server.ConnectionMade -= this.Server_ConnectionMade;
			this.session.Disconnect();

			this.Started = false;
		}

		public void Disconnect (uint NetworkID)
		{
			var player = this.players.GetPlayer(NetworkID);
			player.State = PlayerState.LoggedOut;

			if(player != null)
				this.Disconnect (player.Connection);
		}

		public void Disconnect(IConnection connection)
		{
			connection.MessageReceived -= this.Server_MessageReceived;
			connection.Disconnected -= this.Server_Disconnected;

			if(connection.IsConnected)
				connection.Disconnect();

			NetworkPlayer player = this.players.GetPlayer(connection);
			
			this.players.RemovePlayer(connection);

			// fire logged out event
			var handler = this.UserLoggedOut;
			if(handler != null)
				handler(this, new UserEventArgs(player));

			PlayerUpdateMessage updatemsg = new PlayerUpdateMessage();
			updatemsg.Action = PlayerUpdateAction.Remove;
			updatemsg.NetworkID = player.NetworkID;

			foreach (var nplayer in this.players.GetPlayers())
				nplayer.Connection.Send(updatemsg);

			this.SaveCharacter(player.Character);
		}

		private void SaveCharacter(Character character)
		{
			this.session.SaveOrUpdate(character);
			this.session.Flush();
		}

		private void LoadWorlds()
		{
			//this.worlds.Load(new FileInfo(Path.Combine(Environment.CurrentDirectory, this.settings[ServerSettingName.MapDirectory] + "PokeWorld.xml")), new XMLMapProvider() );
		}

		private string GetChunkName (string WorldName, MapPoint location)
		{
			return string.Empty;
		}

		private void UpdateMovementThread()
		{
			double lastupdate = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds * 1000;
			double newupdate = lastupdate;

			while(this.Started)
			{
				newupdate = (DateTime.UtcNow - new DateTime(1970,1,1,0,0,0)).TotalSeconds * 1000;
				int difference = (int)(newupdate - lastupdate); // Difference is the difference in milliseconds between update
				lastupdate = newupdate;

				TimeSpan span = new TimeSpan(0, 0, 0, 0, difference);

				GameTime time = new GameTime(TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, span);
				this.movement.Update(time);
                Thread.Sleep(16);
			}
		}
	}
}
