using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gablarski.Audio;
using Gablarski.Client;
using Valkyrie.Engine;

namespace Valkyrie.Library.Providers
{
	public class ValkyrieAudioChatReceiver
		: IAudioReceiver
	{
		public ValkyrieAudioChatReceiver (ClientSourceManager sourcemanager, IEngineContext context)
		{
			sourcemanager.AudioSourceMuted += this.Receiver_AudioSourceMuted;
			sourcemanager.AudioSourceStarted += this.Receiver_AudioSourceStarted;
			sourcemanager.AudioSourceStopped += this.Receiver_AudioSourceStopped;
			sourcemanager.ReceivedAudio += this.Receiver_ReceivedAudio;

			this.context = context;
		}

		public void Receiver_AudioSourceMuted (object sender, AudioSourceMutedEventArgs e)
		{
			var source_muted = this.AudioSourceMuted;
			if(source_muted != null)
				source_muted (sender, e);
		}

		public void Receiver_AudioSourceStarted (object sender, AudioSourceEventArgs e)
		{
			var source_started = this.AudioSourceStarted;
			if(source_started != null)
				source_started (sender, e);
		}

		public void Receiver_AudioSourceStopped (object sender, AudioSourceEventArgs e)
		{
			var source_stopped = this.AudioSourceStopped;
			if(source_stopped != null)
				source_stopped (sender, e);
		}

		public void Receiver_ReceivedAudio (object sender, ReceivedAudioEventArgs e)
		{
			var foreignplayer = this.context.NetworkProvider.GetPlayer (e.Source.OwnerId);
			var homeplayer = this.context.SceneProvider.GetPlayer ("player1");

			if(foreignplayer == null || homeplayer == null)
				return;

			var range = foreignplayer.GlobalTileLocation - homeplayer.GlobalTileLocation;

			if(Math.Abs (range.X) >= 12 || Math.Abs (range.Y) > 12)
				return;

			var received_audio = this.ReceivedAudio;
			if(received_audio != null)
				received_audio (sender, e);
		}

		public event EventHandler<AudioSourceMutedEventArgs> AudioSourceMuted;

		public event EventHandler<AudioSourceEventArgs> AudioSourceStarted;

		public event EventHandler<AudioSourceEventArgs> AudioSourceStopped;

		public event EventHandler<ReceivedAudioEventArgs> ReceivedAudio;

		private IEngineContext context;
	}
}
