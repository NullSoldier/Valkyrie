using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Messages;

namespace Valkyrie.Messages.Valkyrie
{
	public class PlayerStartMovingMessage
		: ClientMessage
	{
		public PlayerStartMovingMessage()
			: base(ClientMessageType.PlayerStartMoving)
		{
		}

		public uint NetworkID { get; set; }
		public int Direction { get; set; }
		public int MovementType { get; set; }
		public string Animation { get; set; }

		public int X { get; set; }
		public int Y { get; set; }

		public float Speed { get; set; }
		public float MoveDelay { get; set; }
	
		public override void  WritePayload(IValueWriter writerm)
		{
			writerm.WriteUInt32(this.NetworkID);
			writerm.WriteInt32(this.Direction);
			writerm.WriteInt32(this.MovementType);
			writerm.WriteString(this.Animation);

			writerm.WriteInt32 (this.X);
			writerm.WriteInt32 (this.Y);

			writerm.WriteBytes(BitConverter.GetBytes(this.Speed));
			writerm.WriteBytes(BitConverter.GetBytes(this.MoveDelay));
		}

		public override void  ReadPayload(IValueReader reader)
		{
			this.NetworkID = reader.ReadUInt32();
			this.Direction = reader.ReadInt32();
			this.MovementType = reader.ReadInt32();
			this.Animation = reader.ReadString();

			this.X = reader.ReadInt32 ();
			this.Y = reader.ReadInt32 ();

			this.Speed = BitConverter.ToSingle(reader.ReadBytes(), 0);
			this.MoveDelay = BitConverter.ToSingle(reader.ReadBytes(), 0);
		}
}
}
