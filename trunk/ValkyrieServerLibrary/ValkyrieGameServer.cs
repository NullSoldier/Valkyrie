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
using ValkyrieServerLibrary.Network.Messages.Valkyrie;
using Valkyrie.Library.Providers;

namespace ValkyrieServerLibrary.Core
{
	public partial class ValkyrieGameServer
	{
		private readonly NetworkServerConnectionProvider server;
		private ISessionFactory sessionfactory;
		private Version MinimumVersion = new Version(0, 0, 0, 0);

		private NetworkPlayerCache players;
		private readonly GameServerSettings settings;

		private readonly IWorldManager worlds;
		private readonly ServerNewMovementProvider movement;
		private readonly ICollisionProvider collision;

		private bool Started = false;
		private bool Loaded = false;
		private int RangeUpdateFrequency = 60;
		//private uint LastNetworkID = 0;

		public event EventHandler<UserEventArgs> UserLoggedIn;
		public event EventHandler<UserEventArgs> UserLoggedOut;

		Thread MovementUpdateThread;
		Thread RangeUpdateThread;

		public ValkyrieGameServer(GameServerSettings settings)
			: this()
		{
			var assemblies = settings[ServerSettingName.EventAssemblies].Split (';')
				.Where ( s => !string.IsNullOrEmpty(s))
				.Select ( s => Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, s)));

			this.worlds = new ServerWorldManager(assemblies, settings[ServerSettingName.MapDirectory] );
			this.collision = new ServerCollisionProvider(this.worlds);
			this.movement = new ServerNewMovementProvider(this.collision);
			this.movement.PlayerMoved += this.MovementProvider_PlayerMoved;
			this.movement.FailedMovementVerification += this.MovementProvider_FailedVerify;

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

			this.sessionfactory = Fluently.Configure ()
				.Database (PostgreSQLConfiguration.Standard.ConnectionString (s => s.Host (this.settings[ServerSettingName.DatabaseAddress]).Username (this.settings[ServerSettingName.DatabaseUser]).Password (this.settings[ServerSettingName.DatabasePassword]).Database (this.settings[ServerSettingName.DatabaseName]).Port (Convert.ToInt32 (this.settings[ServerSettingName.DatabasePort]))))
				.Mappings (m => m.FluentMappings.AddFromAssembly (Assembly.GetExecutingAssembly ()))
				.BuildSessionFactory ();

			this.server.StartListening();
			this.server.ConnectionMade += this.Server_ConnectionMade;

			this.MovementUpdateThread = new Thread(this.UpdateMovementThread);
			this.MovementUpdateThread.Name = "Movement Update";
			this.MovementUpdateThread.IsBackground = false;
			this.MovementUpdateThread.Start();

			this.RangeUpdateThread = new Thread (this.UpdateRanges);
			this.RangeUpdateThread.Name = "Range Update";
			this.RangeUpdateThread.IsBackground = false;
			this.RangeUpdateThread.Start ();

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
			this.sessionfactory.Close();

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

			PlayerUpdateMessage updatemsg = new PlayerUpdateMessage();
			updatemsg.Action = PlayerUpdateAction.Remove;
			updatemsg.NetworkID = player.NetworkID;

			lock(this.players.PlayerLock)
			{
				foreach(var nplayer in this.players.PlayerRanges[player.NetworkID])
					nplayer.Connection.Send (updatemsg);
			}

			this.players.RemovePlayer (connection);

			this.SaveCharacter(player.Character);

			// fire logged out event
			var handler = this.UserLoggedOut;
			if(handler != null)
				handler (this, new UserEventArgs (player));
		}

		private void SaveCharacter(Character character)
		{
			ISession session = this.sessionfactory.OpenSession ();
			session.SaveOrUpdate(character);
			session.Flush();
			session.Close ();
		}

		private void LoadWorlds()
		{
			this.worlds.Load(new Uri(Path.Combine(Environment.CurrentDirectory, this.settings[ServerSettingName.MapDirectory] + "PokeWorld.xml")), new ValkyrieEventProvider());
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

		private void UpdateRanges ()
		{
			while(this.Started)
			{
				var result = this.players.UpdateRanges ();

				foreach(var player in result)
				{
					foreach(var playerchange in player.Value)
					{
						if(playerchange.State == RangeChangeStates.Add)
						{
							PlayerUpdateMessage updmsg = new PlayerUpdateMessage ()
							{
								NetworkID = playerchange.Player.NetworkID,
								Action = PlayerUpdateAction.Add,
								CharacterName = playerchange.Player.Character.Name,
							};

							player.Key.Connection.Send (updmsg);
						}
						else
						{
							PlayerUpdateMessage updmsg = new PlayerUpdateMessage ()
							{
								NetworkID = playerchange.Player.NetworkID,
								Action = PlayerUpdateAction.Remove,
								CharacterName = playerchange.Player.Character.Name,
							};

							player.Key.Connection.Send (updmsg);
						}
						
					}
				}

				Thread.Sleep (this.RangeUpdateFrequency);
			}
		}
	}
}
