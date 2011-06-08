using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine.Core.Sound;
using Gablarski.OpenAL;
using Microsoft.Xna.Framework;
using Valkyrie.Engine;
using Microsoft.Xna.Framework.Audio;
using Valkyrie.Library.Managers;

namespace Valkyrie.Library.Providers
{
	public class ValkyrieContentSoundProvider : ISoundProvider
	{
		public ValkyrieContentSoundProvider(ValkyrieSoundContentManager manager)
		{
			this.manager = manager;
		}

		public float MasterGainModifier
		{
			get { return this.mastergainmodifer; }
			set { this.mastergainmodifer = value; }
		}

		public float SoundGainModifier
		{
			get { return this.soundgainmodifier; }
			set { this.soundgainmodifier = value; }
		}

		public float MusicGainModifier
		{
			get { return this.musicgainmodifier; }
			set { this.musicgainmodifier = value; }
		}

		public bool IsMusicEnabled
		{
			get { return this.ismusicenabled; }
			set { this.ismusicenabled = value; }
		}

		public bool IsSoundEnabled
		{
			get { return this.issoundenabled; }
			set { this.issoundenabled = value; }
		}

		public void PlaySound(string sound, bool loop)
		{
			if(!this.IsSoundEnabled)
				return;

			this.manager.GetSoundEffect(sound).Play(this.SoundGainModifier, 0f, 0f);
		}

		public void PlayBGM(string sound, bool loop)
		{
			if(!this.IsMusicEnabled)
				return;

			
		}

		public void StopBGM ()
		{
			
		}

		public void Update (GameTime gameTime)
		{
			
		}

		public void LoadEngineContext (IEngineContext engineContext)
		{
			if(this.context != null)
				return;

			this.context = engineContext;
			this.manager.LoadEngineContext(engineContext);

			this.isloaded = true;
		}

		public void Unload ()
		{
			this.isloaded = false;
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		private IEngineContext context = null;
		private ValkyrieGenericManager<SoundEffect> Effects = new ValkyrieGenericManager<SoundEffect>();
		private ValkyrieSoundContentManager manager = null;

		private float mastergainmodifer = 0;
		private float soundgainmodifier = 0;
		private float musicgainmodifier = -0.3f;
		private bool ismusicenabled = false;
		private bool issoundenabled = false;
		private bool isloaded = false;
	}
}
