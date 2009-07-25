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
    public class PreviewRender : XNARenderControl
    {
        private Dictionary<ComponentID, IEditorComponent> ComponentList;
        private IEditorComponent curComponent = null;
        public RenderComponent Render = null;
        private float lastScale = 0.0f;

        private Texture2D lastSelectImage= null;
        ScreenPoint lastSelectImageSize = new ScreenPoint(0, 0);

        public IEditorComponent CurComponent
        {
            get
            {
                if (this.curComponent == null)
                    this.curComponent = this.ComponentList[ComponentID.Hand];

                return curComponent;
            }
        }

        public PreviewRender()
        {
            this.ComponentList = new Dictionary<ComponentID, IEditorComponent>();
            this.ComponentList.Add(ComponentID.Select, new SelectComponent());
            this.ComponentList.Add(ComponentID.Hand, new HandComponent(false, true));
            this.ComponentList.Add(ComponentID.Move, new MoveComponent());
            this.Render = new RenderComponent();

            this.MouseDown += this.OnMouseDown_Event;
            this.MouseUp += this.OnMouseUp_Event;
            this.MouseMove += this.OnMouseMove_Event;
            this.MouseClick += this.OnMouseClick_Event;
            this.MouseDoubleClick += this.OnMouseDoubleClick_Event;
        }

        public void SetActiveComponent(ComponentID compId)
        {
            //this.curComponent = this.ComponentList[compId];
        }

        protected override void Initialize(GraphicsDevice gfxDevice)
        {
    
        }

        public override void Update(float gameTime)
        {
            //this.Render.Update(gameTime);
            //this.CurComponent.Update(gameTime);
        }

        public void Resized(object sender, ScreenResizedEventArgs e)
        {
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

        public void OnMouseDoubleClick_Event(object sender, MouseEventArgs e)
        {
            this.Render.OnMouseDoubleClicked(sender, e);
            this.CurComponent.OnMouseDoubleClicked(sender, e);
        }


        

        public override void Draw(GraphicsDevice gfxDevice, SpriteBatch spriteBatch)
        {
            if (TileEngine.WorldManager.CurrentWorld == null)
                return;

            TileEngine.Camera.Push();


            gfxDevice.Clear(Color.Gray);

            ScreenPoint screenSize = new ScreenPoint(this.Width, this.Height);
            MapPoint mapSize = TileEngine.WorldManager.CurrentWorld.WorldSize;
            ScreenPoint scalep = mapSize.ToScreenPoint() / screenSize;

            lastScale = (scalep.X > scalep.Y) ? scalep.X : scalep.Y;

            foreach (var com in this.ComponentList)
            {
                com.Value.Scale = lastScale;
            }

            Matrix transform = Matrix.CreateScale(new Vector3((1.0f / lastScale), (1.0f / lastScale), 0));

            TileEngine.Camera.CameraOffset = new Vector2(0, 0);
            TileEngine.Camera.CameraOrigin = new Point(0, 0);
            TileEngine.Camera.Scale(1.0 / (double)(lastScale));

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, transform);
            this.Render.Draw(gfxDevice, spriteBatch);
            this.CurComponent.Draw(gfxDevice, spriteBatch);
            TileEngine.Camera.Pop();

            TileEngine.Camera.Push();
            TileEngine.Camera.Scale((float)WorldEditor.Scale);

            Rectangle rect = (TileEngine.Camera.Offset() * -1).ToRect(new Point(Math.Min(this.Width * (int)lastScale, TileEngine.Camera.Screen.Width), Math.Min(this.Height * (int)lastScale, TileEngine.Camera.Screen.Height)));

            if (lastSelectImage == null || lastSelectImageSize.X != rect.Width || lastSelectImageSize.Y != rect.Height)
            {
                lastSelectImage = RenderComponent.CreateSelectRectangle(gfxDevice, rect.Width, rect.Height, new Color(255, 0, 0, 125));
                lastSelectImageSize = new ScreenPoint(rect.Width, rect.Height);
            }

            if (lastSelectImage != null)
                spriteBatch.Draw(lastSelectImage, rect, Color.White);

            spriteBatch.End();

            TileEngine.Camera.Pop();
        }



    }
}
