using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ValkyrieLibrary.Core;
using System.Xml;
using System.IO;
using ValkyrieLibrary;
using Microsoft.Xna.Framework.Input;
using ValkyrieLibrary.Maps;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Input;
using Valkyrie.Characters;
using System.Diagnostics;
using ValkyrieLibrary.Core.Messages;
using ValkyrieLibrary.Network;
using Gablarski;
using ValkyrieServerLibrary.Network.Messages.Valkyrie;
using System.Runtime.InteropServices;

namespace Valkyrie.States
{
    public class GameModule
		: IModule
    {
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

        private bool Loaded = false;
        private KeybindController KeybindController = new KeybindController();

        private Keys CrntDir;

        public GameModule() 
        {
            KeybindController.LoadKeys();
        }

        #region IModule Members

        public void Update(GameTime gameTime)
        {
			if (!this.Loaded)
				return;

			if(!TileEngine.Camera.ManualControl)
				TileEngine.Camera.CenterOnPoint(new ScreenPoint(TileEngine.Player.Location.X + (TileEngine.Player.CurrentAnimation.FrameRectangle.Width / 2), TileEngine.Player.Location.Y + (TileEngine.Player.CurrentAnimation.FrameRectangle.Height / 2)));

            this.KeybindController.Update();

			TileEngine.Player.Update(gameTime);
			foreach (PokePlayer player in TileEngine.NetworkPlayerCache.Values)
				player.Update(gameTime);

			TileEngine.MovementManager.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
			if (!this.Loaded)
				return;

            spriteBatch.GraphicsDevice.Viewport = TileEngine.Camera.Viewport;
            spriteBatch.GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin();

            TileEngine.DrawAllLayers(spriteBatch, true);
            TileEngine.DrawOverlay(spriteBatch);

			spriteBatch.End();
        }

        public void Load()
        {
			TileEngine.WorldManager.SetWorld("Main", "Default", false);

            this.KeybindController.AddKey(Keys.Left, "MoveLeft");
            this.KeybindController.AddKey(Keys.Up, "MoveUp");
            this.KeybindController.AddKey(Keys.Down, "MoveDown");
            this.KeybindController.AddKey(Keys.Right, "MoveRight");
			this.KeybindController.AddKey(Keys.Q, "Noclip");

            this.KeybindController.KeyDown += this.GameModule_KeyDown;
			this.KeybindController.KeyUp += this.GameModule_KeyUp;

			TileEngine.NetworkManager.MessageReceived += this.Game_MessageReceived;
			TileEngine.NetworkManager.Send(new PlayerRequestListMessage());

			TileEngine.Player.StartedMoving += this.GameModule_StartedMoving;
			TileEngine.Player.StoppedMoving += this.GameModule_StoppedMoving;
			TileEngine.Player.TileLocationChanged += this.GameModule_TileLocationChanged;
			TileEngine.Player.Collided += this.GameModule_Collided;

			TileEngine.Player.TileLocationChanged += TestTileLocationChanged;

			this.Loaded = true;
        }

		public void Game_MessageReceived(object sender, MessageReceivedEventArgs ev)
		{
			try
			{
				if (ev.Message is PlayerUpdateMessage)
				{
					var message = (PlayerUpdateMessage)ev.Message;

					if (message.Action == PlayerUpdateAction.Add)
					{
						PokePlayer player = new PokePlayer();
						player.Loaded = false;

						TileEngine.NetworkPlayerCache.Add(message.NetworkID, player);

						PlayerRequestMessage msg = new PlayerRequestMessage();
						msg.NetworkID = message.NetworkID;

						TileEngine.NetworkManager.Send(msg);
					}
					else
					{
						TileEngine.NetworkPlayerCache.Remove(message.NetworkID);
					}
				}
				else if (ev.Message is PlayerInfoMessage)
				{
					PlayerInfoMessage message = (PlayerInfoMessage)ev.Message;

					PokePlayer player = (PokePlayer)TileEngine.NetworkPlayerCache[message.NetworkID];

					player.Sprite = TileEngine.TextureManager.GetTexture("MaleSprite.png");
					player.Name = message.Name;
					player.CurrentAnimationName = message.Animation;
					player.Location = new ScreenPoint(message.Location.X, message.Location.Y);

					if(!player.Loaded)
						player.Loaded = true;
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
			catch (System.Exception ex)
			{
				MessageBox(new IntPtr(0), ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace, "Error!", 0);
			}
		}

		public void TestTileLocationChanged(object sender, EventArgs e)
		{
			// Send Test
			LocationUpdateMessage msg = new LocationUpdateMessage();
			msg.X = TileEngine.Player.Location.X;
			msg.Y = TileEngine.Player.Location.Y;
			msg.Animation = TileEngine.Player.CurrentAnimationName;
			msg.NetworkID = TileEngine.NetworkID;
			TileEngine.NetworkManager.Send(msg);
			// End Send Test
		}

        public void Unload()
        {
            
        }

        public void Activate()
        {
            if( !this.Loaded )
                this.Load();
        }

        public void Deactivate()
        {
            throw new NotImplementedException();
        }

        #endregion

		public void GameModule_Collided(object sender, EventArgs ev)
		{
			TileEngine.EventManager.HandleEvent(TileEngine.Player, ActivationTypes.Collision);
		}

		public void GameModule_TileLocationChanged(object sender, EventArgs ev)
		{
			TileEngine.EventManager.HandleEvent(TileEngine.Player, ActivationTypes.Movement);
		}

		public void GameModule_StartedMoving(object sender, EventArgs ev)
		{
			PokePlayer player = (PokePlayer)sender;

			player.CurrentAnimationName = "Walk" + player.Direction.ToString();

		}

		public void GameModule_StoppedMoving(object sender, EventArgs ev)
		{
			PokePlayer player = (PokePlayer)sender;

			player.CurrentAnimationName = player.Direction.ToString();

			this.TestTileLocationChanged(this, EventArgs.Empty);
		}

        public void GameModule_KeyDown(object sender, KeyPressedEventArgs ev)
        {
			if (this.IsDir(ev.KeyPressed))
			{
				UpdateDirection(ev.KeyPressed);
			
				if (!TileEngine.Player.IgnoreMoveInput)
				{
					switch (this.KeybindController.GetKeyAction(CrntDir))
					{
						case "MoveUp":
							TileEngine.MovementManager.BeginMove(TileEngine.Player, Directions.North);
							break;
						case "MoveDown":
							TileEngine.MovementManager.BeginMove(TileEngine.Player, Directions.South);
							break;
						case "MoveLeft":
							TileEngine.MovementManager.BeginMove(TileEngine.Player, Directions.West);
							break;
						case "MoveRight":
							TileEngine.MovementManager.BeginMove(TileEngine.Player, Directions.East);
							break;
					}
				}
			}
        }

		public void GameModule_KeyUp(object sender, KeyPressedEventArgs ev)
		{
			// Did we activate?
			switch (ev.Action)
			{
				case "Noclip":
					TileEngine.Player.Density = Convert.ToInt32(!(TileEngine.Player.Density == 1));
					return;
				case "AButton":
				case "BButton":
					TileEngine.Player.Action(ev.Action);
					return;
				default:
					break;
			}

			// Check other pressed keys
			if (!TileEngine.Player.IgnoreMoveInput)
			{
				if (this.IsDir(ev.KeyPressed))
				{
					TileEngine.MovementManager.EndMove(TileEngine.Player, true);
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
            return (key == Keys.Left || key == Keys.Right || key == Keys.Up || key == Keys.Down);
        }
    }
}