﻿using System;
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
			this.Add(map.Name, mapevent);
		}

		public void Add (string mapname, IMapEvent mapevent)
		{
			lock(this.events)
			{
				if(!this.events.ContainsKey(mapname))
					this.events.Add(mapname, new List<IMapEvent>());

				this.events[mapname].Add(mapevent);
			}
		}

		public void ReferenceSetOrAdd (Map map, IMapEvent mapevent)
		{
			this.ReferenceSetOrAdd(map.Name, mapevent);
		}

		public void ReferenceSetOrAdd (string mapname, IMapEvent mapevent)
		{
			lock(this.events)
			{
				if(this.events[mapname].Contains(mapevent))
				{
					int index = this.events[mapname].IndexOf(mapevent);

					this.events[mapname][index] = (IMapEvent)mapevent.Clone();
				}
				else
					this.events[mapname].Add(mapevent);
			}
		}

		public bool Remove (Map map, IMapEvent mapevent)
		{
			return this.Remove(map.Name, mapevent);
		}

		public bool Remove (string mapname, IMapEvent mapevent)
		{
			lock(this.events)
			{
				if(!this.events.ContainsKey(mapname))
					return false;

				return this.events[mapname].Remove(mapevent);
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
					mapevent.Trigger(player, this.context);
					handledevents = true;
				}
			}

			return handledevents;
		}

		public void ClearEvents ()
		{
			lock(this.events)
			{
				this.events.Clear();
			}
		}

		public IEnumerable<IMapEvent> GetMapsEvents (Map map)
		{
			return this.GetMapsEvents(map.Name);
		}

		public IEnumerable<IMapEvent> GetMapsEvents (string mapname)
		{
			lock(this.events)
			{
				if(!this.events.ContainsKey(mapname))
					this.events.Add(mapname, new List<IMapEvent>());

				return this.events[mapname];
			}
		}

		public int GetMapsEventCount (Map map)
		{
			return this.GetMapsEventCount(map.Name);
		}

		public int GetMapsEventCount (string mapname)
		{
			if(!this.events.ContainsKey(mapname))
				this.events.Add(mapname, new List<IMapEvent>());

			return this.events[mapname].Count;
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
				MapHeader header = this.context.WorldManager.GetWorld(player.WorldName).GetLocalMapFromPoint(position);
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
