using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gablarski.Messages;

namespace ValkyrieServerLibrary.Network.Messages.Valkyrie
{
	public class PlayerRequestMessage
		: ClientMessage
	{
		public PlayerRequestMessage()
			: base(ClientMessageType.PlayerInfoRequest)
		{
		}

		public uint NetworkID
		{
			get;
			set;
		}

		public override void WritePayload(Gablarski.IValueWriter writerm)
		{
			writerm.WriteUInt32(this.NetworkID);
		}

		public override void ReadPayload(Gablarski.IValueReader reader)
		{
			this.NetworkID = reader.ReadUInt32();
		}
	}
}
