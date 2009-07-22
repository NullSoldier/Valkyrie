﻿using System;
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
using ValkyrieLibrary.Core.Points;
using Valkyrie.Characters;

namespace Valkyrie.States
{
    class GameModule : IModule
    {
        private bool Loaded = false;
        private KeybindController KeybindController = new KeybindController();

        private Keys CrntDir;

        public GameModule() 
        {
            KeybindController.LoadKeys();
        }

        #region IModule Members

        public void Tick(GameTime gameTime)
        {
			TileEngine.Camera.CenterOnPoint(new ScreenPoint(TileEngine.Player.Location.X + (TileEngine.Player.CurrentAnimation.FrameRectangle.Width / 2), TileEngine.Player.Location.Y + (TileEngine.Player.CurrentAnimation.FrameRectangle.Height / 2)));

            this.KeybindController.Update();
			TileEngine.Player.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.GraphicsDevice.Viewport = TileEngine.Camera.Viewport;
            spriteBatch.GraphicsDevice.Clear(Color.Black);

            TileEngine.DrawAllLayers(spriteBatch, true);
            TileEngine.DrawOverlay(spriteBatch);
        }

        public void Load()
        {
            // Player
			PokePlayer player = new PokePlayer();
			player.Gender = Genders.Male;
			player.Sprite = TileEngine.TextureManager.GetTexture("MaleSprite.png");
			player.Location = new MapPoint(14, 62).ToScreenPoint();

			TileEngine.Player = player;


            this.KeybindController.AddKey(Keys.Left, "MoveLeft");
            this.KeybindController.AddKey(Keys.Up, "MoveUp");
            this.KeybindController.AddKey(Keys.Down, "MoveDown");
            this.KeybindController.AddKey(Keys.Right, "MoveRight");
            this.KeybindController.KeyAction += this.GameModule_KeyPressed;
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

        public void GameModule_KeyPressed(object sender, KeyPressedEventArgs ev)
        {
            if (this.IsDir(ev.KeyPressed))
            {
                UpdateDirection(ev.KeyPressed);
            }

            switch (ev.Action)
            {
                case "AButton":
                case "BButton":
                    TileEngine.Player.Action(ev.Action);
                    return;
            }

            switch (this.KeybindController.GetKeyAction(CrntDir))
            {
                case "MoveUp":
					TileEngine.Player.Move(new ScreenPoint(TileEngine.Player.Location.X, TileEngine.Player.Location.Y - 32));
                    break;
                case "MoveDown":
					TileEngine.Player.Move(new ScreenPoint(TileEngine.Player.Location.X, TileEngine.Player.Location.Y + 32));
                    break;
                case "MoveLeft":
					TileEngine.Player.Move(new ScreenPoint(TileEngine.Player.Location.X - 32, TileEngine.Player.Location.Y));
                    break;
                case "MoveRight":
					TileEngine.Player.Move(new ScreenPoint(TileEngine.Player.Location.X + 32, TileEngine.Player.Location.Y));
                    break;
            }
        }

        public void UpdateDirection(Keys NewDir)
        {
            if (!this.KeybindController.LastKeys.Contains<Keys>(NewDir) || !this.KeybindController.CrntKeys.Contains<Keys>(CrntDir))
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