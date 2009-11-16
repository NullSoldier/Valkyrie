using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Valkyrie.Library.Core;
using Valkyrie.Library.Maps;
using Valkyrie.Library;
using ValkyrieWorldEditor.Forms;

namespace ValkyrieWorldEditor.Core
{
    public class HandComponent : IEditorComponent
    {
        public float Scale { get; set; }
        private bool dragging = false;

        private ScreenPoint startLoc;
        private ScreenPoint endLoc;
        private bool invertMoveDir;
        private bool jumpOnMove;

        public HandComponent(bool invertDir, bool jumpOnMove)
        {
            this.invertMoveDir = invertDir;
            this.jumpOnMove = jumpOnMove;
        }

        public void OnSizeChanged(object sender, ScreenResizedEventArgs e)
        {
        }

        public void OnScrolled(object sender, ScrollEventArgs e)
        {
        }

        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (jumpOnMove)
                MoveCameraTo(e);

            startLoc = new ScreenPoint(e.X, e.Y) * (this.Scale) - TileEngine.Camera.Offset();
            endLoc = new ScreenPoint(e.X, e.Y) * (this.Scale) - TileEngine.Camera.Offset();
            dragging = true;
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                endLoc = new ScreenPoint(e.X, e.Y) * (this.Scale) - TileEngine.Camera.Offset();
                MoveCamera();
                startLoc = new ScreenPoint(e.X, e.Y) * (this.Scale) - TileEngine.Camera.Offset();
            }
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
            MoveCameraTo(e);
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

        private void MoveCamera()
        {
            if (TileEngine.WorldManager.CurrentWorld == null)
                return;

            ScreenPoint posAsScreen; 
                
            if (this.invertMoveDir == true)
                posAsScreen = startLoc - endLoc;
            else
                posAsScreen = endLoc - startLoc;

            ScreenPoint pos = posAsScreen + (new ScreenPoint(TileEngine.Camera.MapOffset)*-1);
            ScreenPoint screenSize = new ScreenPoint(TileEngine.Camera.Screen.Width, TileEngine.Camera.Screen.Height);
            ScreenPoint mapSize = TileEngine.WorldManager.CurrentWorld.WorldSize.ToScreenPoint() - screenSize;

            //pos -= screenSize / 2;

            pos.X = Math.Max(0, pos.X);
            pos.Y = Math.Max(0, pos.Y);

            pos.X = Math.Min(mapSize.X, pos.X);
            pos.Y = Math.Min(mapSize.Y, pos.Y);

            TileEngine.Camera.MapOffset = (pos * -1).ToVector2();

            WorldEditor.MainForm.UpdateScrollBars();
        }

        private void MoveCameraTo(MouseEventArgs e)
        {
			if (TileEngine.WorldManager.CurrentWorld == null)
				return;

            ScreenPoint pos = new ScreenPoint(e.X, e.Y) * this.Scale + new ScreenPoint(TileEngine.Camera.CameraOffset);
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
