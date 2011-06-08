using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Maps;
using Valkyrie.Engine.Events;
using Valkyrie.Engine.Characters;
using System.Reflection;
using Valkyrie.Engine.Core;

namespace Valkyrie.Engine.Providers
{
	public interface IEventProvider
		: IEngineProvider
	{
		/// <summary>
		/// Adds a <paramref name="mapevent"/> to the IEventProvider.
		/// </summary>
		/// <param name="map">The map the event is located on.</param>
		/// <param name="mapevent">The event to add to the <seealso cref="IEventProvider"/>.</param>
		/// <exception cref="ArgumentException">Thrown when the event you are trying to add is already added.</exception>
		void AddEvent (Map map, IMapEvent mapevent);

		/// <summary>
		/// Adds a <paramref name="mapevent"/> to the IEventProvider.
		/// </summary>
		/// <param name="map">The name of the map the event is located on.</param>
		/// <param name="mapevent">The event to add to the <seealso cref="IEventProvider"/>.</param>
		/// <exception cref="ArgumentException">Thrown when the event you are trying to add is already added.</exception>
		void AddEvent (string mapname, IMapEvent mapevent);

		/// <summary>
		/// Replaces the reference for a current IMapEvent if it's already in the IEventProvider and adds it otherwise.
		/// </summary>
		/// <param name="map">The map the event is located on.</param>
		/// <param name="mapevent">The event to replace or add.</param>
		void ReferenceSetOrAdd (Map map, IMapEvent mapevent);

		/// <summary>
		/// Replaces the reference for a current IMapEvent if it's already in the IEventProvider and adds it otherwise.
		/// </summary>
		/// <param name="map">The name of the map the event is located on.</param>
		/// <param name="mapevent">The event to replace or add.</param>
		void ReferenceSetOrAdd (string mapname, IMapEvent mapevent);

		/// <summary>
		/// Removes a <paramref name="mapevent"/> from the <seealso cref="IEventProvider"/>.
		/// </summary>
		/// <param name="map">The map the event is located on.</param>
		/// <param name="mapevent">The map event to remove.</param>
		/// <returns>True if it removed the <paramref name="mapevent"/> from the <seealso cref="IEventProvider"/>.</returns>
		bool RemoveEvent (Map map, IMapEvent mapevent);

		/// <summary>
		/// Removes a <paramref name="mapevent"/> from the <seealso cref="IEventProvider"/>.
		/// </summary>
		/// <param name="map">The name of the map the event is located on.</param>
		/// <param name="mapevent">The map event to remove.</param>
		/// <returns>True if it removed the <paramref name="mapevent"/> from the <seealso cref="IEventProvider"/>.</returns>
		bool RemoveEvent (string mapname, IMapEvent mapevent);

		/// <summary>
		/// Handle an event that <paramref name="character"/> has fired.
		/// </summary>
		/// <param name="character">The <seealso cref="BaseCharacter"/> that fired the event.</param>
		/// <param name="action">The type of event that was fired.</param>
		/// <returns>True if the event was handled.</returns>
		bool HandleEvent (BaseCharacter character, ActivationTypes action);

		/// <summary>
		/// Gets all of the events at a specified <paramref name="mappoint"/>
		/// </summary>
		/// <param name="map">The mapheader that stores the map that the mappoint is located on.</param>
		/// <param name="mappoint">The map coordinent to get events at.</param>
		/// <param name="worldname">The name of the world that the <paramref name="map"/> is located in.</param>
		/// <returns>A collection of MapEvents from the <paramref name="mappoint"/>.</returns>
		IEnumerable<IMapEvent> GetMapsEvents (string worldname, MapHeader mapheader, MapPoint mappoint);

		/// <summary>
		/// Gets all of the events in a specified <paramref name="map"/>
		/// </summary>
		/// <param name="map">The map to get events from.</param>
		/// <returns>A collection of MapEvents from the <paramref name="map"/>.</returns>
		IEnumerable<IMapEvent> GetMapsEvents (Map map);

		/// <summary>
		/// Gets all of the events in a specified map
		/// </summary>
		/// <param name="map">The name of the map to get events from.</param>
		/// <returns>A collection of MapEvents from the specified <paramref name="mapname"/>.</returns>
		IEnumerable<IMapEvent> GetMapsEvents (string mapname);

		/// <summary>
		/// Returns a count of how many events are on a <paramref name="map"/>.
		/// </summary>
		/// <param name="map">The map to count events on.</param>
		/// <returns>A count of how many events are on the specified <paramref name="map"/>.</returns>
		int GetMapsEventCount (Map map);

		/// <summary>
		/// Returns a count of how many events are on a map.
		/// </summary>
		/// <param name="map">The name of a map to count events on.</param>
		/// <returns>A count of how many events are on the specified map.</returns>
		int GetMapsEventCount (string mapname);

		/// <summary>
		/// Clears all events from the <seealso cref="IEventProvider"/>
		/// </summary>
		void ClearEvents ();
	}
}
