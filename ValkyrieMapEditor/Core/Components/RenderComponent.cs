using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ValkyrieLibrary.Core;
using ValkyrieLibrary.Maps;
using ValkyrieLibrary;

namespace ValkyrieMapEditor.Core
{
    public class RenderComponent : IEditorComponent
    {
        public RenderComponent()
        {

        }

        public void OnSizeChanged(object sender, ScreenResizedEventArgs e)
        {
            EditorXNA.graphics.PreferredBackBufferWidth = e.Width;
            EditorXNA.graphics.PreferredBackBufferHeight = e.Height;
            EditorXNA.graphics.IsFullScreen = false;
            EditorXNA.graphics.ApplyChanges();

            if (TileEngine.Camera != null)
            {
                TileEngine.Camera.Screen.Width = e.Width;
                TileEngine.Camera.Screen.Height = e.Height;

                if (TileEngine.IsMapLoaded)
                {
                    // Move the X origin
                    int DisplayedWidth = (TileEngine.CurrentMapChunk.MapSize.X * TileEngine.CurrentMapChunk.TileSize.X) + (int)TileEngine.Camera.MapOffset.X;
                    if (DisplayedWidth < e.Width)
                    {
                        if (TileEngine.CurrentMapChunk.MapSize.X * TileEngine.CurrentMapChunk.TileSize.X < e.Width)
                            TileEngine.Camera.CenterOriginOnPoint(0, (int)(TileEngine.Camera.MapOffset.Y * -1));
                        else
                        {
                            int newOffset = (e.Width - DisplayedWidth);
                            TileEngine.Camera.CenterOriginOnPoint((int)(TileEngine.Camera.MapOffset.X * -1) - newOffset, (int)(TileEngine.Camera.MapOffset.Y * -1));
                        }
                    }

                    // Move the Y origin
                    int DisplayedHeight = (TileEngine.CurrentMapChunk.MapSize.Y * TileEngine.CurrentMapChunk.TileSize.Y) + (int)TileEngine.Camera.MapOffset.Y;
                    if (DisplayedHeight < e.Height)
                    {
                        if (TileEngine.CurrentMapChunk.MapSize.Y * TileEngine.CurrentMapChunk.TileSize.Y < e.Height)
                            TileEngine.Camera.CenterOriginOnPoint((int)(TileEngine.Camera.MapOffset.X * -1), 0);
                        else
                        {
                            int newOffset = (e.Height - DisplayedHeight);
                            TileEngine.Camera.CenterOriginOnPoint((int)(TileEngine.Camera.MapOffset.X * -1), (int)(TileEngine.Camera.MapOffset.Y * -1) - newOffset);
                        }
                    }
                }

            }
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

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!TileEngine.IsMapLoaded)
                return;

            spriteBatch.End();

            if (MapEditorManager.ViewMode == ViewMode.All)
            {
                TileEngine.DrawAllLayers(spriteBatch, false);
            }
            else if (MapEditorManager.ViewMode == ViewMode.Below)
            {
                if (MapEditorManager.CurrentLayer == MapLayers.TopLayer)
                {
                    TileEngine.DrawAllLayers(spriteBatch, false);
                }
                else if (MapEditorManager.CurrentLayer == MapLayers.MiddleLayer)
                {
                    TileEngine.DrawLayerMap(spriteBatch, MapLayers.BaseLayer);
                    TileEngine.DrawLayerMap(spriteBatch, MapLayers.MiddleLayer);
                }
                else
                {
                    TileEngine.DrawLayerMap(spriteBatch, MapLayers.BaseLayer);
                }
            }
            else
            {
                if (MapEditorManager.CurrentLayer == MapLayers.TopLayer)
                {
                    TileEngine.DrawLayerMap(spriteBatch, MapLayers.TopLayer);
                }
                else if (MapEditorManager.CurrentLayer == MapLayers.MiddleLayer)
                {
                    TileEngine.DrawLayerMap(spriteBatch, MapLayers.MiddleLayer);
                }
                else
                {
                    TileEngine.DrawLayerMap(spriteBatch, MapLayers.BaseLayer);
                }
            }

            spriteBatch.Begin();
        }

        public void Update(GameTime gameTime)
        {
        }

        public void LoadContent(GraphicsDevice graphicsDevice)
        {
        }
    }
}
