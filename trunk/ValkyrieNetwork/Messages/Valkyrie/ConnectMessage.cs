using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gablarski.Messages;
using Gablarski;

namespace ValkyrieNetwork.Messages.Valkyrie
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

		public override void WritePayload(Gablarski.IValueWriter writerm)
		{
			writerm.WriteVersion(this.Version);
		}

		public override void ReadPayload(Gablarski.IValueReader reader)
		{
			this.Version = reader.ReadVersion();
		}
	}
}
