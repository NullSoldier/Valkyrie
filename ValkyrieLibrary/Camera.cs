﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ValkyrieLibrary.Player;
using Microsoft.Xna.Framework.Graphics;

namespace ValkyrieLibrary
{
	public class Camera
	{
		public Rectangle Screen;
		public Vector2 MapOffset;
		public Vector2 CameraOffset;
		public Viewport Viewport;

		public Camera()
		{
			this.Screen = new Rectangle(0, 0, 0, 0);
			this.MapOffset = new Vector2(0, 0);
			this.CameraOffset = new Vector2(0, 0);
		}

		public Camera(int X, int Y, int Width, int Height)
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

		public void CenterOnCharacter(BaseCharacter Char)
		{

		}

		public void CenterOnPoint(Vector2 Point)
		{
            this.MapOffset.X = Point.X * -1;
			this.MapOffset.X += (this.Screen.Width / 2);

            this.MapOffset.Y = Point.Y * -1;
			this.MapOffset.Y += (this.Screen.Height / 2);
		}

		public void Tween(Point startPoint, Point endPoint, float Milliseconds)
		{
		}

		public void Quake(int Magnitude)
		{
		}
		#endregion


	}
}