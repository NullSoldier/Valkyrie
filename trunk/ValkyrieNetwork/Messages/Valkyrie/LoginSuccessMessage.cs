using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gablarski.Messages;

namespace ValkyrieLibrary.Network
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

		public override void WritePayload(Gablarski.IValueWriter writerm)
		{
			writerm.WriteUInt32(this.NetworkIDAssigned);
		}

		public override void ReadPayload(Gablarski.IValueReader reader)
		{
			this.NetworkIDAssigned = reader.ReadUInt32();
		}
	}
}
