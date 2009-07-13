using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using valkyrie.Core;
using System.Xml;
using System.IO;
using valkyrie;
using Microsoft.Xna.Framework.Input;
using ValkyrieLibrary.Player;

namespace ValkyrieLibrary.States
{
    class GameModule : IModule
    {
        private bool Loaded = false;
        private KeybindController KeybindController = new KeybindController();

        public GameModule() { }

        #region IModule Members

        public void Tick(GameTime gameTime)
        {
            this.KeybindController.Update();
			TileEngine.Player.Update(gameTime);
			TileEngine.Map.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //SpriteBatch spriteBatch, bool DrawBaseLayer, bool DrawMiddleLayer, bool DrawTopLayer, bool DrawCharacters)

            spriteBatch.GraphicsDevice.Viewport = TileEngine.Camera.Viewport;
            spriteBatch.GraphicsDevice.Clear(Color.Black);
            
            TileEngine.DrawBaseLayer(spriteBatch);
            TileEngine.DrawMiddleLayer(spriteBatch);
			TileEngine.DrawCharacters(spriteBatch);
			TileEngine.DrawTopLayer(spriteBatch);
        }

        public void Load()
        {
            // Load Textures
            TileEngine.TextureManager.AddTexture("PokemonTiles.png");

            // Player
            TileEngine.Player = new PokePlayer();
			TileEngine.Player.Gender = Genders.Male;
            TileEngine.Player.Sprite = TileEngine.TextureManager.GetTexture("MaleSprite.png");
			TileEngine.Player.Location = new Point(96, 96);

            // Map
            Map map = new Map();
            map.LoadMap(new FileInfo(TileEngine.Configuration["MapRoot"] + "\\" + TileEngine.Configuration["EntryMap"]));

            TileEngine.SetMap(map);

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
            switch (ev.Action)
            {
                case "MoveUp":
					TileEngine.Player.Move(new Point(TileEngine.Player.Location.X, TileEngine.Player.Location.Y - 32));
                    break;
                case "MoveDown":
					TileEngine.Player.Move(new Point(TileEngine.Player.Location.X, TileEngine.Player.Location.Y + 32));
                    break;
                case "MoveLeft":
					TileEngine.Player.Move(new Point(TileEngine.Player.Location.X - 32, TileEngine.Player.Location.Y));
                    break;
                case "MoveRight":
					TileEngine.Player.Move(new Point(TileEngine.Player.Location.X + 32, TileEngine.Player.Location.Y));
                    break;
                default:
                    break;
            }
        }
    }
}
