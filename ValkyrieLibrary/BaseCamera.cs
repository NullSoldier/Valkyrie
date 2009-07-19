﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ValkyrieLibrary.Core;
using ValkyrieLibrary.Characters;

namespace ValkyrieLibrary
{
	public class BaseCamera
	{
		public Rectangle Screen;
		public Vector2 MapOffset;
		public Vector2 CameraOffset;
		public Viewport Viewport;

		public BaseCamera()
		{
			this.Screen = new Rectangle(0, 0, 0, 0);
			this.MapOffset = new Vector2(0, 0);
		}

		public BaseCamera(int X, int Y, int Width, int Height)
		{
			this.Screen = new Rectangle(X, Y, Width, Height);
			this.MapOffset = new Vector2(0, 0);

			this.Viewport.X = this.Screen.X;
			this.Viewport.Y = this.Screen.Y;
			this.Viewport.Width = this.Screen.Width;
			this.Viewport.Height = this.Screen.Height;

		}

		#region Public Methods/Effects
		public bool CheckVisible(Rectangle destRect)
		{
			if (destRect.X >= this.MapOffset.X - destRect.Width)
			{
				if(	destRect.Y >= this.MapOffset.Y - destRect.Height )
				{
					if (destRect.X <= this.Screen.Width )
					{
						if( destRect.Y <= this.Screen.Height)
							return true;
					}
			}	}

			return false;
		}

		public void CenterOriginOnPoint(Point Point)
		{
			this.MapOffset.X = Point.X * -1;
			this.MapOffset.Y = Point.Y * -1;
		}

		public virtual void CenterOnCharacter(BaseCharacter Char)
		{
			this.CenterOnPoint(Char.Location);
		}

		public void CenterOnPoint(Point Point)
		{
            this.MapOffset.X = Point.X * -1;
			this.MapOffset.X += (this.Screen.Width / 2);

            this.MapOffset.Y = Point.Y * -1;
			this.MapOffset.Y += (this.Screen.Height / 2);
		}

		public void Tween(Point startPoint, Point endPoint, float Milliseconds)
		{
			throw new NotSupportedException();
		}

		public void Quake(int Magnitude)
		{
			throw new NotSupportedException();
		}
		#endregion
	}
}
