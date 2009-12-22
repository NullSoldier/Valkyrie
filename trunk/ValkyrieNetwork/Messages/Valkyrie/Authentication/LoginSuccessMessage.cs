using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Valkyrie.Messages;

namespace Valkyrie.Library.Network
{
	public class LoginSuccessMessage
		: ServerMessage
	{
		public LoginSuccessMessage()
			: base(ServerMessageType.LoginSuccess)
		{
		}

		public uint NetworkIDAssigned
		{
			get;
			set;
		}

		public override void WritePayload (IValueWriter writerm)
		{
			writerm.WriteUInt32(this.NetworkIDAssigned);
		}

		public override void ReadPayload (IValueReader reader)
		{
			this.NetworkIDAssigned = reader.ReadUInt32();
		}
	}
}
