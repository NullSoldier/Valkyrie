﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Valkyrie.Messages;
using ValkyrieServerLibrary.Network.Messages.Valkyrie;

namespace Valkyrie.Library.Core.Messages
{
	public class LocationUpdateMessage
		: ClientMessage
	{
		public LocationUpdateMessage()
			: base (ClientMessageType.LocationData)
		{
		}

		public uint NetworkID { get; set; }

		public string Animation { get; set;	}

		public int Direction { get; set; }

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

		public override void WritePayload (IValueWriter writer)
		{
			writer.WriteUInt32 (this.NetworkID);

			writer.WriteString (this.Animation);
			writer.WriteInt32 (this.Direction);

			writer.WriteInt32 (this.X);
			writer.WriteInt32 (this.Y);
		}

		public override void ReadPayload (IValueReader reader)
		{
			this.NetworkID = reader.ReadUInt32();

			this.Animation = reader.ReadString();
			this.Direction = reader.ReadInt32();

			this.X = reader.ReadInt32();
			this.Y = reader.ReadInt32();
		}
	}
}