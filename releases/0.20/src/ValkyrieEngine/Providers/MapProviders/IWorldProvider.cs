using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Maps;

namespace Valkyrie.Engine.Providers
{
	public interface IWorldProvider
	{
		World GetWorld(Uri location, string name);

		IEnumerable<World> GetWorlds(Uri location);
	}
}
