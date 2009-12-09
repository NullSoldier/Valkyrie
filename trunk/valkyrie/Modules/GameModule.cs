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
using Valkyrie.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Valkyrie.Engine.Providers;
using Gablarski;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine.Core;
using Valkyrie.Engine;
using Valkyrie.Engine.Maps;

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

			BaseCamera camera = this.scene.GetCamera("camera1");

			if(camera.ManualControl)
				camera.CenterOnCharacter(this.scene.GetPlayer("player1")); // Center camera on player

			this.KeybindController.Update();

			this.scene.Update(gameTime);
			this.context.MovementProvider.Update(gameTime);
			this.networkmovementprovider.Update(gameTime);

			foreach(var player in this.network.GetPlayers())
				player.Update(gameTime);

			// To do
			// Update network players
			// Update movement manager
	    }

	    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
	    {
			if(!this.IsLoaded)
				throw new ProviderNotLoadedException();

			spriteBatch.GraphicsDevice.Clear(Color.Black);

			if(this.underlayer)
				this.scene.DrawCameraLayer(spriteBatch, "camera1", MapLayers.UnderLayer);
			if(this.baselayer)
				this.scene.DrawCameraLayer(spriteBatch, "camera1", MapLayers.BaseLayer);
			if(this.middlelayer)
				this.scene.DrawCameraLayer(spriteBatch, "camera1", MapLayers.MiddleLayer);
			if(this.showplayers)
			{
				spriteBatch.Begin();
				this.scene.DrawPlayer(spriteBatch, "player1", this.scene.GetCamera("camera1"));
				this.scene.DrawNetworkedPlayers(spriteBatch, this.scene.GetCamera("camera1"));
				
				spriteBatch.End();
			}
			if(this.toplayer)
				this.scene.DrawCameraLayer(spriteBatch, "camera1", MapLayers.TopLayer);			

			//this.scene.DrawAllCameras(spriteBatch);
	    }

	    public void Load(IEngineContext enginecontext)
	    {
			this.context = enginecontext;
			this.network = (PokeNetworkProvider)enginecontext.NetworkProvider;
			this.scene = (PokeSceneProvider)enginecontext.SceneProvider;
			this.networkmovementprovider = new NetworkMovementProvider(enginecontext.CollisionProvider);

			this.context.WorldManager.Load(new Uri(Path.Combine(Environment.CurrentDirectory, Path.Combine(this.context.Configuration[EngineConfigurationName.DataRoot], this.context.Configuration[EngineConfigurationName.WorldFile]))), this.context.EventProvider);
			
			this.scene.AddCamera("camera1", new ValkyrieCamera(0, 0, 800, 600) { WorldName = this.scene.GetPlayer("player1").WorldName });

	        this.KeybindController.AddKey(Keys.Left, "MoveLeft");
	        this.KeybindController.AddKey(Keys.Up, "MoveUp");
	        this.KeybindController.AddKey(Keys.Down, "MoveDown");
	        this.KeybindController.AddKey(Keys.Right, "MoveRight");
	        this.KeybindController.AddKey(Keys.Q, "Noclip");
			this.KeybindController.AddKey(Keys.Add, "ZoomIn");
			this.KeybindController.AddKey(Keys.Subtract, "ZoomOut");
			this.KeybindController.AddKey(Keys.NumPad6, "SpeedUp");
			this.KeybindController.AddKey(Keys.NumPad9, "SlowDown");
			this.KeybindController.AddKey(Keys.F1, "ToggleUnderlayer");
			this.KeybindController.AddKey(Keys.F2, "ToggleBaselayer");
			this.KeybindController.AddKey(Keys.F3, "ToggleMiddlelayer");
			this.KeybindController.AddKey(Keys.F4, "ToggleToplayer");
			this.KeybindController.AddKey(Keys.F5, "TogglePlayers");

	        this.KeybindController.KeyDown += this.GameModule_KeyDown;
	        this.KeybindController.KeyUp += this.GameModule_KeyUp;

	        this.network.MessageReceived += this.Game_MessageReceived;
			this.network.Disconnected += this.Game_Disconnected;
			this.network.Send(new PlayerRequestListMessage());

			this.scene.GetPlayer("player1").StartedMoving += this.GameModule_StartedMoving;
			this.scene.GetPlayer("player1").StoppedMoving += this.GameModule_StoppedMoving;
			this.scene.GetPlayer("player1").TileLocationChanged += this.GameModule_TileLocationChanged;
			this.scene.GetPlayer("player1").Collided += this.GameModule_Collided;
			this.scene.GetPlayer("player1").TileLocationChanged += TestTileLocationChanged; // for testing purposes

			this.context.SoundManager.AddSound("PalletTown.wav");
			
	        this.IsLoaded = true;
	    }

	    public void Game_MessageReceived(object sender, MessageReceivedEventArgs ev)
	    {
			if (ev.Message is PlayerUpdateMessage)
			{
				this.Message_PlayerUpdateReceived((PlayerUpdateMessage)ev.Message);
			}
			else if(ev.Message is PlayerInfoMessage)
			{
				this.Message_PlayerInfoReceived((PlayerInfoMessage)ev.Message);
			}
			else if(ev.Message is PlayerStartedMovingMessage)
			{
				this.Message_PlayerStartedMovingReceived((PlayerStartedMovingMessage)ev.Message);
			}
			else if(ev.Message is PlayerStoppedMovingMessage)
			{
				this.Message_PlayerStoppedMovingReceived((PlayerStoppedMovingMessage)ev.Message);
			}
		}

		public void Game_Disconnected (object sender, ConnectionEventArgs ev)
		{
			MessageBox(new IntPtr(0), "You lost connection to the server.", "Disconnected", 0);
			Environment.Exit(1);
		}

		#region Messages Received

		public void Message_PlayerUpdateReceived (PlayerUpdateMessage message)
		{

			if(message.Action == PlayerUpdateAction.Add)
			{
				PokePlayer player = new PokePlayer();
				player.NetworkID = message.NetworkID;
				player.Loaded = false;

				this.network.AddPlayer(message.NetworkID, player);

				PlayerRequestMessage msg = new PlayerRequestMessage();
				msg.RequestedPlayerNetworkID = message.NetworkID;

				this.network.Send(msg);
			}
			else
			{
				this.network.RemovePlayer(message.NetworkID);
			}
		}

		public void Message_PlayerInfoReceived (PlayerInfoMessage message)
		{
			PokePlayer player = null;

			if(!this.network.ContainsPlayer(message.NetworkID))
			{
				player = new PokePlayer();
				player.NetworkID = message.NetworkID;
				this.network.AddPlayer(message.NetworkID, player);
			}
			else
			{
				player = this.network.GetPlayer(message.NetworkID);
			}

			player.Sprite = this.context.TextureManager.GetTexture(message.TileSheet);
			player.Name = message.Name;
			player.CurrentAnimationName = message.Animation;
			player.Location = new ScreenPoint(message.Location.X, message.Location.Y);
			player.WorldName = message.WorldName;

			if(!player.Loaded)
				player.Loaded = true;
		}

		public void Message_PlayerStartedMovingReceived (PlayerStartedMovingMessage message)
		{
		    if(!this.network.ContainsPlayer(message.NetworkID))
		        return;

			lock(this.network)
			{
				PokePlayer player = (PokePlayer)this.network.GetPlayer(message.NetworkID);

				if(player == null) return; // Wait till you load the person

				var direction = (Directions)message.Direction;

				this.networkmovementprovider.BeginMove(player, direction, message.Animation);
			}
		}

		public void Message_PlayerStoppedMovingReceived (PlayerStoppedMovingMessage message)
		{
			lock(this.networkmovementcache)
			{
				PokePlayer player = (PokePlayer)this.network.GetPlayer(message.NetworkID);

				if(player == null) return; // Wait till you load the person

				((NetworkMovementProvider)this.networkmovementprovider).EndMoveLocation(player, new MapPoint(message.X / 32, message.Y / 32), message.Animation);

				player.StoppedMoving += this.NetworkPlayer_Stopped;
			}
		}
		
		#endregion

		private void NetworkPlayer_Stopped (object sender, EventArgs e)
		{
			var player = (PokePlayer)sender;

			if(player.Direction == Directions.Any)
				return;

			player.CurrentAnimationName = player.Direction.ToString();

			player.StoppedMoving -= this.NetworkPlayer_Stopped;
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

			this.context.SoundProvider.PlayBGM(this.context.SoundManager.GetSound("PalletTown.wav"), true);
	    }

	    public void Deactivate()
	    {
	        throw new NotImplementedException();
	    }

	    #endregion

		private void GameModule_Collided (object sender, EventArgs ev)
	    {
	        this.context.EventProvider.HandleEvent((BaseCharacter)sender, ActivationTypes.Collision);
	    }

		private void GameModule_TileLocationChanged (object sender, EventArgs ev)
	    {
			this.context.EventProvider.HandleEvent((BaseCharacter)sender, ActivationTypes.Movement);

	        this.TestTileLocationChanged(this, EventArgs.Empty);
	    }

	    private void GameModule_StartedMoving(object sender, EventArgs ev)
	    {
	        PokePlayer player = (PokePlayer)sender;

			if(string.IsNullOrEmpty(player.HandleAnimationTag))
				player.CurrentAnimationName = "Walk" + player.Direction.ToString();

			PlayerStartMovingMessage msg = new PlayerStartMovingMessage();
			msg.NetworkID = player.NetworkID;
			msg.Direction = (int)player.Direction;
			msg.MovementType = (int)MovementType.TileBased;
			msg.Animation = player.CurrentAnimationName;
			msg.Speed = player.Speed;
			msg.MoveDelay = player.MoveDelay;
			this.network.Send(msg);
	    }

	    private void GameModule_StoppedMoving(object sender, EventArgs ev)
	    {
	        PokePlayer player = (PokePlayer)sender;

			PlayerStopMovingMessage msg = new PlayerStopMovingMessage();
			msg.NetworkID = player.NetworkID;
			msg.MapX = player.GlobalTileLocation.X;
			msg.MapY = player.GlobalTileLocation.Y;
			msg.Animation = player.CurrentAnimationName;
			this.network.Send(msg);

			player.CurrentAnimationName = player.Direction.ToString();
	    }

	    private void GameModule_KeyDown(object sender, KeyPressedEventArgs ev)
	    {
	        if (this.IsDir(ev.KeyPressed))
	        {
	            UpdateDirection(ev.KeyPressed);

	            if (!this.scene.GetPlayer("player1").IgnoreMoveInput)
	            {
	                switch (this.KeybindController.GetKeyAction(CrntDir))
	                {
	                    case "MoveUp":
							this.context.MovementProvider.BeginMove(this.scene.GetPlayer("player1"), Directions.North);
	                        break;
	                    case "MoveDown":
							this.context.MovementProvider.BeginMove(this.scene.GetPlayer("player1"), Directions.South);
	                        break;
	                    case "MoveLeft":
							this.context.MovementProvider.BeginMove(this.scene.GetPlayer("player1"), Directions.West);
	                        break;
	                    case "MoveRight":
							this.context.MovementProvider.BeginMove(this.scene.GetPlayer("player1"), Directions.East);
	                        break;
	                }
	            }
	        }
	    }

	    private void GameModule_KeyUp(object sender, KeyPressedEventArgs ev)
	    {
			PokePlayer player = (PokePlayer)this.scene.GetPlayer("player1");
			BaseCamera camera = this.scene.GetCamera("camera1"); ;

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
				case "SpeedUp":
					player.Speed += 5;
					break;
				case "SlowDown":
					player.Speed -= 5;
					if(player.Speed <= 0) player.Speed = 1;
					break;
				case "TogglePlayers":
					this.showplayers = !showplayers;
					break;
				case "ToggleUnderlayer":
					this.underlayer = !this.underlayer;
					break;
				case "ToggleBaselayer":
					this.baselayer = !this.baselayer;
					break;
				case "ToggleMiddlelayer":
					this.middlelayer = !this.middlelayer;
					break;
				case "ToggleToplayer":
					this.toplayer = !this.toplayer;
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

	    private void UpdateDirection(Keys NewDir)
	    {
	        if (!this.KeybindController.LastKeys.Contains<Keys>(NewDir) || !this.KeybindController.CurrentKeys.Contains<Keys>(CrntDir))
	        {
	            CrntDir = NewDir;
	        }
	    }

	    private bool IsDir(Keys key)
	    {
	        return (key == Keys.Left || key == Keys.Right || key == Keys.Up || key == Keys.Down ||
				key == Keys.NumPad4 || key == Keys.NumPad6 || key == Keys.NumPad8 || key == Keys.NumPad2);
	    }

		private void AddToMovementCache (PokePlayer player, MovementItem moveitem)
		{
			lock(this.networkmovementcache)
			{
				if(!this.networkmovementcache.ContainsKey(player.NetworkID))
					this.networkmovementcache.Add(player.NetworkID, new Queue<MovementItem>());

				this.networkmovementcache[player.NetworkID].Enqueue(moveitem);				
			}
		}

		private bool RemoveFromMovementCache (PokePlayer player)
		{
			lock(this.networkmovementcache)
			{
				return this.networkmovementcache.Remove(player.NetworkID);
			}
		}

		private IEngineContext context = null;
		private PokeNetworkProvider network = null;
		private PokeSceneProvider scene = null;
		private bool isloaded = false;
		private KeybindController KeybindController = new KeybindController();
		private Keys CrntDir = Keys.None;
		private GraphicsDevice graphicsdevice = null;
		private Dictionary<uint, Queue<MovementItem>> networkmovementcache = new Dictionary<uint, Queue<MovementItem>>();

		private bool underlayer = true;
		private bool baselayer = true;
		private bool middlelayer = true;
		private bool toplayer = true;
		private bool showplayers = true;

		private NetworkMovementProvider networkmovementprovider = null;
		//private PokePlayer player = null;

		#region Statics and Internals

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern uint MessageBox (IntPtr hWnd, String text, String caption, uint type);

		#endregion
	}

}