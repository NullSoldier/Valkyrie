using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Microsoft.Xna.Framework.Audio;
using System.Media;
using Valkyrie.Engine.Core.Sound;

namespace Valkyrie.Engine.Managers
{
	public interface ISoundManager
	: IEngineProvider
	{
		string SoundRoot { get; set; }

		void Load (string soundroot);

		void AddSound (string name, AudioSource newSound);
		void AddSound (string filename);

		AudioSource GetSound (string filename);
		bool ContainsSound (string filename);

		void ClearCache ();
		void ClearFromCache (string resourcename);
	}
}
