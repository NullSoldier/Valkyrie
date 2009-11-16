using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gablarski.Messages;

namespace ValkyrieServerLibrary.Network.Messages.Valkyrie
{
	/// <summary>
	/// Request a single player's information using their network ID
	/// </summary>
	public class PlayerRequestMessage
		: ClientMessage
	{
		public PlayerRequestMessage()
			: base(ClientMessageType.PlayerInfoRequest)
		{
		}

		/// <summary>
		/// The network ID of the requested player's information
		/// </summary>
		public uint RequestedPlayerNetworkID
		{
			get;
			set;
		}

		public override void WritePayload(Gablarski.IValueWriter writerm)
		{
			writerm.WriteUInt32(this.RequestedPlayerNetworkID);
		}

		public override void ReadPayload(Gablarski.IValueReader reader)
		{
			this.RequestedPlayerNetworkID = reader.ReadUInt32();
		}
	}
}
