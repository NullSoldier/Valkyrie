using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Characters;

namespace Valkyrie.Engine.Providers
{
	public interface IVoiceChatProvider
		: IEngineProvider
	{
		event EventHandler<TalkingChangedEventArgs> UserStartedTalking;
		event EventHandler<TalkingChangedEventArgs> UserStoppedTalking;
		event EventHandler Connected;
		event EventHandler Disconnected;

		bool UseVoiceActivation { get; set; }
		bool IsConnected { get; }

		void Connect (string host, int port, string username, string password);
		void ConnectAsync (string host, int port, string username, string password);
		void Disconnect ();

		void BeginTalk (BaseCharacter character);
		void EndTalk (BaseCharacter character);
	}

	public class TalkingChangedEventArgs
		: EventArgs
	{
		public TalkingChangedEventArgs (object id)
		{
			this.ID = id;
		}

		public object ID
		{
			get { return this.id; }
			set { this.id = value; }
		}

		private object id;
	}
}
