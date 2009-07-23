using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Core;
using ValkyrieLibrary.Maps;

namespace ValkyrieLibrary.Events
{
    public class EventManager
    {
        private readonly List<IEventHandler> EventHandlerList;
    
        public EventManager()
        {
            this.EventHandlerList = new List<IEventHandler>();
        }

		public void AddEventHandler(IEventHandler handler)
		{
			this.EventHandlerList.Add(handler);
		}

        //A button pressed
        public bool Action(BaseCharacter player)
        {
            bool res = false;

            if (HandleEvent(player, ActivationTypes.Activate))
                res = true;

            if (HandleEvent(player, ActivationTypes.LookActivate))
                res = true;

            return res;
        }

        public bool Collision(BaseCharacter player)
        {
            return HandleEvent(player, ActivationTypes.Collision);
        }

        public bool Movement(BaseCharacter player)
        {
            return HandleEvent(player, ActivationTypes.Movement);
        }

        public bool HandleEvent(BaseCharacter player, ActivationTypes type)
        {
			// This code needs handling
            BasePoint pos = player.MapLocation;

            if (type == ActivationTypes.LookActivate || type == ActivationTypes.Movement || type == ActivationTypes.Collision)
                pos += player.GetLookPoint();

            bool handled = false;

			// Should be generic to any point and not aware of the maps tiles so if it was pixel based movement you could technically have events at pixel locations
            Event e = GetEvent(new MapPoint(pos.X, pos.Y));
            if (e != null && e.IsSameFacing(player.Direction) == true)
            {
                foreach (IEventHandler eh in EventHandlerList)
                {
                    if (eh.Type != e.Type || !eh.MeetsCriteria(type))
                        continue;

                    eh.Trigger(player, e);
                    handled = true;
                }
            }

            return handled;
        }

        //that is a map
        public Event GetEventInRect(MapPoint pos, MapPoint size)
        {
            foreach (Event ev in TileEngine.CurrentMapChunk.EventList)
            {
                if (pos.ToRect(size.ToPoint()).Intersects(ev.ToRect()) == true)
                    return ev;
            }

            return null;
        }

        public Event GetEvent(MapPoint pos)
        {
            foreach (Event ev in TileEngine.CurrentMapChunk.EventList)
            {
                if (ev.ToRect().Contains(pos.ToPoint()) == true)
                    return ev;
            }

            return null;
        }

        public void SetEvent(Event e)
        {
            foreach (Event ev in TileEngine.CurrentMapChunk.EventList)
            {
                if (ev.Location == e.Location && ev.Size == e.Size)
                {
                    ev.Copy(e);
                    return;
                }
            }

            TileEngine.CurrentMapChunk.EventList.Add(e);
        }

        public void DelEvent(Event e)
        {
            foreach (Event ev in TileEngine.CurrentMapChunk.EventList)
            {
                if (ev.Location == e.Location && ev.Size == e.Size)
                {
                    TileEngine.CurrentMapChunk.EventList.Remove(ev);
                    return;
                }
            }
        }

        public void LoadEvents(Map map, XmlNode root)
        {
            XmlNodeList events = root.ChildNodes;

            foreach (XmlNode node in events)
            {
                Event e = new Event(node);
                map.EventList.Add(e);
            }
        }



    }
}
