using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Messages;
using Valkyrie;

namespace Valkyrie.Messages.Movement
{
	public class ServerMovementMessage
		: ServerMessage
	{
		public ServerMovementMessage ()
			: base (ServerMessageType.ServerMovementMessage)
		{
		}

		public uint NetworkID { get; set; }

		public int X { get; set; }
		public int Y { get; set; }
		public int Direction { get; set; }

		public string Animation { get; set; }

		public override void WritePayload (IValueWriter writer)
		{
			writer.WriteUInt32 (this.NetworkID);

			writer.WriteInt32 (this.X);
			writer.WriteInt32 (this.Y);
			writer.WriteInt32 (this.Direction);

			writer.WriteString (this.Animation);
		}

		public override void ReadPayload (IValueReader reader)
		{
			this.NetworkID = reader.ReadUInt32 ();

			this.X = reader.ReadInt32 ();
			this.Y = reader.ReadInt32 ();
			this.Direction = reader.ReadInt32 ();

			this.Animation = reader.ReadString ();
		}
	}
}
