using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Managers;
using System.Reflection;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine.Maps;
using Mono.Rocks;
using Valkyrie.Engine;
using Valkyrie.Library.Providers;
using System.IO;
using Valkyrie.Library.Core;

namespace Valkyrie.Library.Managers
{
	public class ValkyrieWorldManager
		: IWorldManager
	{
		#region Constructor

		public ValkyrieWorldManager (IEnumerable<Assembly> eventassemblies)
		{
			this.mapprovider = new XMLMapProvider(eventassemblies);
		}

		#endregion

		#region Public Methods/Properties

		public void Load (Uri location, IEventProvider eventprovider)
		{
			if(!this.IsLoaded)
				throw new ProviderNotLoadedException();

			lock(this.worldmanagerSync)
			{
				this.worlds = this.WorldProvider.GetWorlds(location).ToDictionary(p => p.Name, p => p);
				this.currentworldName = this.worlds.Values.FirstOrDefault().Name;

				string mapdirectory = Path.Combine(Environment.CurrentDirectory, this.maproot);

				foreach(MapHeader header in this.worlds.Values.SelectMany(p => p.Maps.Values))
				{
					header.Map = this.MapProvider.GetMap(new Uri(Path.Combine(mapdirectory, header.MapPath)), eventprovider);
					header.Map.Texture = this.texturemanager.GetTexture(header.Map.TextureName);
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

		public ReadOnlyDictionary<string, World> Worlds
		{
			get { return new ReadOnlyDictionary<string, World>(this.worlds); }
		}

		#endregion

		private Dictionary<string, World> worlds = new Dictionary<string, World>();
		private string currentworldName = string.Empty;
		private object worldmanagerSync = new object();

		private IWorldProvider worldprovider = new XMLWorldProvider();
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

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		#endregion
	}
}
