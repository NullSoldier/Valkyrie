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

namespace Valkyrie.States
{
    public class GameModule : IModule
    {
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
			TileEngine.Camera.CenterOnPoint(new ScreenPoint(TileEngine.Player.Location.X + (TileEngine.Player.CurrentAnimation.FrameRectangle.Width / 2), TileEngine.Player.Location.Y + (TileEngine.Player.CurrentAnimation.FrameRectangle.Height / 2)));

            this.KeybindController.Update();

			TileEngine.Player.Update(gameTime);
			TileEngine.MovementManager.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.GraphicsDevice.Viewport = TileEngine.Camera.Viewport;
            spriteBatch.GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin();

            TileEngine.DrawAllLayers(spriteBatch, true);
            TileEngine.DrawOverlay(spriteBatch);

			spriteBatch.End();
        }

        public void Load()
        {
            // Player
			PokePlayer player = new PokePlayer();
			player.Gender = Genders.Male;
			player.Sprite = TileEngine.TextureManager.GetTexture("MaleSprite.png");
			TileEngine.Player = player;

			TileEngine.WorldManager.SetWorld("Main", "Default");

            this.KeybindController.AddKey(Keys.Left, "MoveLeft");
            this.KeybindController.AddKey(Keys.Up, "MoveUp");
            this.KeybindController.AddKey(Keys.Down, "MoveDown");
            this.KeybindController.AddKey(Keys.Right, "MoveRight");

            this.KeybindController.KeyDown += this.GameModule_KeyDown;
			this.KeybindController.KeyUp += this.GameModule_KeyUp;

			TileEngine.Player.StartedMoving += this.GameModule_StartedMoving;
			TileEngine.Player.StoppedMoving += this.GameModule_StoppedMoving;
			TileEngine.Player.TileLocationChanged += this.GameModule_TileLocationChanged;
			TileEngine.Player.Collided += this.GameModule_Collided;

			TileEngine.Player.TileLocationChanged += TestTileLocationChanged;
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
            throw new NotImplementedException();
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
		}

        public void GameModule_KeyDown(object sender, KeyPressedEventArgs ev)
        {
            if (this.IsDir(ev.KeyPressed))
            {
                UpdateDirection(ev.KeyPressed);
            }

            switch (ev.Action)
            {
                case "AButton":
                case "BButton":
					TileEngine.MovementManager.Move(TileEngine.Player, new ScreenPoint(10, 10));
                    TileEngine.Player.Action(ev.Action);
                    return;
            }

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

		public void GameModule_KeyUp(object sender, KeyPressedEventArgs ev)
		{
			if (!TileEngine.Player.IgnoreMoveInput)
			{
				if (this.IsDir(ev.KeyPressed))
					TileEngine.MovementManager.EndMove(TileEngine.Player, true);
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