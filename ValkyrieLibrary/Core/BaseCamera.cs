using System;
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

		public Point CameraOrigin
		{
			get { return new Point((int)(this.MapOffset.X * -1), (int)(this.MapOffset.Y * -1));	}
			set { this.MapOffset = new Vector2(value.X * -1, value.Y * -1);	}
		}

        public ScreenPoint Offset()
        {
            ScreenPoint sp = new ScreenPoint(0,0);
            sp.X = (int)TileEngine.Camera.MapOffset.X + (int)TileEngine.Camera.CameraOffset.X;
            sp.Y = (int)TileEngine.Camera.MapOffset.Y + (int)TileEngine.Camera.CameraOffset.Y;
            return sp;
        }

		#region Public Methods/Effects
        public bool CheckIsVisible(Rectangle rect)
		{
            Rectangle screen = new Rectangle(0,0,this.Screen.Width, this.Screen.Height);
            return (screen.Intersects(rect) == true);
		}


		public void CenterOriginOnPoint(Point Point)
		{
			this.MapOffset.X = Point.X * -1;
			this.MapOffset.Y = Point.Y * -1;
		}

		public void CenterOriginOnPoint(int x, int y)
		{
			this.CenterOriginOnPoint(new Point(x, y));
		}

		public virtual void CenterOnCharacter(BaseCharacter Char)
		{
			this.CenterOnPoint(Char.Location);
		}

		public void CenterOnPoint(ScreenPoint Point)
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

        public void Scale(double Scale)
        {
            this.Screen = new Rectangle(Screen.X, Screen.Y, (int)(Screen.Width * (1.0 / Scale)), (int)(Screen.Height * (1.0 / Scale)));
            this.MapOffset = new Vector2(0, 0);

            this.Viewport.X = this.Screen.X;
            this.Viewport.Y = this.Screen.Y;
            this.Viewport.Width = this.Screen.Width;
            this.Viewport.Height = this.Screen.Height;
        }
    }
}
