using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieServerMonitor.Core;
using System.Xml;
using System.IO;

namespace ValkyrieServerMonitor
{
	public static class ConfigManager
	{
		public static MonitorConfiguration LoadConfig(string file)
		{
			FileInfo info = new FileInfo(Path.Combine(Environment.CurrentDirectory, file));
			if (!info.Exists)
			{
				ConfigManager.SaveConfig("MonitorConfiguration.xml", new DefaultMonitorConfiguration());
				return new DefaultMonitorConfiguration();
			}
			
			// Validate with XSD
			XmlDocument document = new XmlDocument();
			document.Load(info.FullName);
			XmlNode root = document.GetElementsByTagName("Configuration")[0];

			MonitorConfiguration config = new MonitorConfiguration();

			foreach (XmlNode node in root.ChildNodes)
			{
				if (node.Name == "RetryTime")
					config[MonitorConfigurationName.RetryTime] = node.InnerText;
				else if (node.Name == "TimeFormat")
					config[MonitorConfigurationName.TimeStampFormat] = node.InnerText;
				else if (node.Name == "GameServerAddress")
					config[MonitorConfigurationName.GameServerAddress] = node.InnerText;
				else if (node.Name == "ChatServerAddress")
					config[MonitorConfigurationName.ChatServerAddress] = node.InnerText;
			}

			return config;
		}

		public static void SaveConfig(string file, MonitorConfiguration configuration)
		{
			FileInfo info = new FileInfo(Path.Combine(Environment.CurrentDirectory, file));

			XmlDocument document = new XmlDocument();
			
			XmlElement config = document.CreateElement("Configuration");

			XmlNode retrynode = document.CreateElement("RetryTime");
			retrynode.InnerText = configuration[MonitorConfigurationName.RetryTime];

			XmlNode timenode = document.CreateElement("TimeFormat");
			timenode.InnerText = configuration[MonitorConfigurationName.TimeStampFormat];

			XmlNode gamenode = document.CreateElement("GameServerAddress");
			gamenode.InnerText = configuration[MonitorConfigurationName.GameServerAddress];

			XmlNode chatnode = document.CreateElement("ChatServerAddress");
			chatnode.InnerText = configuration[MonitorConfigurationName.ChatServerAddress];

			config.AppendChild(retrynode);
			config.AppendChild(timenode);
			config.AppendChild(gamenode);
			config.AppendChild(chatnode);
			document.AppendChild(config);

			document.Save(info.FullName);
		}
	}
}
