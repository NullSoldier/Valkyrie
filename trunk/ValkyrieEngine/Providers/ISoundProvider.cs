using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using System.Media;
using Valkyrie.Engine.Core.Sound;
using Microsoft.Xna.Framework;

namespace Valkyrie.Engine.Providers
{
	public interface ISoundProvider
		: IEngineProvider
	{
		void PlaySound (AudioSource sound, bool loop);
		void PlayBGM (AudioSource sound, bool loop);
		void StopBGM ();

		void Update (GameTime gameTime);
	}
}
