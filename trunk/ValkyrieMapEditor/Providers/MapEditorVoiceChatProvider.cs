using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine;
using Valkyrie.Engine.Characters;

namespace ValkyrieMapEditor.Providers
{
	public class MapEditorVoiceChatProvider
		: IVoiceChatProvider
	{
		public event EventHandler<TalkingChangedEventArgs> UserStartedTalking;
		public event EventHandler<TalkingChangedEventArgs> UserStoppedTalking;
		public event EventHandler Connected;
		public event EventHandler Disconnected;

		public bool UseVoiceActivation
		{
			get; set;
		}

		public bool IsConnected
		{
			get { return false; }
		}

		public void Connect (string host, int port, string username, string password)
		{
			
		}

		public void ConnectAsync (string host, int port, string username, string password)
		{
			
		}

		public void Disconnect ()
		{
			
		}

		public void BeginTalk (BaseCharacter character)
		{
			throw new NotImplementedException ();
		}

		public void EndTalk (BaseCharacter character)
		{
			throw new NotImplementedException ();
		}

		public void LoadEngineContext (IEngineContext context)
		{
			this.isloaded = true;
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		private bool isloaded = false;
	}
}
