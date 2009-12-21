using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine;
using Valkyrie.Engine.Characters;

namespace Valkyrie.Library.Providers
{
	public class ValkyrieVoiceChatProvider
		: IVoiceChatProvider
	{
		public event EventHandler<TalkingChangedEventArgs> UserStartedTalking;
		public event EventHandler<TalkingChangedEventArgs> UserStoppedTalking;
		public event EventHandler Connected;
		public event EventHandler Disconnected;

		public bool IsConnected { get; set; }
		public bool UseVoiceActivation { get; set; }

		public void Connect (string host, int port, string username, string password)
		{
			throw new NotImplementedException ();
		}

		public void ConnectAsync (string host, int port, string username, string password)
		{
			throw new NotImplementedException ();
		}

		public void Disconnect ()
		{
			throw new NotImplementedException ();
		}

		public void BeginTalk (BaseCharacter character)
		{
			throw new NotImplementedException ();
		}

		public void EndTalk (BaseCharacter character)
		{
			throw new NotImplementedException ();
		}

		// Loads the engine context which you can use to access the other providers
		public void LoadEngineContext (IEngineContext context)
		{
			this.context = context;

			this.isloaded = true;
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		// Point of access for other providers
		private IEngineContext context;
		private bool isloaded = false;
	}
}
