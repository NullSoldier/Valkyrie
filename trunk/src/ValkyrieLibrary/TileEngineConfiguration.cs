using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Valkyrie.Library
{
	public enum TileEngineConfigurationName
	{
		MapRoot,
		GraphicsRoot,
		DefaultModule,
		DataRoot,
		EntryMap,
		EntryPoint
	}

	public class TileEngineConfiguration
	{
		public TileEngineConfiguration ()
		{
			this.configuration = new Dictionary<TileEngineConfigurationName, string>();
		}

		public TileEngineConfiguration (IDictionary<TileEngineConfigurationName, string> configuration)
		{
			this.configuration = new Dictionary<TileEngineConfigurationName, string> (configuration);
		}

		public string this[TileEngineConfigurationName key]
		{
			get
			{
				lock (configuration)
				{
					return configuration[key];
				}
			}

			set
			{
				lock (configuration)
				{
					configuration[key] = value;
				}
			}
		}

		public bool ContainsKey (TileEngineConfigurationName key)
		{
			lock (configuration)
			{
				return configuration.ContainsKey(key);
			}
		}

		private readonly Dictionary<TileEngineConfigurationName, string> configuration;
	}
}