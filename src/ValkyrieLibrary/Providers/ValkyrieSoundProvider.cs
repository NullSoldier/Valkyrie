using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine;
using Microsoft.Xna.Framework.Audio;
using System.Media;
using Microsoft.Xna.Framework;
using AudioSource = Valkyrie.Engine.Core.Sound.AudioSource;

namespace Valkyrie.Library.Providers
{
	public class ValkyrieSoundProvider
	{
		public float MasterGainModifier
		{
			get;
			set;
		}

		public float SoundGainModifier
		{
			get;
			set;
		}

		public float MusicGainModifier
		{
			get;
			set;
		}

		public bool IsMusicEnabled
		{
			get;
			set;
		}

		public bool IsSoundEnabled
		{
			get;
			set;
		}

		public void PlaySound (AudioSource sound, bool loop)
		{
			throw new NotImplementedException();
		}

		public void PlayBGM (AudioSource sound, bool loop)
		{
			throw new NotImplementedException();
		}

		public void StopBGM ()
		{
			throw new NotImplementedException();
		}

		public void Update (GameTime gameTime)
		{
			throw new NotImplementedException();
		}

		public void LoadEngineContext (IEngineContext engineContext)
		{
			throw new NotImplementedException();
		}

		public void Unload ()
		{
			throw new NotImplementedException();
		}

		public bool IsLoaded
		{
			get { throw new NotImplementedException(); }
		}
	}
}
