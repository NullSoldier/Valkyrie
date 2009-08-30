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
using Microsoft.Xna.Framework.Content;
using SeeSharp.Xna.Video;
using System.IO;

namespace Valkyrie.States
{
	public class MenuModule : IModule
	{
		private bool Loaded = false;

		private VideoPlayerOld player;

		private KeybindController KeyManager;

		#region IModule Members

		public void Update(GameTime gameTime)
		{
			this.KeyManager.Update();

			if(this.Loaded)
				this.player.Update();
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			spriteBatch.GraphicsDevice.Clear(Color.Black);

			if (this.Loaded)
			{
				spriteBatch.Begin();
				spriteBatch.Draw(this.player.OutputFrame, TileEngine.Camera.Screen, Color.White);
				spriteBatch.End();
			}
		}

		public void Load()
		{
			this.KeyManager = new KeybindController();
			this.KeyManager.AddKey(Keys.Enter, "MainMenuEnter");
			this.KeyManager.KeyUp += MainMenu_KeyPressed;


			try
			{
				this.player = new VideoPlayerOld(Path.Combine(Directory.GetCurrentDirectory(), @"Media\PokemonIntro.avi"), TileEngine.GraphicsDevice);
				this.player.Play();
			}
			catch (System.Exception)
			{
				// Can't play video for some reason, skip.
				TileEngine.ModuleManager.PushModuleToScreen("Login");
			}

			this.Loaded = true;
		}

		public void Unload()
		{
			if (this.Loaded)
			{
				if(this.player != null)
					this.player.Dispose();

				this.Loaded = false;
			}
		}

		public void Activate()
		{
			if (!this.Loaded)
				this.Load();
		}

		public void Deactivate()
		{
			if( this.player != null)
				this.player.Stop();

			if (this.Loaded)
				this.Unload();
		}

		#endregion

		public void MainMenu_KeyPressed(object sender, KeyPressedEventArgs e)
		{
			if (e.KeyPressed == Keys.Enter)
			{
				TileEngine.ModuleManager.PushModuleToScreen("Login");
			}
		}

	}
}
