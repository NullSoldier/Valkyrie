using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Valkyrie.Messages;

namespace Valkyrie.Messages.Valkyrie
{
	public class ConnectMessage
		: ClientMessage
	{
		public ConnectMessage()
			: base(ClientMessageType.Connect)
		{
		}

		public Version Version
		{
			get;
			set;
		}

		public override void WritePayload(IValueWriter writerm)
		{
			writerm.WriteVersion(this.Version);
		}

		public override void ReadPayload(IValueReader reader)
		{
			this.Version = reader.ReadVersion();
		}
	}
}
