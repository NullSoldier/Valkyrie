using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Valkyrie.Engine.Core.Points
{
    public static class BasePointHelperGeneric
    {
        /*public static void ToXml (this BasePoint<T> self, XmlDocument doc, XmlElement parent)
        {
            var sizex = doc.CreateElement("X");
            sizex.InnerText = self.X.ToString();

            var sizey = doc.CreateElement("Y");
            sizey.InnerText = self.Y.ToString();

            parent.AppendChild(sizex);
            parent.AppendChild(sizey);
        }*/
    }

    public static class BasePointHelperInt
    {
        public static void FromXmlNode (this BasePoint<int> self, XmlNode cnode)
        {
            foreach (XmlNode locNode in cnode.ChildNodes)
            {
                if (locNode.Name == "X")
                    self.X = Convert.ToInt32(locNode.InnerText);
                else if (locNode.Name == "Y")
                   self.Y = Convert.ToInt32(locNode.InnerText);
            }
        }

        public static Point ToPoint(this BasePoint<int> self)
        {
            return new Point(self.X, self.Y);
        }

        public static Vector2 ToVector2(this BasePoint<int> self)
        {
            return new Vector2((float)self.X, (float)self.Y);
        }

     

        public static Rectangle ToRect(this BasePoint<int> self, Point size)
        {
            return new Rectangle(self.X, self.Y, size.X, size.Y);
        }
    }

    public static class BasePointHelperFloat
    {
        public static Point ToPoint(this BasePoint<float> self)
        {
            return new Point((int)self.X, (int)self.Y);
        }

        public static Vector2 ToVector2(this BasePoint<float> self)
        {
            return new Vector2(self.X, self.Y);

        }
        public static Rectangle ToRect(this BasePoint<float> self, Point size)
        {
            return new Rectangle((int)self.X, (int)self.Y, size.X, size.Y);
        }
    }
}
