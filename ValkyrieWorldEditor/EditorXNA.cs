using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using ValkyrieLibrary;
using ValkyrieLibrary.Maps;
using ValkyrieLibrary.Core;
using ValkyrieWorldEditor.Core;

namespace ValkyrieWorldEditor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class EditorXNA : Microsoft.Xna.Framework.Game
    {
        public static SpriteFont font;
        public static GraphicsDevice graphicsDevice = null;
        public static GraphicsDeviceManager graphics = null;

        SpriteBatch spriteBatch;

        private IntPtr drawSurface;
        private IntPtr drawPreviewSurface; 

        public EditorXNA()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public EditorXNA(IntPtr drawSurface, IntPtr drawTilesSurface)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.drawSurface = drawSurface;
            this.drawPreviewSurface = drawTilesSurface;
            graphics.PreparingDeviceSettings +=
                new EventHandler<PreparingDeviceSettingsEventArgs>(PreparingDeviceSettings);

            System.Windows.Forms.Control.FromHandle((this.Window.Handle)).VisibleChanged += new EventHandler(VisibleChanged);
        }

        void PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = drawSurface;
        }

        private void VisibleChanged(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Control.FromHandle((this.Window.Handle)).Visible == true)
                System.Windows.Forms.Control.FromHandle((this.Window.Handle)).Visible = false;
        }


        protected override void Initialize()
        {
            base.Initialize();
            Mouse.WindowHandle = this.drawSurface;
            
        }


        protected override void LoadContent()
        {
            graphicsDevice = this.GraphicsDevice;
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Custom loading
            TileEngine.Initialize(this.Content, this.GraphicsDevice);
            TileEngine.Viewport = this.GraphicsDevice.Viewport;
            TileEngine.Camera = new BaseCamera(0, 0, 800, 600);
            TileEngine.Load(new FileInfo("Data/TileEngineConfig.xml"));
            TileEngine.Camera.CenterOriginOnPoint(new Point(0, 0));
            TileEngine.Player = new WorldEditorPlayer();
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
 
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
