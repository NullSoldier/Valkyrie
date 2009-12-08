using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine;
using Valkyrie.Engine.Providers;

namespace Valkyrie.Providers
{
	public class PokeSoundProvider
		:  ISoundProvider
	{
		public void PlaySound (System.Media.SoundPlayer sound, bool loop)
		{
			throw new NotImplementedException();
		}

		public void PlayBGM (System.Media.SoundPlayer sound, bool loop)
		{
			throw new NotImplementedException();
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		public void LoadEngineContext (IEngineContext context)
		{
			this.context = context;

			this.isloaded = true;
		}

		private IEngineContext context = null;
		private bool isloaded = false;
	}
}
