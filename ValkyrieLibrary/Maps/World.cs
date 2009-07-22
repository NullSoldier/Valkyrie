using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Collision;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Maps;
using ValkyrieLibrary.Events;
using ValkyrieLibrary.Core;

namespace ValkyrieLibrary.Maps
{
    public class World
    {
        public Dictionary<string, MapHeader> WorldList;
        public String DefaultSpawn { get; set; }

        public World()
        {
            this.WorldList = new Dictionary<string, MapHeader>();
            this.DefaultSpawn = "";
        }

        public ScreenPoint FindStartLocation(String name)
        {
            if (name == "Default")
                name = DefaultSpawn;

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

            if (name != DefaultSpawn)
                return FindStartLocation(DefaultSpawn);

            return new ScreenPoint(0,0);
        }
    }
}
