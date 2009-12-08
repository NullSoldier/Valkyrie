using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine;
using Microsoft.Xna.Framework.Audio;
using System.Media;

namespace Valkyrie.Library.Providers
{
	public class ValkyrieSoundProvider
		: ISoundProvider
	{
		public void PlaySound (SoundPlayer sound, bool loop)
		{
			if(loop)
				sound.PlayLooping();
			else
				sound.Play();
		}

		public void PlayBGM (SoundPlayer sound, bool loop)
		{
			this.PlaySound(sound, loop);
		}

		#region IEngineProvider Members

		public void LoadEngineContext (IEngineContext context)
		{
			this.context = context;

			this.isloaded = true;
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		private IEngineContext context = null;
		private bool isloaded = false;
		#endregion
	}
}
