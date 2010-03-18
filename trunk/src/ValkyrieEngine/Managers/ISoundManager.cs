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
		/// <summary>
		/// The folder relative to the game where audio is stored
		/// </summary>
		string SoundRoot { get; set; }

		/// <summary>
		/// Add a sound to the ISoundManager
		/// </summary>
		/// <param name="name">The name of the sound to add</param>
		/// <param name="newSound">The Audiosource to add</param>
		void AddSound (string name, AudioSource newSound);

		/// <summary>
		/// Retrieves and adds a sound from disk from the sound root
		/// </summary>
		/// <param name="filename">The file name of the sound.</param>
		void AddSound (string filename);

		/// <summary>
		/// Gets a specified sound first from the cache and then from disk if it is not cached.
		/// </summary>
		/// <param name="filename">The file name of the sound to get from the sound root.</param>
		/// <returns>Returns the requested sound if it is found, otherwise it loads it. Returns null if it fails to load.</returns>
		AudioSource GetSound (string filename);

		/// <summary>
		/// Get a sound from the cache or load a sound Asynchronously if it's not cached
		/// </summary>
		/// <param name="filename">The file name of the sound to get from the sound root</param>
		/// <param name="callback">The callback to use when the sound is loaded or ready.</param>
		void GetSoundAsync (string filename, Action<SoundLoadedEventArgs> callback);

		/// <summary>
		/// Checks to see if a sound is stored in the cache
		/// </summary>
		/// <param name="filename">The file name of the sound</param>
		/// <returns>True if the sound is stored in the cache</returns>
		bool ContainsSound (string filename);

		/// <summary>
		/// Clears all sounds from the ISoundProvider's cache
		/// </summary>
		void ClearCache ();

		/// <summary>
		/// Clears a specific sound from the ISoundProvider's cache
		/// </summary>
		/// <param name="filename">The file name of the sound to clear.</param>
		/// <exception cref="ArgumentException">Thrown when the texture to clear is not found.</exception>
		void ClearFromCache (string filename);
	}

	public class SoundLoadedEventArgs
		: EventArgs
	{
		public SoundLoadedEventArgs (string name)
		{
			this.Name = name;
		}

		public string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

		private string name;
	}
}
