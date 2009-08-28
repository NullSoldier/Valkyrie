using System;
using Gablarski.Server;

namespace Gablarski
{
	public class ServerInfo
	{
		internal ServerInfo (IValueReader reader)
		{
			this.Deserialize (reader);
		}

		internal ServerInfo (ServerSettings settings)
		{
			if (settings == null)
				throw new ArgumentNullException("settings");

			this.ServerName = settings.Name;
			this.ServerDescription = settings.Description;
		}

		/// <summary>
		/// Gets the name of the server.
		/// </summary>
		public string ServerName
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the server description.
		/// </summary>
		public string ServerDescription
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the url of the server's logo.
		/// </summary>
		public string ServerLogo
		{
			get;
			private set;
		}

		internal void Serialize (IValueWriter writer)
		{
			writer.WriteString (this.ServerName);
			writer.WriteString (this.ServerDescription);
			writer.WriteString (this.ServerLogo);
		}

		internal void Deserialize (IValueReader reader)
		{
			this.ServerName = reader.ReadString();
			this.ServerDescription = reader.ReadString();
			this.ServerLogo = reader.ReadString();
		}
	}
}