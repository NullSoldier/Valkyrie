using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ValkyrieLibrary.Core;
using ValkyrieLibrary.Maps;
using ValkyrieLibrary;
using ValkyrieWorldEditor.Forms;

namespace ValkyrieWorldEditor.Core
{
    public class HandComponent : IEditorComponent
    {
        public float Scale { get; set; }
        private bool dragging = false;

        public HandComponent()
        {
        }

        public void OnSizeChanged(object sender, ScreenResizedEventArgs e)
        {
        }

        public void OnScrolled(object sender, ScrollEventArgs e)
        {
        }

        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
                MoveCamera(e);
        }

        public void OnMouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        public void OnMouseClicked(object sender, MouseEventArgs e)
        {
        }

        public void OnMouseDoubleClicked(object sender, MouseEventArgs e)
        {
            MoveCamera(e);
        }

        public void Draw(GraphicsDevice gfxDevice, SpriteBatch spriteBatch)
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public void LoadContent(GraphicsDevice graphicsDevice)
        {
        }

        private void MoveCamera(MouseEventArgs e)
        {
            if (TileEngine.WorldManager.CurrentWorld == null)
                return;

            ScreenPoint pos = new ScreenPoint(e.X, e.Y) * this.Scale;
            ScreenPoint screenSize = new ScreenPoint(TileEngine.Camera.Screen.Width, TileEngine.Camera.Screen.Height);
            ScreenPoint mapSize = TileEngine.WorldManager.CurrentWorld.WorldSize.ToScreenPoint() - screenSize;

            pos -= screenSize / 2;

            pos.X = Math.Max(0, pos.X);
            pos.Y = Math.Max(0, pos.Y);

            pos.X = Math.Min(mapSize.X, pos.X);
            pos.Y = Math.Min(mapSize.Y, pos.Y);

            TileEngine.Camera.MapOffset = (pos * -1).ToVector2();

            WorldEditor.MainForm.UpdateScrollBars();
        }
    }
}
