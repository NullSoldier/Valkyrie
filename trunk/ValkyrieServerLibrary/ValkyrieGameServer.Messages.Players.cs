using System.Drawing;
using System.Linq;
using Valkyrie;
using Valkyrie.Library.Network;
using Valkyrie.Messages.Valkyrie;
using ValkyrieServerLibrary.Network.Messages.Valkyrie;

namespace ValkyrieServerLibrary.Core
{
	public partial class ValkyrieGameServer
	{
		private void PlayerRequestReceived (MessageReceivedEventArgs ev)
		{
			var msg = (PlayerRequestMessage) ev.Message;

			NetworkPlayer player = this.players.GetPlayers ().Where (p => p.NetworkID == msg.RequestedPlayerNetworkID).FirstOrDefault ();

			if(player == null)
				return;

			PlayerInfoMessage infomsg = new PlayerInfoMessage ()
			{
				NetworkID = player.NetworkID,
				Name = player.Character.Name,
				Animation = player.Character.Animation,
				Moving = player.Character.Moving,
				TileSheet = player.Character.TileSheet,
				Location = new Point (player.Character.Location.X, player.Character.Location.Y),
				WorldName = player.Character.WorldName
			};

			ev.Connection.Send (infomsg);
		}

		private void PlayersRequestReceived (MessageReceivedEventArgs ev)
		{
			// Get the player, figure out which players are around, and send send those players
			var playerlist = this.players.GetPlayers ();

			foreach(NetworkPlayer player in playerlist)
			{
				if(player.Connection == ev.Connection)
					continue;

				PlayerInfoMessage infomsg = new PlayerInfoMessage ()
				{
					NetworkID = player.NetworkID,
					Name = player.Character.Name,
					Animation = player.Character.Animation,
					Moving = player.Character.Moving,
					TileSheet = player.Character.TileSheet,
					Location = new Point (player.Character.Location.X, player.Character.Location.Y),
					WorldName = player.Character.WorldName
				};

				ev.Connection.Send (infomsg);
			}

		}

		private void PlayerLoadedReceived (MessageReceivedEventArgs ev)
		{
			PlayerLoadedMessage message = (PlayerLoadedMessage) ev.Message;

			NetworkPlayer player = this.players.GetPlayers ().Where (p => p.NetworkID == message.NetworkID).FirstOrDefault ();

			if(player == null)
				return;

			PlayerInfoMessage infomsg = new PlayerInfoMessage ()
			{
				NetworkID = player.NetworkID,
				Name = player.Character.Name,
				Animation = player.Character.Animation,
				Moving = player.Character.Moving,
				TileSheet = player.Character.TileSheet,
				Location = new Point (player.Character.Location.X, player.Character.Location.Y),
				WorldName = player.Character.WorldName
			};

			foreach(NetworkPlayer destPlayer in this.players.GetPlayers ())
			{
				if(destPlayer.Connection != player.Connection)
					destPlayer.Connection.Send (infomsg);
			}
		}
	}
}
