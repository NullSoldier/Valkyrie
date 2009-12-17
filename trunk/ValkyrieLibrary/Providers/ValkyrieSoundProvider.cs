using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine;
using Microsoft.Xna.Framework.Audio;
using System.Media;
using Gablarski.OpenAL;
using Valkyrie.Engine.Core.Sound;
using Microsoft.Xna.Framework;

namespace Valkyrie.Library.Providers
{
	public class ValkyrieSoundProvider
		: ISoundProvider
	{
		public void PlaySound (AudioSource sound, bool loop)
		{
			
		}

		public void PlayBGM (AudioSource sound, bool loop)
		{
			return;

			SourceBuffer buffer = SourceBuffer.Generate ();
			buffer.Buffer (sound.PCM, ((sound.Channels == 1) ? AudioFormat.Mono16Bit : AudioFormat.Stereo16Bit), sound.Frequency);
			
			if(this.currentbgmsource != null && this.currentbgmsource.Source != null &&
				this.currentbgmsource.Source.IsPlaying)
			{
				Source source = Source.Generate();
				source.Queue(buffer);

				// If we already have BGM playing, fade it out
				this.currentbgmsource.FadeState = FadeState.FadeOut;
				this.nextbgmsource = new LeasedAudioSource (source, FadeState.None, loop);

				this.leased.Add (nextbgmsource);
			}
			else
			{
				Source source = Source.Generate();
				source.QueueAndPlay (buffer);

				// If nothing is currently playing
				this.currentbgmsource = new LeasedAudioSource (source, FadeState.None, loop);

				this.leased.Add (currentbgmsource);
			}
		}

		public void StopBGM ()
		{
			this.leased.Remove (this.currentbgmsource);
			if(this.currentbgmsource != null && this.currentbgmsource.Source != null)
				this.currentbgmsource.Source.Stop ();

			this.leased.Remove(this.nextbgmsource);
			if(this.nextbgmsource != null && this.nextbgmsource.Source != null)
				this.nextbgmsource.Source.Stop ();
		}

		public void Update (GameTime gameTime)
		{
			return;

			List<LeasedAudioSource> remove = new List<LeasedAudioSource> ();

			// Revise, total crap
			foreach(var source in this.leased)
			{
				source.LastTimeUpdated += gameTime.ElapsedGameTime.Milliseconds;

				if(source.LastTimeUpdated > 60)
				{
					if(source.FadeState == FadeState.FadeOut)
					{
						if((source.Source.Gain - 0.1f) < source.Source.MinimumGain)
						{
							source.Source.Gain = source.Source.MinimumGain;

							if(source.LastTimeUpdated >= 1000)
							{
								source.Source.Stop ();

								if(this.nextbgmsource.Source != null)
								{
									// Play the next BGM now that the current one has faded out
									this.currentbgmsource = this.nextbgmsource;
									this.currentbgmsource.Source.Play ();
								}
							}
						}
						else
						{
							source.Source.Gain -= 0.1f;
							source.LastTimeUpdated = 0;
						}
					}
				}

				if(source.Source.State == SourceState.Paused
					|| source.Source.State == SourceState.Stopped)
				{
					if(source.Loop && source.FadeState == FadeState.None)
					{
						source.Source.Replay ();
						continue;
					}

					remove.Add (source);
					source.Source.Dispose ();
				}
			}

			foreach(var source in remove)
				this.leased.Remove (source);
		}

		public void LoadEngineContext (IEngineContext context)
		{
			this.context = context;

			this.device = OpenAL.GetDefaultPlaybackDevice ();
			if(!device.IsOpen)
				device = device.Open ();

			this.audiocontext = device.CreateAndActivateContext ();

			this.isloaded = true;
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		private IEngineContext context = null;
		private PlaybackDevice device = null;
		private Context audiocontext = null;
		private LeasedAudioSource currentbgmsource;
		private LeasedAudioSource nextbgmsource;
		private List<LeasedAudioSource> leased = new List<LeasedAudioSource> ();
		private bool isloaded = false;
	}

	public class LeasedAudioSource
	{
		public LeasedAudioSource (Source source, FadeState state, bool loop)
		{
			this.Source = source;
			this.FadeState = state;
			this.Loop = loop;

			// Start at 0
			if(state == FadeState.FadeIn)
				this.Source.Gain = 0;
		}

		public Source Source;
		public FadeState FadeState = FadeState.None;
		public bool Loop = false;
		public int LastTimeUpdated = 0;
	}

	public enum FadeState
	{
		None,
		FadeIn,
		FadeOut
	}
}
