using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gablarski.Messages;

namespace Gablarski.Server
{
	internal class ConnectionCollection
	{
		public int ConnectionCount
		{
			get { return this.connections.Count; }
		}

		public UserInfo this[IConnection key]
		{
			get
			{
				lock (lck)
				{
					if (this.users.ContainsKey (key))
						return this.users[key];
				}

				return null;
			}
		}

		public IConnection this[UserInfo key]
		{
			get
			{
				lock (lck)
				{
					return (from kvp in this.users where kvp.Value == key select kvp.Key).FirstOrDefault();
				}
			}
		}

		public KeyValuePair<IConnection, UserInfo> this[int index]
		{
			get
			{
				lock (lck)
				{
					IConnection c = this.connections[index];
					UserInfo p;
					this.users.TryGetValue (c, out p);

					return new KeyValuePair<IConnection, UserInfo> (c, p);
				}
			}
		}

		public IEnumerable<UserInfo> Users
		{
			get
			{
				lock (lck)
				{
					UserInfo[] copiedPlayers = new UserInfo[this.users.Count];
					this.users.Values.CopyTo (copiedPlayers, 0);

					return copiedPlayers;
				}
			}
		}

		public bool UserLoggedIn (string nickname)
		{
			lock (lck)
			{
				return this.users.Values.Any (p => p.Nickname == nickname);
			}
		}

		public void Add (IConnection connection)
		{
			lock (lck)
			{
				connections.Add (connection);
			}
		}


		public void Add (IConnection connection, UserInfo user)
		{
			lock (lck)
			{
				if (!this.connections.Contains (connection))
					this.connections.Add (connection);

				this.users[connection] = user;
			}
		}

		public IConnection GetConnection (object userId)
		{
			lock (lck)
			{
				return (from kvp in this.users where kvp.Value.UserId.Equals (userId) select kvp.Key).FirstOrDefault();
			}
		}

		public bool UpdateIfExists (IConnection connection, UserInfo user)
		{
			lock (lck)
			{
				if (!this.connections.Contains (connection))
					return false;

				this.users[connection] = user;
				return true;
			}
		}

		public bool UpdateIfExists (UserInfo user)
		{
			lock (lck)
			{
				var old = this.users.FirstOrDefault (kvp => kvp.Value.UserId.Equals (user.UserId));
				if (old.Equals (default(KeyValuePair<IConnection, UserInfo>)))
					return false;

				this.users[old.Key] = user;
				return true;
			}
		}

		public bool Remove (IConnection connection)
		{
			lock (lck)
			{
				this.users.Remove (connection);
				return this.connections.Remove (connection);
			}
		}

		public bool Remove (IConnection connection, out int userId)
		{
			userId = 0;

			lock (lck)
			{
				this.connections.Remove (connection);
				if (!this.users.ContainsKey (connection))
					return false;

				var info = this.users[connection];
				userId = info.UserId;
				this.users.Remove (connection);

				return true;
			}
		}

		public void Send (MessageBase message)
		{
			lock (lck)
			{
				foreach (IConnection c in this.connections)
					c.Send (message);
			}
		}

		public void Send (MessageBase message, Func<IConnection, bool> selector)
		{
			lock (lck)
			{
				foreach (IConnection c in this.connections.Where (selector))
					c.Send (message);
			}
		}

		public void Send (MessageBase message, Func<UserInfo, bool> selector)
		{
			lock (lck)
			{
				foreach (var kvp in this.users)
				{
					if (!selector (kvp.Value))
						continue;

					kvp.Key.Send (message);
				}
			}
		}

		public void Send (MessageBase message, Func<IConnection, UserInfo, bool> selector)
		{
			lock (lck)
			{
				foreach (var kvp in this.users)
				{
					if (!selector (kvp.Key, kvp.Value))
						continue;

					kvp.Key.Send (message);
				}
			}
		}

		private object lck = new object();
		private readonly List<IConnection> connections = new List<IConnection>();
		private readonly Dictionary<IConnection, UserInfo> users = new Dictionary<IConnection, UserInfo>();
	}
}