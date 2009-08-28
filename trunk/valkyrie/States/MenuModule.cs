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
		KeybindController KeyManager;
		TitleScreenState state = TitleScreenState.CopyrightIn;

		FrameAnimation grass = new FrameAnimation(new Rectangle(0, 0, 240, 160), 3, 0.06f);
		FrameAnimation sidegrassfar = new FrameAnimation(new Rectangle(0, 161, 240, 160), 1);
		FrameAnimation sidegrassclose = new FrameAnimation(new Rectangle(0, 0, 240, 160), 1);

		FrameAnimation p1Battle = new FrameAnimation(new Rectangle(0, 17, 62, 63), 1);
		FrameAnimation p2Battle = new FrameAnimation(new Rectangle(14, 106, 52, 43), 1);

		FrameAnimation p1Battle2 = new FrameAnimation(new Rectangle(438, 5, 93, 69), 1);
		FrameAnimation p2Battle2 = new FrameAnimation(new Rectangle(415, 78, 111, 101), 1);

		FrameAnimation p1Battle3 = new FrameAnimation(new Rectangle(438, 5, 93, 69), 1);
		FrameAnimation p2Battle3 = new FrameAnimation(new Rectangle(415, 78, 111, 101), 1);

		int currentAlpha = 0;
		int alphaIncrement = 5;
		double sincelast;
		double timeonframe;
		int zoomScale = 5;

		int scrollaccell = 0;
		int increasedscroll = 0;

		// Battle2
		int bobvalue = 0;
		int bobmodifier = 2;

		// Battle
		int scrollx1 = 0;
		int scrollx2 = 0;

		public MenuModule()
		{
			this.Load();
		}

		#region IModule Members

		public void Update(GameTime gameTime)
		{
			KeyManager.Update();

			this.sincelast += gameTime.ElapsedRealTime.TotalMilliseconds;
			this.timeonframe += gameTime.ElapsedGameTime.TotalMilliseconds;

			if (this.state == TitleScreenState.CopyrightIn)
			{
				if (this.sincelast > 10)
					this.sincelast = 0;
				else
					return;

				if (this.currentAlpha < 255)
					this.currentAlpha += 5;
				else
				{
					this.state = TitleScreenState.Copyright;
					this.timeonframe = 0;
				}
			}
			else if (this.state == TitleScreenState.Copyright)
			{
				if (sincelast > 3000)
				{
					this.state = TitleScreenState.CopyrightOut;
					this.timeonframe = 0;
				}
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
					this.timeonframe = 0;
				}
			}
			else if (this.state == TitleScreenState.Grass)
			{
				if (this.sincelast > 3000)
				{
					this.sincelast = 0;
					this.state = TitleScreenState.GrassOut;
					this.timeonframe = 0;
				}

				this.grass.Update(gameTime);
			}
			else if (this.state == TitleScreenState.GrassOut)
			{
				if (sincelast > 10)
				{
					this.zoomScale += 20;
					this.sincelast = 0;
				}

				if (zoomScale >= 260)
				{
					this.state = TitleScreenState.Battle;
					this.scrollx2 = (TileEngine.Camera.Screen.Width * -1);
					this.timeonframe = 0;
				}

				this.grass.Update(gameTime);
			}
			else if (this.state == TitleScreenState.Battle)
			{
				if (this.scrollx1 >= TileEngine.Camera.Screen.Width)
					this.scrollx1 = (TileEngine.Camera.Screen.Width * -1);

				if (this.scrollx2 >= TileEngine.Camera.Screen.Width)
					this.scrollx2 = (TileEngine.Camera.Screen.Width * -1);

				if (sincelast > 10)
				{
					this.scrollx1 += 2;
					this.scrollx2 += 2;

					sincelast = 0;
				}

				if (this.timeonframe > 2000)
				{
					this.timeonframe = 0;
					this.state = TitleScreenState.Battle2;
				}
			}
			else if (this.state == TitleScreenState.Battle2)
			{
				if (sincelast > 200)
				{
					this.sincelast = 0;

					this.bobvalue += this.bobmodifier;

					if (this.bobvalue >= 2 || this.bobvalue < 1)
						this.bobmodifier = this.bobmodifier * -1;
				}

				if (this.timeonframe > 2500)
				{
					this.timeonframe = 0;
					this.state = TitleScreenState.Battle3In;

					this.scrollx1 = 0;
					this.scrollx2 = (TileEngine.Camera.Screen.Width * -1);
					this.scrollaccell = 10;
				}
			}
			else if (this.state == TitleScreenState.Battle3In)
			{
				if (this.scrollx1 >= TileEngine.Camera.Screen.Width)
					this.scrollx1 = (TileEngine.Camera.Screen.Width * -1);

				if (this.scrollx2 >= TileEngine.Camera.Screen.Width)
					this.scrollx2 = (TileEngine.Camera.Screen.Width * -1);

				if (this.sincelast > 20)
				{
					this.scrollx1 += this.scrollaccell;
					this.scrollx2 += this.scrollaccell;

					if (timeonframe > 500 && this.increasedscroll == 0)
					{
						this.scrollaccell = 1;
						this.increasedscroll++;
					}
				}
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
				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\Cline.png"), TileEngine.Camera.Screen, Color.White);
				spriteBatch.End();
			}
			else if (this.state == TitleScreenState.GrassOut)
			{
				Rectangle rect = TileEngine.Camera.Screen;
				rect.X -= this.zoomScale;
				rect.Y -= (this.zoomScale / 2);
				rect.Width += (this.zoomScale * 2);
				rect.Height += (this.zoomScale * 2);

				spriteBatch.Begin();
				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\GrassSky.png"), rect, Color.White);
				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\GrassValley.png"), rect, this.grass.FrameRectangle, new Color(255, 255, 255, (byte)MathHelper.Clamp(this.currentAlpha, 0, 255)));
				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\Cline.png"), TileEngine.Camera.Screen, Color.White);
				spriteBatch.End();
			}
			else if (this.state == TitleScreenState.Battle)
			{
				Rectangle screen1 = TileEngine.Camera.Screen;
				screen1.X = this.scrollx1;

				Rectangle screen2 = TileEngine.Camera.Screen;
				screen2.X = this.scrollx2;

				Rectangle grass1 = TileEngine.Camera.Screen;
				grass1.X = (this.scrollx1 * -1);

				Rectangle grass2 = TileEngine.Camera.Screen;
				grass2.X = (this.scrollx2 * -1);


				spriteBatch.Begin();
				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\SideGrass.png"), screen1, this.sidegrassfar.FrameRectangle, Color.White);
				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\SideGrass.png"), screen2, this.sidegrassfar.FrameRectangle, Color.White);

				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\PokemonSprites.png"), new Rectangle(40, 190, 230, 229), this.p1Battle.FrameRectangle, Color.White);
				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\PokemonSprites.png"), new Rectangle(500, 235, 200, 190), this.p2Battle.FrameRectangle, Color.White);

				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\GrassOverlay.png"), grass1, Color.White);
				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\GrassOverlay.png"), grass2, Color.White);
				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\Cline.png"), TileEngine.Camera.Screen, Color.White);
				spriteBatch.End();
			}
			else if (state == TitleScreenState.Battle2)
			{
				spriteBatch.GraphicsDevice.Clear(Color.Orange);
				spriteBatch.Begin();

				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\PokemonSprites.png"), new Rectangle(38, 250 + (this.bobvalue * -1), 240, 234), this.p1Battle2.FrameRectangle, Color.White);
				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\PokemonSprites.png"), new Rectangle(503, 117 + this.bobvalue, 300, 364), this.p2Battle2.FrameRectangle, Color.White);
				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\Cline.png"), TileEngine.Camera.Screen, Color.White);

				spriteBatch.End();
			}
			else if (state == TitleScreenState.Battle3In)
			{
				Rectangle screen1 = TileEngine.Camera.Screen;
				screen1.X = this.scrollx1;

				Rectangle screen2 = TileEngine.Camera.Screen;
				screen2.X = this.scrollx2;

				spriteBatch.Begin();

				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\SideGrass.png"), screen1, this.sidegrassfar.FrameRectangle, Color.White);
				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\SideGrass.png"), screen2, this.sidegrassfar.FrameRectangle, Color.White);

				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\PokemonSprites.png"), new Rectangle(-110 + screen1.X, 220, 230, 229), this.p1Battle3.FrameRectangle, Color.White);
				//spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\PokemonSprites.png"), new Rectangle(600 + (screen1.X * -1), 190, 230, 229), this.p2Battle3.FrameRectangle, Color.White);

				spriteBatch.Draw(TileEngine.TextureManager.GetTexture(@"Intro\Cline.png"), TileEngine.Camera.Screen, Color.White);

				spriteBatch.End();
			}
			else if (state == TitleScreenState.FinalTitle)
			{
				spriteBatch.Begin();
				spriteBatch.Draw(TileEngine.TextureManager.GetTexture("PokeBackground.png"), new Vector2(0, 0), Color.White);
				spriteBatch.End();
			}
		}

		public void Load()
		{
			TileEngine.TextureManager.AddTexture(@"Intro\Copyright.png");
			TileEngine.TextureManager.AddTexture(@"Intro\GrassValley.png");
			TileEngine.TextureManager.AddTexture(@"Intro\GrassSky.png");
			TileEngine.TextureManager.AddTexture(@"Intro\Cline.png");
			TileEngine.TextureManager.AddTexture(@"Intro\SideGrass.png");
			TileEngine.TextureManager.AddTexture(@"Intro\GrassOverlay.png");
			TileEngine.TextureManager.AddTexture(@"Intro\PokemonSprites.png");

			this.KeyManager = new KeybindController();
			this.KeyManager.AddKey(Keys.Enter, "MainMenuEnter");
			this.KeyManager.KeyAction += MainMenu_KeyPressed;

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

		public void MainMenu_KeyPressed(object sender, KeyPressedEventArgs e)
		{
			if (e.KeyPressed == Keys.Enter)
			{
				if (this.state != TitleScreenState.FinalTitle)
					this.state = TitleScreenState.FinalTitle;
				else if (this.state == TitleScreenState.FinalTitle)
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
			GrassOut,
			BattleIn,
			Battle,
			Battle2,
			Battle3In,
			Battle3,
			FinalTitle
		}
	}
}
