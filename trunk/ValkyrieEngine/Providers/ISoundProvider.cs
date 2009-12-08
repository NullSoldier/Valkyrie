using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using System.Media;

namespace Valkyrie.Engine.Providers
{
	public interface ISoundProvider
		: IEngineProvider
	{
		void PlaySound(SoundPlayer sound, bool loop);
		void PlayBGM (SoundPlayer sound, bool loop);
	}
}
