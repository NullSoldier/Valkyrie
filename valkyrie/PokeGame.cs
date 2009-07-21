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
using ValkyrieLibrary.States;
using ValkyrieLibrary.Maps;
using ValkyrieLibrary.Events;

namespace ValkyrieLibrary
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PokeGame : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
		public Map testMap;
		public static SpriteFont font;

        public PokeGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            TileEngine.Initialize(this.Content, this.GraphicsDevice);
            TileEngine.Viewport = this.GraphicsDevice.Viewport;
            TileEngine.Camera = new PokeCamera(0, 0, 800, 600);
			TileEngine.CollisionManager = new PokeCollisionManager();
			TileEngine.TileSize = 32;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

			PokeGame.font = Content.Load<SpriteFont>("GameTextFont");

            TileEngine.ModuleManager.AddModule(new MenuModule(), "Menu");
            TileEngine.ModuleManager.AddModule(new GameModule(), "Game");

            TileEngine.Load(new FileInfo("Data/TileEngineConfig.xml"));
			TileEngine.WorldManager.Load(new FileInfo("Data/PokeWorld.xml"));
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        float deltaFPSTime = 0;


        protected override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedRealTime.TotalSeconds;

            float fps = 1 / elapsed;
            deltaFPSTime += elapsed;
            if (deltaFPSTime > 1)
            {
                Window.Title = "I am running at  <" + fps.ToString() + "> FPS";
                deltaFPSTime -= 1;
            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

			TileEngine.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
			TileEngine.Draw(spriteBatch, gameTime);
			 
            base.Draw(gameTime);
        }
    }
}
