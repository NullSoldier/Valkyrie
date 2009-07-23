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
        public RenderComponent Render = null;
        private float lastScale = 0.0f;
        private bool dragging = false;


        public PreviewRender()
        {
            this.Render = new RenderComponent();

            this.MouseDown += this.OnMouseDown_Event;
            this.MouseUp += this.OnMouseUp_Event;
            this.MouseMove += this.OnMouseMove_Event;
            this.MouseClick += this.OnMouseClick_Event;
            this.MouseDoubleClick += this.OnMouseDoubleClick_Event;
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

        }

        public void Scrolled(object sender, ScrollEventArgs e)
        {

        }

        public void OnMouseDown_Event(object sender, MouseEventArgs e)
        {
            this.Render.OnMouseDown(sender, e);
            dragging = true;
        }

        public void OnMouseMove_Event(object sender, MouseEventArgs e)
        {
            this.Render.OnMouseMove(sender, e);

            if (dragging)
                MoveCamera(e);
        }

        public void OnMouseUp_Event(object sender, MouseEventArgs e)
        {
            this.Render.OnMouseUp(sender, e);
            dragging = false;
        }

        public void OnMouseClick_Event(object sender, MouseEventArgs e)
        {
            this.Render.OnMouseClicked(sender, e);
        }

        public void OnMouseDoubleClick_Event(object sender, MouseEventArgs e)
        {
            //this.Render.OnMouseClicked(sender, e);

            MoveCamera(e);
        }

        private void MoveCamera(MouseEventArgs e)
        {
            if (TileEngine.WorldManager.CurrentWorld == null)
                return;

            ScreenPoint pos = new ScreenPoint(e.X, e.Y) * lastScale;
            ScreenPoint mapSize = TileEngine.WorldManager.CurrentWorld.WorldSize.ToScreenPoint();

            pos.X = Math.Max(0, pos.X);
            pos.Y = Math.Max(0, pos.Y);

            pos.X = Math.Min(mapSize.X, pos.X);
            pos.Y = Math.Min(mapSize.Y, pos.Y);

            TileEngine.Camera.MapOffset = (pos*-1).ToVector2();

            WorldEditor.MainForm.UpdateScrollBars();
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


            Matrix transform = Matrix.CreateScale(new Vector3((1.0f / lastScale), (1.0f / lastScale), 0));

            TileEngine.Camera.CameraOffset = new Vector2(0, 0);
            TileEngine.Camera.CameraOrigin = new Point(0, 0);
            TileEngine.Camera.Scale(1.0 / (double)(lastScale));

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, transform);
            this.Render.Draw(spriteBatch);

            TileEngine.Camera.Pop();

            Rectangle rect = (TileEngine.Camera.Offset() * -1).ToRect(new Point(Math.Min(this.Width * (int)lastScale, TileEngine.Camera.Screen.Width), Math.Min(this.Height * (int)lastScale, TileEngine.Camera.Screen.Height)));
            Texture2D img = CreateSelectRectangle(gfxDevice, rect.Width, rect.Height);

            spriteBatch.Draw(img, rect, Color.White);

            spriteBatch.End();
        }

        private Texture2D CreateSelectRectangle(GraphicsDevice gfxDevice, int width, int height)
        {
            if (width <= 2 || height <= 2)
                return null;

            // create the rectangle texture, ,but it will have no color! lets fix that
            Texture2D rectangleTexture = new Texture2D(gfxDevice, width, height, 1, TextureUsage.None, SurfaceFormat.Color);
            Color[] color = new Color[width * height];//set the color to the amount of pixels

            //loop through all the colors setting them to whatever values we want
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    color[x + (y * width)] = new Color(255, 0, 0, 125);
                }
            }

            //outer four
            for (int y = 0; y < height; y++)
                color[0 + (y * width)] = new Color(0, 0, 0, 255);

            for (int y = 0; y < height; y++)
                color[width - 1 + (y * width)] = new Color(0, 0, 0, 255);

            for (int x = 0; x < width; x++)
                color[x + (0 * width)] = new Color(0, 0, 0, 255);

            for (int x = 0; x < width; x++)
                color[x + ((height - 1) * width)] = new Color(0, 0, 0, 255);


            rectangleTexture.SetData(color);//set the color data on the texture
            return rectangleTexture;
        }
    }
}
