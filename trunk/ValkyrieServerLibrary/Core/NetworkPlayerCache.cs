using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Library.Network;
using Valkyrie;
using Microsoft.Xna.Framework;
using Cadenza;
using Cadenza.Collections;

namespace ValkyrieServerLibrary.Core
{
	public class NetworkPlayerCache
	{
		public ReadOnlyDictionary<uint, List<NetworkPlayer>> PlayerRanges
		{
			get { return new ReadOnlyDictionary<uint, List<NetworkPlayer>> (this.ranges); }
		}

		public object PlayerLock { get; set; }

		public NetworkPlayerCache ()
		{
			this.PlayerLock = new object ();
			this.players = new Dictionary<uint, NetworkPlayer> ();
			this.ranges = new Dictionary<uint, List<NetworkPlayer>>();
		}

		public IEnumerable<NetworkPlayer> GetPlayers()
		{
			lock (this.PlayerLock)
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
			lock(this.PlayerLock)
			{
				return ((this.players.ContainsKey(NetworkID)) ? this.players[NetworkID] : null);
			}
		}

		public NetworkPlayer GetPlayer (IConnection connection)
		{
			lock(this.PlayerLock)
			{
				return this.players.Values.Where(p => p.Connection == connection).FirstOrDefault();
			}
		}

		public bool SetPlayer (uint NetworkID, NetworkPlayer player)
		{
			lock(this.PlayerLock)
			{
				if(!this.players.ContainsKey(NetworkID))
					return false;

				this.players[NetworkID] = player;
			}

			return true;
		}

		public bool RemoveFromCache (uint NetworkID)
		{
			lock(this.PlayerLock)
			{
				if(!this.players.ContainsKey(NetworkID))
					return false;

				this.players.Remove (NetworkID);
				this.ranges.Remove (NetworkID);
			}

			return true;
		}

		public bool RemovePlayer(IConnection connection)
		{
			lock(this.PlayerLock)
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

				this.players.Remove (removeNID);
				this.ranges.Remove (removeNID);
				return true;
			}
		}

		public void AddPlayer(NetworkPlayer player)
		{
			lock(this.PlayerLock)
			{
				this.players.Add(player.NetworkID, player);
				this.ranges.Add (player.NetworkID, new List<NetworkPlayer> ());
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

		public Dictionary<NetworkPlayer, List<RangeChangeInfo>> UpdateRanges ()
		{
			Dictionary<NetworkPlayer, List<RangeChangeInfo>> changes = new Dictionary<NetworkPlayer, List<RangeChangeInfo>> ();

			lock(this.PlayerLock)
			{
				foreach(var player in this.players.Values)
				{
					// Rebuild the players collection
					Vector2 location = new Vector2 (player.Character.GlobalTileLocation.X, player.Character.GlobalTileLocation.Y);
					changes.Add (player, new List<RangeChangeInfo> ());

					foreach(var other in this.players.Values)
					{
						if(other.NetworkID == player.NetworkID)
							continue;

						Vector2 locationother = new Vector2 (other.Character.GlobalTileLocation.X, other.Character.GlobalTileLocation.Y);

						// Out of range
						if(Vector2.Distance (location, locationother) >= 20 || player.Character.WorldName != other.Character.WorldName)
						{
							if(this.ranges[player.NetworkID].Contains (other))
							{
								changes[player].Add (new RangeChangeInfo (other, RangeChangeStates.Remove));
								this.ranges[player.NetworkID].Remove (other);
							}
						}
						// In range
						else if(Vector2.Distance (location, locationother) <= 15 && !this.ranges[player.NetworkID].Contains (other) &&
							player.Character.WorldName == other.Character.WorldName)
						{
							changes[player].Add (new RangeChangeInfo (other, RangeChangeStates.Add));
							this.ranges[player.NetworkID].Add (other);
						}
					}
				}
			}

			return changes;
		}

		private readonly Dictionary<uint, NetworkPlayer> players;
		private readonly Dictionary<uint, List<NetworkPlayer>> ranges;
	}

	public class RangeChangeInfo
	{
		public RangeChangeInfo (NetworkPlayer player, RangeChangeStates state)
		{
			this.Player = player;
			this.State = state;
		}

		public NetworkPlayer Player { get; private set; }
		public RangeChangeStates State { get; private set; }
	}

	public enum RangeChangeStates
	{
		Add,
		Remove
	};
}
