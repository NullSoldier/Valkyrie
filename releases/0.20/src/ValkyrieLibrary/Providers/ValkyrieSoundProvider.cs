using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gablarski.Audio;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine;
using Microsoft.Xna.Framework.Audio;
using System.Media;
using Gablarski.OpenAL;
using Microsoft.Xna.Framework;
using AudioFormat = Gablarski.OpenAL.AudioFormat;
using AudioSource = Valkyrie.Engine.Core.Sound.AudioSource;

namespace Valkyrie.Library.Providers
{
	public class ValkyrieSoundProvider
	{
		public float MasterGainModifier
		{
			get { return this.mastergainmodifer; }
			set { this.mastergainmodifer = value; }
		}

		public float SoundGainModifier
		{
			get { return this.soundgainmodifier; }
			set { this.soundgainmodifier = value; }
		}

		public float MusicGainModifier
		{
			get { return this.musicgainmodifier; }
			set { this.musicgainmodifier = value; }
		}

		public bool IsMusicEnabled
		{
			get { return this.ismusicenabled; }
			set { this.ismusicenabled = value; }
		}

		public bool IsSoundEnabled
		{
			get { return this.issoundenabled; }
			set { this.issoundenabled = value; }
		}

		public void PlaySound (AudioSource sound, bool loop)
		{
			if(!this.IsSoundEnabled)
				return;
		}

		public void PlayBGM (AudioSource sound, bool loop)
		{
			if(!this.IsMusicEnabled)
				return;

			SourceBuffer buffer = SourceBuffer.Generate ();
			buffer.Buffer (sound.PCM, ((sound.Channels == 1) ? AudioFormat.Mono16Bit : AudioFormat.Stereo16Bit), sound.Frequency);

			if(this.currentbgmsource != null && this.currentbgmsource.Source != null &&
			    this.currentbgmsource.Source.IsPlaying)
			{
			    Source source = Source.Generate ();
			    source.Queue (buffer);

			    // If we already have BGM playing, fade it out
			    this.currentbgmsource.FadeState = FadeState.FadeOut;
			    this.nextbgmsource = new LeasedAudioSource (source, FadeState.None, loop);

			    lock(this.leased)
			        this.leased.Add (nextbgmsource);
			}
			else
			{
			    Source source = Source.Generate ();
			    source.QueueAndPlay (buffer);

			    // If nothing is currently playing
			    this.currentbgmsource = new LeasedAudioSource (source, FadeState.None, loop);

			    lock(this.leased)
			        this.leased.Add (currentbgmsource);
			}
		}

		public void StopBGM ()
		{
			lock(this.leased)
				this.leased.Remove (this.currentbgmsource);

			if(this.currentbgmsource != null && this.currentbgmsource.Source != null)
				this.currentbgmsource.Source.Stop ();

			lock(this.leased)
				this.leased.Remove (this.nextbgmsource);

			if(this.nextbgmsource != null && this.nextbgmsource.Source != null)
				this.nextbgmsource.Source.Stop ();
		}

		public void Update (GameTime gameTime)
		{
			List<LeasedAudioSource> remove = new List<LeasedAudioSource> ();

			//Revise, total crap
			lock(this.leased)
			{
				foreach(var source in this.leased)
				{
					//Alter the real gain with the gain modifier
					source.Source.Gain = Helpers.Clamp (source.Gain + this.MasterGainModifier, 0, 1);

					source.LastTimeUpdated += gameTime.ElapsedGameTime.Milliseconds;

					if(source.LastTimeUpdated > 60)
					{
						if(source.FadeState == FadeState.FadeOut)
						{
							if((source.Gain - 0.1f) < source.Source.MinimumGain)
							{
								source.Gain = source.Source.MinimumGain;

								if(source.LastTimeUpdated >= 1000)
								{
									source.Source.Stop ();
									if(source.Source.ProcessedBuffers > 0)
									{
										SourceBuffer[] buffers = source.Source.Dequeue ();
										for(int i = 0; i < buffers.Length; ++i)
											buffers[i].Dispose ();
									}

									if(this.nextbgmsource.Source != null)
									{
										//Play the next BGM now that the current one has faded out
										this.currentbgmsource = this.nextbgmsource;
										this.currentbgmsource.Source.Play ();
									}
								}
							}
							else
							{
								source.Gain -= 0.1f;
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
		}

		public void LoadEngineContext (IEngineContext engineContext)
		{
			if(this.context != null)
				return;

			this.context = engineContext;

			this.device = OpenAL.GetDefaultPlaybackDevice ();
			if(!device.IsOpen)
				device = device.Open ();

			this.audiocontext = device.CreateAndActivateContext ();

			this.isloaded = true;
		}

		public void Unload ()
		{

		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		private IEngineContext context = null;
		private PlaybackDevice device = null;
		internal Context audiocontext = null;
		private LeasedAudioSource currentbgmsource;
		private LeasedAudioSource nextbgmsource;
		private List<LeasedAudioSource> leased = new List<LeasedAudioSource> ();
		private float mastergainmodifer = 0;
		private float soundgainmodifier = 0;
		private float musicgainmodifier = -0.3f;
		private bool ismusicenabled = false;
		private bool issoundenabled = false;
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
		public float Gain = 1;
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
