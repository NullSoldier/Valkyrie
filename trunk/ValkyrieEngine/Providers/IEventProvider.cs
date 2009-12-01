using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Maps;
using Valkyrie.Engine.Events;
using Valkyrie.Engine.Characters;
using System.Reflection;

namespace Valkyrie.Engine.Providers
{
	public interface IEventProvider
		: IEngineProvider
	{
		void Add (Map map, IMapEvent mapevent);
		void Add (string mapname, IMapEvent mapevent);
		void ReferenceSetOrAdd (Map map, IMapEvent mapevent);
		void ReferenceSetOrAdd (string mapname, IMapEvent mapevent);
		bool Remove (Map map, IMapEvent mapevent);
		bool Remove (string mapname, IMapEvent mapevent);
		bool HandleEvent (BaseCharacter character, ActivationTypes action);
		IEnumerable<IMapEvent> GetMapsEvents (Map map);
		IEnumerable<IMapEvent> GetMapsEvents (string mapname);
		int GetMapsEventCount (Map map);
		int GetMapsEventCount (string mapname);
		void ClearEvents ();
	}
}
