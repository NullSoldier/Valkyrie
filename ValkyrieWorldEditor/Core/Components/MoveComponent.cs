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
    public class MoveComponent : IEditorComponent
    {
        private bool dragging = false;
        public float Scale { get; set; }

        private ScreenPoint startLoc;
        private ScreenPoint endLoc;
      

        public MoveComponent()
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
            startLoc = new ScreenPoint(e.X, e.Y) * (this.Scale) - TileEngine.Camera.Offset();
            endLoc = new ScreenPoint(e.X, e.Y) * (this.Scale) - TileEngine.Camera.Offset();
            dragging = true;
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                endLoc = new ScreenPoint(e.X, e.Y) * (this.Scale) - TileEngine.Camera.Offset();
                MoveSelected();
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

        public void Draw(GraphicsDevice gfxDevice, SpriteBatch spriteBatch)
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public void LoadContent(GraphicsDevice graphicsDevice)
        {
        }

        public void OnMouseDoubleClicked(object sender, MouseEventArgs ev)
        {

        }

        public void MoveSelected()
        {
            ScreenPoint posAsScreen = startLoc - endLoc;
            MapPoint posAsMap = posAsScreen.ToMapPoint();

            ScreenPoint diff = posAsScreen - posAsMap.ToScreenPoint();
            ScreenPoint defSize = (new MapPoint(1,1).ToScreenPoint())/2;

            //if (diff.X > defSize.X)
            //{
            //    posAsMap.X++;
            //}

            //if (diff.Y > defSize.Y)
            //{
            //    posAsMap.Y++;
            //}

            foreach (var mh in WorldEditor.SelectedMaps)
            {
                mh.MapLocation -= posAsMap;
            }

            TileEngine.WorldManager.CurrentWorld.CalcWorldSize();
            WorldEditor.MainForm.UpdateScrollBars();
        }
    }
}
