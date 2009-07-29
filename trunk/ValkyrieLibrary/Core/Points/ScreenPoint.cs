using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;

namespace ValkyrieLibrary.Core
{
    public class ScreenPoint : BasePoint
    {
        public ScreenPoint(int x, int y) : base(x,y)
        {
        }

        public ScreenPoint(Point point) : base(point)
        {
        }

        public ScreenPoint(ScreenPoint point) : base(point.ToPoint())
        {
        }

        public ScreenPoint(MapPoint mapPoint) : base((mapPoint*32).ToPoint())
        {
        }

        public ScreenPoint(XmlNode cnode) : base(cnode)
        {
        }

        public ScreenPoint(Vector2 vect) : base((int)vect.X, (int)vect.Y)
        {
        }

        public MapPoint ToMapPoint()
        {
            return new MapPoint(this);
        }

        public static ScreenPoint operator +(ScreenPoint a, ScreenPoint b)
        {
            return new ScreenPoint(a.X + b.X, a.Y + b.Y);
        }

        public static ScreenPoint operator *(ScreenPoint a, ScreenPoint b)
        {
            return new ScreenPoint(a.X * b.X, a.Y * b.Y);
        }

        public static ScreenPoint operator /(ScreenPoint a, ScreenPoint b)
        {
            return new ScreenPoint(a.X / b.X, a.Y / b.Y);
        }

        public static ScreenPoint operator -(ScreenPoint a, ScreenPoint b)
        {
            return new ScreenPoint(a.X - b.X, a.Y - b.Y);
        }

        public static ScreenPoint operator +(ScreenPoint a, int b)
        {
            return new ScreenPoint(a.X + b, a.Y + b);
        }

        public static ScreenPoint operator *(ScreenPoint a, int b)
        {
            return new ScreenPoint(a.X * b, a.Y * b);
        }

        public static ScreenPoint operator /(ScreenPoint a, int b)
        {
            return new ScreenPoint(a.X / b, a.Y / b);
        }

        public static ScreenPoint operator -(ScreenPoint a, int b)
        {
            return new ScreenPoint(a.X - b, a.Y - b);
        }


        public static ScreenPoint operator +(ScreenPoint a, double b)
        {
            return new ScreenPoint((int)(a.X + b), (int)(a.Y + b));
        }

        public static ScreenPoint operator *(ScreenPoint a, double b)
        {
            return new ScreenPoint((int)(a.X * b), (int)(a.Y * b));
        }

        public static ScreenPoint operator /(ScreenPoint a, double b)
        {
            return new ScreenPoint((int)(a.X / b), (int)(a.Y / b));
        }

        public static ScreenPoint operator -(ScreenPoint a, double b)
        {
            return new ScreenPoint((int)(a.X - b), (int)(a.Y - b));
        }
    }
}
