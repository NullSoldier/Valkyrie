using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Valkyrie.Library.Core;
using Valkyrie.Library.Maps;
using Valkyrie.Library;
using ValkyrieWorldEditor.Forms;

namespace ValkyrieWorldEditor.Core
{
    public class RenderComponent : IEditorComponent
    {
        public float Scale { get; set; }
        public RenderComponent()
        {

        }

        public void OnSizeChanged(object sender, ScreenResizedEventArgs e)
        {
            //EditorXNA.graphics.PreferredBackBufferWidth = e.Width;
            //EditorXNA.graphics.PreferredBackBufferHeight = e.Height;
            //EditorXNA.graphics.IsFullScreen = false;
            //EditorXNA.graphics.ApplyChanges();

            //if (TileEngine.Camera != null)
            //{
            //    TileEngine.Camera.Screen.Width = e.Width;
            //    TileEngine.Camera.Screen.Height = e.Height;

            //    if (TileEngine.IsMapLoaded)
            //    {
            //        // Move the X origin
            //        int DisplayedWidth = (TileEngine.CurrentMapChunk.MapSize.X * TileEngine.CurrentMapChunk.TileSize.X) + (int)TileEngine.Camera.MapOffset.X;
            //        if (DisplayedWidth < e.Width)
            //        {
            //            if (TileEngine.CurrentMapChunk.MapSize.X * TileEngine.CurrentMapChunk.TileSize.X < e.Width)
            //                TileEngine.Camera.CenterOriginOnPoint(0, (int)(TileEngine.Camera.MapOffset.Y * -1));
            //            else
            //            {
            //                int newOffset = (e.Width - DisplayedWidth);
            //                TileEngine.Camera.CenterOriginOnPoint((int)(TileEngine.Camera.MapOffset.X * -1) - newOffset, (int)(TileEngine.Camera.MapOffset.Y * -1));
            //            }
            //        }

            //        // Move the Y origin
            //        int DisplayedHeight = (TileEngine.CurrentMapChunk.MapSize.Y * TileEngine.CurrentMapChunk.TileSize.Y) + (int)TileEngine.Camera.MapOffset.Y;
            //        if (DisplayedHeight < e.Height)
            //        {
            //            if (TileEngine.CurrentMapChunk.MapSize.Y * TileEngine.CurrentMapChunk.TileSize.Y < e.Height)
            //                TileEngine.Camera.CenterOriginOnPoint((int)(TileEngine.Camera.MapOffset.X * -1), 0);
            //            else
            //            {
            //                int newOffset = (e.Height - DisplayedHeight);
            //                TileEngine.Camera.CenterOriginOnPoint((int)(TileEngine.Camera.MapOffset.X * -1), (int)(TileEngine.Camera.MapOffset.Y * -1) - newOffset);
            //            }
            //        }
            //    }

            //}
        }

        public void OnScrolled(object sender, ScrollEventArgs e)
        {
            int dif = (e.NewValue - e.OldValue);

            if (e.Type == ScrollEventType.EndScroll)
                return;

            int x = (int)(TileEngine.Camera.MapOffset.X) * -1;
            int y = (int)(TileEngine.Camera.MapOffset.Y) * -1;

            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
                y += dif * TileEngine.CurrentMapChunk.TileSize.Y;
            else
                x += dif * TileEngine.CurrentMapChunk.TileSize.X;

            TileEngine.Camera.CenterOriginOnPoint(new Point(x, y));
        }

        public void OnMouseDown(object sender, MouseEventArgs ev)
        {
        }

        public void OnMouseMove(object sender, MouseEventArgs ev)
        {
        }

        public void OnMouseUp(object sender, MouseEventArgs ev)
        {
        }

        public void OnMouseClicked(object sender, MouseEventArgs ev)
        {
        }

        public void OnMouseDoubleClicked(object sender, MouseEventArgs ev)
        {

        }

        public void Draw(GraphicsDevice gfxDevice, SpriteBatch spriteBatch)
        {
            if (!TileEngine.IsMapLoaded)
                return;

            foreach (var mh in TileEngine.WorldManager.CurrentWorld.MapList)
            {
                Rectangle rect = (mh.Value.MapLocation.ToScreenPoint() + TileEngine.Camera.Offset()).ToRect(mh.Value.Map.MapSize.ToScreenPoint().ToPoint());
                Texture2D img = WorldEditor.GetMapImage(mh.Value);

                if (img != null)
                {
                    spriteBatch.Draw(img, rect, Color.White);
                }
                else
                {
                    TileEngine.DrawMapLocal(spriteBatch, mh.Value);
                }
            }

            //TileEngine.DrawEverything(spriteBatch);

            foreach (var mh in WorldEditor.SelectedMaps)
            {
                Rectangle rect = (mh.MapLocation.ToScreenPoint() + TileEngine.Camera.Offset()).ToRect(mh.Map.MapSize.ToScreenPoint().ToPoint());
                Texture2D img = WorldEditor.GetMapSelectImage(gfxDevice, mh);
                spriteBatch.Draw(img, rect, Color.White);
            }
        }

        public void Update(GameTime gameTime)
        {
        }

        public void LoadContent(GraphicsDevice graphicsDevice)
        {
        }

        public static Texture2D CreateSelectRectangle(GraphicsDevice gfxDevice, int width, int height, Color fillColor)
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
                    color[x + (y * width)] = fillColor;
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
