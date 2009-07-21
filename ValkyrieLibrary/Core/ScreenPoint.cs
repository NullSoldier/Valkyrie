using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using System.Xml;

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
    }
}
