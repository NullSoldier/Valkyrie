using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public ScreenPoint(ScreenPoint point) : base(point.toPoint())
        {
        }

        public ScreenPoint(MapPoint mapPoint) : base((mapPoint*32).toPoint())
        {
        }

        public MapPoint toMapPoint()
        {
            return new MapPoint(this);
        }
    }
}
