using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gablarski.Messages;

namespace ValkyrieNetwork.Messages.Valkyrie
{
	public class PlayerLoadedMessage
		: ClientMessage
	{
		public PlayerLoadedMessage ()
			: base(ClientMessageType.PlayerLoaded)
		{

		}

		public uint NetworkID
		{
			get;
			set;
		}

		public override void WritePayload (Gablarski.IValueWriter writer)
		{
			writer.WriteUInt32(this.NetworkID);
		}

		public override void ReadPayload (Gablarski.IValueReader reader)
		{
			this.NetworkID = reader.ReadUInt32();
		}
	}
}
