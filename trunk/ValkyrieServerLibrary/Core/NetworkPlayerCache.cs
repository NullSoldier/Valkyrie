using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Library.Network;
using Valkyrie;

namespace ValkyrieServerLibrary.Core
{
	public class NetworkPlayerCache
	{
		private readonly Dictionary<uint, NetworkPlayer> players;

		public NetworkPlayerCache ()
		{
			this.players = new Dictionary<uint, NetworkPlayer> ();
		}

		public IEnumerable<NetworkPlayer> GetPlayers()
		{
			lock (this.players)
			{
				return players.Values.ToList();
			}
		}

		public NetworkPlayer this[uint NetworkID]
		{
			get { return this.GetPlayer(NetworkID); }
			set { this.SetPlayer(NetworkID, value); }
		}

		public NetworkPlayer this[IConnection connection]
		{
			get { return this.GetPlayer(connection); }
		}

		#region Cache Management

		public NetworkPlayer GetPlayer (uint NetworkID)
		{
			lock(this.players)
			{
				return ((this.players.ContainsKey(NetworkID)) ? this.players[NetworkID] : null);
			}
		}

		public NetworkPlayer GetPlayer (IConnection connection)
		{
			lock(this.players)
			{
				return this.players.Values.Where(p => p.Connection == connection).FirstOrDefault();
			}
		}

		public bool SetPlayer (uint NetworkID, NetworkPlayer player)
		{
			lock(this.players)
			{
				if(!this.players.ContainsKey(NetworkID))
					return false;

				this.players[NetworkID] = player;
			}

			return true;
		}

		public bool RemoveFromCache (uint NetworkID)
		{
			lock(this.players)
			{
				if(!this.players.ContainsKey(NetworkID))
					return false;

				this.players.Remove(NetworkID);
			}

			return true;
		}

		public bool RemovePlayer(IConnection connection)
		{
			lock (this.players)
			{
				uint removeNID = 0;

				foreach(KeyValuePair<uint, NetworkPlayer> pair in this.players)
				{
					if(pair.Value.Connection == connection)
					{
						removeNID = pair.Key;
						break;
					}
				
				}

				if(removeNID == 0)
					return false;

				this.players.Remove(removeNID);
				return true;
			}
		}

		public void AddPlayer(NetworkPlayer player)
		{
			lock (this.players)
			{
				this.players.Add(player.NetworkID, player);
			}
		}

		public bool ContainsPlayer (NetworkPlayer player)
		{
			return this.players.ContainsValue (player);
		}

		public bool ContainsPlayer (uint networkid)
		{
			return this.players.ContainsKey (networkid);
		}

		#endregion
	}
}
