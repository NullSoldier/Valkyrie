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
using ValkyrieLibrary.Animation;

namespace Valkyrie.States
{
    public class MenuModule : IModule
    {
        KeybindController keyManager;
		TitleScreenState state = TitleScreenState.CopyrightIn;

    	readonly FrameAnimation grass = new FrameAnimation(new Rectangle(0, 0, 240, 160), 3, 0.06f);
		
		int currentAlpha = 0;
		int alphaIncrement = 5;
		double sincelast;
		int zoomScale = 5;
        
        public MenuModule()
        {
            this.Load();
        }

        #region IModule Members

        public void Tick(GameTime gameTime)
        {
            keyManager.Update();

			this.sincelast += gameTime.ElapsedRealTime.TotalMilliseconds;

			if (this.state == TitleScreenState.CopyrightIn)
			{
				if (this.sincelast > 10)
					this.sincelast = 0;
				else
					return;

				if (this.currentAlpha < 255)
					this.currentAlpha += 5;
				else
					this.state = TitleScreenState.Copyright;
			}
			else if (this.state == TitleScreenState.Copyright)
			{
				if (sincelast > 5000)
					this.state = TitleScreenState.CopyrightOut;
			}
			else if (this.state == TitleScreenState.CopyrightOut)
			{
				if (this.sincelast > 10)
					this.sincelast = 0;
				else
					return;

				if (this.currentAlpha > 0)
					this.currentAlpha -= 5;
				else
				{
					this.currentAlpha = 255;
					this.state = TitleScreenState.Grass;
				}
			}
			else if (this.state == TitleScreenState.Grass)
			{
				if(this.sincelast > 3000)
				{
					this.sincelast = 0;
					this.state = TitleScreenState.GrassOut;
				}

				this.grass.Update(gameTime);
			}
			else if (this.state == TitleScreenState.GrassOut)
			{
				if (sincelast > 100000)
				{
					this.zoomScale += 1;
					this.sincelast = 0;
				}

				this.grass.Update(gameTime);
			}
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.GraphicsDevice.Clear(Color.Black);

			if (this.state == TitleScreenState.Copyright
				|| this.state == TitleScreenState.CopyrightIn
				|| this.state == TitleScreenState.CopyrightOut)
			{
				spriteBatch.Begin();
				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\Copyright.png"), TileEngine.Camera.Screen, new Color(255, 255, 255, (byte)MathHelper.Clamp(this.currentAlpha, 0, 255)));
				spriteBatch.End();
			}
			else if (this.state == TitleScreenState.Grass)
			{
				spriteBatch.Begin();
				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\GrassSky.png"), TileEngine.Camera.Screen, new Color(255, 255, 255, (byte)MathHelper.Clamp(this.currentAlpha, 0, 255)));
				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\GrassValley.png"), TileEngine.Camera.Screen, this.grass.FrameRectangle, new Color(255, 255, 255, (byte)MathHelper.Clamp(this.currentAlpha, 0, 255)));
				spriteBatch.End();
			}
			else if (this.state == TitleScreenState.GrassOut)
			{
				Matrix transform = Matrix.CreateScale(new Vector3((1 * 1 * 1), (1 * 1 * 1), 0));

				spriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None, transform);
				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\GrassSky.png"), TileEngine.Camera.Screen, new Color(255, 255, 255, (byte)MathHelper.Clamp(this.currentAlpha, 0, 255)));
				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\GrassValley.png"), TileEngine.Camera.Screen, this.grass.FrameRectangle, new Color(255, 255, 255, (byte)MathHelper.Clamp(this.currentAlpha, 0, 255)));
				spriteBatch.End();
			}

			//spriteBatch.Draw(TileEngine.TextureManager.GetTexture("PokeBackground.png"), new Vector2(0, 0), Color.White);
        }

        public void Load()
        {
			TileEngine.TextureManager.AddTexture(@"Intro\Copyright.png");
			TileEngine.TextureManager.AddTexture(@"Intro\GrassValley.png");
			TileEngine.TextureManager.AddTexture(@"Intro\GrassSky.png");

            this.keyManager = new KeybindController();
            this.keyManager.AddKey(Keys.Enter, "MainMenuEnter");
            this.keyManager.KeyAction += MainMenu_KeyPressed;

			this.currentAlpha = 0;
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

        public void MainMenu_KeyPressed (object sender, KeyPressedEventArgs e)
        {
            if (e.KeyPressed == Keys.Enter)
            {
                TileEngine.ModuleManager.PushModuleToScreen("Game");
            }
        }

		private enum TitleScreenState
		{
			CopyrightIn,
			Copyright,
			CopyrightOut,
			GrassIn,
			Grass,
			GrassSky,
			GrassOut
		}
    }
}
