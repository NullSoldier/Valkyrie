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
using Valkyrie.Engine.Core;
using Microsoft.Xna.Framework;

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

		public bool HandleEvent (BaseCharacter player, ActivationTypes activation)
		{
			bool handledevents = false;
			MapPoint pos = player.GlobalTileLocation;

			if(activation == ActivationTypes.Activate || activation == ActivationTypes.Collision)
			{
				MapPoint newpt = player.GetLookValue();
				pos = new MapPoint(pos.X + newpt.X, pos.Y + newpt.Y);
			}

			foreach(IMapEvent mapevent in this.GetEvent(player, pos))
			{
				if(mapevent.Direction == Directions.Any || mapevent.Direction == player.Direction
					&& mapevent.Activation == activation)
				{
					mapevent.Trigger(player);
					handledevents = true;
				}
			}

			return handledevents;
		}

		#endregion

		private IEngineContext context = null;
		private bool isloaded = false;
		private Dictionary<string, List<IMapEvent>> events = new Dictionary<string, List<IMapEvent>>();

		private IEnumerable<IMapEvent> GetEvent (BaseCharacter player, MapPoint position)
		{
			player.CurrentMap = this.context.SceneProvider.GetPositionableLocalMap(player);
			MapPoint pos = player.LocalTileLocation;

			if(pos.X < 0 || pos.Y < 0 || pos.X > player.CurrentMap.Map.MapSize.X || pos.Y > player.CurrentMap.Map.MapSize.Y)
			{
				// Find globally
				MapHeader header = this.context.WorldManager.Worlds[player.WorldName].GetLocalMapFromPoint(position);
				MapPoint localpos = player.GlobalTileLocation - header.MapLocation;

				return this.events[header.MapName].Where(m => m.Rectangle.Contains((localpos).ToPoint()));
			}
			else
			{
				MapPoint localpos = position - player.CurrentMap.MapLocation;

				return this.events[player.CurrentMap.MapName].Where(m => m.Rectangle.Contains((localpos).ToPoint()));
			}
			
		}
	}
}
