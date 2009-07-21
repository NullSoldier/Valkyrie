using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using ValkyrieLibrary.Core;

namespace ValkyrieLibrary.Core
{

    // one unit in a map point represents 32 units in a screen point
    public class MapPoint : BasePoint
    {
        public MapPoint(int x, int y) : base(x,y)
        {
        }

        public MapPoint(Point point) : base(point)
        {
        }

        public MapPoint(MapPoint point) : base(point.ToPoint())
        {
        }

        public MapPoint(ScreenPoint mapPoint) : base((mapPoint / 32).ToPoint())
        {
        }

        public MapPoint(XmlNode cnode) : base(cnode)
        {
        }

        public ScreenPoint ToScreenPoint()
        {
            return new ScreenPoint(this);
        }

        public static MapPoint operator +(MapPoint a, MapPoint b)
        {
            return new MapPoint(a.X + b.X, a.Y + b.Y);
        }

        public static MapPoint operator *(MapPoint a, MapPoint b)
        {
            return new MapPoint(a.X * b.X, a.Y * b.Y);
        }

        public static MapPoint operator /(MapPoint a, MapPoint b)
        {
            return new MapPoint(a.X / b.X, a.Y / b.Y);
        }

        public static MapPoint operator -(MapPoint a, MapPoint b)
        {
            return new MapPoint(a.X - b.X, a.Y - b.Y);
        }

        public static MapPoint operator +(MapPoint a, int b)
        {
            return new MapPoint(a.X + b, a.Y + b);
        }

        public static MapPoint operator *(MapPoint a, int b)
        {
            return new MapPoint(a.X * b, a.Y * b);
        }

        public static MapPoint operator /(MapPoint a, int b)
        {
            return new MapPoint(a.X / b, a.Y / b);
        }

        public static MapPoint operator -(MapPoint a, int b)
        {
            return new MapPoint(a.X - b, a.Y - b);
        }
    }
}
