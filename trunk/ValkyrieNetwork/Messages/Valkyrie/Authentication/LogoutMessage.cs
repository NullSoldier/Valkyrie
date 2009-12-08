using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gablarski.Messages;
using Gablarski;

namespace ValkyrieNetwork.Messages.Valkyrie.Authentication
{
	public class LogoutMessage
		: ClientMessage
	{
		public LogoutMessage ()
			: base(ClientMessageType.Logout)
		{
		}

		public uint NetworkID
		{
			get;
			set;
		}

		public override void WritePayload (IValueWriter writer)
		{
			writer.WriteUInt32(this.NetworkID);
		}

		public override void ReadPayload (IValueReader reader)
		{
			this.NetworkID = reader.ReadUInt32();
		}
	}
}
