using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Valkyrie.Engine;
using Valkyrie.Engine.Input;
using Microsoft.Xna.Framework.Media;

namespace Valkyrie.Modules
{
	public class MenuModule
		: IModule
	{
		#region Constructor

		public MenuModule (Video video)
		{
			this.video = video;
		}
		
		#endregion

		public string Name { get { return "Menu"; } }

		public void Update(GameTime gameTime)
		{
			//this.KeyManager.Update();
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			spriteBatch.GraphicsDevice.Clear(Color.Black);

			if (this.loaded)
			{
				//spriteBatch.Begin();
				//spriteBatch.Draw(this.player.GetTexture(), new Rectangle(0, 0, 800, 600), Color.White);
				//spriteBatch.End();
			}
		}

		public void Load (IEngineContext enginecontext)
		{
			this.context = enginecontext;

			//this.KeyManager = new KeybindController();
			//this.KeyManager.AddKey(Keys.Enter, "MainMenuEnter");
			//this.KeyManager.KeyUp += MainMenu_KeyPressed;

			//this.player.IsLooped = true;
			//this.player.Play(this.video);

			this.loaded = true;
		}

		public void Unload()
		{
			if (this.loaded)
			{
				this.loaded = false;
			}
		}

		public void Activate()
		{
			this.context.ModuleProvider.PushModule ("Login");

			//this.player.Resume();

			if (!this.loaded)
				this.Load(this.context);
		}

		public void Deactivate()
		{
			//this.player.Pause();

			if (this.loaded)
				this.Unload();
		}

		public void MainMenu_KeyPressed(object sender, KeyPressedEventArgs e)
		{
			if (e.KeyPressed == Keys.Enter && this.loaded)
			{
				this.player.Stop();
				this.context.ModuleProvider.PushModule("Login");
			}
		}

		private IEngineContext context = null;
		//private KeybindController KeyManager;
		private bool loaded = false;
		private Video video = null;
		private VideoPlayer player = new VideoPlayer();
	}
}
