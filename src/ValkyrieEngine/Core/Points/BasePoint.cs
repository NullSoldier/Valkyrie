using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Valkyrie.Engine.Core
{
    public class BasePoint : IEquatable<BasePoint>
    {
        public float X
        {
            get { return this.floatx; }
            set
            {
                this.floatx = value;
                this.intx = (int)value;
            }
        }

        public float Y
        {
            get { return this.floaty; }
            set
            {
                this.floaty = value;
                this.inty = (int)value;
            }
        }

        public int IntX
        {
            get { return (int)X; }
            set { this.floatx = (float)value; }
        }

        public int IntY
        {
            get { return inty; }
            set { this.floaty = (float)value; }
        }

        private float floatx=0f, floaty=0f;
        private int intx=0, inty=0;

        public BasePoint(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public BasePoint(Point point)
        {
            this.X = point.X;
            this.Y = point.Y;
        }

        public BasePoint(XmlNode cnode)
        {
            foreach (XmlNode locNode in cnode.ChildNodes)
            {
                if (locNode.Name == "X")
                    X = Convert.ToInt32(locNode.InnerText);
                else if (locNode.Name == "Y")
                    Y = Convert.ToInt32(locNode.InnerText);
            }
        }

        public Point ToPoint()
        {
            return new Point((int)this.X, (int)this.Y);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }

        public Rectangle ToRect(Point size)
        {
            return new Rectangle((int)this.X, (int)this.Y, size.X, size.Y);
        }

        public static BasePoint operator +(BasePoint a, BasePoint b)
        {
            return new BasePoint(a.X + b.X, a.Y + b.Y);
        }

        public static BasePoint operator *(BasePoint a, BasePoint b)
        {
            return new BasePoint(a.X * b.X, a.Y * b.Y);
        }

        public static BasePoint operator /(BasePoint a, BasePoint b)
        {
            return new BasePoint(a.X / b.X, a.Y / b.Y);
        }

        public static BasePoint operator -(BasePoint a, BasePoint b)
        {
            return new BasePoint(a.X - b.X, a.Y - b.Y);
        }

        public static BasePoint operator +(BasePoint a, int b)
        {
            return new BasePoint(a.X + b, a.Y + b);
        }

        public static BasePoint operator *(BasePoint a, int b)
        {
            return new BasePoint(a.X * b, a.Y * b);
        }

        public static BasePoint operator /(BasePoint a, int b)
        {
            return new BasePoint(a.X / b, a.Y / b);
        }

        public static BasePoint operator -(BasePoint a, int b)
        {
            return new BasePoint(a.X - b, a.Y - b);
        }

        public static bool operator !=(BasePoint a, BasePoint b)
        {
            return !(a.Equals(b));
        }

        public static bool operator ==(BasePoint a, BasePoint b)
        {
            return (a.Equals(b));
        }

        public override bool Equals(object obj)
        {
            if (typeof(BasePoint) == obj.GetType())
                return Equals((BasePoint)obj);

            return false;
        }

        public bool Equals(BasePoint other)
        {
            return (this.X == other.X && this.Y == other.Y);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override string ToString()
        {
            return String.Format("X: {0}, Y: {1}", this.X, this.Y);
        }

        public void ToXml(XmlDocument doc, XmlElement parent)
        {
            var sizex = doc.CreateElement("X");
            sizex.InnerText = X.ToString();
            var sizey = doc.CreateElement("Y");
            sizey.InnerText = Y.ToString();

            parent.AppendChild(sizex);
            parent.AppendChild(sizey);
        }
    }
}
