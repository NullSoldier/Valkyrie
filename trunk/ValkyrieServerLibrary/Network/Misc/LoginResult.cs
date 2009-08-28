using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gablarski
{
	public enum LoginResultState
		: byte
	{
		/// <summary>
		/// Failed for an unknown reason.
		/// </summary>
		FailedUnknown = 0,

		/// <summary>
		/// Succeeded.
		/// </summary>
		Success = 1,

		/// <summary>
		/// Failed because the username does not exist, is invalid.
		/// </summary>
		FailedUsername = 2,

		/// <summary>
		/// Failed because the password does not match the username.
		/// </summary>
		FailedPassword = 3,

		/// <summary>
		/// Failed because the username and password combination was not found.
		/// </summary>
		/// <remarks>For providers that do not wish to reveal if a username exists.</remarks>
		FailedUsernameAndPassword = 4,

		/// <summary>
		/// Failed because the nickname is already in use.
		/// </summary>
		FailedNicknameInUse = 5,

		FailedPermissions = 6,

		/// <summary>
		/// Failed because the supplied nickname is invalid.
		/// </summary>
		FailedInvalidNickname = 7,
	}

	/// <summary>
	/// Provides results for an attempted login.
	/// </summary>
	public class LoginResult
	{
		internal LoginResult (IValueReader reader)
		{
			this.Deserialize (reader);
		}

		public LoginResult (int userId, LoginResultState state)
		{
			this.UserId = userId;
			this.ResultState = state;
		}

		/// <summary>
		/// Gets the logged-in user's ID.
		/// </summary>
		public int UserId
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets whether the login succeeded or not.
		/// </summary>
		public bool Succeeded
		{
			get { return (this.ResultState == LoginResultState.Success); }
		}

		/// <summary>
		/// Gets the reason for a login failure, <c>null</c> otherwise.
		/// </summary>
		public LoginResultState ResultState
		{
			get;
			internal set;
		}

		internal void Serialize (IValueWriter writer)
		{
			writer.WriteInt32 (this.UserId);
			writer.WriteByte ((byte)this.ResultState);
		}

		internal void Deserialize (IValueReader reader)
		{
			this.UserId = reader.ReadInt32();
			this.ResultState = (LoginResultState)reader.ReadByte ();
		}
	}
}