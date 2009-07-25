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
        public Dictionary<string, MapHeader> MapList;
        public String DefaultSpawn { get; set; }
        public String Name;
        public MapPoint WorldSize { get; set; }

        public World()
        {
            this.MapList = new Dictionary<string, MapHeader>();
            this.DefaultSpawn = "";
            this.Name = "No Name";
            this.WorldSize = new MapPoint(0, 0);
        }

        public ScreenPoint FindStartLocation(String name)
        {
            if (name == "Default")
                name = DefaultSpawn;

            foreach (var mapHeader in MapList)
            {
                foreach (Event e in mapHeader.Value.Map.EventList)
                {
                    if (e.Type != "Load")
                        continue;

                    String eName = e.Parameters["Name"];

                    if (eName == name)
                        return e.Location.ToScreenPoint() + mapHeader.Value.MapLocation.ToScreenPoint();
                }
            }

            if (name != DefaultSpawn)
                return FindStartLocation(DefaultSpawn);

            return new ScreenPoint(0,0);
        }


        public void CalcWorldSize()
        {
            this.WorldSize.X = 0;
            this.WorldSize.Y = 0;

            foreach (var mh in this.MapList)
            {
                int xSize = mh.Value.MapLocation.X + mh.Value.Map.MapSize.X;
                int ySize = mh.Value.MapLocation.Y + mh.Value.Map.MapSize.Y;

                if (xSize > this.WorldSize.X)
                    this.WorldSize.X = xSize;

                if (ySize > this.WorldSize.Y)
                    this.WorldSize.Y = ySize;
            }
        }
    }
}
