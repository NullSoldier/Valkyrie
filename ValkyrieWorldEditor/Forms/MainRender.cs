using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;

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
using ValkyrieWorldEditor.Forms;

namespace ValkyrieWorldEditor.Forms
{
    public class MainRender : XNARenderControl
    {
        private Dictionary<ComponentID, IEditorComponent> ComponentList;
        private IEditorComponent curComponent = null;
        public RenderComponent Render = null;

        private ContentManager content;
        private GameServiceContainer gameServices;

        public IEditorComponent CurComponent
        {
            get
            {
                if (this.curComponent == null)
                    this.curComponent = this.ComponentList[ComponentID.Hand];

                return curComponent;
            }
        }

        public MainRender()
        {
            this.gameServices = new GameServiceContainer();
            this.content = new ContentManager(this.gameServices);



            this.content.RootDirectory = "Content";

            ComponentList = new Dictionary<ComponentID, IEditorComponent>();
            this.Render = new RenderComponent();

            this.ComponentList = new Dictionary<ComponentID, IEditorComponent>();
            this.ComponentList.Add(ComponentID.Select, new SelectComponent());
            this.ComponentList.Add(ComponentID.Hand, new HandComponent());

            this.MouseDown += this.OnMouseDown_Event;
            this.MouseUp += this.OnMouseUp_Event;
            this.MouseMove += this.OnMouseMove_Event;
            this.MouseClick += this.OnMouseClick_Event;
        }


        protected override void Initialize(GraphicsDevice gfxDevice)
        {
            // Custom loading
            TileEngine.Initialize(this.content, gfxDevice);
            TileEngine.Viewport = gfxDevice.Viewport;
            TileEngine.Camera = new BaseCamera(0, 0, this.Width, this.Height);
            TileEngine.Load(new FileInfo("Data/TileEngineConfig.xml"));
            TileEngine.Camera.CenterOriginOnPoint(new Point(0, 0));
            TileEngine.Player = new WorldEditorPlayer();
        }

        public override void Update(float gameTime)
        {
            //this.Render.Update(gameTime);
            //this.CurComponent.Update(gameTime);
        }

        public void Resized(object sender, ScreenResizedEventArgs e)
        {
            TileEngine.Camera.Screen = new Rectangle(0,0,Width, Height);
            TileEngine.Camera.Scale(WorldEditor.Scale);

            this.Render.OnSizeChanged(sender, e);
            this.CurComponent.OnSizeChanged(sender, e);
        }

        public void Scrolled(object sender, ScrollEventArgs e)
        {
            this.Render.OnScrolled(sender, e);
            this.CurComponent.OnScrolled(sender, e);
        }

        public void OnMouseDown_Event(object sender, MouseEventArgs e)
        {
            this.Render.OnMouseDown(sender, e);
            this.CurComponent.OnMouseDown(sender, e);
        }

        public void OnMouseMove_Event(object sender, MouseEventArgs e)
        {
            this.Render.OnMouseMove(sender, e);
            this.CurComponent.OnMouseMove(sender, e);
        }

        public void OnMouseUp_Event(object sender, MouseEventArgs e)
        {
            this.Render.OnMouseUp(sender, e);
            this.CurComponent.OnMouseUp(sender, e);
        }

        public void OnMouseClick_Event(object sender, MouseEventArgs e)
        {
            this.Render.OnMouseClicked(sender, e);
            this.CurComponent.OnMouseClicked(sender, e);
        }

        public override void Draw(GraphicsDevice gfxDevice, SpriteBatch spriteBatch)
        {
            gfxDevice.Clear(Color.Gray);

            float scale = (float)WorldEditor.Scale;

            Matrix transform = Matrix.CreateScale(new Vector3(scale, scale, 0));
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, transform);

            this.Render.Draw(spriteBatch);
            this.CurComponent.Draw(spriteBatch);

            spriteBatch.End();
        }
    }

    public enum ComponentID
    {
        Hand,
        Select
    };
}
