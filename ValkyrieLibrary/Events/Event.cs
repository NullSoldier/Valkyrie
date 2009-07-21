using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;
using ValkyrieLibrary.Core;

namespace ValkyrieLibrary.Events
{
    public class Event
    {
        public MapPoint Location { get; set; }
        public MapPoint Size { get; set; }
        public String Type { get; set; }
        public String ParmOne { get; set; }
        public String ParmTwo { get; set; }
        public String Dir { get; set; }

        public Event(MapPoint loc, MapPoint size)
        {
            Location = loc;
            Size = size;
            Type = "";
            ParmOne = "";
            ParmTwo = "";
            Dir = "";
        }

        public Event(XmlNode node)
        {
			foreach (XmlNode cnode in node.ChildNodes)
			{
                switch (cnode.Name)
                {
                    case "Type": Type = cnode.InnerText; break;
                    case "Dir": Dir = cnode.InnerText; break;
                    case "ParmOne": ParmOne = cnode.InnerText; break;
                    case "ParmTwo": ParmTwo = cnode.InnerText; break;
                    case "Location": Location = new MapPoint(cnode); break;
                    case "Size": Size = new MapPoint(cnode); break;
                }
            }
        }

        public void Copy(Event e)
        {
            this.Type = e.Type;
            this.Location = e.Location;
            this.Size = e.Size;
            this.ParmOne = e.ParmOne;
            this.ParmTwo = e.ParmTwo;
        }

        public Rectangle ToRect()
        {
            return new Rectangle(Location.X, Location.Y, Size.X, Size.Y);
        }

        public bool IsSameFacing(ValkyrieLibrary.Characters.Directions facing)
        {
            if (Dir == "All")
                return true;

            if (facing == ValkyrieLibrary.Characters.Directions.North && Dir == "North")
                return true;

            if (facing == ValkyrieLibrary.Characters.Directions.South && Dir == "South")
                return true;

            if (facing == ValkyrieLibrary.Characters.Directions.West && Dir == "West")
                return true;

            if (facing == ValkyrieLibrary.Characters.Directions.East && Dir == "East")
                return true;

            return false;
        }

        public void toXml(XmlDocument doc, XmlElement parent)
        {
            XmlElement loc = doc.CreateElement("Location");
            Location.ToXml(doc, loc);

            XmlElement size = doc.CreateElement("Size");
            Size.ToXml(doc, loc);

            XmlElement type = doc.CreateElement("Type");
            type.InnerText = Type;

            XmlElement pone = doc.CreateElement("ParmOne");
            pone.InnerText = ParmOne;

            XmlElement ptwo = doc.CreateElement("ParmTwo");
            ptwo.InnerText = ParmTwo;

            XmlElement dir = doc.CreateElement("Dir");
            dir.InnerText = Dir;

            parent.AppendChild(loc);
            parent.AppendChild(size);
            parent.AppendChild(type);
            parent.AppendChild(pone);
            parent.AppendChild(ptwo);
            parent.AppendChild(dir);
        }
    }
}