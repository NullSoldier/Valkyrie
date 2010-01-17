using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Valkyrie.Messages;

namespace Valkyrie.Messages.Valkyrie.Movement
{
	public class PlayerStopMovingMessage
		: ClientMessage
	{
		public PlayerStopMovingMessage()
			: base(ClientMessageType.PlayerStopMoving)
		{
		}

		public uint NetworkID { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public string Animation { get; set; }
		public int Direction { get; set; }

		public override void WritePayload(IValueWriter writerm)
		{
			writerm.WriteUInt32(this.NetworkID);
			writerm.WriteInt32(this.X);
			writerm.WriteInt32(this.Y);

			writerm.WriteInt32 (this.Direction);
			writerm.WriteString(this.Animation);
		}

		public override void ReadPayload(IValueReader reader)
		{
			this.NetworkID = reader.ReadUInt32();
			this.X = reader.ReadInt32();
			this.Y = reader.ReadInt32();

			this.Direction = reader.ReadInt32 ();
			this.Animation = reader.ReadString();
		}
	}
}
