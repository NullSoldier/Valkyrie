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
using ValkyrieLibrary.Player;
using ValkyrieLibrary;
using ValkyrieLibrary.States;

namespace valkyrie
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class RPGGame : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
		public Map testMap;
		public static SpriteFont font;

        public RPGGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            TileEngine.Initialize(this.Content, this.GraphicsDevice);
            TileEngine.Viewport = this.GraphicsDevice.Viewport;
            TileEngine.Camera = new Camera(0, 0, 800, 600);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

			RPGGame.font = Content.Load<SpriteFont>("GameTextFont");

            TileEngine.ModuleManager.AddModule(new MenuModule(), "Menu");
            TileEngine.ModuleManager.AddModule(new GameModule(), "Game");

            TileEngine.Load(new FileInfo("Data/TileEngineConfig.xml"));
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

			TileEngine.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
			TileEngine.DrawScreen(spriteBatch, gameTime);
			 
            base.Draw(gameTime);
        }
    }
}
