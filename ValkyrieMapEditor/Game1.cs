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
using valkyrie.Core;
using System.IO;
using ValkyrieLibrary;
using System.Windows.Forms;

namespace ValkyrieMapEditor
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
        GraphicsDeviceManager graphics2;
		SpriteBatch spriteBatch;
		private Map testMap;
		private IntPtr drawSurface;
        private IntPtr drawTilesSurface;
		private Texture2D CollisionSprite;
		private Texture2D SelectionSprite;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		
		public Game1(IntPtr drawSurface, IntPtr drawTilesSurface)
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			this.drawSurface = drawSurface;
            this.drawTilesSurface = drawTilesSurface;
			graphics.PreparingDeviceSettings +=
				new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);


            System.Windows.Forms.Control.FromHandle((this.Window.Handle)).VisibleChanged +=
                new EventHandler(Game1_VisibleChanged);
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
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// Custom loading
			TileEngine.Initialize(this.Content, this.GraphicsDevice);
			TileEngine.Viewport = this.GraphicsDevice.Viewport;
            TileEngine.Camera = new Camera(0, 0, 800, 600);
            TileEngine.Load(new FileInfo("Data/TileEngineConfig.xml"));

			this.CollisionSprite = Texture2D.FromFile(this.GraphicsDevice, "Graphics/EditorCollision.png");
			this.SelectionSprite = Texture2D.FromFile(this.GraphicsDevice, "Graphics/EditorSelection.png");
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
			if (!MapManager.IgnoreInput)
			{

				// TODO: Add your update logic here
				KeyboardState keyState = Keyboard.GetState();

				if (TileEngine.IsMapLoaded && MapManager.CurrentLayer != MapLayer.CollisionLayer)
				{
					var mouseState = Mouse.GetState();

					if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
						mouseState.X > 0 && mouseState.Y > 0)
					{
						Point tileLocation = new Point(mouseState.X / 32, mouseState.Y / 32);

						for (int y = 0; y <= MapManager.SelectedTilesRect.Height; y++)
						{
							for (int x = 0; x <= MapManager.SelectedTilesRect.Width; x++)
							{
								Point tilesheetPoint = new Point(MapManager.SelectedTilesRect.X + x, MapManager.SelectedTilesRect.Y + y);
								Point point = new Point(tileLocation.X + x, tileLocation.Y + y);

								if (TileEngine.Map.TilePointInMap(point))
									TileEngine.Map.SetData(MapManager.CurrentLayer, point, TileEngine.Map.GetTileSetValue(tilesheetPoint));
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
			}
		}

        public void SurfaceClicked(object sender, SurfaceClickedEventArgs e)
        {
           if (TileEngine.Map != null)
            {
                if (e.Button == MouseButtons.Left )
                {
					Point point = new Point(e.Location.X / 32, e.Location.Y / 32);

					if( TileEngine.Map.TilePointInMap(point) )
						TileEngine.Map.SetData(MapManager.CurrentLayer, point, 1);
                }
            }
        }

        public bool MapChanged { get; set; }

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Gray);

            if (TileEngine.Map != null)
            {
                TileEngine.DrawBaseLayer(this.spriteBatch);
                TileEngine.DrawMiddleLayer(this.spriteBatch);
                TileEngine.DrawTopLayer(this.spriteBatch);
            }

			// TODO: Add your drawing code here
			if (TileEngine.IsMapLoaded)
			{
				// Draw the current location of the mouse
				var mouseState = Mouse.GetState();
				if (mouseState.X > 0 && mouseState.Y > 0)
				{
					Point tileLocation = new Point(mouseState.X / 32, mouseState.Y / 32);

					Vector2 currentLocation = new Vector2(tileLocation.X * 32, tileLocation.Y * 32);


					this.spriteBatch.Begin();
					this.spriteBatch.Draw(this.SelectionSprite, currentLocation, Color.White);
					this.spriteBatch.End();
				}

				// Render Collision Layer
				for (int y = 0; y < TileEngine.Map.MapSize.Y; y++)
				{
					for (int x = 0; x < TileEngine.Map.MapSize.X; x++)
					{
						int value = TileEngine.Map.GetCollisionLayerValue(new Point(x, y));

						if (value == -1)
							continue;

						Rectangle destRectangle = new Rectangle(0, 0, TileEngine.Map.TileSize.X, TileEngine.Map.TileSize.Y);
						destRectangle.X = (int)TileEngine.Camera.MapOffset.X + (int)TileEngine.Camera.CameraOffset.X + (x * TileEngine.Map.TileSize.X);
						destRectangle.Y = (int)TileEngine.Camera.MapOffset.Y + (int)TileEngine.Camera.CameraOffset.Y + (y * TileEngine.Map.TileSize.Y);

						this.spriteBatch.Begin();
						this.spriteBatch.Draw(this.CollisionSprite, destRectangle, new Rectangle(0, 0, this.CollisionSprite.Width, this.CollisionSprite.Height), Color.White);
						this.spriteBatch.End();
					}
				}
			}

			base.Draw(gameTime);
		}
	}
}

