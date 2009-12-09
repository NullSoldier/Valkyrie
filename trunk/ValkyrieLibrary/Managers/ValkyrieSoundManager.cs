using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Managers;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Valkyrie.Engine;
using System.Media;

namespace Valkyrie.Library.Managers
{
	public class ValkyrieSoundManager
		: ISoundManager
	{
		#region Properties and Methods

		public string SoundRoot
		{
			get { return this.soundroot; }
			set { this.soundroot = value; }
		}

		public void Load (string soundroot)
		{
			this.SoundRoot = soundroot;
		}

		public void AddSound (string Name, SoundPlayer newSound)
		{
			this.Resources.Add(Name, newSound);
		}

		public void AddSound (SoundPlayer newSound)
		{
			this.Resources.Add(new FileInfo(newSound.SoundLocation).Name, newSound);
		}

		public void AddSound (string FileName)
		{
			this.AddSound(FileName, new SoundPlayer(Path.Combine(Environment.CurrentDirectory, Path.Combine(SoundRoot, FileName))));
		}

		public SoundPlayer GetSound (string FileName)
		{
			if(!this.Resources.ContainsKey(FileName))
				this.AddSound(FileName);

			return this.Resources[FileName];
		}

		public bool ContainsSound (string FileName)
		{
			return (this.Resources.ContainsKey(FileName));
		}

		public void ClearCache ()
		{
			foreach(var resource in this.Resources)
				resource.Value.Dispose();

			this.Resources.Clear();
		}

		public void ClearFromCache (string resourcename)
		{
			if(this.Resources.Keys.Contains(resourcename))
			{
				this.Resources[resourcename].Dispose();
				this.Resources.Remove(resourcename);
			}
			else
				throw new ArgumentException("Resource does not exist in the cache"); // uneccessary?
		}

		#endregion

		private Dictionary<string, SoundPlayer> Resources = new Dictionary<string, SoundPlayer>();
		private IEngineContext context = null;
		private string soundroot = string.Empty;
		private bool isloaded = false;

		#region IEngineProvider Members

		public void LoadEngineContext (IEngineContext context)
		{
			this.context = context;
			this.Load(this.context.Configuration[EngineConfigurationName.SoundsRoot]);

			this.isloaded = true;
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		#endregion
	}
}
