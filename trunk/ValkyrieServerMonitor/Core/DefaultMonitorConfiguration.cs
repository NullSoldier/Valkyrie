using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValkyrieServerMonitor.Core
{
	public class DefaultMonitorConfiguration
		: MonitorConfiguration
	{
		public DefaultMonitorConfiguration()
			: base()
		{
			this[MonitorConfigurationName.RetryTime] = "300";
			this[MonitorConfigurationName.TimeStampFormat] = "t";

			this[MonitorConfigurationName.GameServerAddress] = "127.0.0.1";
			this[MonitorConfigurationName.ChatServerAddress] = "127.0.0.1";
		}
	}
}
