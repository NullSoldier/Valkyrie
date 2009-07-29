using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using ValkyrieLibrary;
using ValkyrieLibrary.Maps;
using ValkyrieLibrary.Core;
using ValkyrieMapEditor.Core;
using System.Reflection;
using ValkyrieLibrary.Events;


namespace ValkyrieMapEditor
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class EditorXNA : Microsoft.Xna.Framework.Game
	{
        public static SpriteFont font;
        public static GraphicsDevice graphicsDevice = null;
		public static GraphicsDeviceManager graphics = null;

        SpriteBatch spriteBatch;

		//private Map testMap;
		private IntPtr drawSurface;
        private IntPtr drawTilesSurface;
		private Texture2D SelectionSprite;

        private Dictionary<ComponentID, IEditorComponent> ComponentList;
        private IEditorComponent curComponent = null;
        public RenderComponent Render = null;

        float deltaFPSTime = 0;

        public IEditorComponent CurComponent
        {
            get
            {
                if (this.curComponent == null)
                    this.curComponent = this.ComponentList[ComponentID.Draw];

                return curComponent;
            }
        }

        public bool MapChanged { get; set; }

		public EditorXNA()
		{
            Init();

			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		
		public EditorXNA(IntPtr drawSurface, IntPtr drawTilesSurface)
		{
            Init();

			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			this.drawSurface = drawSurface;
            this.drawTilesSurface = drawTilesSurface;
			graphics.PreparingDeviceSettings +=
				new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);

            System.Windows.Forms.Control.FromHandle((this.Window.Handle)).VisibleChanged += new EventHandler(Game1_VisibleChanged);         
		}

        private void Init()
        {
			ComponentList = new Dictionary<ComponentID, IEditorComponent>();
			this.Render = new RenderComponent();

			this.ComponentList = new Dictionary<ComponentID, IEditorComponent>();
			this.ComponentList.Add(ComponentID.Draw, new DrawComponent());
			this.ComponentList.Add(ComponentID.Events, new EventsComponent());
			this.ComponentList.Add(ComponentID.Collsion, new CollisionComponent());
        }

		
        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
			e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = drawSurface;
        }

        private void Game1_VisibleChanged(object sender, EventArgs e)
        {
			if (System.Windows.Forms.Control.FromHandle((this.Window.Handle)).Visible == true)
				System.Windows.Forms.Control.FromHandle((this.Window.Handle)).Visible = false;
        }

		protected override void Initialize()
		{
			base.Initialize();
			Mouse.WindowHandle = this.drawSurface;
		}

		protected override void LoadContent()
		{
            font = Content.Load<SpriteFont>("GameTextFont");

            graphicsDevice = this.GraphicsDevice;
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// Custom loading
			TileEngine.Initialize(this.Content, this.GraphicsDevice);
			TileEngine.EventManager.LoadEventTypesFromAssemblies(new Assembly[] { Assembly.GetEntryAssembly(), Assembly.LoadFile(frmMain.ValkyrieGameInstallationAssemblyPath), Assembly.LoadWithPartialName("ValkyrieLibrary") });
			TileEngine.Viewport = this.GraphicsDevice.Viewport;
            TileEngine.Camera = new BaseCamera(0, 0, 800, 600);
            TileEngine.Load(new FileInfo("Data/TileEngineConfig.xml"));
			TileEngine.Camera.CenterOriginOnPoint(new Point(0, 0));
			TileEngine.Player = new MapEditorPlayer();
			
			this.SelectionSprite = Texture2D.FromFile(this.GraphicsDevice, "Graphics/EditorSelection.png");

            this.Render.LoadContent(this.GraphicsDevice);
            foreach (var c in this.ComponentList)
            {
                c.Value.LoadContent(this.GraphicsDevice);
            }
		}

		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

        public void EnlistEvents(PictureBox pictureBox)
        {
            pictureBox.MouseDown += this.MouseDown;
            pictureBox.MouseUp += this.MouseUp;
            pictureBox.MouseMove += this.MouseMove;
            pictureBox.MouseClick += this.MouseClicked;
        }

        public void SwitchTo(ComponentID component)
        {
            if (this.ComponentList.Keys.Contains(component))
                curComponent = this.ComponentList[component];
        }

        #region Callbacks


        protected override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedRealTime.TotalSeconds;

            float fps = 1 / elapsed;
            deltaFPSTime += elapsed;
            if (deltaFPSTime > 1)
            {
                Window.Title = "MapEditor  [" + fps.ToString() + " FPS]";
                deltaFPSTime -= 1;
            }


            this.Render.Update(gameTime);
            this.CurComponent.Update(gameTime);

			base.Update(gameTime);
		}

        public void Resized(object sender, ScreenResizedEventArgs e)
		{
            this.Render.OnSizeChanged(sender, e);
            this.CurComponent.OnSizeChanged(sender, e);
		}

        public void Scrolled(object sender, ScrollEventArgs e)
		{
            this.Render.OnScrolled(sender, e);
            this.CurComponent.OnScrolled(sender, e);
		}

        public void MouseDown(object sender, MouseEventArgs e)
        {
            this.Render.OnMouseDown(sender, e);
            this.CurComponent.OnMouseDown(sender, e);
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            this.Render.OnMouseMove(sender, e);
            this.CurComponent.OnMouseMove(sender, e);
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
            this.Render.OnMouseUp(sender, e);
            this.CurComponent.OnMouseUp(sender, e);
        }

        public void MouseClicked(object sender, MouseEventArgs e)
        {
            this.Render.OnMouseClicked(sender, e);
            this.CurComponent.OnMouseClicked(sender, e);
        }


		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Gray);

            this.spriteBatch.Begin();
            this.Render.Draw(this.spriteBatch);
            this.CurComponent.Draw(this.spriteBatch);
            this.spriteBatch.End();

			base.Draw(gameTime);
        }

        #endregion

        static public Texture2D CreateSelectRectangle(int width, int height)
        {
            int limit = 4;

            if (width <= limit * 2 || height <= limit * 2)
                return null;

            // create the rectangle texture, ,but it will have no color! lets fix that
            Texture2D rectangleTexture = new Texture2D(EditorXNA.graphicsDevice, width, height, 1, TextureUsage.None, SurfaceFormat.Color);
           
            Color[] color = new Color[width * height];//set the color to the amount of pixels

            //loop through all the colors setting them to whatever values we want
            for (int y = 1; y < height-1; y++)
            {
                for (int x = 1; x < width-1; x++)
                {
                    if ((x < limit) || (x > width - limit - 2) || (y < limit) || (y > height - limit - 2))
                    {
                        color[x + (y * width)] = new Color(255, 255, 255, 255);
                    }
                    else
                    {
                        color[x + (y * width)] = new Color(0, 0, 0, 0);
                    }
                }
            }

            //outer four
            for (int y = 0; y < height; y++)
                color[0 + (y * width)] = new Color(0, 0, 0, 255);

            for (int y = 0; y < height; y++)
                color[width - 2 + (y * width)] = new Color(0, 0, 0, 255);

            for (int x = 0; x < width; x++)
                color[x + (0 * width)] = new Color(0, 0, 0, 255);

            for (int x = 0; x < width; x++)
                color[x + ((height - 2) * width)] = new Color(0, 0, 0, 255);

            //inner four
            for (int y = limit; y < (height - limit -1); y++)
                color[limit + (y * width)] = new Color(0, 0, 0, 255);

            for (int y = limit; y < (height - limit - 1); y++)
                color[width - limit - 2 + (y * width)] = new Color(0, 0, 0, 255);

            for (int x = limit; x < (width - limit - 1); x++)
                color[x + (limit * width)] = new Color(0, 0, 0, 255);

            for (int x = limit; x < (width - limit - 1); x++)
                color[x + ((height - limit -2) * width)] = new Color(0, 0, 0, 255);

            rectangleTexture.SetData(color);//set the color data on the texture
            return rectangleTexture;
        }
    }

    public enum ComponentID
    {
        Draw,
        Events,
        Collsion,
    };
}
