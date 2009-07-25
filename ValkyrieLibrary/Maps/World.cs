using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
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

        public World(XmlNode worldNode)
        {
            this.MapList = new Dictionary<string, MapHeader>();
            this.WorldSize = new MapPoint(0, 0);
            this.DefaultSpawn = "";
            this.Name = "No Name";

            foreach (XmlNode node in worldNode.ChildNodes)
            {
                if (node.Name == "DefaultSpawn")
                {
                    this.DefaultSpawn = node.InnerText;
                }
                else if (node.Name == "MapLoc")
                {
                    MapHeader header = new MapHeader(node);
                    this.MapList.Add(header.MapName, header);
                }
            }

            var worldName = worldNode.Attributes.GetNamedItem("Name");
            this.Name = worldName.InnerText;
            CalcWorldSize();
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

        public void SaveXml(XmlNode parent, XmlDocument doc)
        {
            XmlElement world = doc.CreateElement("World");
            var worldAtt = doc.CreateAttribute("Name");
            worldAtt.InnerText = this.Name;
            world.Attributes.Append(worldAtt);

            XmlElement defSpawn = doc.CreateElement("DefaultSpawn");
            defSpawn.InnerText = this.DefaultSpawn;

            world.AppendChild(defSpawn);

            foreach (var m in this.MapList)
            {
                m.Value.SaveXml(world, doc);
            }

            parent.AppendChild(world);
        }
    }
}
