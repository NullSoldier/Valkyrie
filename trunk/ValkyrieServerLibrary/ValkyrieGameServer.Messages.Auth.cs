using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Valkyrie.Messages;
using Valkyrie.Messages.Valkyrie;
using Valkyrie.Library.Core.Messages;
using System.Collections;
using System.Diagnostics;
using Valkyrie;
using ValkyrieServerLibrary.Entities;
using NHibernate.Criterion;
using Valkyrie.Library.Network;
using System.Drawing;
using ValkyrieServerLibrary.Network.Messages.Valkyrie;
using Valkyrie.Library.Core;
using Valkyrie.Messages.Movement;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Characters;
using Valkyrie.Messages.Valkyrie.Authentication;
using NHibernate;
using Valkyrie.Engine.Providers;

namespace ValkyrieServerLibrary.Core
{
	public partial class ValkyrieGameServer
	{
		private void ClientConnected (MessageReceivedEventArgs ev)
		{
			var msg = (ConnectMessage)ev.Message;

			if (msg.Version < this.MinimumVersion)
			{
				ev.Connection.Send (new ConnectionRejectedMessage () { Reason = ConnectionRejectedReason.IncompatableVersion });

				this.Disconnect(ev.Connection);
			}
		}

		private uint networkid = 1;

		private void LoginRequestReceived (MessageReceivedEventArgs ev)
		{
			var msg = (LoginMessage)ev.Message;

			ISession session = this.sessionfactory.OpenSession ();

			Account account = session.CreateCriteria<Account>()
				.Add(Restrictions.InsensitiveLike("Username", msg.Username ))
				.Add(Restrictions.Eq("Password", msg.Password ))
				.List<Account>().FirstOrDefault();
			
			if (account == null || this.players.ContainsPlayer((uint)account.ID))
			{
				ev.Connection.Send (new LoginFailedMessage (ConnectionRejectedReason.BadLogin));
				return;
			}
			
			Character character = session.CreateCriteria<Character> ()
				.Add(Restrictions.Eq("AccountID", account.ID))
				.List<Character>().FirstOrDefault();

			character.Location = new ScreenPoint(character.MapX * 32, character.MapY * 32);
			character.Animation = Enum.GetName (typeof(Directions), character.Direction);
			character.NextMoveActive = true;

			// Create player
			NetworkPlayer player = new NetworkPlayer();
			player.Character = character;
			player.AccountID = account.ID;
			//player.NetworkID = ++networkid;
			player.NetworkID = (uint) account.ID;
			player.Character.NetworkID = player.NetworkID;
			player.Connection = ev.Connection;
			player.Character.MapChunkName = this.GetChunkName(player.Character.WorldName, player.Character.MapLocation);
			player.State = PlayerState.LoggedIn;

			// Add to the list of players in the servers memory
			this.players.AddPlayer(player);

			// Send them authentication ID
			player.Connection.Send(new LoginSuccessMessage () { NetworkIDAssigned = player.NetworkID });

			var handler = this.UserLoggedIn;
			if(handler != null)
				handler(this, new UserEventArgs(player));

			// Update other players
			var updatemsg = new PlayerUpdateMessage ()
			{
				CharacterName = player.Character.Name,
				NetworkID = player.NetworkID,
				Action = PlayerUpdateAction.Add
			};

			foreach (var tmp in this.players.GetPlayers())
				tmp.Connection.Send(updatemsg);

			session.Close ();
		}

		private void LogoutRequestReceived (MessageReceivedEventArgs ev)
		{
			var message = (LogoutMessage)ev.Message;

			this.Disconnect(message.NetworkID);
		}
	}
}
