using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using ValkyrieLibrary.Core;
using System.IO;
using ValkyrieLibrary;
using System.Windows.Forms;
using ValkyrieLibrary.Maps;
using ValkyrieMapEditor.Core;
using System.Diagnostics;

namespace ValkyrieMapEditor
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class EditorXNA : Microsoft.Xna.Framework.Game
	{
        public static GraphicsDevice graphicsDevice = null;
        public static SpriteFont font;

		GraphicsDeviceManager graphics;
        GraphicsDeviceManager graphics2;
        SpriteBatch spriteBatch;

		private Map testMap;
		private IntPtr drawSurface;
        private IntPtr drawTilesSurface;
		private Texture2D CollisionSprite;
		private Texture2D SelectionSprite;
        private MapEditorEvents MapEventHandler = new MapEditorEvents();

		public EditorXNA()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		
		public EditorXNA(IntPtr drawSurface, IntPtr drawTilesSurface)
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			this.drawSurface = drawSurface;
            this.drawTilesSurface = drawTilesSurface;
			graphics.PreparingDeviceSettings +=
				new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);


            System.Windows.Forms.Control.FromHandle((this.Window.Handle)).VisibleChanged += new EventHandler(Game1_VisibleChanged);
		}

		
        /// <summary>
        /// Event capturing the construction of a draw surface and makes sure this gets redirected to
        /// a predesignated drawsurface marked by pointer drawSurface
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
			e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = drawSurface;
        }

        /// <summary>
        /// Occurs when the original gamewindows' visibility changes and makes sure it stays invisible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Game1_VisibleChanged(object sender, EventArgs e)
        {
			if (System.Windows.Forms.Control.FromHandle((this.Window.Handle)).Visible == true)
				System.Windows.Forms.Control.FromHandle((this.Window.Handle)).Visible = false;
        }
 

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			base.Initialize();
			Mouse.WindowHandle = this.drawSurface;
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
            font = Content.Load<SpriteFont>("GameTextFont");

            graphicsDevice = this.GraphicsDevice;

			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// Custom loading
			TileEngine.Initialize(this.Content, this.GraphicsDevice);
			TileEngine.Viewport = this.GraphicsDevice.Viewport;
            TileEngine.Camera = new BaseCamera(0, 0, 800, 600);
            TileEngine.Load(new FileInfo("Data/TileEngineConfig.xml"));
			TileEngine.Camera.CenterOriginOnPoint(new Point(0, 0));

			this.CollisionSprite = Texture2D.FromFile(this.GraphicsDevice, "Graphics/EditorCollision.png");
			this.SelectionSprite = Texture2D.FromFile(this.GraphicsDevice, "Graphics/EditorSelection.png");

            MapEventHandler.LoadContent(this.GraphicsDevice);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
            if (MapEventHandler.Enabled == true)
                return;

			if (!MapEditorManager.IgnoreInput)
			{
				// TODO: Add your update logic here
				KeyboardState keyState = Keyboard.GetState();

				if (TileEngine.IsMapLoaded && MapEditorManager.CurrentLayer != MapLayer.CollisionLayer)
				{
					var mouseState = Mouse.GetState();

					if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && mouseState.X > 0 && mouseState.Y > 0)
					{
						Point tileLocation = new Point((mouseState.X - (int)TileEngine.Camera.MapOffset.X) / 32, (mouseState.Y - (int)TileEngine.Camera.MapOffset.Y) / 32);

                        if (TileEngine.CurrentMapChunk.TilePointInMapLocal(tileLocation))
                        {
                            for (int y = 0; y <= MapEditorManager.SelectedTilesRect.Height; y++)
                            {
                                for (int x = 0; x <= MapEditorManager.SelectedTilesRect.Width; x++)
                                {
                                    Point tilesheetPoint = new Point(MapEditorManager.SelectedTilesRect.X + x, MapEditorManager.SelectedTilesRect.Y + y);
                                    Point point = new Point(tileLocation.X + x, tileLocation.Y + y);

                                    if (TileEngine.CurrentMapChunk.TilePointInMapLocal(point))
                                        TileEngine.CurrentMapChunk.SetData(MapEditorManager.CurrentLayer, point, TileEngine.CurrentMapChunk.GetTileSetValue(tilesheetPoint));
                                }
                            }
                        }
					}
				}
			}

			base.Update(gameTime);
		}

		public void SurfaceSizeChanged(object sender, ScreenResizedEventArgs e)
		{
			graphics.PreferredBackBufferWidth = e.Width;
			graphics.PreferredBackBufferHeight = e.Height;
			graphics.IsFullScreen = false;
			graphics.ApplyChanges();

			if (TileEngine.Camera != null)
			{
				TileEngine.Camera.Screen.Width = e.Width;
				TileEngine.Camera.Screen.Height = e.Height;

                if (TileEngine.IsMapLoaded)
                {
                    // Move the X origin
                    int DisplayedWidth = (TileEngine.CurrentMapChunk.MapSize.X * TileEngine.CurrentMapChunk.TileSize.X) + (int)TileEngine.Camera.MapOffset.X;
                    if (DisplayedWidth < e.Width)
                    {
                        if (TileEngine.CurrentMapChunk.MapSize.X * TileEngine.CurrentMapChunk.TileSize.X < e.Width)
                            TileEngine.Camera.CenterOriginOnPoint(0, (int)(TileEngine.Camera.MapOffset.Y * -1));
                        else
                        {
                            int newOffset = (e.Width - DisplayedWidth);
                            TileEngine.Camera.CenterOriginOnPoint((int)(TileEngine.Camera.MapOffset.X * -1) - newOffset, (int)(TileEngine.Camera.MapOffset.Y * -1));
                        }
                    }

                    // Move the Y origin
                    int DisplayedHeight = (TileEngine.CurrentMapChunk.MapSize.Y * TileEngine.CurrentMapChunk.TileSize.Y) + (int)TileEngine.Camera.MapOffset.Y;
                    if (DisplayedHeight < e.Height)
                    {
                        if (TileEngine.CurrentMapChunk.MapSize.Y * TileEngine.CurrentMapChunk.TileSize.Y < e.Height)
                            TileEngine.Camera.CenterOriginOnPoint((int)(TileEngine.Camera.MapOffset.X * -1), 0);
                        else
                        {
                            int newOffset = (e.Height - DisplayedHeight);
                            TileEngine.Camera.CenterOriginOnPoint((int)(TileEngine.Camera.MapOffset.X * -1), (int)(TileEngine.Camera.MapOffset.Y * -1) - newOffset);
                        }
                    }
                }

			}

			
		}

		public void ScrolledMap(object sender, ScrollEventArgs e)
		{
            int dif = (e.NewValue - e.OldValue);

			if (e.Type == ScrollEventType.EndScroll) 
                return;

			int x = (int)(TileEngine.Camera.MapOffset.X);
			int y = (int)(TileEngine.Camera.MapOffset.Y);

			if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
                y -= dif*TileEngine.CurrentMapChunk.TileSize.Y;
			else
                x -= dif*TileEngine.CurrentMapChunk.TileSize.X;

            TileEngine.Camera.CenterOriginOnPoint(new Point(x, y));
		}

        public void SurfaceClicked(object sender, SurfaceClickedEventArgs e)
        {
            if (MapEventHandler.Enabled == false)
            {
                if (TileEngine.IsMapLoaded && MapEditorManager.CurrentLayer == MapLayer.CollisionLayer)
                {
                    Point point = new Point((e.Location.X - (int)TileEngine.Camera.MapOffset.X) / 32, (e.Location.Y - (int)TileEngine.Camera.MapOffset.Y) / 32);

                    if (e.Button == MouseButtons.Left)
                        TileEngine.CurrentMapChunk.SetData(MapEditorManager.CurrentLayer, point, 1);
                }
            }
        }

        public void MouseDown(object sender, MouseEventArgs ev)
        {
            if (MapEventHandler.Enabled == true)
                MapEventHandler.MouseDown(sender, ev);
        }

        public void MouseMove(object sender, MouseEventArgs ev)
        {
            if (MapEventHandler.Enabled == true)
                MapEventHandler.MouseMove(sender, ev);
        }

        public void MouseUp(object sender, MouseEventArgs ev)
        {
            if (MapEventHandler.Enabled == true)
                MapEventHandler.MouseUp(sender, ev);
        }

        public void MouseClicked(object sender, MouseEventArgs ev)
        {
            if (MapEventHandler.Enabled == true)
                MapEventHandler.MouseClicked(sender, ev);
        }

        public bool MapChanged { get; set; }

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Gray);

			if (TileEngine.IsMapLoaded)
            {
                TileEngine.DrawBaseLayer(this.spriteBatch);
                TileEngine.DrawMiddleLayer(this.spriteBatch);
                TileEngine.DrawTopLayer(this.spriteBatch);
            }

			// TODO: Add your drawing code here
			if (TileEngine.IsMapLoaded)
			{
                this.spriteBatch.Begin();

				// Render Collision Layer
				for (int y = 0; y < TileEngine.CurrentMapChunk.MapSize.Y; y++)
				{
					for (int x = 0; x < TileEngine.CurrentMapChunk.MapSize.X; x++)
					{
						int value = TileEngine.CurrentMapChunk.GetCollisionLayerValue(new Point(x, y));

						if (value == -1)
							continue;

						Rectangle destRectangle = new Rectangle(0, 0, TileEngine.CurrentMapChunk.TileSize.X, TileEngine.CurrentMapChunk.TileSize.Y);
						destRectangle.X = (int)TileEngine.Camera.MapOffset.X + (int)TileEngine.Camera.CameraOffset.X + (x * TileEngine.CurrentMapChunk.TileSize.X);
						destRectangle.Y = (int)TileEngine.Camera.MapOffset.Y + (int)TileEngine.Camera.CameraOffset.Y + (y * TileEngine.CurrentMapChunk.TileSize.Y);

						this.spriteBatch.Draw(this.CollisionSprite, destRectangle, new Rectangle(0, 0, this.CollisionSprite.Width, this.CollisionSprite.Height), Color.White);
					}
                }

                if (MapEventHandler.Enabled == true)
                {
                    MapEventHandler.Draw(this.spriteBatch);
                }
                else
                {
                    // Draw the current location of the mouse
                    var mouseState = Mouse.GetState();
                    if (mouseState.X > 0 && mouseState.Y > 0)
                    {
                        Point tileLocation = new Point(mouseState.X / 32, mouseState.Y / 32);
                        Vector2 cLoc = new Vector2(tileLocation.X * 32, tileLocation.Y * 32);
                        Rectangle pos = new Rectangle((int)cLoc.X, (int)cLoc.Y, MapEditorManager.SelectedTilesRect.Width * 32+32, MapEditorManager.SelectedTilesRect.Height * 32+32);

                        Texture2D text = CreateSelectRectangle(pos.Width, pos.Height);

                        if (text == null)
                            text = this.SelectionSprite;

                        this.spriteBatch.Draw(text, pos, new Rectangle(0,0, text.Width, text.Height), Color.White); 
                    }
                }

                this.spriteBatch.End();
			}

			base.Draw(gameTime);
		}


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
}
