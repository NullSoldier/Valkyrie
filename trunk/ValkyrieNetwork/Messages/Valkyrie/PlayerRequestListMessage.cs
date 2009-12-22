using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie;
using Valkyrie.Messages;

namespace ValkyrieServerLibrary.Network.Messages.Valkyrie
{
	public class PlayerRequestListMessage
		: ClientMessage
	{
		public PlayerRequestListMessage()
			: base(ClientMessageType.PlayerRequest)
		{
		}

		public override void WritePayload (IValueWriter writerm)
		{
			
		}

		public override void ReadPayload (IValueReader reader)
		{
			
		}
	}
}
