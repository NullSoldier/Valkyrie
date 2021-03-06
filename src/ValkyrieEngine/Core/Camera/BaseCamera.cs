﻿using System;
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

		public BaseCamera(Rectangle screen)
			 : this(screen.X, screen.Y, screen.Width, screen.Height) { }

		public BaseCamera (int x, int y, int width, int height)
		{
			this.screen = new Rectangle(x, y, width, height);
            this.halfscreen = new ScreenPoint(screen.Width / 2, screen.Height / 2);

            this.Viewport = new Viewport()
            {
                X = x,
                Y = y,
                Width = width,
                Height = height
            };
		}

		#endregion

		#region Public Methods/Properties

		public Rectangle Screen
        {
            get { return this.screen; }
        }

        public ScreenPoint Origin
        {
			get { return mapoffset * -1; }
            set { mapoffset = value * -1; }
        }

        public ScreenPoint Offset
        {
            get
            {
				return new ScreenPoint(
					(int)mapoffset.X,
					(int)mapoffset.Y);
            }
        }

		public ScreenPoint Center
		{
			get { return this.Origin + this.halfscreen; }
		}

        public Viewport Viewport
        {
            get { return viewport;}
            set { viewport = value; }
        }

		public RenderTarget2D Buffer
		{
			get { return buffer; }
			set { buffer = value; }
		}

        public Vector2 CurrentSize
        {
            get { return Vector2.Multiply(new Vector2(screen.Width, screen.Height), 1 / zoom); }
        }

        #region Camera Effects

        public List<ICameraEffect> Effects
        {
            get { return this.effects; }
        }

        public event EventHandler<EffectFinishedEventArgs> EffectFinished
        {
            add { this.effectfinished += value; }
            remove { this.effectfinished -= value; }
        }

        #endregion

        public bool ManualControl
		{
			get { return manualcontrol; }
			set { manualcontrol = value; }
		}

        #region Stack Management

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
            this.halfscreen = new ScreenPoint(screen.Width / 2, screen.Height / 2);

            this.mapoffset = new ScreenPoint(cam.Location.X, cam.Location.Y);
		}

        #endregion

		public void Load(GraphicsDevice device)
		{
			this.device = device;

			this.buffer = new RenderTarget2D(device, Screen.Width, Screen.Height);
		}

		public bool CheckIsVisible(Rectangle rect)
		{
			var extrapolate = new Rectangle (Origin.IntX, Origin.IntY, (int)(Screen.Width / Zoom), (int)(Screen.Height / Zoom));

			return extrapolate.Intersects(rect);

            //return (new Rectangle(Origin.IntX, Origin.IntY, Screen.Width, Screen.Height).Intersects(rect) == true);
		}

		public void ResizeScreen (Rectangle rectangle)
		{
			this.ResizeScreen(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		}

		public void ResizeScreen (int x, int y, int width, int height)
		{
			this.screen = new Rectangle(x, y, width, height);
            this.halfscreen = new ScreenPoint(screen.Width / 2, screen.Height / 2);

			this.Viewport = new Viewport()
			{
				X = x,
				Y = y,
				Width=width,
				Height=height
			};

			this.buffer = new RenderTarget2D(device, Screen.Width, Screen.Height);
        }

        #region Center Methods

        public void CenterOriginOnPoint(ScreenPoint point)
		{
            this.Origin = new ScreenPoint(point.X, point.Y);
		}

		public void CenterOriginOnPoint(int x, int y)
		{
            this.CenterOriginOnPoint(new ScreenPoint(x, y));
		}

		public virtual void CenterOnCharacter(BaseCharacter Char)
		{
			this.worldname = Char.WorldName;
			this.CenterOnPoint(new ScreenPoint(
										Char.Location.IntX + (Char.CurrentAnimation.FrameRectangle.Width / 2),
										Char.Location.IntY + (Char.CurrentAnimation.FrameRectangle.Height / 2) + 8));
		}

		public void CenterOnPoint(ScreenPoint point)
		{
			this.Origin = point - halfscreen;
        }

        #endregion

        #region Matrix Methods/Properties

        public float Rotation
        {
            get { return this.rotate; }
        }

        public float Zoom
        {
            get { return this.zoom; }
        }

        public void Scale(float scale)
        {
            this.zoom = scale;
        }

        public void Rotate(float radians)
        {
            this.rotate = radians;
        }

        public Matrix TransformMatrix
        {
            get
            {
                return
                    Matrix.CreateTranslation(new Vector3(mapoffset.X, mapoffset.Y, 0)) *
                    Matrix.CreateRotationZ(this.rotate) *
                    Matrix.CreateScale(this.zoom);
            }
        }

        #endregion

		#endregion

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

		private Rectangle screen = Rectangle.Empty;
        private ScreenPoint mapoffset = ScreenPoint.Zero;
        private ScreenPoint cameraoffset = ScreenPoint.Zero; // Uneeded
        private ScreenPoint halfscreen = ScreenPoint.Zero;
        private Viewport viewport;
		private RenderTarget2D buffer;
		private GraphicsDevice device;
		
        private bool manualcontrol = true;
		private string worldname = string.Empty;

        private float zoom = 1f;
        private float rotate = 0f;

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
            get { return this.Origin;  }
			set { throw new NotImplementedException(); }
		}

		public string WorldName
		{
			get { return this.worldname; }
			set { this.worldname = value; }
		}

		public MapHeader CurrentMap
		{
			get;
			set;
		}

		#endregion
	}
}
