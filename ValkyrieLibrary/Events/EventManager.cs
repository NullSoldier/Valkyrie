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

            this.EventHandlerList.Add(new LoadEvent());
        }

        //A button pressed
        public bool Action(Player player)
        {
            bool res = false;

            if (HandleEvent(player, ActivationTypes.Activate))
                res = true;

            if (HandleEvent(player, ActivationTypes.LookActivate))
                res = true;

            return res;
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
            MapPoint pos = player.MapLocation;

            if (type == ActivationTypes.LookActivate || type == ActivationTypes.Movement)
                pos += player.GetLookPoint();

            bool handled = false;

            Event e = GetEvent(pos);
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
