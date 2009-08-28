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

namespace ValkyrieLibrary
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PokeGame
		: Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
		public static SpriteFont font;

        public PokeGame()
        {
            graphics = new GraphicsDeviceManager(this);
			graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

		private void TestConnected(object sender, EventArgs ev)
		{
			LoginMessage msg = new LoginMessage();
			msg.Username = "testuser";
			msg.Password = "testpassword";

			TileEngine.NetworkManager.Send(msg);
		}

		private void TestDisconnected(object sender, EventArgs ev)
		{
			Environment.Exit(0);
		}		

		public void TestMessageReceived(object sender, MessageReceivedEventArgs ev)
		{
			if (ev.Message is LoginSuccessMessage)
			{
				TileEngine.NetworkID = ((LoginSuccessMessage)ev.Message).NetworkIDAssigned;

				TileEngine.NetworkManager.Send(new PlayerRequestListMessage());
			}
			else if (ev.Message is PlayerUpdateMessage)
			{
				var message = (PlayerUpdateMessage)ev.Message;
				
				if(message.Action == PlayerUpdateAction.Add)
				{
					PokePlayer player = new PokePlayer();
					player.Sprite = TileEngine.TextureManager.GetTexture("MaleSprite.png");

					TileEngine.NetworkPlayerCache.Add(message.NetworkID, player);
				}
				else
				{
					TileEngine.NetworkPlayerCache.Remove(message.NetworkID);
				}
			}
			else if (ev.Message is LocationUpdateMessage)
			{
				var message = (LocationUpdateMessage)ev.Message;

				if (!TileEngine.NetworkPlayerCache.ContainsKey(message.NetworkID))
					return; // Throw exception or get player data

				UInt32 NID = message.NetworkID;
				int x = message.X;
				int y = message.Y;
				string animation = message.Animation;

				TileEngine.NetworkPlayerCache[NID].Location = new ScreenPoint(x, y);
				TileEngine.NetworkPlayerCache[NID].CurrentAnimationName = animation;
			}

		}

        protected override void Initialize()
        {
            TileEngine.Initialize(this.Content, this.GraphicsDevice);
			//TileEngine.EventManager.LoadEventTypesFromAssemblies( new Assembly[] {Assembly.GetEntryAssembly(), Assembly.LoadWithPartialName("ValkyrieLibrary")});
            TileEngine.Viewport = this.GraphicsDevice.Viewport;
            TileEngine.Camera = new PokeCamera(0, 0, 800, 600);
			TileEngine.CollisionManager = new PokeCollisionManager();
			TileEngine.MovementManager = new PokeMovementManager();
			TileEngine.EventManager = new MapEventManager(new Assembly[] { Assembly.GetEntryAssembly(), Assembly.Load("ValkyrieLibrary") });
			TileEngine.TileSize = 32;

			TileEngine.NetworkManager.Connected += this.TestConnected;
			TileEngine.NetworkManager.Disconnected += this.TestDisconnected;
			TileEngine.NetworkManager.MessageReceived += this.TestMessageReceived;
			TileEngine.NetworkManager.Connect(new IPEndPoint(IPAddress.Parse("173.65.132.130"), 6112));

			LoginMessage msg = new LoginMessage();
			msg.Username = "testuser";
			msg.Password = "testpassword";

			TileEngine.NetworkManager.Send(msg);

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
                Window.Title = "PokeGame [" + fps.ToString() + " FPS]";
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
