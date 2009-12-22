using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie;
using Valkyrie.Library.Core.Messages;
using System.Drawing;
using Valkyrie.Messages;

namespace ValkyrieServerLibrary.Network.Messages.Valkyrie
{
	public class PlayerInfoMessage
		: ServerMessage
	{
		public PlayerInfoMessage()
			: base(ServerMessageType.PlayerInfo)
		{

		}

		public string Name
		{
			get;
			set;
		}

		public bool Moving
		{
			get;
			set;
		}

		public uint NetworkID
		{
			get;
			set;
		}

		public string Animation
		{
			get;
			set;
		}

		public string TileSheet
		{
			get;
			set;
		}

		public Point Location
		{
			get { return new Point(this.X, this.Y); }
			set
			{
				this.X = value.X;
				this.Y = value.Y;
			}
		}

		public string WorldName
		{
			get;
			set;
		}

		public int X
		{
			get;
			set;
		}

		public int Y
		{
			get;
			set;
		}

		public override void WritePayload(IValueWriter writer)
		{
			writer.WriteUInt32(this.NetworkID);

			writer.WriteString(this.Name);
			writer.WriteInt32(Convert.ToInt32(this.Moving));

			writer.WriteString(this.Animation);
			writer.WriteInt32(this.X);
			writer.WriteInt32(this.Y);

			writer.WriteString(this.WorldName);

			writer.WriteString(this.TileSheet);
		}

		public override void ReadPayload(IValueReader reader)
		{
			this.NetworkID = reader.ReadUInt32();

			this.Name = reader.ReadString();
			this.Moving = Convert.ToBoolean(reader.ReadInt32());

			this.Animation = reader.ReadString();
			this.X = reader.ReadInt32();
			this.Y = reader.ReadInt32();

			this.WorldName = reader.ReadString();

			this.TileSheet = reader.ReadString();
		}
	}
}
