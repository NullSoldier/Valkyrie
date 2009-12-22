using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Valkyrie.Library.Core.Messages;
using Valkyrie.Messages;

namespace Valkyrie.Messages.Valkyrie
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

		public override void WritePayload(IValueWriter writerm)
		{
			writerm.WriteByte((byte)this.Reason);
		}

		public override void ReadPayload(IValueReader reader)
		{
			this.Reason = (ConnectionRejectedReason)reader.ReadByte();
		}
	}
}
