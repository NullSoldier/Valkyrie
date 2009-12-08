using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Microsoft.Xna.Framework.Audio;
using System.Media;

namespace Valkyrie.Engine.Managers
{
	public interface ISoundManager
	: IEngineProvider
	{
		string SoundRoot { get; set; }

		void Load (string soundroot);

		void AddSound (string Name, SoundPlayer newSound);
		void AddSound (SoundPlayer newSound);
		void AddSound (string FileName);

		SoundPlayer GetSound (string FileName);
		bool ContainsSound (string FileName);

		void ClearCache ();
		void ClearFromCache (string resourcename);
	}
}
