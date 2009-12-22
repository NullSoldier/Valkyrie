using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Valkyrie.Messages;

namespace Valkyrie.Messages.Valkyrie.Movement
{
	public class PlayerStoppedMovingMessage
		: ServerMessage
	{
		public PlayerStoppedMovingMessage()
			: base (ServerMessageType.PlayerStoppedMoving)
		{
		}

		public uint NetworkID { get; set; }

		public int Direction { get; set; }
		public string Animation { get; set; }

		public Point Location
		{
			get { return new Point(this.X, this.Y); }
			set
			{
				this.X = value.X;
				this.Y = value.Y;
			}
		}

		public int X { get; set; }
		public int Y { get; set; }

		public override void WritePayload (IValueWriter writerm)
		{
			writerm.WriteUInt32(this.NetworkID);
			writerm.WriteInt32(this.X);
			writerm.WriteInt32(this.Y);

			writerm.WriteInt32 (this.Direction);
			writerm.WriteString(this.Animation);
		}

		public override void ReadPayload (IValueReader reader)
		{
			this.NetworkID = reader.ReadUInt32();
			this.X = reader.ReadInt32();
			this.Y = reader.ReadInt32();

			this.Direction = reader.ReadInt32();
			this.Animation = reader.ReadString();
		}
	}
}
