using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Valkyrie.Library.Collision;
using Valkyrie.Library.Characters;
using Valkyrie.Library.Maps;
using Valkyrie.Library.Events;
using Valkyrie.Library.Core;
using Valkyrie.Library.Events.EngineEvents;
using Valkyrie.Library.Maps.MapProvider;
using System.IO;

namespace Valkyrie.Library.Maps
{
    public class World
    {
        public Dictionary<string, MapHeader> MapList;
        public String DefaultSpawn { get; set; }
        public String Name;
        public MapPoint WorldSize { get; set; }

		public World ()
		{
			this.MapList = new Dictionary<string, MapHeader>();
			this.DefaultSpawn = "";
			this.Name = "No Name";
			this.WorldSize = new MapPoint(0, 0);
		}

		public void Load (XmlNode worldNode, IMapProvider mapprovider)
		{
			this.Load(worldNode, mapprovider);
		}

		public void Load(XmlNode worldNode, IMapProvider mapprovider, MapEventManager mapmanager)
		{
			foreach (XmlNode node in worldNode.ChildNodes)
            {
                if (node.Name == "DefaultSpawn")
                {
                    this.DefaultSpawn = node.InnerText;
                }
                else if (node.Name == "MapLoc")
                {
					/* Load the map header from an XML node */
					MapHeader header = new MapHeader();
					header.MapProvider = new XMLMapProvider();
					header.MapManager = mapmanager;

					int x = 0, y = 0;

					foreach(XmlNode cnode in node.ChildNodes)
					{
						if(cnode.Name == "Name")
							header.MapName = cnode.InnerText;
						else if(cnode.Name == "FilePath")
							header.MapFileLocation = Path.Combine(Environment.CurrentDirectory, "Maps\\" + cnode.InnerText);
						else if(cnode.Name == "X")
							x = Convert.ToInt32(cnode.InnerText);
						else if(cnode.Name == "Y")
							y = Convert.ToInt32(cnode.InnerText);
					}

					header.MapLocation = new MapPoint(x, y);

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

            foreach (var mapheader in this.MapList.Values)
            {
				/* Save the map header to XML*/
				XmlElement mapLoc = doc.CreateElement("MapLoc");

				XmlElement n = doc.CreateElement("Name");
				XmlElement f = doc.CreateElement("FilePath");
				XmlElement x = doc.CreateElement("X");
				XmlElement y = doc.CreateElement("Y");

				n.InnerText = mapheader.MapName;
				f.InnerText = mapheader.MapFileLocation;
				x.InnerText = String.Format("{0}", mapheader.MapLocation.X);
				y.InnerText = String.Format("{0}", mapheader.MapLocation.Y);

				mapLoc.AppendChild(n);
				mapLoc.AppendChild(f);
				mapLoc.AppendChild(x);
				mapLoc.AppendChild(y);

				parent.AppendChild(mapLoc);
            }

            parent.AppendChild(world);
        }
    }
}
