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
using Gablarski.OpenAL.Providers;

namespace Valkyrie.Library.Providers
{
	public class ValkyrieSoundProvider
		: ISoundProvider
	{
		public float MasterGainModifier
		{
			get { return this.mastergainmodifer; }
			set { this.mastergainmodifer = value; }
		}

		public void PlaySound (AudioSource sound, bool loop)
		{
			if(!this.IsLoaded) return;
		}

		public void PlayBGM (AudioSource sound, bool loop)
		{
			if(!this.IsLoaded) return;

			SourceBuffer buffer = this.GetBuffer ();
			buffer.Buffer (sound.PCM, ((sound.Channels == 1) ? AudioFormat.Mono16Bit : AudioFormat.Stereo16Bit), sound.Frequency);

			if(this.currentbgmsource != null && !this.currentbgmsource.IsDisposed &&
				this.currentbgmsource.Source != null && this.currentbgmsource.Source.IsPlaying)
			{
				Source source = Source.Generate ();
				source.Queue (buffer);

				// If we already have BGM playing, fade it out
				this.currentbgmsource.FadeState = FadeState.FadeOut;

				if(this.nextbgmsource != null)
				{
					this.freebuffers.Push (this.nextbgmsource.DisposeAndReturn ());
					this.leased.Remove (this.nextbgmsource);
				}

				this.nextbgmsource = new LeasedAudioSource (source, buffer, FadeState.None, loop);

				lock(this.leased)
					this.leased.Add (nextbgmsource);
			}
			else
			{
				Source source = Source.Generate ();
				source.QueueAndPlay (buffer);

				// If nothing is currently playing
				this.currentbgmsource = new LeasedAudioSource (source, buffer, FadeState.None, loop);

				lock(this.leased)
					this.leased.Add (currentbgmsource);
			}
		}

		public void StopBGM ()
		{
			if(!this.IsLoaded) return;

			lock(this.leased)
			{
				this.leased.Remove (this.currentbgmsource);
				if(this.currentbgmsource != null && this.currentbgmsource.Source != null &&
					!this.currentbgmsource.IsDisposed)
				{
					this.SetBuffer (this.currentbgmsource.DisposeAndReturn ());
					this.leased.Remove (this.currentbgmsource);
				}

				this.leased.Remove (this.nextbgmsource);
				if(this.nextbgmsource != null && this.nextbgmsource.Source != null &&
					!this.nextbgmsource.IsDisposed)
				{
					this.SetBuffer (this.nextbgmsource.DisposeAndReturn ());
					this.leased.Remove (this.nextbgmsource);
				}
			}
		}

		public void Update (GameTime gameTime)
		{
			if(!this.IsLoaded) return;

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

			        if(source.Source.State == SourceState.Paused ||
						source.Source.State == SourceState.Stopped)
			        {
			            if(source.Loop && source.FadeState == FadeState.None)
			            {
			                source.Source.Replay ();
			                continue;
			            }

			            remove.Add (source);
						this.freebuffers.Push (source.DisposeAndReturn ());
			        }
			    }

			    foreach(var source in remove)
			        this.leased.Remove (source);
			}
		}

		public void LoadEngineContext (IEngineContext engineContext)
		{
			if (this.IsLoaded) return;

			this.context = engineContext;

			this.device = OpenAL.GetDefaultPlaybackDevice ();

			var test = new OpenALPlaybackProvider ();
			test.Open (this.device);

			this.audiocontext = Context.CurrentContext;

			this.isloaded = true;
		}

		public void Unload ()
		{
			lock(this.leased)
			{
				foreach(var leased in this.leased)
					leased.Source.Stop ();

				this.leased.Clear ();

				this.audiocontext.Dispose ();
				this.device.Dispose ();
			}

			this.isloaded = false;
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		private void RequireBuffers (int num)
		{
			lock(this.freebuffers)
			{
				if(this.freebuffers.Count < num)
				{
					int addcount = (num - this.freebuffers.Count);
					for(int i = 0; i < addcount; i++)
						freebuffers.Push (SourceBuffer.Generate ());
				}
			}
		}

		private SourceBuffer GetBuffer ()
		{
			lock(this.freebuffers)
			{
				this.RequireBuffers(4);
				return this.freebuffers.Pop ();
			}
		}

		private void SetBuffer (SourceBuffer buffer)
		{
			lock(this.freebuffers)
			{
				this.freebuffers.Push (buffer);
			}
		}


		private IEngineContext context = null;
		private PlaybackDevice device = null;
		internal Context audiocontext = null;
		private LeasedAudioSource currentbgmsource;
		private LeasedAudioSource nextbgmsource;
		private List<LeasedAudioSource> leased = new List<LeasedAudioSource> ();
		private Stack<SourceBuffer> freebuffers = new Stack<SourceBuffer> ();
		private float mastergainmodifer = -0.5f;
		private bool isloaded = false;
	}

	public class LeasedAudioSource
	{
		public LeasedAudioSource (Source source, SourceBuffer buffer, FadeState state, bool loop)
		{
			this.Source = source;
			this.Buffer = buffer;
			this.FadeState = state;
			this.Loop = loop;

			// Start at 0
			if(state == FadeState.FadeIn)
				this.Source.Gain = 0;
		}

		public Source Source;
		public SourceBuffer Buffer;
		public FadeState FadeState = FadeState.None;
		public float Gain = 1;
		public bool Loop = false;
		public int LastTimeUpdated = 0;
		public bool IsDisposed { get { return this.isdisposed; } }

		private Boolean isdisposed = false;

		public SourceBuffer DisposeAndReturn ()
		{
			this.Source.Stop ();
			this.Source.Dispose ();

			this.isdisposed = true;

			return this.Buffer;
		}

	}

	public enum FadeState
	{
		None,
		FadeIn,
		FadeOut
	}
}
