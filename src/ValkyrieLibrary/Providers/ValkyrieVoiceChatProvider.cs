using System;
using System.Collections.Generic;
using Gablarski;
using Gablarski.Audio;
using Gablarski.Client;
using Gablarski.Messages;
using Gablarski.Network;
using Gablarski.OpenAL;
using Gablarski.OpenAL.Providers;
using Valkyrie.Engine;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine.Providers;

namespace Valkyrie.Library.Providers
{
	public class ValkyrieVoiceChatProvider
		: IVoiceChatProvider
	{
		public ValkyrieVoiceChatProvider (ValkyrieSoundProvider sound)
		{
			this.oalContext = sound.audiocontext;
		}
		
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

			SetupGablarski ((this.setupcount >= 1));
			
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
			
			if (voice == null || capture == null)
				return;

			List<UserInfo> infos = new List<UserInfo> ();

			foreach(var player in this.context.NetworkProvider.GetPlayers ())
			{
				var user = gb.Users[Convert.ToInt32 (player.ID)];

				if(user != null)
					infos.Add (user);
			}

			gb.Audio.BeginCapture (voice, infos);
		}

		public void EndTalk (BaseCharacter character)
		{
			if (character == null)
				throw new ArgumentNullException ("character");

			if (voice == null || capture == null)
				return;

			gb.Audio.EndCapture (voice);
		}

		// Loads the engine context which you can use to access the other providers
		public void LoadEngineContext (IEngineContext engineContext)
		{
			this.context = engineContext;
			this.IsLoaded = true;
		}

		public void Unload ()
		{
			this.TearDownGablarski ();

			this.IsLoaded = false;
		}

		public bool IsLoaded
		{
			get;
			private set;
		}

		// Point of access for other providers
		private IEngineContext context;
		
		private readonly object key = new object();
		private int setupcount = 0;
		
		private bool useVA;
		private GablarskiClient gb;
		private AudioSource voice;
		
		private readonly Context oalContext;
		private IPlaybackProvider playback; 
		private ICaptureProvider capture;
		private AudioEngine audioengine;
		
		private string username;
		private string password;

		private void SetupGablarski ()
		{
			this.SetupGablarski (false);
		}

		private void SetupGablarski (bool reconnect)
		{
			if (gb != null && !reconnect)
				return;				

			playback = new OpenALPlaybackProvider ();
			playback.Device = playback.DefaultDevice;

			capture = new OpenALCaptureProvider();
			capture.Device = capture.DefaultDevice;

			if (capture.Device == null)
				capture = null;

			AudioEngine audioengine = new AudioEngine ();

			gb = new GablarskiClient (new NetworkClientConnection (), audioengine);
			gb.Audio.AudioReceiver = new ValkyrieAudioChatReceiver (gb.Sources, this.context);

			gb.Connected += HandleGbConnected;
			gb.ConnectionRejected += HandleGbConnectionRejected;
			gb.Disconnected += OnDisconnected;
			
			gb.CurrentUser.ReceivedLoginResult += HandleGbCurrentUserReceivedLoginResult;
			gb.CurrentUser.ReceivedJoinResult += HandleGbCurrentUserReceivedJoinResult;

			gb.Sources.ReceivedSourceList += HandleGbSourcesReceivedSourceList;
			gb.Sources.ReceivedAudioSource += HandleGbSourcesReceivedAudioSource;
			gb.Sources.AudioSourceStarted += HandleGbSourcesAudioSourceStarted;
			gb.Sources.AudioSourceStopped += HandleGbSourcesAudioSourceStopped;

			this.setupcount++;
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
					OnConnected (this, EventArgs.Empty);

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
			
			if (capture != null)
				gb.Sources.Request ("voice", 1, 48000, 512);
			else
				OnConnected (this, EventArgs.Empty);
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

		private void OnConnected (object sender, EventArgs e)
		{
			var connected = Connected;
			if (connected != null)
				connected (this, EventArgs.Empty);
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

			gb.Audio.Attach (capture, Gablarski.Audio.AudioFormat.Mono16Bit, this.voice, coptions);
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