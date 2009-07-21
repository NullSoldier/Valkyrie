using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ValkyrieLibrary.Core;

namespace ValkyrieLibrary.Core
{

    // one unit in a map point represents 32 units in a screen point
    class MapPoint : BasePoint
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

        public MapPoint(ScreenPoint mapPoint) : base((mapPoint * 32).toPoint())
        {
        }

        public ScreenPoint toScreenPoint()
        {
            return new ScreenPoint(this);
        }

        public static override BasePoint newPoint(int X, int Y)
        {
            return new MapPoint(X, Y);
        }
    }
}
