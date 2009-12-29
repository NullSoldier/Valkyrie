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

		/// <summary>
		/// The <seealso cref="IMapProvider"/> used for loading maps
		/// </summary>
		IMapProvider MapProvider { get; set; }

		/// <summary>
		/// The <seealso cref="IWorldProvider"/> used for loading worlds
		/// </summary>
		IWorldProvider WorldProvider { get; set; }

		/// <summary>
		/// Adds a <paramref name="world"/> to the IWorldManager
		/// </summary>
		/// <param name="world">The world to add</param>
		/// <exception cref="WorldAlreadyExistsException">Thrown when the world you are trying to add already exists</exception>
		void AddWorld (World world);

		/// <summary>
		/// Removes a world from the IWorldManager
		/// </summary>
		/// <param name="name">The name of the world to remove.</param>
		/// <returns>True if the world was removed.</returns>
		bool RemoveWorld (string name);

		/// <summary>
		/// Clears all worlds from the IWorldManager's storage
		/// </summary>
		void ClearWorlds ();

		/// <summary>
		/// Gets a specific world
		/// </summary>
		/// <param name="name">The name of the world to get.</param>
		/// <returns>The world with the specified <paramref name="name"/>. Returns null if it does not exist.</returns>
		World GetWorld (string name);

		/// <summary>
		/// Gets a non-modifiable collection of currently stored worlds
		/// </summary>
		/// <returns>All worlds stored in the IWorldManager</returns>
		ReadOnlyDictionary<string, World> GetWorlds ();
	}

	public class WorldAlreadyExistsException
		: Exception
	{
		public WorldAlreadyExistsException (string worldname)
			: base (string.Format ("World {0} already exists!", worldname))
		{
		}
	}
}
