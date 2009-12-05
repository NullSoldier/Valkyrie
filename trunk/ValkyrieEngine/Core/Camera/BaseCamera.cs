using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Valkyrie.Library.Core;
using System.Threading;
using Valkyrie.Engine.Camera;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine.Maps;

namespace Valkyrie.Engine
{
	public class BaseCamera
		: IPositionable
	{
		#region Constructors

		public BaseCamera (BaseCamera camera)
		{
			this.SetCamera(camera);
		}

		public BaseCamera (Rectangle screen)
			 : this(screen.X, screen.Y, screen.Width, screen.Height)
		{
		}

		public BaseCamera (int x, int y, int width, int height)
		{
			this.screen = new Rectangle(x, y, width, height);
			this.viewport = new Viewport() { X = x, Y = y, Width = width, Height = height };
		}

		public BaseCamera (Viewport viewport)
		{
			this.screen = new Rectangle(viewport.X, viewport.Y, viewport.Width, viewport.Height);
			this.viewport = viewport;
		}

		#endregion

		#region Public Methods/Properties

		public Rectangle Screen { get { return this.screen; } }
		public Viewport Viewport { get { return this.viewport; } }
		public List<ICameraEffect> Effects { get { return this.effects; } }
		public Vector2 CameraOffset { get { return this.cameraoffset; } }

		public Vector2 MapOffset
		{
			get { return this.mapoffset; }
			set { this.mapoffset = value; }
		}

		public bool ManualControl
		{
			get { return this.manualcontrol; }
			set { this.manualcontrol = value; }
		}

		public event EventHandler<EffectFinishedEventArgs> EffectFinished
		{
			add { this.effectfinished += value; }
			remove { this.effectfinished -= value; }
		}

		public Point CameraOrigin
		{
			get { return new Point((int)(this.MapOffset.X * -1), (int)(this.MapOffset.Y * -1)); }
			set { this.MapOffset = new Vector2(value.X * -1, value.Y * -1); }
		}

		public ScreenPoint Offset
		{
			get
			{
				ScreenPoint sp = new ScreenPoint(0, 0);
				sp.X = (int)this.MapOffset.X + (int)this.CameraOffset.X;
				sp.Y = (int)this.MapOffset.Y + (int)this.CameraOffset.Y;
				return sp;
			}
		}

		public void Push()
		{
			this.camStack.Push(new BaseCamera(this));
		}

		public void Pop()
		{
			this.SetCamera(this.camStack.Pop());
		}

		public void SetCamera(BaseCamera cam)
		{
			this.screen = new Rectangle(cam.Screen.X, cam.Screen.Y, cam.Screen.Width, cam.Screen.Height);
			this.mapoffset = new Vector2(cam.MapOffset.X, cam.MapOffset.Y);
			this.cameraoffset = new Vector2(cam.CameraOffset.X, cam.CameraOffset.Y);

			this.viewport = new Viewport() { X = cam.screen.X, Y = cam.screen.Y, Width = cam.screen.Width, Height = cam.screen.Height};
		}

		public bool CheckIsVisible(Rectangle rect)
		{
			return (this.screen.Intersects(rect) == true);
		}

		public void ResizeScreen (Rectangle rectangle)
		{
			this.ResizeScreen(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		}

		public void ResizeScreen (int x, int y, int width, int height)
		{
			this.screen = new Rectangle(x, y, width, height);
			this.viewport = new Viewport() { X = x, Y = y, Width = width, Height = height };
		}

		public void CenterOriginOnPoint(Point Point)
		{
			this.MapOffset = new Vector2(Point.X * -1, Point.Y * -1);
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
			int prepvaluex = Point.X * -1;
			prepvaluex += (this.Screen.Width / 2);

			int prepvaluey = Point.Y * -1;
			prepvaluey += (this.Screen.Height / 2);

			Vector2 loc = new Vector2(prepvaluex, prepvaluey);
			this.MapOffset = loc;
		}

		public void Update(GameTime gameTime)
		{
			List<ICameraEffect> removed = new List<ICameraEffect>();

			lock (this.Effects)
			{
				foreach (var effect in this.Effects)
				{
					effect.Update(gameTime);
					effect.Draw(this);

					if (effect.Finished)
						removed.Add(effect);
				}

				foreach (var effect in removed)
				{
					var handler = this.effectfinished;
					if (handler != null)
						handler(this, new EffectFinishedEventArgs(effect));

					this.Effects.Remove(effect);
				}
			}
		}

		public void Scale (double scale)
		{
			this.Scale(scale, scale);
		}

		public void Scale(double x, double y)
        {
			this.ResizeScreen(new Rectangle(Screen.X, Screen.Y, (int)(Screen.Width * (1.0 / x)), (int)(Screen.Height * (1.0 / y))));
		}

		#endregion

		private Rectangle screen = Rectangle.Empty;
		private Vector2 mapoffset = Vector2.Zero;
		private Vector2 cameraoffset = Vector2.Zero;
		private Viewport viewport; // eh?
		private bool manualcontrol = true;
		private string worldname = string.Empty;

		private List<ICameraEffect> effects = new List<ICameraEffect>();
		private event EventHandler<EffectFinishedEventArgs> effectfinished;
		private Stack<BaseCamera> camStack = new Stack<BaseCamera>();

		#region IPositionable Members

		public MapPoint LocalTileLocation
		{
			get { throw new NotImplementedException(); }
		}

		public MapPoint GlobalTileLocation
		{
			get { return this.Location.ToMapPoint(); }
		}

		public ScreenPoint Location
		{
			get
			{
				return new ScreenPoint(this.CameraOrigin.X, this.CameraOrigin.Y);
			}
			set
			{
				this.CameraOrigin = value.ToPoint();
			}
		}

		public string WorldName
		{
			get { return this.worldname; }
			set { this.worldname = value; }
		}

		public MapHeader CurrentMap
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		#endregion
	}
}
