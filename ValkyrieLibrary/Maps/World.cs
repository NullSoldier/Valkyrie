using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Collision;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Maps;
using ValkyrieLibrary.Events;
using ValkyrieLibrary.Core;
using ValkyrieLibrary.Core.Points;

namespace ValkyrieLibrary.Maps
{
    public class World
    {
        public Dictionary<string, MapHeader> WorldList;

        public World()
        {
            this.WorldList = new Dictionary<string, MapHeader>();
        }

        public ScreenPoint FindStartLocation(String name)
        {
            foreach (var mapHeader in WorldList)
            {
                foreach (Event e in mapHeader.Value.Map.EventList)
                {
                    if (e.Type != "Load")
                        continue;

                    String eName = e.Parms["Name"];

                    if (eName == name)
                        return e.Location.ToScreenPoint() + mapHeader.Value.MapLocation.ToScreenPoint();
                }
            }

            return new ScreenPoint(0,0);
        }
    }
}
