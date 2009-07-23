using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;
using ValkyrieLibrary.Core;
using ValkyrieLibrary.Characters;

namespace ValkyrieLibrary.Events
{
    public class Event
    {
        public MapPoint Location { get; set; }
        public MapPoint Size { get; set; }
        public String Type { get; set; }
        public Directions Direction { get; set; }
		public ActivationTypes Activation { get; set; }

        public Dictionary<String, String> Parameters;

        public Event(MapPoint loc, MapPoint size)
        {
            Location = loc;
            Size = size;
            Type = "";
			Direction = Directions.North;
            Parameters = new Dictionary<String, String>();
        }

        public Event(XmlNode node)
        {
            Parameters = new Dictionary<String, String>();

			foreach (XmlNode cnode in node.ChildNodes)
			{
                switch (cnode.Name)
                {
                    case "Type": Type = cnode.InnerText; break;
                    case "Dir": Direction = (Directions)Enum.Parse(typeof(Directions), cnode.InnerText); break;
                    case "Parameters": LoadParms(cnode); break;
                    case "Location": Location = new MapPoint(cnode); break;
                    case "Size": Size = new MapPoint(cnode); break;
                }
            }
        }

        public void LoadParms(XmlNode root)
        {
            foreach (XmlNode parameter in root.ChildNodes)
            {
                String name = "";
                String type = "";

                foreach (XmlNode child in parameter.ChildNodes)
                {
                    switch (child.Name)
                    {
                        case "Name": name = child.InnerText; break;
                        case "Type": type = child.InnerText; break;
                    }
                }

                if (name != "" && type != "")
                    Parameters.Add(name, type);
            }
        }

        public void Copy(Event e)
        {
            this.Type = e.Type;
            this.Location = e.Location;
            this.Size = e.Size;

            foreach (var parm in e.Parameters.Keys)
            {
                this.Parameters.Add(parm, e.Parameters[parm]);
            }
        }

        public Rectangle ToRect()
        {
            return new Rectangle(Location.X, Location.Y, Size.X, Size.Y);
        }

        public bool IsSameFacing(Directions facing)
        {
            if (this.Direction == Directions.Any)
                return true;

            if (facing == Directions.North && this.Direction == Directions.North)
                return true;

			if (facing == Directions.South && this.Direction == Directions.South)
                return true;

            if (facing == Directions.West && this.Direction == Directions.West)
                return true;

            if (facing == Directions.East && this.Direction == Directions.East)
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
            dir.InnerText = Direction.ToString();

            XmlElement parmRoot = doc.CreateElement("Parameters");

            foreach (var parm in this.Parameters.Keys)
            {
                XmlElement pname = doc.CreateElement("Name");
                pname.InnerText = parm;

                XmlElement ptype = doc.CreateElement("Type");
                ptype.InnerText = this.Parameters[parm];

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