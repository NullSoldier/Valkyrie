using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValkyrieServerLibrary.Core
{
	public enum ServerSettingName
	{
		ServerName,
		ServerMaxPlayers,

		DatabaseAddress,
		DatabasePort,
		DatabaseUser,
		DatabasePassword,
		DatabaseName,

		ExperienceRate,
		DropRate,

		MapDirectory
	}

	public class GameServerSettings
	{
		public GameServerSettings ()
		{
			this.configuration = new Dictionary<ServerSettingName, string> ();
		}

		public GameServerSettings (Dictionary<ServerSettingName, string> config)
		{
			this.configuration = new Dictionary<ServerSettingName, string> (config);
		}

		public string this[ServerSettingName setting]
		{
			get
			{
				lock (this.configuration)
				{
					return this.configuration[setting];
				}
			}
			set
			{
				lock (this.configuration)
				{
					this.configuration[setting] = value;
				}
			}
		}

		private Dictionary<ServerSettingName, string> configuration;
	}
}
