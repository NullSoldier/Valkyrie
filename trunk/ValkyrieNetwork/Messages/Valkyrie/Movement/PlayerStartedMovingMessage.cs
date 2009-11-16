using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gablarski.Messages;
using System.Drawing;

namespace ValkyrieNetwork.Messages.Valkyrie.Movement
{
	public class PlayerStartedMovingMessage
		: ServerMessage
	{
		public PlayerStartedMovingMessage()
			: base(ServerMessageType.PlayerStartedMoving)
		{
		}

		public uint NetworkID { get; set; }
		public int Direction { get; set; }
		public string Animation { get; set; }

		public float Speed { get; set; }
		public float MoveDelay { get; set; }

		public override void WritePayload(Gablarski.IValueWriter writerm)
		{
			writerm.WriteUInt32(this.NetworkID);

			writerm.WriteInt32(this.Direction);
			writerm.WriteString(this.Animation);

			writerm.WriteBytes(BitConverter.GetBytes(this.Speed));
			writerm.WriteBytes(BitConverter.GetBytes(this.MoveDelay));
		}

		public override void ReadPayload(Gablarski.IValueReader reader)
		{
			this.NetworkID = reader.ReadUInt32();

			this.Direction = reader.ReadInt32();
			this.Animation = reader.ReadString();

			this.Speed = BitConverter.ToSingle(reader.ReadBytes(), 0);
			this.MoveDelay = BitConverter.ToSingle(reader.ReadBytes(), 0);
		}
	}
}
