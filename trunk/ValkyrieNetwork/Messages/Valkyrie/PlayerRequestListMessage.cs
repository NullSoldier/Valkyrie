using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gablarski.Messages;

namespace ValkyrieServerLibrary.Network.Messages.Valkyrie
{
	public class PlayerRequestListMessage
		: ClientMessage
	{
		public PlayerRequestListMessage()
			: base(ClientMessageType.PlayerRequest)
		{
		}

		public override void WritePayload(Gablarski.IValueWriter writerm)
		{
			
		}

		public override void ReadPayload(Gablarski.IValueReader reader)
		{
			
		}
	}
}
