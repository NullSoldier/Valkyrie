using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Valkyrie.Library.Core;
using System.Xml;
using System.IO;
using Valkyrie.Library;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Valkyrie.Library.Core.Messages;
using Valkyrie.Library.Network;
using Gablarski;
using ValkyrieServerLibrary.Network.Messages.Valkyrie;
using System.Runtime.InteropServices;
using ValkyrieNetwork.Messages.Valkyrie;
using ValkyrieNetwork.Messages.Valkyrie.Movement;
using Valkyrie.Engine;
using Valkyrie.Characters;
using Valkyrie.Engine.Input;
using Valkyrie.Engine.Events;
using Valkyrie.Engine.Characters;
using Valkyrie.Library.Camera;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Providers;

namespace Valkyrie.Modules
{
	public class GameModule
		: IModule
	{
		#region Constructors

		public GameModule (GraphicsDevice graphicsdevice)
		{
			KeybindController.LoadKeys();
		}
		
		#endregion

		#region Properties and Methods
		public bool IsLoaded
		{
			get { return this.isloaded; }
			set { this.isloaded = value; }
		}

		public string Name
		{
			get { return "Game"; }
		}

		public void Update(GameTime gameTime)
	    {
	        if (!this.IsLoaded)
	            return;

			BaseCamera camera = this.context.SceneProvider.GetCamera("camera1");

			if(camera.ManualControl)
				camera.CenterOnCharacter(this.context.SceneProvider.GetPlayer("player1")); // center camera on player

			this.KeybindController.Update();

			this.context.SceneProvider.Update(gameTime);
			this.context.MovementProvider.Update(gameTime);

			// To do
			// Update network players
			// Update movement manager
	    }

	    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
	    {
			if(!this.IsLoaded)
				throw new ProviderNotLoadedException();

			spriteBatch.GraphicsDevice.Clear(Color.Black);

			this.context.SceneProvider.DrawAllCameras(spriteBatch);
	    }

	    public void Load(IEngineContext enginecontext)
	    {
			this.context = enginecontext;

			this.context.WorldManager.Load(new Uri(Path.Combine(Environment.CurrentDirectory, Path.Combine(this.context.Configuration[EngineConfigurationName.DataRoot], this.context.Configuration[EngineConfigurationName.WorldFile]))), this.context.EventProvider);
			
			this.context.SceneProvider.AddCamera("camera1", new ValkyrieCamera(0, 0, 800, 600) { WorldName = "Kanto" });
			this.context.SceneProvider.AddPlayer("player1", new PokePlayer()
			{
				WorldName = "Kanto",
				Location = new MapPoint(87, 210).ToScreenPoint(),
				Sprite = this.context.TextureManager.GetTexture("MaleSprite.png")
			});

	        this.KeybindController.AddKey(Keys.Left, "MoveLeft");
	        this.KeybindController.AddKey(Keys.Up, "MoveUp");
	        this.KeybindController.AddKey(Keys.Down, "MoveDown");
	        this.KeybindController.AddKey(Keys.Right, "MoveRight");
	        this.KeybindController.AddKey(Keys.Q, "Noclip");
			this.KeybindController.AddKey(Keys.Add, "ZoomIn");
			this.KeybindController.AddKey(Keys.Subtract, "ZoomOut");

			this.KeybindController.AddKey(Keys.NumPad4, "MoveLeftPad");
			this.KeybindController.AddKey(Keys.NumPad8, "MoveUpPad");
			this.KeybindController.AddKey(Keys.NumPad2, "MoveDownPad");
			this.KeybindController.AddKey(Keys.NumPad6, "MoveRightPad");

	        this.KeybindController.KeyDown += this.GameModule_KeyDown;
	        this.KeybindController.KeyUp += this.GameModule_KeyUp;

	        //TileEngine.NetworkManager.MessageReceived += this.Game_MessageReceived;
	       // TileEngine.NetworkManager.Send(new PlayerRequestListMessage());

			this.context.SceneProvider.GetPlayer("player1").StartedMoving += this.GameModule_StartedMoving;
			this.context.SceneProvider.GetPlayer("player1").StoppedMoving += this.GameModule_StoppedMoving;
			this.context.SceneProvider.GetPlayer("player1").TileLocationChanged += this.GameModule_TileLocationChanged;
			this.context.SceneProvider.GetPlayer("player1").Collided += this.GameModule_Collided;
			this.context.SceneProvider.GetPlayer("player1").TileLocationChanged += TestTileLocationChanged; // for testing purposes

	        this.IsLoaded = true;
	    }

	    public void Game_MessageReceived(object sender, MessageReceivedEventArgs ev)
	    {

			//if (ev.Message is PlayerUpdateMessage)
			//{
			//    var message = (PlayerUpdateMessage)ev.Message;

			//    if (message.Action == PlayerUpdateAction.Add)
			//    {
			//        PokePlayer player = new PokePlayer();
			//        player.Loaded = false;

			//        TileEngine.NetworkPlayerCache.Add(message.NetworkID, player);

			//        PlayerRequestMessage msg = new PlayerRequestMessage();
			//        msg.RequestedPlayerNetworkID = message.NetworkID;

			//        TileEngine.NetworkManager.Send(msg);
			//    }
			//    else
			//    {
			//        TileEngine.NetworkPlayerCache.Remove(message.NetworkID);
			//    }
			//}
			//else if (ev.Message is PlayerInfoMessage)
			//{
			//    PlayerInfoMessage message = (PlayerInfoMessage)ev.Message;

			//    PokePlayer player = null;

			//    lock(TileEngine.NetworkPlayerCache)
			//    {
			//        if(!TileEngine.NetworkPlayerCache.ContainsKey(message.NetworkID))
			//        {
			//            // If it's not in the cache, add it
			//            player = new PokePlayer();
			//            TileEngine.NetworkPlayerCache.Add(message.NetworkID, player);
			//        }
			//        else
			//        {
			//            // Otherwise get it
			//            player = (PokePlayer)TileEngine.NetworkPlayerCache[message.NetworkID];
			//        }

			//        player.Sprite = TileEngine.TextureManager.GetTexture("MaleSprite.png");
			//        player.Name = message.Name;
			//        player.CurrentAnimationName = message.Animation;
			//        player.Location = new ScreenPoint(message.Location.X, message.Location.Y);

			//        if(!player.Loaded)
			//            player.Loaded = true;
			//    }
			//}
			//else if (ev.Message is LocationUpdateReceived)
			//{
			//    var message = (LocationUpdateReceived)ev.Message;

			//    if (!TileEngine.NetworkPlayerCache.ContainsKey(message.NetworkID))
			//        throw new IndexOutOfRangeException("Player does not exist in the network cache.");

			//    UInt32 NID = message.NetworkID;
			//    int x = message.X;
			//    int y = message.Y;
			//    string animation = message.Animation;

			//    lock(TileEngine.NetworkPlayerCache)
			//    {
			//        PokePlayer player = (PokePlayer)TileEngine.NetworkPlayerCache[NID];

			//        player.Location = new ScreenPoint(x, y);
			//        player.CurrentAnimationName = animation;
			//    }
			//}
			//else if(ev.Message is PlayerStartedMovingMessage)
			//{
			//    var message = (PlayerStartedMovingMessage)ev.Message;

			//    if(!TileEngine.NetworkPlayerCache.ContainsKey(message.NetworkID))
			//        return;

			//    lock(TileEngine.NetworkPlayerCache)
			//    {
			//        PokePlayer player = (PokePlayer)TileEngine.NetworkPlayerCache[message.NetworkID];
			//        player.Direction = (Directions)message.Direction;
			//        player.CurrentAnimationName = message.Animation;
			//        TileEngine.MovementManager.BeginMove(player, player.Direction);
			//    }
			//}
			//else if(ev.Message is PlayerStoppedMovingMessage)
			//{
			//    var message = (PlayerStoppedMovingMessage)ev.Message;

			//    if(message.NetworkID == TileEngine.NetworkID)
			//    {
			//        TileEngine.Player.Location = new ScreenPoint(message.Location.X, message.Location.Y);
			//    }
			//    else
			//    {
			//        if(!TileEngine.NetworkPlayerCache.ContainsKey(message.NetworkID))
			//            return;

			//        lock(TileEngine.NetworkPlayerCache)
			//        {
			//            PokePlayer player = (PokePlayer)TileEngine.NetworkPlayerCache[message.NetworkID];
			//            TileEngine.MovementManager.EndMoveFunctional(player);

			//            player.CurrentAnimationName = message.Animation;
			//            player.Location = new ScreenPoint(message.X, message.Y);
			//        }
			//    }
			//}
	    }

	    public void TestTileLocationChanged(object sender, EventArgs e)
	    {
	        // Send Test
			//LocationUpdateMessage msg = new LocationUpdateMessage();
			//msg.X = TileEngine.Player.Location.X;
			//msg.Y = TileEngine.Player.Location.Y;
			//msg.Animation = TileEngine.Player.CurrentAnimationName;
			//msg.NetworkID = TileEngine.NetworkID;
			//msg.Direction = (int)TileEngine.Player.Direction;
			//TileEngine.NetworkManager.Send(msg);
	        // End Send Test
	    }

	    public void Unload()
	    {

	    }

	    public void Activate()
	    {
	        if( !this.IsLoaded )
	            this.Load(this.context);
	    }

	    public void Deactivate()
	    {
	        throw new NotImplementedException();
	    }

	    #endregion

	    public void GameModule_Collided(object sender, EventArgs ev)
	    {
	        this.context.EventProvider.HandleEvent((BaseCharacter)sender, ActivationTypes.Collision);
	    }

	    public void GameModule_TileLocationChanged(object sender, EventArgs ev)
	    {
			this.context.EventProvider.HandleEvent((BaseCharacter)sender, ActivationTypes.Movement);

	        this.TestTileLocationChanged(this, EventArgs.Empty);
	    }

	    public void GameModule_StartedMoving(object sender, EventArgs ev)
	    {
	        PokePlayer player = (PokePlayer)sender;

	        player.CurrentAnimationName = "Walk" + player.Direction.ToString();

			//PlayerStartMovingMessage msg = new PlayerStartMovingMessage();
			//msg.NetworkID = .NetworkID;
			//msg.Direction = (int)player.Direction;
			//msg.MovementType = (int)MovementType.TileBased;
			//msg.Animation = player.CurrentAnimationName;
			//msg.Speed = player.Speed;
			//msg.MoveDelay = player.MoveDelay;
			//TileEngine.NetworkManager.Send(msg);
	    }

	    public void GameModule_StoppedMoving(object sender, EventArgs ev)
	    {
	        PokePlayer player = (PokePlayer)sender;

	        player.CurrentAnimationName = player.Direction.ToString();

			//PlayerStopMovingMessage msg = new PlayerStopMovingMessage();
			//msg.NetworkID = TileEngine.NetworkID;
			//msg.MapX = TileEngine.Player.Location.X / 32;
			//msg.MapY = TileEngine.Player.Location.Y / 32;
			//TileEngine.NetworkManager.Send(msg);
	    }

	    public void GameModule_KeyDown(object sender, KeyPressedEventArgs ev)
	    {
	        if (this.IsDir(ev.KeyPressed))
	        {
	            UpdateDirection(ev.KeyPressed);

	            if (!this.context.SceneProvider.GetPlayer("player1").IgnoreMoveInput)
	            {
	                switch (this.KeybindController.GetKeyAction(CrntDir))
	                {
	                    case "MoveUp":
							this.context.MovementProvider.BeginMove(this.context.SceneProvider.GetPlayer("player1"), Directions.North);
	                        break;
	                    case "MoveDown":
							this.context.MovementProvider.BeginMove(this.context.SceneProvider.GetPlayer("player1"), Directions.South);
	                        break;
	                    case "MoveLeft":
							this.context.MovementProvider.BeginMove(this.context.SceneProvider.GetPlayer("player1"), Directions.West);
	                        break;
	                    case "MoveRight":
							this.context.MovementProvider.BeginMove(this.context.SceneProvider.GetPlayer("player1"), Directions.East);
	                        break;
	                }
	            }
	        }
	    }

	    public void GameModule_KeyUp(object sender, KeyPressedEventArgs ev)
	    {
			PokePlayer player = (PokePlayer)this.context.SceneProvider.GetPlayer("player1");
			BaseCamera camera = this.context.SceneProvider.GetCamera("camera1"); ;

	        // Did we activate?
	        switch (ev.Action)
	        {
	            case "Noclip":
					player.Density = Convert.ToInt32(!(player.Density == 1));
					break;
				case "ZoomIn":
					camera.Scale(1.2);
					break;
				case "ZoomOut":
					camera.Scale(0.9);
					break;
	            default:
	                break;
	        }

	        // Check other pressed keys
	        if (!player.IgnoreMoveInput)
	        {
	            if (this.IsDir(ev.KeyPressed))
	            {
					this.context.MovementProvider.EndMove(player, true);
	            }
	        }
	    }

	    public void UpdateDirection(Keys NewDir)
	    {
	        if (!this.KeybindController.LastKeys.Contains<Keys>(NewDir) || !this.KeybindController.CurrentKeys.Contains<Keys>(CrntDir))
	        {
	            CrntDir = NewDir;
	        }
	    }

	    public bool IsDir(Keys key)
	    {
	        return (key == Keys.Left || key == Keys.Right || key == Keys.Up || key == Keys.Down ||
				key == Keys.NumPad4 || key == Keys.NumPad6 || key == Keys.NumPad8 || key == Keys.NumPad2);
	    }

		private IEngineContext context = null;
		private bool isloaded = false;
		private KeybindController KeybindController = new KeybindController();
		private Keys CrntDir = Keys.None;
		private GraphicsDevice graphicsdevice = null;

		#region Statics and Internals

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern uint MessageBox (IntPtr hWnd, String text, String caption, uint type);

		#endregion
	}

}