using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gablarski.Messages
{
	public enum ClientMessageType
		: ushort
	{
		Connect = 1,
		Login = 3,
		Disconnect = 9,

		LocationData = 11,
		PlayerRequest = 19,
		PlayerInfoRequest = 21
	}

	public enum ServerMessageType
		: ushort
	{
		LoginFailed = 13,
		LoginSuccess = 15,
		PlayerUpdate = 17,
		PlayerInfo = 23
	}

	public abstract class Message<TMessage>
		: MessageBase
	{
		protected Message (TMessage messageType)
		{
			this.MessageType = messageType;
		}

		public TMessage MessageType
		{
			get;
			protected set;
		}
	}

	//public abstract class DualMessage
	//    : MessageBase
	//{
	//    protected DualMessage (ClientMessage clientMessageType, ServerMessage serverMessageType)
	//    {
	//        this.ClientMessageType = clientMessageType;
	//        this.ServerMessageType = serverMessageType;
	//    }

	//    public ClientMessage ClientMessageType
	//    {
	//        get;
	//        protected set;
	//    }

	//    public ServerMessage ServerMessageType
	//    {
	//        get;
	//        protected set;
	//    }
	//}
}