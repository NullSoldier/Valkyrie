using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine;
using Valkyrie.Engine.Maps;
using Valkyrie.Engine.Characters;
using System.Reflection;
using Valkyrie.Engine.Events;

namespace Valkyrie.Library.Providers
{
	public class ValkyrieEventProvider
		: IEventProvider
	{
		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		public void LoadEngineContext (IEngineContext context)
		{
			this.context = context;

			this.isloaded = true;
		}

		#region IEventProvider Members

		public void Add (Map map, IMapEvent mapevent)
		{
			lock(this.events)
			{
				if(!this.events.ContainsKey(map.Name))
					this.events.Add(map.Name, new List<IMapEvent>());

				this.events[map.Name].Add(mapevent);
			}
		}

		public bool Remove (Map map, IMapEvent mapevent)
		{
			lock(this.events)
			{
				return this.events[map.Name].Remove(mapevent);
			}
		}

		public bool Handle (BaseCharacter character, ActivationTypes action)
		{
			throw new NotImplementedException();
		}

		#endregion

		private IEngineContext context = null;
		private bool isloaded = false;
		private Dictionary<string, List<IMapEvent>> events = new Dictionary<string, List<IMapEvent>>();
	}
}
