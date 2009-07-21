using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ValkyrieLibrary.Core;

namespace ValkyrieLibrary
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

        public MapPoint(MapPoint point) : base(point.toPoint())
        {
        }

        public MapPoint(ScreenPoint mapPoint) : base((mapPoint / 32).toPoint())
        {
        }

        public MapPoint(XmlNode cnode) : base(cnode)
        {
        }

        public ScreenPoint ToScreenPoint()
        {
            return new ScreenPoint(this);
        }
    }
}
