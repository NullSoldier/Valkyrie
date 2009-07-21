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
        public List<IEventHandler> EventHandlerList;
        

        public EventManager()
        {
            EventHandlerList = new List<IEventHandler>();
            
        }

        //A button pressed
        public bool Action(Player player)
        {
            return HandleEvent(player, ActivationTypes.Activate);
        }

        public bool Collision(Player player)
        {
            return HandleEvent(player, ActivationTypes.Collision);
        }

        public bool Movement(Player player)
        {
            return HandleEvent(player, ActivationTypes.Movement);
        }

        public bool HandleEvent(Player player, ActivationTypes type)
        {
            Event e = GetEvent(player.MapLocation);
            if (e != null && e.IsSameFacing(player.Direction) == true)
            {
                IEventHandler eHandle = GetEventHandler(e);
                if (eHandle != null && eHandle.MeetsCriteria(type))
                {
                    eHandle.Trigger(player, e);
                    return true;
                }
            }

            return false;
        }

        public IEventHandler GetEventHandler(Event e)
        {
            foreach (IEventHandler eh in EventHandlerList)
            {
                if (eh.Type == e.Type)
                    return eh;
            }

            return null;
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
