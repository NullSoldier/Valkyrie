using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Core;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;

namespace ValkyrieLibrary.Maps
{
	public class MapHeader
	{
		//public event EventHandler MapLoaded;
		//public event EventHandler MapUnloaded;

		#region Constructors
		public MapHeader(string mapname, string filepath)
            : this(mapname, filepath, new MapPoint(0, 0)) { }

		public MapHeader(string mapname, string filepath, MapPoint point)
		{
			this.MapName = mapname;
			this.MapLocation = point;
			this.MapFileLocation = filepath;
		}

        public MapHeader(XmlNode node)
        {
            int x = 0, y = 0;

            foreach (XmlNode cnode in node.ChildNodes)
            {
                if (cnode.Name == "Name")
                    this.MapName = cnode.InnerText;
                else if (cnode.Name == "FilePath")
                    this.MapFileLocation = cnode.InnerText;
                else if (cnode.Name == "X")
                    x = Convert.ToInt32(cnode.InnerText);
                else if (cnode.Name == "Y")
                    y = Convert.ToInt32(cnode.InnerText);
            }

            this.MapLocation = new MapPoint(x, y);
        }

		#endregion

		public Map Map
		{
			get
			{
				if (this.map == null)
					this.Load();

				return this.map;
			}
			set { this.map = value; }
		}

        public string MapName { get; set; }
        public string MapFileLocation { get; set; }
        public MapPoint MapLocation { get; set; }

		private Map map;


		public void Load()
		{
			this.map = new Map();
			this.map.LoadMap(new FileInfo(Path.Combine(TileEngine.Configuration[TileEngineConfigurationName.MapRoot], this.MapFileLocation)));
			this.map.Name = this.MapName;
		}

		public void Unload()
		{
			
		}

        public void SaveXml(XmlNode parent, XmlDocument doc)
        {
            XmlElement mapLoc = doc.CreateElement("MapLoc");

            XmlElement n = doc.CreateElement("Name");
            XmlElement f = doc.CreateElement("FilePath");
            XmlElement x = doc.CreateElement("X");
            XmlElement y = doc.CreateElement("Y");

            n.InnerText = this.MapName;
            f.InnerText = this.MapFileLocation;
            x.InnerText = String.Format( "{0}", this.MapLocation.X);
            y.InnerText = String.Format( "{0}", this.MapLocation.Y);

            mapLoc.AppendChild(n);
            mapLoc.AppendChild(f);
            mapLoc.AppendChild(x);
            mapLoc.AppendChild(y);

            parent.AppendChild(mapLoc);
        }

	}
}
