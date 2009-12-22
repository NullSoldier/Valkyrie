using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gablarski.OpenAL.Providers;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine;
using Valkyrie.Engine.Characters;
using Gablarski.Client;
using Gablarski.Network;
using Gablarski.Audio;
using Gablarski.Messages;

namespace Valkyrie.Library.Providers
{
	public class ValkyrieVoiceChatProvider
		: IVoiceChatProvider
	{
		public event EventHandler<TalkingChangedEventArgs> UserStartedTalking;
		public event EventHandler<TalkingChangedEventArgs> UserStoppedTalking;
		public event EventHandler Connected;
		public event EventHandler Disconnected;

		public bool IsConnected
		{
			get { return (gb != null) ? gb.IsConnected : false; }
		}
		
		public bool UseVoiceActivation
		{
			get { return this.useVA; }
			set
			{
				if (value)
					throw new NotSupportedException();
			}
		}
		
		public void Connect (string host, int port, string userName, string passWord)
		{
			throw new NotSupportedException();
		}

		public void ConnectAsync (string host, int port, string userName, string passWord)
		{
			if (host == null)
				throw new ArgumentNullException ("host");
			if (port <= 0)
				throw new ArgumentException ("port");
			if (userName == null)
				throw new ArgumentNullException ("username");
			if (passWord == null)
				throw new ArgumentNullException ("password");
			
			SetupGablarski();
			
			this.username = userName;
			this.password = passWord;
			gb.Connect (host, port);
		}

		public void Disconnect ()
		{
			TearDownGablarski();
		}

		public void BeginTalk (BaseCharacter character)
		{
			if (character == null)
				throw new ArgumentNullException ("character");
			
			if (voice == null)
				return;

			gb.Audio.BeginCapture (voice, gb.CurrentChannel);
		}

		public void EndTalk (BaseCharacter character)
		{
			if (character == null)
				throw new ArgumentNullException ("character");

			gb.Audio.EndCapture (voice, gb.CurrentChannel);
		}

		// Loads the engine context which you can use to access the other providers
		public void LoadEngineContext (IEngineContext engineContext)
		{
			this.context = engineContext;
			this.IsLoaded = true;
		}

		public bool IsLoaded
		{
			get;
			private set;
		}

		// Point of access for other providers
		private IEngineContext context;
		
		private readonly object key = new object();
		
		private bool useVA;
		private GablarskiClient gb;
		private AudioSource voice;
		
		private IPlaybackProvider playback; 
		private ICaptureProvider capture;
		
		private string username;
		private string password;
		
		private void SetupGablarski()
		{
			if (gb != null)
				return;

			playback = new OpenALPlaybackProvider();
			playback.Device = playback.DefaultDevice;

			capture = new OpenALCaptureProvider();
			capture.Device = capture.DefaultDevice;
			
			gb = new GablarskiClient (new NetworkClientConnection());
			gb.Connected += HandleGbConnected;
			gb.ConnectionRejected += HandleGbConnectionRejected;
			gb.Disconnected += OnDisconnected;
			
			gb.CurrentUser.ReceivedLoginResult += HandleGbCurrentUserReceivedLoginResult;
			gb.CurrentUser.ReceivedJoinResult += HandleGbCurrentUserReceivedJoinResult;

			gb.Sources.ReceivedSourceList += new EventHandler<ReceivedListEventArgs<AudioSource>>(HandleGbSourcesReceivedSourceList);
			gb.Sources.ReceivedAudioSource += HandleGbSourcesReceivedAudioSource;
			gb.Sources.AudioSourceStarted += HandleGbSourcesAudioSourceStarted;
			gb.Sources.AudioSourceStopped += HandleGbSourcesAudioSourceStopped;
		}

		void HandleGbSourcesReceivedSourceList(object sender, ReceivedListEventArgs<AudioSource> e)
		{
			gb.Audio.Attach (playback, e.Data, new AudioEnginePlaybackOptions());
		}

		void HandleGbSourcesAudioSourceStarted (object sender, AudioSourceEventArgs e)
		{
			var startedTalking = UserStartedTalking;
			if (startedTalking != null)
				startedTalking (this, new TalkingChangedEventArgs (e.Source.OwnerId));
		}

		void HandleGbSourcesAudioSourceStopped (object sender, AudioSourceEventArgs e)
		{
			var stoppedTalking = UserStoppedTalking;
			if (stoppedTalking != null)
				stoppedTalking (this, new TalkingChangedEventArgs (e.Source.OwnerId));
		}

		void HandleGbSourcesReceivedAudioSource (object sender, ReceivedAudioSourceEventArgs e)
		{
			switch (e.Result)
			{	
				case SourceResult.Succeeded:
					this.voice = e.Source;
					SetupSource();

					break;

				case SourceResult.NewSource:
					gb.Audio.Attach (playback, e.Source, new AudioEnginePlaybackOptions());
					break;

				case SourceResult.SourceRemoved:
					gb.Audio.Detach (e.Source);
					break;
					
				case SourceResult.FailedInvalidArguments:
				case SourceResult.FailedLimit:
				case SourceResult.FailedPermissions:
				case SourceResult.FailedUnknown:
					Disconnect();
					break;
			}
		}

		void HandleGbCurrentUserReceivedJoinResult (object sender, ReceivedJoinResultEventArgs e)
		{
			if (e.Result != Gablarski.LoginResultState.Success)
			{
				Disconnect();
				return;
			}
			
			gb.Sources.Request ("voice", 1);
		}

		void HandleGbCurrentUserReceivedLoginResult (object sender, ReceivedLoginResultEventArgs e)
		{
			if (!e.Result.Succeeded)
			{
				Disconnect();
				return;
			}
			
			gb.CurrentUser.Join (username, null);
		}

		void HandleGbConnectionRejected (object sender, RejectedConnectionEventArgs e)
		{
			Disconnect();
		}

		void OnDisconnected (object sender, EventArgs e)
		{
			var disconnected = Disconnected;
			if (disconnected != null)
				disconnected (this, EventArgs.Empty);
		}

		void HandleGbConnected (object sender, EventArgs e)
		{
			gb.CurrentUser.Login (username, password);
		}
		
		private void SetupSource()
		{
			var coptions = new AudioEngineCaptureOptions();
			if (UseVoiceActivation)
			{
				coptions.Mode = AudioEngineCaptureMode.Activated;
				// uhh?
				//coptions.StartVolume = ?
				//coptions.ContinuationVolume = ?
				//coptions.ContinueThreshold = ?
			}
			else
				coptions.Mode = AudioEngineCaptureMode.Explicit;

			gb.Audio.Attach (capture, AudioFormat.Mono16Bit, this.voice, coptions);
		}
		
		private void TearDownGablarski()
		{
			if (gb == null)
				return;
			
			gb.Connected -= HandleGbConnected;
			gb.ConnectionRejected -= HandleGbConnectionRejected;
			gb.Disconnected -= OnDisconnected;
			
			gb.CurrentUser.ReceivedJoinResult -= HandleGbCurrentUserReceivedJoinResult;
			gb.CurrentUser.ReceivedLoginResult -= HandleGbCurrentUserReceivedLoginResult;
			
			gb.Sources.ReceivedAudioSource -= HandleGbSourcesReceivedAudioSource;
			gb.Sources.AudioSourceStarted -= HandleGbSourcesAudioSourceStarted;
			gb.Sources.AudioSourceStopped -= HandleGbSourcesAudioSourceStopped;

			if (gb.IsConnected)
				gb.Disconnect();
			else
				OnDisconnected (this, EventArgs.Empty);
		}
	}
}