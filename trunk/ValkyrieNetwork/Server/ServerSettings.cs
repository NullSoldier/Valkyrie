using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Gablarski.Server
{
	public class ServerSettings
		: INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private string name = "Gablarski Server";
		public virtual string Name
		{
			get
			{
				lock (settingsLock)
				{
					return this.name;
				}
			}

			set
			{
				lock (settingsLock)
				{
					if (value == this.name)
						return;

					this.name = value;
					this.OnPropertyChanged ("Name");
				}
			}
		}

		private string description = "Default Gablarski Server";
		public virtual string Description
		{
			get
			{
				lock (settingsLock)
				{
					return this.description;
				}
			}

			set
			{
				lock (settingsLock)
				{
					if (value == this.description)
						return;

					this.description = value;
					this.OnPropertyChanged ("Description");
				}
			}
		}

		private int minbitrate = 32000;
		public virtual int MinimumAudioBitrate
		{
			get
			{
				lock (settingsLock)
				{
					return this.minbitrate;
				}
			}

			set
			{
				lock (settingsLock)
				{
					if (value == this.minbitrate)
						return;

					this.minbitrate = value;
					this.OnPropertyChanged ("MinimumAudioBitrate");
				}
			}
		}

		private int maxbitrate = 96000;
		public virtual int MaximumAudioBitrate
		{
			get
			{
				lock (settingsLock)
				{
					return this.maxbitrate;
				}
			}

			set
			{
				lock (settingsLock)
				{
					if (value == this.maxbitrate)
						return;

					this.maxbitrate = value;
					this.OnPropertyChanged ("MaximumAudioBitrate");
				}
			}
		}

		private int defaultbitrate = 64500;
		public virtual int DefaultAudioBitrate
		{
			get
			{
				lock (settingsLock)
				{
					return this.defaultbitrate;
				}
			}

			set
			{
				lock (settingsLock)
				{
					if (value == this.defaultbitrate)
						return;

					this.defaultbitrate = value;
					this.OnPropertyChanged ("DefaultAudioBitrate");
				}
			}
		}

		private string serverLogo = null;
		public virtual string ServerLogo
		{
			get
			{
				lock(settingsLock)
				{
					return this.serverLogo;
				}
			}

			set
			{
				lock (settingsLock)
				{
					if (value == this.serverLogo)
						return;

					this.serverLogo = value;
					this.OnPropertyChanged ("ServerLogo");
				}
			}
		}

		protected readonly object settingsLock = new object();

		protected void OnPropertyChanged (string propertyName)
		{
			var changed = this.PropertyChanged;
			if (changed != null)
				changed (this, new PropertyChangedEventArgs (propertyName));
		}
	}
}