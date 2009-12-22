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
	public enum PermissionName
	{
		/// <summary>
		/// Login to the server.
		/// </summary>
		Login = 1,

		/// <summary>
		/// Kick a player from the channel (to the default channel.)
		/// </summary>
		KickPlayerFromChannel = 2,

		/// <summary>
		/// Kick a player from the server entirely.
		/// </summary>
		KickPlayerFromServer = 9,

		#region Channels
		/// <summary>
		/// Move yourself from channel to channel
		/// </summary>
		ChangeChannel = 7,
		
		/// <summary>
		/// Move a different player from channel to channel
		/// </summary>
		ChangePlayersChannel = 8,

		/// <summary>
		/// Create a new channel.
		/// </summary>
		AddChannel = 11,

		/// <summary>
		/// Edit an existing channel.
		/// </summary>
		EditChannel = 12,

		/// <summary>
		/// Delete a channel.
		/// </summary>
		DeleteChannel = 13,
		#endregion

		/// <summary>
		/// Request a media source.
		/// </summary>
		RequestSource = 3,

		/// <summary>
		/// Broadcast audio to the current channel.
		/// </summary>
		SendAudioToCurrentChannel = 4,

		/// <summary>
		/// Broadcast audio to the entire server.
		/// </summary>
		SendAudioToAll = 5,

		/// <summary>
		/// Broadcast audio to a different channel then the player is in.
		/// </summary>
		SendAudioToDifferentChannel = 6,

		/// <summary>
		/// Request a channel list.
		/// </summary>
		RequestChannelList = 10,

		MuteAudioSource = 14,

		MuteUser = 15,

		ModifyPermissions = 16,

		AdminPanel = 17,

		// Next: 18
	}

	public class Permission
	{
		internal Permission (IValueReader reader)
		{
			Deserialize (reader);
		}

		public Permission (PermissionName name)
		{
			this.Name = name;
		}

		public Permission (PermissionName name, bool isAllowed)
			: this (name)
		{
			this.IsAllowed = isAllowed;
		}

		public virtual PermissionName Name
		{
			get;
			private set;
		}

		public virtual bool IsAllowed
		{
			get;
			set;
		}

		public static IEnumerable<PermissionName> GetAllNames ()
		{
			return Enum.GetValues (typeof (PermissionName)).Cast<PermissionName> ();
		}

		internal void Serialize (IValueWriter writer)
		{
			writer.WriteInt32 ((int)this.Name);
			writer.WriteBool (this.IsAllowed);
		}

		internal void Deserialize (IValueReader reader)
		{
			this.Name = (PermissionName)reader.ReadInt32();
			this.IsAllowed = reader.ReadBool();
		}
	}

	public static class PermissionExtensions
	{
		public static bool CheckPermission (this IEnumerable<Permission> self, PermissionName name)
		{
			return self.Any (p => p.Name == name && p.IsAllowed);
		}
	}
}