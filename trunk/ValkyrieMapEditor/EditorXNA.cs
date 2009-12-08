using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using Valkyrie.Library;
using Valkyrie.Library.Core;
using ValkyrieMapEditor.Core;
using System.Reflection;
using Valkyrie.Library.Events;
using Valkyrie.Engine;
using System.Xml;
using Valkyrie.Library.Providers;
using Valkyrie.Library.Managers;


namespace ValkyrieMapEditor
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class EditorXNA : Game
	{
		#region Constructors

		public EditorXNA (IntPtr drawSurface, IntPtr drawTilesSurface)
		{
			InitializeComponents();

			graphics = new GraphicsDeviceManager(this);
			graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings;

			Content.RootDirectory = "Content";

			this.drawSurface = drawSurface;
			this.drawTilesSurface = drawTilesSurface;

			/* Hook to the window because When the window isn't visible
			 * we need to ignore window events from XNA like moving the
			 * mouse. */
			Control.FromHandle((this.Window.Handle)).VisibleChanged += new EventHandler(Game_VisibleChanged);
		}

		#endregion

		#region Public Properties and Methods

		public ValkyrieEngine Engine
		{
			get { return this.valkyrieengine; }
			set { this.valkyrieengine = value; }
		}

		public IEditorComponent CurrentComponent
		{
			get
			{
				if(this.currentcomponent == null)
					this.currentcomponent = this.componentlist[ComponentID.Draw];

				return currentcomponent;
			}
		}

		public bool MapChanged
		{
			get { return this.mapchanged; }
			set { this.mapchanged = value; }
		}

		public void EnlistEvents (PictureBox pictureBox)
		{
			pictureBox.MouseDown += this.MouseDown;
			pictureBox.MouseUp += this.MouseUp;
			pictureBox.MouseMove += this.MouseMove;
			pictureBox.MouseClick += this.MouseClicked;
		}

		public void SwitchToComponent (ComponentID component)
		{
			if(this.componentlist.Keys.Contains(component))
				currentcomponent = this.componentlist[component];
		}

		#endregion

		#region XNA Game Members

		protected override void Initialize()
		{
			this.Engine = new ValkyrieEngine(this.LoadEngineConfiguration());

			Mouse.WindowHandle = this.drawSurface;
			
			base.Initialize();
		}

		protected override void LoadContent()
		{
			EditorXNA.font = Content.Load<SpriteFont>("GameTextFont");
			EditorXNA.graphicsDevice = this.GraphicsDevice;

			this.spriteBatch = new SpriteBatch(GraphicsDevice);

			ValkyrieWorldManager worldmanager = new ValkyrieWorldManager(new XMLMapProvider(new Assembly[] { }));

			this.Engine.Load(new ValkyrieSceneProvider(this.GraphicsDevice),
				new ValkyrieEventProvider(),
				new ValkyrieNetworkProvider(),
				new ValkyrieSoundProvider(),
				new ValkyrieModuleProvider(),
				new ValkyrieMovementProvider(),
				new ValkyrieCollisionProvider(),
				worldmanager,
				new ValkyrieTextureManager(this.Content, this.GraphicsDevice),
				new ValkyrieSoundManager());

			this.Engine.SceneProvider.AddCamera("camera1", new BaseCamera(this.GraphicsDevice.Viewport) { WorldName = "Default" });
			this.Engine.SceneProvider.GetCamera("camera1").CenterOriginOnPoint(0, 0);
			
			this.SelectionSprite = Texture2D.FromFile(this.GraphicsDevice, "Graphics/EditorSelection.png");

			this.Render.LoadContent(this.GraphicsDevice, this.Engine);
			foreach (var component in this.componentlist)
			{
				component.Value.LoadContent(this.GraphicsDevice, this.Engine);
			}
		}

		protected override void UnloadContent()
		{
			this.Engine.Unload();
		}

		protected override void Update (GameTime gameTime)
		{
			this.Render.Update(gameTime);
			this.CurrentComponent.Update(gameTime);

			base.Update(gameTime);
		}

		protected override void Draw (GameTime gameTime)
		{
			this.RenderFPS(gameTime);

			GraphicsDevice.Clear(Color.Gray);

			this.Render.Draw(this.spriteBatch);

			spriteBatch.Begin();
			this.CurrentComponent.Draw(this.spriteBatch);
			spriteBatch.End();

			base.Draw(gameTime);
		}

		#endregion

		#region Public Callbacks

		public void Resized(object sender, ScreenResizedEventArgs e)
		{
			this.Render.OnSizeChanged(sender, e);
			this.CurrentComponent.OnSizeChanged(sender, e);
		}

		public void Scrolled(object sender, ScrollEventArgs e)
		{
			this.Render.OnScrolled(sender, e);
			this.CurrentComponent.OnScrolled(sender, e);
		}

		public void MouseDown(object sender, MouseEventArgs e)
		{
			this.Render.OnMouseDown(sender, e);
			this.CurrentComponent.OnMouseDown(sender, e);
		}

		public void MouseMove(object sender, MouseEventArgs e)
		{
			this.Render.OnMouseMove(sender, e);
			this.CurrentComponent.OnMouseMove(sender, e);
		}

		public void MouseUp(object sender, MouseEventArgs e)
		{
			this.Render.OnMouseUp(sender, e);
			this.CurrentComponent.OnMouseUp(sender, e);
		}

		public void MouseClicked(object sender, MouseEventArgs e)
		{
			this.Render.OnMouseClicked(sender, e);
			this.CurrentComponent.OnMouseClicked(sender, e);
		}

		#endregion

		private ValkyrieEngine valkyrieengine = null;
		private SpriteBatch spriteBatch;
		private IntPtr drawSurface;
		private IntPtr drawTilesSurface;
		private Texture2D SelectionSprite;
		private RenderComponent Render = null;
		private Dictionary<ComponentID, IEditorComponent> componentlist = new Dictionary<ComponentID, IEditorComponent>();
		private IEditorComponent currentcomponent = null;
		private float deltaFPSTime = 0;
		private bool mapchanged = false;

		private void InitializeComponents ()
		{
			this.Render = new RenderComponent();

			this.componentlist = new Dictionary<ComponentID, IEditorComponent>();
			this.componentlist.Add(ComponentID.Draw, new DrawComponent());
			this.componentlist.Add(ComponentID.Events, new EventsComponent());
			this.componentlist.Add(ComponentID.Collsion, new CollisionComponent());
		}

		private void RenderFPS(GameTime gameTime)
		{
			float elapsed = (float)gameTime.ElapsedRealTime.TotalSeconds;

			float fps = 1 / elapsed;
			deltaFPSTime += elapsed;
			if(deltaFPSTime > 1)
			{
				Window.Title = "MapEditor  [" + fps.ToString() + " FPS]";
				deltaFPSTime -= 1;
			}
		}

		private void graphics_PreparingDeviceSettings (object sender, PreparingDeviceSettingsEventArgs e)
		{
			e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = drawSurface;
		}

		private void Game_VisibleChanged (object sender, EventArgs e)
		{
			if(Control.FromHandle((this.Window.Handle)).Visible == true)
				Control.FromHandle((this.Window.Handle)).Visible = false;
		}

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

		#region Statics and Internals

		public static SpriteFont font;
		public static GraphicsDevice graphicsDevice = null;
		public static GraphicsDeviceManager graphics = null;

		static public Texture2D CreateSelectRectangle(int width, int height)
		{
			int limit = 4;

			if (width <= limit * 2 || height <= limit * 2)
				return null;

			// Create the rectangle texture, but it will have no color! lets fix that
			Texture2D rectangleTexture = new Texture2D(EditorXNA.graphicsDevice, width, height, 1, TextureUsage.None, SurfaceFormat.Color);
		   
			Color[] color = new Color[width * height]; // Set the color to the amount of pixels
			Color black = new Color(0, 0, 0, 255);
			Color blacknoalpha = new Color(0, 0, 0, 0);
			Color white = new Color(255, 255, 255, 255);

			// Loop through all the colors setting them to whatever values we want
			for (int y = 1; y < height; y++)
			{
				for (int x = 1; x < width; x++)
				{
					if ((x < limit) || (x > width - limit - 2) || (y < limit) || (y > height - limit - 2))
					{
						color[x + (y * width)] = white;
					}
					else
					{
						color[x + (y * width)] = blacknoalpha;
					}
				}
			}

			// Outer four
			for(int y = 0; y < height; y++)
				color[0 + (y * width)] = black;

			for (int y = 0; y < height; y++)
				color[width - 1 + (y * width)] = black;

			for (int x = 0; x < width; x++)
				color[x + (0 * width)] = black;

			for (int x = 0; x < width; x++)
				color[x + ((height - 1) * width)] = black;

			// Inner four
			for (int y = limit; y < (height - limit -1); y++)
				color[limit + (y * width)] = black;

			for (int y = limit; y < (height - limit); y++)
				color[width - limit - 1 + (y * width)] = black;

			for (int x = limit; x < (width - limit - 1); x++)
				color[x + (limit * width)] = black;

			for (int x = limit; x < (width - limit); x++)
				color[x + ((height - limit - 1) * width)] = black;

			rectangleTexture.SetData(color);//set the color data on the texture
			return rectangleTexture;
		}

		static public Texture2D CreateSelectRectangleFilled (int width, int height, Color border, Color mainfill)
		{
			Texture2D texture = new Texture2D(EditorXNA.graphicsDevice, width, height, 1, TextureUsage.None, SurfaceFormat.Color);
			Color[] color = new Color[width * height];

			for(int i = 0; i < width * height; i++)
				color[i] = mainfill;

			// Outer four
			for(int y = 0; y < height; y++)
				color[0 + (y * width)] = border;

			for(int y = 0; y < height; y++)
				color[width - 1 + (y * width)] = border;

			for(int x = 0; x < width; x++)
				color[x + (0 * width)] = border;

			for(int x = 0; x < width; x++)
				color[x + ((height - 1) * width)] = border;

			texture.SetData(color);//set the color data on the texture
			return texture;
		}
		
		#endregion
	}
}
