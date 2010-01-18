using Valkyrie;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Providers;
using Valkyrie.Library.Network;
using Valkyrie.Messages.Movement;
using ValkyrieServerLibrary.Entities;

namespace ValkyrieServerLibrary.Core
{
	public partial class ValkyrieGameServer
	{
		private void ClientMovementMessageReceived (MessageReceivedEventArgs ev)
		{
			ClientMovementMessage message = (ClientMovementMessage) ev.Message;
			NetworkPlayer player = this.players.GetPlayer (message.NetworkID);

			if(player == null)
				return; // Must have gotten disconnected

			Directions direction = (Directions) message.Direction;

			player.Character.MovementQueue.Enqueue (new MovementInfo (new ScreenPoint (message.X, message.Y), direction, MovementStage.EndMovement, MovementType.Destination, message.Animation));

			if(!this.movement.ContainsCharacter (player.Character))
				this.movement.AddToProvider (player.Character);
		}

		private void MovementProvider_FailedVerify (object sender, MovementChangedEventArgs ev)
		{
			var player = this.players.GetPlayer (ev.Character.NetworkID);

			this.Disconnect (player.Connection);
		}
	}
}
