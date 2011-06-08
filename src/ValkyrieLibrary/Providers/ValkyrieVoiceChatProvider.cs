using System;
using Valkyrie.Engine;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine.Providers;

namespace Valkyrie.Library.Providers
{
	public class ValkyrieVoiceChatProvider
		: IVoiceChatProvider
	{
		public event EventHandler<TalkingChangedEventArgs> UserStartedTalking;

		public event EventHandler<TalkingChangedEventArgs> UserStoppedTalking;

		public event EventHandler Connected;

		public event EventHandler Disconnected;

		public bool UseVoiceActivation
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public bool IsConnected
		{
			get { throw new NotImplementedException(); }
		}

		public void Connect(string host, int port, string username, string password)
		{
			throw new NotImplementedException();
		}

		public void ConnectAsync(string host, int port, string username, string password)
		{
			throw new NotImplementedException();
		}

		public void Disconnect()
		{
			throw new NotImplementedException();
		}

		public void BeginTalk(BaseCharacter character)
		{
			throw new NotImplementedException();
		}

		public void EndTalk(BaseCharacter character)
		{
			throw new NotImplementedException();
		}

		public void LoadEngineContext(IEngineContext context)
		{
			throw new NotImplementedException();
		}

		public void Unload()
		{
			throw new NotImplementedException();
		}

		public bool IsLoaded
		{
			get { throw new NotImplementedException(); }
		}
	}
}