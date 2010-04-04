using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ValkyrieLibrary.Maps
{
    public class MapEvent
    {
        public Point Location { get; set; }
        public Point Size { get; set; }
        public String Type { get; set; }
        public String ParmOne { get; set; }
        public String ParmTwo { get; set; }
        public String Dir { get; set; }

        public MapEvent(Point loc, Point size)
        {
            Location = loc;
            Size = size;
            Type = "Null Event";
            ParmOne = "";
            ParmTwo = "";
            Dir = "All";
        }

        public MapEvent(XmlNode node)
        {
			foreach (XmlNode cnode in node.ChildNodes)
			{
                if (cnode.Name == "Type")
                {
                    Type = cnode.InnerText;
                }
                if (cnode.Name == "Dir")
                {
                    Dir = cnode.InnerText;
                }
                if (cnode.Name == "ParmOne")
                {
                    ParmOne = cnode.InnerText;
                }
                if (cnode.Name == "ParmTwo")
                {
                    ParmTwo = cnode.InnerText;
                }
                else if (cnode.Name == "Location")
                {
                    int x = 0, y = 0;

                    foreach (XmlNode locNode in cnode.ChildNodes)
                    {
                        if (locNode.Name == "X")
                            x = Convert.ToInt32(locNode.InnerText);
                        else if (locNode.Name == "Y")
                            y = Convert.ToInt32(locNode.InnerText);
                    }
                    this.Location = new Point(x, y);
                }
                else if (cnode.Name == "Size")
                {
                    int x = 0, y = 0;

                    foreach (XmlNode sizeNode in cnode.ChildNodes)
                    {
                        if (sizeNode.Name == "X")
                            x = Convert.ToInt32(sizeNode.InnerText);
                        else if (sizeNode.Name == "Y")
                            y = Convert.ToInt32(sizeNode.InnerText);
                    }
                    this.Size = new Point(x, y);
                }
            }
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
            
            var locx = doc.CreateElement("X");
            locx.InnerText = Location.X.ToString();
            var locy = doc.CreateElement("Y");
            locy.InnerText = Location.Y.ToString();

            loc.AppendChild(locx);
            loc.AppendChild(locy);

            XmlElement size = doc.CreateElement("Size");
            var sizex = doc.CreateElement("X");
            sizex.InnerText = Size.X.ToString();
            var sizey = doc.CreateElement("Y");
            sizey.InnerText = Size.Y.ToString();

            size.AppendChild(sizex);
            size.AppendChild(sizey);

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
