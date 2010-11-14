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
		/// <summary>
		/// The master sound modifier that offsets the gain of all audio played.
		/// </summary>
		/// <remarks>
		/// The maximum value is 1. The minimum value is 0.
		/// </remarks>
		float MasterGainModifier { get; set; }

		/// <summary>
		/// The sound modifier that offsets the gain of all sound effects played.
		/// </summary>
		/// <remarks>
		/// The maximum value is 1. The minimum value is 0.
		/// </remarks>
		float SoundGainModifier { get; set; }

		/// <summary>
		/// The music modifier that offsets the gain of all background music played.
		/// </summary>
		/// <remarks>
		/// The maximum value is 1. The minimum value is 0.
		/// </remarks>
		float MusicGainModifier { get; set; }

		/// <summary>
		/// Determines whether music is enabled or disabled
		/// </summary>
		bool IsMusicEnabled { get; set; }

		/// <summary>
		/// Determines if sound effects within the game are enabled or disabled
		/// </summary>
		bool IsSoundEnabled { get; set; }

		void PlaySound (string sound, bool loop);

		/// <summary>
		/// Play background music
		/// </summary>
		/// <param name="sound">The <seealso cref="AudioSource"/> to play.</param>
		/// <param name="loop">Whether to loop the music or not.</param>
		void PlayBGM (string sound, bool loop);
		
		/// <summary>
		/// Stop the current background music being played as well as any music that's queued to be played.
		/// </summary>
		void StopBGM ();

		/// <summary>
		/// Update all sounds and music being played.
		/// </summary>
		/// <param name="gameTime">The current GameTime.</param>
		void Update (GameTime gameTime);
	}
}
