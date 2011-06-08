// Copyright (c) 2009, Eric Maupin
// All rights reserved.
//
// Redistribution and use in source and binary forms, with
// or without modification, are permitted provided that
// the following conditions are met:
//
// - Redistributions of source code must retain the above 
//   copyright notice, this list of conditions and the
//   following disclaimer.
//
// - Redistributions in binary form must reproduce the above
//   copyright notice, this list of conditions and the
//   following disclaimer in the documentation and/or other
//   materials provided with the distribution.
//
// - Neither the name of Gablarski nor the names of its
//   contributors may be used to endorse or promote products
//   or services derived from this software without specific
//   prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS
// AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
// WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
// INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH
// DAMAGE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Valkyrie
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
		/// Failed because the username does not exist or is invalid.
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
		
		/// <summary>
		/// Failed because the supplied server password was incorrect.
		/// </summary>
		FailedServerPassword = 8,

		/// <summary>
		/// Failed because the connection is already joined.
		/// </summary>
		FailedAlreadyJoined = 9
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