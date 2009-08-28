using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gablarski
{
	public class UserInfo
	{
		public UserInfo()
		{
		}

		internal UserInfo (UserInfo info)
		{
			if (info == null)
				throw new ArgumentNullException ("info");

			this.Nickname = info.Nickname;
			this.Username = info.Username;
			this.UserId = info.UserId;
		}

		internal UserInfo (string nickname, string username, int userId)
		{
			if (nickname.IsEmpty())
				throw new ArgumentNullException ("nickname");
			if (userId < 0)
				throw new ArgumentOutOfRangeException ("userId");

			this.Nickname = nickname;
			this.Username = (username.IsEmpty()) ? nickname : username;
			this.UserId = userId;
		}

		internal UserInfo (IValueReader reader)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");

			this.Deserialize (reader);
		}

		public virtual string Username
		{
			get;
			protected set;
		}

		public virtual string Nickname
		{
			get;
			protected set;
		}

		public virtual int UserId
		{
			get;
			protected set;
		}

		internal void Serialize (IValueWriter writer)
		{
			writer.WriteInt32 (this.UserId);
			writer.WriteString (this.Username);
			writer.WriteString (this.Nickname);
		}

		internal void Deserialize (IValueReader reader)
		{
			this.UserId = reader.ReadInt32();
			this.Username = reader.ReadString();
			this.Nickname = reader.ReadString();			
		}

		public override bool Equals (object obj)
		{
			var info = (obj as UserInfo);

			return (info != null) ? this.UserId.Equals (info.UserId) : this.UserId.Equals (obj);
		}

		public override int GetHashCode ()
		{
			return this.UserId.GetHashCode();
		}
	}
}