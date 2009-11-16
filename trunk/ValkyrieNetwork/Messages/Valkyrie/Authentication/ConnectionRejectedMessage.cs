using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gablarski;
using Gablarski.Messages;
using Valkyrie.Library.Core.Messages;

namespace ValkyrieNetwork.Messages.Valkyrie
{
	public class ConnectionRejectedMessage
		: ServerMessage
	{
		public ConnectionRejectedMessage()
			: base(ServerMessageType.ConnectionRejected)
		{
		}

		public ConnectionRejectedReason Reason
		{
			get;
			set;
		}

		public override void WritePayload(Gablarski.IValueWriter writerm)
		{
			writerm.WriteByte((byte)this.Reason);
		}

		public override void ReadPayload(Gablarski.IValueReader reader)
		{
			this.Reason = (ConnectionRejectedReason)reader.ReadByte();
		}
	}
}
