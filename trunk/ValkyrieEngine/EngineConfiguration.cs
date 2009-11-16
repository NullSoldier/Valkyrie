using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Valkyrie.Engine
{
	public enum EngineConfigurationName
	{
		MapRoot,
		GraphicsRoot,
		DefaultModule,
		DataRoot,
		WorldFile
	}

	public class EngineConfiguration
	{
		public EngineConfiguration ()
		{
			this.configuration = new Dictionary<EngineConfigurationName, string>();
		}

		public EngineConfiguration (IDictionary<EngineConfigurationName, string> configuration)
		{
			this.configuration = new Dictionary<EngineConfigurationName, string> (configuration);
		}

		public string this[EngineConfigurationName key]
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

		public bool ContainsKey (EngineConfigurationName key)
		{
			lock (configuration)
			{
				return configuration.ContainsKey(key);
			}
		}

		private readonly Dictionary<EngineConfigurationName, string> configuration;
	}
}