using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine;
using System.IO;

namespace Valkyrie.Library.Managers
{
	public class ValkyrieSoundContentManager : IEngineProvider
	{
		public ValkyrieSoundContentManager(ContentManager manager)
		{
			this.manager = manager;
		}

		public SoundEffect GetSoundEffect(string name)
		{
			if (!effects.ContainsKey(name))
				this.LoadResource(name);

			return effects[name];
		}

		private Dictionary<string, SoundEffect> effects = new Dictionary<string, SoundEffect>();
		private ContentManager manager = null;
		private IEngineContext context = null;
		private bool isloaded = false;
		private string soundpath = string.Empty;
		
		private void LoadResource(string name)
		{
			if (!this.isloaded)
				throw new ProviderNotLoadedException();

			var resource = manager.Load<SoundEffect> (name);

			effects.Add(name, resource);
		}

		#region IEngineProvider Members

		public void LoadEngineContext(IEngineContext context)
		{
			this.context = context;
			this.soundpath = context.Configuration[EngineConfigurationName.SoundsRoot];

			this.isloaded = true;
		}

		public void Unload()
		{
			this.isloaded = false;
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		#endregion
	}
}
