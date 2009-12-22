using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gablarski;
using Gablarski.Server;

namespace Valkyrie.Gablarski
{
	public class ValkyrieGablarskiPermissionsProvider
		: IPermissionsProvider
	{
		public event EventHandler<PermissionsChangedEventArgs> PermissionsChanged;

		public bool UpdatedSupported
		{
			get { return false; }
		}

		public IEnumerable<Permission> GetPermissions(int userId)
		{
			return GetNamesAsPermissions (PermissionName.Login, PermissionName.RequestSource, PermissionName.SendAudio,
			                              PermissionName.SendAudioToMultipleTargets);
		}

		public void SetPermissions (int userId, IEnumerable<Permission> permissions)
		{
			throw new NotSupportedException();
		}

		private static IEnumerable<Permission> GetNamesAsPermissions (params PermissionName[] names)
		{
			for (int i = 0; i < names.Length; ++i)
				yield return new Permission (names[i], true);
		}
	}
}