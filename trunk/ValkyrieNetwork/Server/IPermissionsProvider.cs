using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gablarski.Server
{
	public interface IPermissionsProvider
	{
		IEnumerable<Permission> GetPermissions (int playerId);
	}
}