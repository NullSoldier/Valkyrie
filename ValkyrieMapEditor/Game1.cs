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
			// TODO: Add your update logic here
			KeyboardState keyState = Keyboard.GetState();

			base.Update(gameTime);
		}

		public void SurfaceSizeChanged(object sender, ValkyrieMapEditor.frmMain.ScreenResizedEventArgs e)
		{
			graphics.PreferredBackBufferWidth = e.Width;
			graphics.PreferredBackBufferHeight = e.Height;
			graphics.IsFullScreen = false;
			graphics.ApplyChanges();

			TileEngine.Camera.Screen.Width = e.Width;
			TileEngine.Camera.Screen.Height = e.Height;
		}

        public void SurfaceClicked(object sender, ValkyrieMapEditor.frmMain.SurfaceClickedEventArgs e)
        {
            if (TileEngine.Map != null)
            {
                if (e.Button == MouseButtons.Left )
                {
                    int tileX = (e.Location.X / 32);
                    int tileY = (e.Location.Y / 32);

                    TileEngine.Map.SetData(MapManager.CurrentLayer, new Point(tileX, tileY), MapManager.CurrentTile);
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

			base.Draw(gameTime);
		}
	}
}

