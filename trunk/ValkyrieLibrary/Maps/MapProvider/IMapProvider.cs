using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Library.Events;

namespace Valkyrie.Library.Maps.MapProvider
{
	public interface IMapProvider
	{
		/// <summary>
		/// Gets a single map with the specified map name
		/// </summary>
		/// <param name="providerdata">Provider specific data</param>
		/// <returns>The map you have requested</returns>
		Map GetMap (object providerdata, MapEventManager mapmanager);
	}
}
