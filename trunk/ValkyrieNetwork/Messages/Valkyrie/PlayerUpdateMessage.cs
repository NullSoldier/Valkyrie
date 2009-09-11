using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gablarski.Messages;

namespace ValkyrieLibrary.Network
{
	public enum PlayerUpdateAction
	{
		Add,
		Remove
	}

	public class PlayerUpdateMessage
		: ServerMessage
	{
		public PlayerUpdateMessage()
			: base(ServerMessageType.PlayerUpdate)
		{
		}

		public PlayerUpdateAction Action
		{
			get;
			set;
		}

		public string CharacterName
		{
			get;
			set;
		}

		public UInt32 NetworkID
		{
			get;
			set;
		}

		public override void WritePayload(Gablarski.IValueWriter writerm)
		{
			writerm.WriteUInt32(this.NetworkID);
			writerm.WriteString(this.CharacterName);

			writerm.WriteByte((byte)this.Action);
		}

		public override void ReadPayload(Gablarski.IValueReader reader)
		{
			this.NetworkID = reader.ReadUInt32();
			this.CharacterName = reader.ReadString();

			this.Action = (PlayerUpdateAction)reader.ReadByte();
		}
	}
}
