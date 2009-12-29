using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine.Core.Sound;
using Microsoft.Xna.Framework;
using Valkyrie.Engine;

namespace ValkyrieMapEditor.Providers
{
	public class MapEditorSoundProvider
		: ISoundProvider
	{
		public float MasterGainModifier { get; set; }
		public float SoundGainModifier { get; set; }
		public float MusicGainModifier { get; set; }

		public void PlaySound (AudioSource sound, bool loop)
		{
			throw new NotImplementedException ();
		}

		public void PlayBGM (AudioSource sound, bool loop)
		{
			throw new NotImplementedException ();
		}

		public void StopBGM ()
		{
			throw new NotImplementedException ();
		}

		public void Update (GameTime gameTime)
		{
			throw new NotImplementedException ();
		}

		public void LoadEngineContext (IEngineContext context)
		{
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

		private bool isloaded = false;
	}
}
