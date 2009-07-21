﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ValkyrieLibrary
{
    public class BasePoint : IEquatable<BasePoint>
    {
        public int X
        {
            get;
            set;
        }

        public int Y
        {
            get;
            set;
        }

        public BasePoint(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public BasePoint(Point point)
        {
            this.X = point.X;
            this.Y = point.Y;
        }

        public Point toPoint()
        {
            return new Point(this.X, this.Y);
        }

        public static BasePoint operator +(BasePoint a, BasePoint b)
        {
            a.X = a.X + b.X;
            a.Y = a.Y + b.Y;
            return a;
        }

        public static BasePoint operator *(BasePoint a, BasePoint b)
        {
            a.X = a.X * b.X;
            a.Y = a.Y * b.Y;
            return a;
        }

        public static BasePoint operator /(BasePoint a, BasePoint b)
        {
            a.X = a.X / b.X;
            a.Y = a.Y / b.Y;
            return a;
        }

        public static BasePoint operator -(BasePoint a, BasePoint b)
        {
            a.X = a.X - b.X;
            a.Y = a.Y - b.Y;
            return a;
        }

        public static BasePoint operator +(BasePoint a, int b)
        {
            a.X = a.X + b;
            a.Y = a.Y + b;
            return a;
        }

        public static BasePoint operator *(BasePoint a, int b)
        {
            a.X = a.X * b;
            a.Y = a.Y * b;
            return a;
        }

        public static BasePoint operator /(BasePoint a, int b)
        {
            a.X = a.X / b;
            a.Y = a.Y / b;
            return a;
        }

        public static BasePoint operator -(BasePoint a, int b)
        {
            a.X = a.X - b;
            a.Y = a.Y - b;
            return a;
        }

        public static bool operator !=(BasePoint a, BasePoint b)
        {
            return !(a.Equals(b));
        }

        public static bool operator ==(BasePoint a, BasePoint b)
        {
            return !(a == b);
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
            return "TODO";
        }
    }
}