using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gablarski.Messages;
using Gablarski;

namespace Valkyrie.Library.Core.Messages
{
	public class LoginFailedMessage
		: ServerMessage
	{
		public LoginFailedMessage ()
			: base (ServerMessageType.LoginFailed)
		{
		}

		public LoginFailedMessage (ConnectionRejectedReason reason)
			: this()
		{
			this.Reason = reason;
		}

		public ConnectionRejectedReason Reason
		{
			get;
			private set;
		}

		public override void WritePayload (IValueWriter writer)
		{
			writer.WriteByte ((byte)this.Reason);
		}

		public override void ReadPayload (IValueReader reader)
		{
			this.Reason = (ConnectionRejectedReason)reader.ReadByte ();
		}
	}

	public enum ConnectionRejectedReason
	{
		ServerDown,
		BadLogin,
		IncompatableVersion
	}
}