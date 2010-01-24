using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Valkyrie.Core;
using Valkyrie.Engine;
using Valkyrie.Library;
using Valkyrie.Providers;
using Valkyrie.Modules;
using Valkyrie.Engine.Providers;
using Valkyrie.Library.Providers;
using Microsoft.Xna.Framework.Media;
using Valkyrie.Library.Managers;
using Valkyrie.Messages.Valkyrie.Authentication;
using Valkyrie.Characters;
using Valkyrie;

namespace Valkyrie
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
	
    public class PokeGame : Game
    {
		#region Constructors

		public PokeGame ()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.ApplyChanges();

			Content.RootDirectory = "Content";
		}

		#endregion

		#region Public Properties & Methods

		public ValkyrieEngine Engine { get; set; }

		public GraphicsDeviceManager graphics { get; set; }
		public SpriteBatch spriteBatch { get; set; }
		public static SpriteFont font { get; set; }
		public static string[] logindata { get; set; }

		public bool ExitingGame
		{
			get { return this.exitinggame; }
			set { this.exitinggame = value; }
		}

		public void NetworkDisconnected (object sender, ConnectionEventArgs ev)
		{
			if(this.ExitingGame)
				return;

			MessageBox(new IntPtr(0), "Connection to server lost.", "Disconnected", 0);

			this.Engine.Unload();
			this.Exit();
		}

		#endregion

		private float deltaFPSTime = 0;
		private bool exitinggame = false;

		private string version = Assembly.GetExecutingAssembly ().GetName ().Version.ToString ();
		private Vector2 versionloc = Vector2.Zero;

		private EngineConfiguration LoadEngineConfiguration ()
		{
			FileInfo info = new FileInfo(Path.Combine(Environment.CurrentDirectory, "Data/TileEngineConfig.xml"));
			if(!info.Exists)
				throw new FileNotFoundException("Engine config is missing from data directory!");

			XmlDocument doc = new XmlDocument();
			doc.Load(info.FullName);

			XmlNodeList nodes = doc.GetElementsByTagName("Config");

			return new EngineConfiguration(nodes[0].ChildNodes.OfType<XmlNode>().ToDictionary(x => (EngineConfigurationName)Enum.Parse(typeof(EngineConfigurationName), x.Name), x => x.InnerText));
		}

        protected override void Initialize()
        {
			this.Engine = new ValkyrieEngine(this.LoadEngineConfiguration());
			var description = Assembly.GetExecutingAssembly ().GetCustomAttributes (typeof (AssemblyDescriptionAttribute), true).FirstOrDefault ();
			if(description != null)
				this.version = string.Format ("{0} {1}", ((AssemblyDescriptionAttribute) description).Description, this.version);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

			PokeGame.font = Content.Load<SpriteFont>("GameTextFont");

			ValkyrieWorldManager worldmanager = new ValkyrieWorldManager(new Assembly[] { Assembly.GetExecutingAssembly(), Assembly.Load("ValkyrieLibrary") });
			ValkyrieTextureManager texturemanager = new ValkyrieTextureManager(this.Content, this.GraphicsDevice);
			
			ValkyrieSoundProvider sound = new ValkyrieSoundProvider();
        	sound.LoadEngineContext (Engine);

			this.Engine.Load(new PokeSceneProvider(this.GraphicsDevice),
				new ValkyrieEventProvider(),
				new PokeNetworkProvider(),
				sound,
				new ValkyrieModuleProvider(),
				new ValkyrieMovementProvider(),
				new ValkyrieCollisionProvider(),
				new ValkyrieVoiceChatProvider (sound),
				worldmanager,
				texturemanager,
				new ValkyrieSoundManager());

			this.Engine.ModuleProvider.AddModule(new MenuModule(Content.Load<Video>("PokemonIntro")));
			this.Engine.ModuleProvider.AddModule(new LoginModule());
			this.Engine.ModuleProvider.AddModule(new GameModule(this.GraphicsDevice));
			this.Engine.ModuleProvider.GetModule("Menu").Load(this.Engine);
			this.Engine.ModuleProvider.GetModule("Login").Load(this.Engine);
			this.Engine.ModuleProvider.PushModule("Menu");

			this.versionloc = new Vector2 (this.graphics.PreferredBackBufferWidth - (PokeGame.font.MeasureString (this.version).X + 5), this.graphics.PreferredBackBufferHeight - 20);
        }

        protected override void UnloadContent()
        {
			this.ExitingGame = true;

			var player = (PokePlayer)this.Engine.SceneProvider.GetPlayer("player1");

			// Send logout message to the server
			if(this.Engine.IsLoaded && this.Engine.NetworkProvider.IsConnected && player != null)
			{
				var msg = new LogoutMessage() { NetworkID = (uint)player.ID };

				this.Engine.NetworkProvider.Send(msg);
			}
			
			this.Engine.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
			this.Engine.Update(gameTime);

            base.Update(gameTime);
        }

		protected override void Draw (GameTime gameTime)
		{
			/* Render FPS */
			float elapsed = (float)gameTime.ElapsedRealTime.TotalSeconds;

			float fps = 1 / elapsed;
			deltaFPSTime += elapsed;
			if(deltaFPSTime > 1)
			{
				Window.Title = "PokeGame [" + fps.ToString() + " FPS]";
				deltaFPSTime -= 1;
			}

			/* Draw the game */
			this.Engine.Draw(spriteBatch, gameTime);

			spriteBatch.Begin ();
			spriteBatch.DrawString (PokeGame.font, this.version, this.versionloc, Color.Black);

			if(this.Engine.IsLoaded)
			{
				string voicechatstatus = string.Format("Connected to Voice Chat: {0}", this.Engine.VoiceChatProvider.IsConnected.ToString ());
				float statusmod = PokeGame.font.MeasureString (voicechatstatus).X - 90;

				spriteBatch.DrawString (PokeGame.font, "Connected to Voice Chat: " + this.Engine.VoiceChatProvider.IsConnected.ToString (), new Vector2 (this.versionloc.X - statusmod, this.versionloc.Y - 20), Color.Black);
			}

			spriteBatch.End ();

			base.Draw(gameTime);
		}

		#region Statics & Internals

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern uint MessageBox (IntPtr hWnd, String text, String caption, uint type);

		#endregion
	}
}
