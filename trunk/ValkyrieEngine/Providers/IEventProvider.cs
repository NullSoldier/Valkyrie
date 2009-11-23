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
		bool Remove (Map map, IMapEvent mapevent);
		bool HandleEvent (BaseCharacter character, ActivationTypes action);
		IEnumerable<IMapEvent> GetMapsEvents (Map map);
		IEnumerable<IMapEvent> GetMapsEvents (string mapname);
		void ClearEvents ();
	}
}
