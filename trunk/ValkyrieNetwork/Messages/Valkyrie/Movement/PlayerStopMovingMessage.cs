using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gablarski.Messages;
using Gablarski;

namespace ValkyrieNetwork.Messages.Valkyrie.Movement
{
	public class PlayerStopMovingMessage
		: ClientMessage
	{
		public PlayerStopMovingMessage()
			: base(ClientMessageType.PlayerStopMoving)
		{
		}

		public uint NetworkID { get; set; }
		public int MapX { get; set; }
		public int MapY { get; set; }
		public string Animation { get; set; }
		public int Direction { get; set; }

		public override void WritePayload(IValueWriter writerm)
		{
			writerm.WriteUInt32(this.NetworkID);
			writerm.WriteInt32(this.MapX);
			writerm.WriteInt32(this.MapY);

			writerm.WriteInt32 (this.Direction);
			writerm.WriteString(this.Animation);
		}

		public override void ReadPayload(IValueReader reader)
		{
			this.NetworkID = reader.ReadUInt32();
			this.MapX = reader.ReadInt32();
			this.MapY = reader.ReadInt32();

			this.Direction = reader.ReadInt32 ();
			this.Animation = reader.ReadString();
		}
	}
}
