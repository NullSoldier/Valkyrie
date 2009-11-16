using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValkyrieServerMonitor
{
	public enum MonitorConfigurationName
	{
		RetryTime,
		GameServerAddress,
		ChatServerAddress,
		TimeStampFormat
	}

	public class MonitorConfiguration
	{
		public MonitorConfiguration()
		{
			this.configuration = new Dictionary<MonitorConfigurationName,string>();
		}

		public MonitorConfiguration(Dictionary<MonitorConfigurationName, string> configuration)
		{
			this.configuration = new Dictionary<MonitorConfigurationName, string>(configuration);
		}

		public string this[MonitorConfigurationName key]
		{
			get
			{
				lock (this.configuration)
				{
					return this.configuration[key];
				}
			}
			set
			{
				lock (this.configuration)
				{
					this.configuration[key] = value;
				}
			}
		}

		private readonly Dictionary<MonitorConfigurationName, string> configuration;
	}
}
