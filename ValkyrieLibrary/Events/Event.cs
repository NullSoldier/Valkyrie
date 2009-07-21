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
        public String Dir { get; set; }

        public Dictionary<String, String> Parms;

        public Event(MapPoint loc, MapPoint size)
        {
            Location = loc;
            Size = size;
            Type = "";
            Dir = "";
            Parms = new Dictionary<String, String>();
        }

        public Event(XmlNode node)
        {
			foreach (XmlNode cnode in node.ChildNodes)
			{
                switch (cnode.Name)
                {
                    case "Type": Type = cnode.InnerText; break;
                    case "Dir": Dir = cnode.InnerText; break;
                    case "Parameters": LoadParms(cnode); break;
                    case "Location": Location = new MapPoint(cnode); break;
                    case "Size": Size = new MapPoint(cnode); break;
                }
            }
        }

        public void LoadParms(XmlNode root)
        {
            foreach (XmlNode child in root.ChildNodes)
            {
                String name = "";
                String type = "";

                switch (child.Name)
                {
                    case "Name": name = child.InnerText; break;
                    case "Type": type = child.InnerText; break;
                }

                if (name != "" && type != "")
                    Parms.Add(name, type); 
            }
        }

        public void Copy(Event e)
        {
            this.Type = e.Type;
            this.Location = e.Location;
            this.Size = e.Size;

            foreach (var parm in e.Parms.Keys)
            {
                this.Parms.Add(parm, e.Parms[parm]);
            }
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
            Size.ToXml(doc, size);

            XmlElement type = doc.CreateElement("Type");
            type.InnerText = Type;

            XmlElement dir = doc.CreateElement("Dir");
            dir.InnerText = Dir;

            XmlElement parmRoot = doc.CreateElement("Parameters");

            foreach (var parm in this.Parms.Keys)
            {
                XmlElement pname = doc.CreateElement("Name");
                pname.InnerText = parm;

                XmlElement ptype = doc.CreateElement("Type");
                ptype.InnerText = this.Parms[parm];

                XmlElement parmNode = doc.CreateElement("Parameter");
                parmNode.AppendChild(pname);
                parmNode.AppendChild(ptype);

                parmRoot.AppendChild(parmNode);
            }

            parent.AppendChild(loc);
            parent.AppendChild(size);
            parent.AppendChild(type);
            parent.AppendChild(parmRoot);
            parent.AppendChild(dir);
        }
    }
}