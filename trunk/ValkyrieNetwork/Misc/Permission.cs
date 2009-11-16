using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gablarski
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
	}

	public class Permission
	{
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
	}

	public static class PermissionExtensions
	{
		public static bool GetPermission (this IEnumerable<Permission> self, PermissionName name)
		{
			var perm = self.Where (p => p.Name == name).FirstOrDefault ();
			return (perm != null && perm.IsAllowed);
		}
	}
}