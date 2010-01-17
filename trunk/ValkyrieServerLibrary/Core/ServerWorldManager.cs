using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Managers;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine.Maps;
using Cadenza;
using Valkyrie.Library.Core;
using Valkyrie.Engine;
using System.Reflection;
using Valkyrie.Library.Providers;
using System.IO;
using Cadenza.Collections;

namespace ValkyrieServerLibrary.Core
{
	public class ServerWorldManager
		: IWorldManager
	{
		#region Constructor

		public ServerWorldManager (IEnumerable<Assembly> eventassemblies, string maproot)
		{
			this.mapprovider = new XMLMapProvider (eventassemblies);
			this.maproot = maproot;
		}

		public ServerWorldManager (IMapProvider mapprovider, string maproot)
		{
			this.mapprovider = mapprovider;
			this.maproot = maproot;
		}

		#endregion

		#region Public Methods/Properties

		public void Load (Uri location, IEventProvider eventprovider)
		{
			lock(this.worldmanagerSync)
			{
				this.worlds = this.WorldProvider.GetWorlds (location).ToDictionary (p => p.Name, p => p);

				string mapdirectory = Path.Combine (Environment.CurrentDirectory, this.maproot);

				foreach(MapHeader header in this.worlds.Values.SelectMany (p => p.Maps.Values))
				{
					header.Map = this.MapProvider.GetMap (new Uri (Path.Combine (mapdirectory, header.MapPath)), eventprovider);
				}
			}
		}

		public IMapProvider MapProvider
		{
			get { return this.mapprovider; }
			set { this.mapprovider = value; }
		}

		public IWorldProvider WorldProvider
		{
			get { return this.worldprovider; }
			set { this.worldprovider = value; }
		}

		public void AddWorld (World world)
		{
			lock(this.worlds)
			{
				if(this.worlds.ContainsKey (world.Name))
					throw new WorldAlreadyExistsException (world.Name);

				this.worlds.Add (world.Name, world);
			}
		}

		public bool RemoveWorld (string name)
		{
			lock(this.worlds)
			{
				return this.worlds.Remove (name);
			}
		}

		public void ClearWorlds ()
		{
			lock(this.worlds)
			{
				this.worlds.Clear ();
			}
		}

		public World GetWorld (string name)
		{
			lock(this.worlds)
			{
				if(!this.worlds.ContainsKey (name))
					throw new Exception ("World not found");

				return this.worlds[name];
			}
		}

		public ReadOnlyDictionary<string, World> GetWorlds ()
		{
			return new ReadOnlyDictionary<string, World> (this.worlds);
		}

		#endregion

		private Dictionary<string, World> worlds = new Dictionary<string, World> ();
		private string currentworldName = string.Empty;
		private object worldmanagerSync = new object ();

		private IWorldProvider worldprovider = new XMLWorldProvider ();
		private IMapProvider mapprovider;
		private bool isloaded = false;
		private string maproot = string.Empty;
		private ITextureManager texturemanager = null;

		#region IEngineProvider Members

		public void LoadEngineContext (IEngineContext context)
		{
			this.maproot = context.Configuration[EngineConfigurationName.MapRoot];

			this.texturemanager = context.TextureManager;

			this.isloaded = true;
		}

		public void Unload ()
		{
			this.isloaded = false;
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		#endregion
	}
}
