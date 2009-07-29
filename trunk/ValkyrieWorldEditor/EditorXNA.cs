//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.IO;
//using System.Windows.Forms;

//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;
//using Microsoft.Xna.Framework.Net;
//using Microsoft.Xna.Framework.Storage;

//using ValkyrieLibrary;
//using ValkyrieLibrary.Maps;
//using ValkyrieLibrary.Core;
//using ValkyrieWorldEditor.Core;
//using ValkyrieWorldEditor.Forms;

//namespace ValkyrieWorldEditor
//{
//    /// <summary>
//    /// This is the main type for your game
//    /// </summary>
//    public class EditorXNA : Microsoft.Xna.Framework.Game
//    {
//        public static SpriteFont font;
//        public static GraphicsDevice graphicsDevice = null;
//        public static GraphicsDeviceManager graphics = null;

//        private Dictionary<ComponentID, IEditorComponent> ComponentList;
//        private IEditorComponent curComponent = null;
//        public RenderComponent Render = null;

//        public IEditorComponent CurComponent
//        {
//            get
//            {
//                if (this.curComponent == null)
//                    this.curComponent = this.ComponentList[ComponentID.Hand];

//                return curComponent;
//            }
//        }

//        SpriteBatch spriteBatch;

//        private PictureBox drawSurface;
//        private PreviewRender previewSurface;

//        public EditorXNA()
//        {
//            Init();

//            graphics = new GraphicsDeviceManager(this);
//            Content.RootDirectory = "Content";
//        }

//        public EditorXNA(PictureBox drawSurface, PreviewRender previewSurface)
//        {
//            Init();

//            graphics = new GraphicsDeviceManager(this);
//            Content.RootDirectory = "Content";

//            this.previewSurface = previewSurface;
//            this.drawSurface = drawSurface;

//            graphics.PreparingDeviceSettings +=
//                new EventHandler<PreparingDeviceSettingsEventArgs>(PreparingDeviceSettings);

//            System.Windows.Forms.Control.FromHandle((this.Window.Handle)).VisibleChanged += new EventHandler(VisibleChanged);
//        }

//        private void Init()
//        {
//            ComponentList = new Dictionary<ComponentID, IEditorComponent>();
//            this.Render = new RenderComponent();

//            this.ComponentList = new Dictionary<ComponentID, IEditorComponent>();
//            this.ComponentList.Add(ComponentID.Select, new SelectComponent());
//            this.ComponentList.Add(ComponentID.Hand, new HandComponent());
//        }

//        void PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
//        {
//            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = drawSurface.Handle;
//        }

//        private void VisibleChanged(object sender, EventArgs e)
//        {
//            if (System.Windows.Forms.Control.FromHandle((this.Window.Handle)).Visible == true)
//                System.Windows.Forms.Control.FromHandle((this.Window.Handle)).Visible = false;
//        }


//        protected override void Initialize()
//        {
//            base.Initialize();
//            //Mouse.WindowHandle = this.drawSurface.Handle;
//        }


//        protected override void LoadContent()
//        {
//            graphicsDevice = this.GraphicsDevice;
//            spriteBatch = new SpriteBatch(GraphicsDevice);

//            // Custom loading
//            TileEngine.Initialize(this.Content, this.GraphicsDevice);
//            TileEngine.Viewport = this.GraphicsDevice.Viewport;
//            TileEngine.Camera = new BaseCamera(0, 0, 800, 600);
//            TileEngine.Load(new FileInfo("Data/TileEngineConfig.xml"));
//            TileEngine.Camera.CenterOriginOnPoint(new Point(0, 0));
//            TileEngine.Player = new WorldEditorPlayer();
//        }


//        protected override void UnloadContent()
//        {
//            // TODO: Unload any non ContentManager content here
//        }


//        public void EnlistEvents(PictureBox pictureBox)
//        {
//            pictureBox.MouseDown += this.MouseDown;
//            pictureBox.MouseUp += this.MouseUp;
//            pictureBox.MouseMove += this.MouseMove;
//            pictureBox.MouseClick += this.MouseClicked;
//        }

//        #region Callbacks

//        protected override void Update(GameTime gameTime)
//        {
//            this.Render.Update(gameTime);
//            this.CurComponent.Update(gameTime);
//            base.Update(gameTime);
//        }

//        public void Resized(object sender, ScreenResizedEventArgs e)
//        {
//            this.Render.OnSizeChanged(sender, e);
//            this.CurComponent.OnSizeChanged(sender, e);
//        }

//        public void Scrolled(object sender, ScrollEventArgs e)
//        {
//            this.Render.OnScrolled(sender, e);
//            this.CurComponent.OnScrolled(sender, e);
//        }

//        public void MouseDown(object sender, MouseEventArgs e)
//        {
//            this.Render.OnMouseDown(sender, e);
//            this.CurComponent.OnMouseDown(sender, e);
//        }

//        public void MouseMove(object sender, MouseEventArgs e)
//        {
//            this.Render.OnMouseMove(sender, e);
//            this.CurComponent.OnMouseMove(sender, e);
//        }

//        public void MouseUp(object sender, MouseEventArgs e)
//        {
//            this.Render.OnMouseUp(sender, e);
//            this.CurComponent.OnMouseUp(sender, e);
//        }

//        public void MouseClicked(object sender, MouseEventArgs e)
//        {
//            this.Render.OnMouseClicked(sender, e);
//            this.CurComponent.OnMouseClicked(sender, e);
//        }


//        protected override void Draw(GameTime gameTime)
//        {
//            GraphicsDevice.Clear(Color.Gray);

//            float scale = (float)WorldEditor.Scale;

//            Matrix transform = Matrix.CreateScale(new Vector3(scale, scale, 0));
//            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, transform);

//            this.Render.Draw(this.spriteBatch);
//            this.CurComponent.Draw(this.spriteBatch);

//            this.spriteBatch.End();


//            base.Draw(gameTime);

//            if (!TileEngine.IsMapLoaded)
//                return;


//            ScreenPoint screenSize = new ScreenPoint(this.previewSurface.Width, this.previewSurface.Height);

//            RenderTarget2D myRenderTarget = new RenderTarget2D(GraphicsDevice, screenSize.X, screenSize.Y, 1, GraphicsDevice.DisplayMode.Format);
//            GraphicsDevice.SetRenderTarget(0, myRenderTarget);

//            GraphicsDevice.Clear(Color.Gray);

//            if (TileEngine.WorldManager.CurrentWorld == null)
//                return;

//            MapPoint mapSize = TileEngine.WorldManager.CurrentWorld.WorldSize;
            

//            ScreenPoint scalep = mapSize.ToScreenPoint() / screenSize;


//            transform = Matrix.CreateScale(new Vector3((1.0f / (float)(scalep.X)), (1.0f / (float)(scalep.Y)), 0));
//            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, transform);

//            this.Render.Draw(this.spriteBatch);
//            this.spriteBatch.End();


//            GraphicsDevice.SetRenderTarget(0, null);
//            this.previewSurface.Draw(GraphicsDevice, this.spriteBatch, myRenderTarget);
//        }

//        #endregion
//    }

//    public enum ComponentID
//    {
//        Hand,
//        Select
//    };
//}
