using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValkyrieServerLibrary.Core
{
	public class DefaultGameServerSettings
		: GameServerSettings
	{
		public DefaultGameServerSettings ()
		{
			this[ServerSettingName.ServerName] = "Conquest";
			this[ServerSettingName.ServerMaxPlayers] = "100";

			this[ServerSettingName.DatabaseAddress] = "127.0.0.1";
			this[ServerSettingName.DatabasePort] = "5432";
			this[ServerSettingName.DatabaseName] = "pokemon";
			this[ServerSettingName.DatabaseUser] = "pokemon";
			this[ServerSettingName.DatabasePassword] = "pokemon";

			this[ServerSettingName.ExperienceRate] = "1";
			this[ServerSettingName.DropRate] = "1";

			this[ServerSettingName.MapDirectory] = "Maps\\";
		}
	}
}
