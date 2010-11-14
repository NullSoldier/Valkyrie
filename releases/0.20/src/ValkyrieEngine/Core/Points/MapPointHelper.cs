using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Valkyrie.Engine.Core.Points
{
    public static class MapPointHelperInt
    {
        /*
        public static MapPoint<T> operator +(MapPoint<T> a, MapPoint<T> b)
        {
            return new MapPoint<T> (a.X + b.X, a.Y + b.Y);
        }

        public static MapPoint operator *(MapPoint<T> a, MapPoint<T> b)
        {
            return new MapPoint<T>(a.X * b.X, a.Y * b.Y);
        }

        public static MapPoint operator /(MapPoint a, MapPoint b)
        {
            return new MapPoint<T>(a.X / b.X, a.Y / b.Y);
        }

        public static MapPoint operator -(MapPoint a, MapPoint b)
        {
            return new MapPoint<T>(a.X - b.X, a.Y - b.Y);
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

        public static MapPoint operator +(MapPoint a, double b)
        {
            return new MapPoint((int)(a.X + b), (int)(a.Y + b));
        }

        public static MapPoint operator *(MapPoint a, double b)
        {
            return new MapPoint((int)(a.X * b), (int)(a.Y * b));
        }

        public static MapPoint operator /(MapPoint a, double b)
        {
            return new MapPoint((int)(a.X / b), (int)(a.Y / b));
        }

        public static MapPoint operator -(MapPoint a, double b)
        {
            return new MapPoint((int)(a.X - b), (int)(a.Y - b));
        }*/
        public static MapPoint<int> GetZeroPoint() { return new MapPoint<int>(0, 0); }
    }

    public static class MapPointHelperFloat
    {
        public static MapPoint<float> GetZeroPoint() { return new MapPoint<float> (0f, 0f); }
    }
}
