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
using ValkyrieLibrary.Maps;
using ValkyrieLibrary.Events;
using Valkyrie.Core;
using Valkyrie.States;
using System.Reflection;
using Gablarski.Network;
using System.Net;
using Gablarski;
using ValkyrieLibrary.Core.Messages;
using ValkyrieLibrary.Network;
using Valkyrie.Characters;
using ValkyrieServerLibrary.Network.Messages.Valkyrie;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.States;
using System.Runtime.InteropServices;

namespace ValkyrieLibrary
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
	
    public class PokeGame
		: Game
    {
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
		public static SpriteFont font;
		private float deltaFPSTime = 0;
		public bool ExitingGame = false;

        public PokeGame()
        {	
            graphics = new GraphicsDeviceManager(this);
			graphics.ApplyChanges();

            Content.RootDirectory = "Content";			
        }

        protected override void Initialize()
        {
            TileEngine.Initialize(this.Content, this.GraphicsDevice);
			//TileEngine.EventManager.LoadEventTypesFromAssemblies( new Assembly[] {Assembly.GetEntryAssembly(), Assembly.LoadWithPartialName("ValkyrieLibrary")});
            TileEngine.Viewport = this.GraphicsDevice.Viewport;
            TileEngine.Camera = new PokeCamera(0, 0, 800, 600);
			TileEngine.CollisionManager = new PokeCollisionManager();
			TileEngine.MovementManager = new PokeMovementManagerNew();
			TileEngine.EventManager = new MapEventManager(new Assembly[] { Assembly.GetEntryAssembly(), Assembly.Load("ValkyrieLibrary") });
			TileEngine.TileSize = 32;

			TileEngine.NetworkManager.Disconnected += NetworkDisconnected;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

			PokeGame.font = Content.Load<SpriteFont>("GameTextFont");

            TileEngine.ModuleManager.AddModule(new MenuModule(), "Menu");
            TileEngine.ModuleManager.AddModule(new GameModule(), "Game");
			TileEngine.ModuleManager.AddModule(new LoginModule(), "Login");

            TileEngine.Load(new FileInfo("Data/TileEngineConfig.xml"));
			TileEngine.WorldManager.Load(new FileInfo("Data/PokeWorld.xml"));
        }

        protected override void UnloadContent()
        {
			this.ExitingGame = true;
			TileEngine.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
			TileEngine.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
			float elapsed = (float)gameTime.ElapsedRealTime.TotalSeconds;

			float fps = 1 / elapsed;
			deltaFPSTime += elapsed;
			if (deltaFPSTime > 1)
			{
				Window.Title = "PokeGame [" + fps.ToString() + " FPS]";
				deltaFPSTime -= 1;
			}

			TileEngine.Draw(spriteBatch, gameTime);

            base.Draw(gameTime);
        }

		public void NetworkDisconnected(object sender, ConnectionEventArgs ev)
		{
			if(this.ExitingGame)
				return;

			MessageBox(new IntPtr(0), "Connection to server lost.", "Disconnected", 0);

			TileEngine.Unload();
			this.Exit();
		}
    }
}
