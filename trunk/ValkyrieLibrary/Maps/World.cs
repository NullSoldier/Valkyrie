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
using ValkyrieLibrary.Events.EngineEvents;

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

		public ScreenPoint FindDefaultStartLocation()
		{
			return this.FindStartLocation(this.DefaultSpawn);
		}

        public ScreenPoint FindStartLocation(String name)
        {
			// Keep a static cache of this for optimization
			//string EntryPointType = ((BaseMapEvent)Activator.CreateInstance(typeof(EntryPointEvent))).GetType();

			foreach (var mapHeader in MapList)
			{
				foreach (IMapEvent e in mapHeader.Value.Map.EventList)
				{
					if ( !(e is EntryPointEvent) || !e.Parameters.ContainsKey("Name"))
						continue;

					if (name == e.Parameters["Name"])
					{
						ScreenPoint point = (new MapPoint(e.Rectangle.X, e.Rectangle.Y) + mapHeader.Value.MapLocation).ToScreenPoint();
						return point;
					}			
				}
			}

			return new ScreenPoint(0, 0);
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
