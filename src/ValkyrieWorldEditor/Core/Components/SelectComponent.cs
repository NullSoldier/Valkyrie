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
    public class SelectComponent : IEditorComponent
    {
        public float Scale { get; set; }
        public SelectComponent()
        {
        }

        public void OnSizeChanged(object sender, ScreenResizedEventArgs e)
        {
        }

        public void OnScrolled(object sender, ScrollEventArgs e)
        {
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

        public void OnMouseClicked(object sender, MouseEventArgs e)
        {
            SelectMap(e);
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

        private void SelectMap(MouseEventArgs e)
        {
            MapHeader mh = null;
            MapPoint pos = (new ScreenPoint(e.X, e.Y) * (this.Scale) - TileEngine.Camera.Offset()).ToMapPoint();

            foreach (MapHeader header in TileEngine.WorldManager.CurrentWorld.MapList.Values)
            {
                if (header.TilePointInMapGlobal(pos))
                {
                    mh = header;
                    break;
                }
            }

            if (mh != null)
            {
                if (WorldEditor.SelectedMaps.Contains(mh))
                {
                    WorldEditor.SelectedMaps.Remove(mh);
                }
                else
                {
                    WorldEditor.SelectedMaps.Add(mh);
                }
            }
            else
            {
                WorldEditor.SelectedMaps.Clear();
            }

            WorldEditor.MainForm.UpdateSelectedMap();
        }
    }
}
