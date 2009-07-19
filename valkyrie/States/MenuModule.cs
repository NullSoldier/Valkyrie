using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ValkyrieLibrary;
using Microsoft.Xna.Framework.Input;
using ValkyrieLibrary.Core;
using ValkyrieLibrary.Input;

namespace ValkyrieLibrary.States
{
    public class MenuModule : IModule
    {
        KeybindController KeyManager;
        
        public MenuModule()
        {
            this.Load();
        }

        #region IModule Members

        public void Tick(GameTime gameTime)
        {
            KeyManager.Update();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Draw(TileEngine.TextureManager.GetTexture("PokeBackground.png"), new Vector2(0,0), Color.White);

            spriteBatch.End();
        }

        public void Load()
        {
            this.KeyManager = new KeybindController();
            this.KeyManager.AddKey(Keys.Enter, "MainMenuEnter");
            this.KeyManager.KeyAction += MainMenu_KeyPressed;
        }

        public void Unload()
        {
            throw new NotImplementedException();
        }

        public void Activate()
        {
            
        }

        public void Deactivate()
        {
            throw new NotImplementedException();
        }

        #endregion

        public void MainMenu_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (e.KeyPressed == Keys.Enter)
            {
                TileEngine.ModuleManager.PushModuleToScreen("Game");
            }
        }
    }
}
