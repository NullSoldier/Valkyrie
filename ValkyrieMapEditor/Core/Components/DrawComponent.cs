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

namespace ValkyrieMapEditor.Core
{
    public class DrawComponent : IEditorComponent
    {
        public DrawComponent()
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

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the current location of the mouse
            var mouseState = Mouse.GetState();
            if (mouseState.X > 0 && mouseState.Y > 0)
            {
                Point tileLocation = new Point(mouseState.X / 32, mouseState.Y / 32);
                Vector2 cLoc = new Vector2(tileLocation.X * 32, tileLocation.Y * 32);
                Rectangle pos = new Rectangle((int)cLoc.X, (int)cLoc.Y, MapEditorManager.SelectedTilesRect.Width * 32 + 32, MapEditorManager.SelectedTilesRect.Height * 32 + 32);

                Texture2D text = EditorXNA.CreateSelectRectangle(pos.Width, pos.Height);

                if (text != null)
                    spriteBatch.Draw(text, pos, new Rectangle(0, 0, text.Width, text.Height), Color.White);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (MapEditorManager.IgnoreInput)
                return;

            // TODO: Add your update logic here
            KeyboardState keyState = Keyboard.GetState();

            // Only do this if your using something other than the pencil
            if (TileEngine.IsMapLoaded && MapEditorManager.CurrentTool == Tools.Pencil)
            {
                var mouseState = Mouse.GetState();

                if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && mouseState.X > 0 && mouseState.Y > 0)
                {
                    MapPoint tileLocation = new MapPoint((mouseState.X - (int)TileEngine.Camera.MapOffset.X) / 32, (mouseState.Y - (int)TileEngine.Camera.MapOffset.Y) / 32);

                    if (TileEngine.CurrentMapChunk.TilePointInMapLocal(tileLocation))
                    {
                        for (int y = 0; y <= MapEditorManager.SelectedTilesRect.Height; y++)
                        {
                            for (int x = 0; x <= MapEditorManager.SelectedTilesRect.Width; x++)
                            {
                                MapPoint tilesheetPoint = new MapPoint(MapEditorManager.SelectedTilesRect.X + x, MapEditorManager.SelectedTilesRect.Y + y);
                                MapPoint point = new MapPoint(tileLocation.X + x, tileLocation.Y + y);

                                if (TileEngine.CurrentMapChunk.TilePointInMapLocal(point))
                                    TileEngine.CurrentMapChunk.SetData(MapEditorManager.CurrentLayer, point, TileEngine.CurrentMapChunk.GetTileSetValue(tilesheetPoint));
                            }
                        }
                    }
                }
            }
        }

        public void LoadContent(GraphicsDevice graphicsDevice)
        {
        }
    }
}
