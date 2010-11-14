using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Library.Events;
using Valkyrie.Engine.Maps;
using System.Reflection;

namespace Valkyrie.Engine.Providers
{
	public interface IMapProvider
	{
		Map GetMap (string filepath, IEventProvider eventprovider);
	}
}
