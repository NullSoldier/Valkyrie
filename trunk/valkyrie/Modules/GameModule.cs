using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Valkyrie.Characters;
using Valkyrie.Engine;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Events;
using Valkyrie.Engine.Input;
using Valkyrie.Engine.Maps;
using Valkyrie.Engine.Providers;
using Valkyrie.Library.Camera;
using Valkyrie.Library.Network;
using Valkyrie.Providers;
using Valkyrie.Messages.Valkyrie;
using Valkyrie.Messages.Valkyrie.Movement;
using Valkyrie;
using ValkyrieServerLibrary.Network.Messages.Valkyrie;
using Valkyrie.Library;

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
			this.context.SoundProvider.Update (gameTime);
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
				this.scene.DrawCameraLayer (spriteBatch, "camera1", MapLayers.UnderLayer, this.showplayers);
			if(this.baselayer)
				this.scene.DrawCameraLayer (spriteBatch, "camera1", MapLayers.BaseLayer, this.showplayers);
			if(this.middlelayer)
				this.scene.DrawCameraLayer (spriteBatch, "camera1", MapLayers.MiddleLayer, this.showplayers);
			if(this.toplayer)
				this.scene.DrawCameraLayer (spriteBatch, "camera1", MapLayers.TopLayer, this.showplayers);			

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

	        this.KeybindController.AddKey (Keys.Left, "MoveLeft");
	        this.KeybindController.AddKey (Keys.Up, "MoveUp");
	        this.KeybindController.AddKey (Keys.Down, "MoveDown");
	        this.KeybindController.AddKey (Keys.Right, "MoveRight");
	        this.KeybindController.AddKey (Keys.Q, "Noclip");
			this.KeybindController.AddKey (Keys.Add, "ZoomIn");
			this.KeybindController.AddKey (Keys.Subtract, "ZoomOut");
			this.KeybindController.AddKey (Keys.NumPad6, "SpeedUp");
			this.KeybindController.AddKey (Keys.NumPad9, "SlowDown");
			this.KeybindController.AddKey (Keys.F1, "ToggleUnderlayer");
			this.KeybindController.AddKey (Keys.F2, "ToggleBaselayer");
			this.KeybindController.AddKey (Keys.F3, "ToggleMiddlelayer");
			this.KeybindController.AddKey (Keys.F4, "ToggleToplayer");
			this.KeybindController.AddKey (Keys.F5, "TogglePlayers");
			this.KeybindController.AddKey (Keys.T, "VoiceChat");

			this.KeybindController.KeyPressed += this.GameModule_KeyPressed;
	        this.KeybindController.KeyDown += this.GameModule_KeyDown;
	        this.KeybindController.KeyUp += this.GameModule_KeyUp;

	        this.network.MessageReceived += this.Game_MessageReceived;
			this.network.Disconnected += this.Game_Disconnected;
			this.network.Send(new PlayerRequestListMessage());

			this.scene.GetPlayer("player1").StartedMoving += this.GameModule_StartedMoving;
			this.scene.GetPlayer("player1").StoppedMoving += this.GameModule_StoppedMoving;
			this.scene.GetPlayer("player1").TileLocationChanged += this.GameModule_TileLocationChanged;
			this.scene.GetPlayer("player1").Collided += this.GameModule_Collided;

			this.context.VoiceChatProvider.UserStartedTalking += this.GameModule_UserStartedTalking;
			this.context.VoiceChatProvider.UserStoppedTalking += this.GameModule_UserStoppedTalking;

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
				player.ID = message.NetworkID;
				player.IsLoaded = false;

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
				player.ID = message.NetworkID;
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

			if(!player.IsLoaded)
				player.IsLoaded = true;
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
			lock(this.networkmovementprovider)
			{
				PokePlayer player = (PokePlayer)this.network.GetPlayer(message.NetworkID);

				if(player == null) return; // Wait till you load the person
				
				player.StoppedMoving += this.NetworkPlayer_Stopped;

				this.networkmovementprovider.EndMoveLocation(player, new MapPoint(message.X / 32, message.Y / 32), message.Animation);
			}
		}
		
		#endregion

		private void NetworkPlayer_Stopped (object sender, EventArgs e)
		{
			var player = (PokePlayer)sender;

			if(player.Direction == Directions.Any)
				return;

			player.CurrentAnimationName = player.Direction.ToString ();

			if(this.networkmovementprovider.GetMoveCount(player) == 0)
				player.StoppedMoving -= this.NetworkPlayer_Stopped;

			if(player.CurrentAnimationName.Contains ("Walk"))
				return;
		}

	    public void Unload()
	    {

	    }

	    public void Activate()
	    {
	        if( !this.IsLoaded )
	            this.Load(this.context);

			//this.context.SoundProvider.PlayBGM(this.context.SoundManager.GetSound("PalletTown.wav"), true);
	    }

	    public void Deactivate()
	    {
	        
	    }

	    #endregion

		private void GameModule_UserStartedTalking (object sender, TalkingChangedEventArgs ev)
		{
			// Find the player talking
			BaseCharacter basecharacter = this.context.SceneProvider.GetPlayer ("player1");
			if(Convert.ToUInt32(basecharacter.ID) == Convert.ToUInt32(ev.ID))
			{
				basecharacter.IsTalking = true;
			}
			else
			{
				// Look in networked players
				basecharacter = this.network.GetPlayer (Convert.ToUInt32 (ev.ID));
				if(basecharacter != null)
					basecharacter.IsTalking = true;
			}


			lock(this.talkingcountlock)
			{
				this.talkingcount++;

				this.context.SoundProvider.MasterGainModifier = -0.80f;
			}
		}

		private void GameModule_UserStoppedTalking (object sender, TalkingChangedEventArgs ev)
		{
			// Find the player that stopped
			BaseCharacter basecharacter = this.context.SceneProvider.GetPlayer ("player1");
			if(Convert.ToUInt32 (basecharacter.ID) == Convert.ToUInt32 (ev.ID))
			{
				basecharacter.IsTalking = false;
			}
			else
			{
				// Look in networked players
				basecharacter = this.network.GetPlayer (Convert.ToUInt32 (ev.ID));
				if(basecharacter != null)
					basecharacter.IsTalking = false;
			}

			lock(this.talkingcountlock)
			{
				this.talkingcount--;

				if(talkingcount == 0)
					this.context.SoundProvider.MasterGainModifier = -0.5f;
			}
		}

		private void GameModule_Collided (object sender, EventArgs ev)
	    {
	        this.context.EventProvider.HandleEvent((BaseCharacter)sender, ActivationTypes.Collision);
	    }

		private void GameModule_TileLocationChanged (object sender, EventArgs ev)
	    {
			this.context.EventProvider.HandleEvent((BaseCharacter)sender, ActivationTypes.Movement);
	    }

	    private void GameModule_StartedMoving(object sender, EventArgs ev)
	    {
	        PokePlayer player = (PokePlayer)sender;

			if(string.IsNullOrEmpty (player.AnimationTag.ToString()))
				player.CurrentAnimationName = "Walk" + player.Direction.ToString();

			if(this.CollideBeforeMove (player, player.Direction))
			{
				this.silentstep = true;
				return;
			}

			PlayerStartMovingMessage msg = new PlayerStartMovingMessage();
			msg.NetworkID = (uint)player.ID;
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

			if(!this.silentstep)
			{
				PlayerStopMovingMessage msg = new PlayerStopMovingMessage ();
				msg.NetworkID = (uint) player.ID;
				msg.MapX = player.GlobalTileLocation.X;
				msg.MapY = player.GlobalTileLocation.Y;
				msg.Animation = player.CurrentAnimationName;
				this.network.Send (msg);
			}
			else
				this.silentstep = false;

			player.CurrentAnimationName = player.Direction.ToString();
	    }

		private void GameModule_KeyPressed (object sender, KeyPressedEventArgs ev)
		{
			var player = this.scene.GetPlayer ("player1");

			if(ev.Action == "VoiceChat" && this.context.VoiceChatProvider.IsConnected)
			{
				this.context.VoiceChatProvider.BeginTalk (player);
			}
		}

	    private void GameModule_KeyDown(object sender, KeyPressedEventArgs ev)
	    {
			var player = this.scene.GetPlayer ("player1");

			if(this.IsDir (ev.KeyPressed))
			{
				UpdateDirection (ev.KeyPressed);

				if(!player.IgnoreMoveInput)
				{
					Directions direction = Directions.Any;

					switch(ev.Action)
					{
						case "MoveUp":
							direction = Directions.North;
							break;
						case "MoveDown":
							direction = Directions.South;
							break;
						case "MoveLeft":
							direction = Directions.West;
							break;
						case "MoveRight":
							direction = Directions.East;
							break;
					}

					this.context.MovementProvider.BeginMove (player, direction);
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
					player.Speed = Helpers.Clamp (player.Speed + 2, 2, 10);
					break;
				case "SlowDown":
					player.Speed = Helpers.Clamp(player.Speed - 2, 2, 10);
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
					this.context.MovementProvider.EndMove(player, true, false);
	            }
	        }

			if(ev.Action == "VoiceChat" && this.context.VoiceChatProvider.IsConnected)
				this.context.VoiceChatProvider.EndTalk (player);
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
	        return (key == Keys.Left || key == Keys.Right || key == Keys.Up || key == Keys.Down);
	    }

		private bool CollideBeforeMove (BaseCharacter movable, Directions directon)
		{
			float x = movable.Location.X;
			float y = movable.Location.Y;

			#region Destination calculation
			if(directon == Directions.North)
			{
				y -= movable.Speed;
			}
			else if(directon == Directions.South)
			{
				y += movable.Speed;
			}
			else if(directon == Directions.East)
			{
				x += movable.Speed;
			}
			else if(directon == Directions.West)
			{
				x -= movable.Speed;
			}
			#endregion

			ScreenPoint destination = new ScreenPoint ((int) x, (int) y);
			ScreenPoint collision = new ScreenPoint (destination.X, destination.Y);

			if(directon == Directions.South)
				collision = new ScreenPoint (destination.X, destination.Y + 32 - (int) movable.Speed);
			else if(directon == Directions.East)
				collision = new ScreenPoint (destination.X + 32 - (int) movable.Speed, destination.Y);

			var collisionevent = this.context.EventProvider.GetMapsEvents(movable, collision.ToMapPoint()).Where( p => p.Activation == ActivationTypes.Collision && p.Direction == movable.Direction).FirstOrDefault();
			if(collisionevent != null)
				return false;

			return !this.context.CollisionProvider.CheckCollision (movable, collision);
		}

		private IEngineContext context = null;
		private PokeNetworkProvider network = null;
		private PokeSceneProvider scene = null;
		private KeybindController KeybindController = new KeybindController();
		private Keys CrntDir = Keys.None;
		private GraphicsDevice graphicsdevice = null;

		private object talkingcountlock = new object();
		private int talkingcount = 0;

		private bool isloaded = false;
		private bool underlayer = true;
		private bool baselayer = true;
		private bool middlelayer = true;
		private bool toplayer = true;
		private bool showplayers = true;
		private bool silentstep = false;

		private NetworkMovementProvider networkmovementprovider = null;
		//private PokePlayer player = null;

		#region Statics and Internals

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern uint MessageBox (IntPtr hWnd, String text, String caption, uint type);

		#endregion
	}

}