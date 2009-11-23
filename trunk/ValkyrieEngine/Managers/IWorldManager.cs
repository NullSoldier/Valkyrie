using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine.Maps;
using Mono.Rocks;

namespace Valkyrie.Engine.Managers
{
	public interface IWorldManager
		: IEngineProvider
	{
		void Load(Uri location, IEventProvider eventprovider);

		IMapProvider MapProvider { get; set; }
		IWorldProvider WorldProvider { get; set; }

		void AddWorld (World world);
		bool RemoveWorld (string name);
		void ClearWorlds ();

		World GetWorld (string name);
		ReadOnlyDictionary<string, World> GetWorlds ();
	}
}
