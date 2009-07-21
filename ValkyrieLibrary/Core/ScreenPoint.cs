using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public ScreenPoint(ScreenPoint point) : base(point.toPoint())
        {
        }

        public ScreenPoint(MapPoint mapPoint) : base((mapPoint*32).toPoint())
        {
        }

        public ScreenPoint(XmlNode cnode) : base(cnode)
        {
        }

        public MapPoint ToMapPoint()
        {
            return new MapPoint(this);
        }
    }
}
