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

namespace ValkyrieMapEditor.Core
{
    public class CollisionComponent : IEditorComponent
    {
        private Texture2D CollisionSprite;

        public CollisionComponent()
        {

        }

        public void LoadContent(GraphicsDevice graphicsDevice)
        {
            this.CollisionSprite = Texture2D.FromFile(graphicsDevice, "Graphics/EditorCollision.png");
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
            if (TileEngine.IsMapLoaded)
            {
                MapPoint point = new MapPoint((e.Location.X - (int)TileEngine.Camera.MapOffset.X) / 32, (e.Location.Y - (int)TileEngine.Camera.MapOffset.Y) / 32);

                if (e.Button == MouseButtons.Left)
                    TileEngine.CurrentMapChunk.SetData(MapLayers.CollisionLayer, point, 1);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < TileEngine.CurrentMapChunk.MapSize.Y; y++)
            {
                for (int x = 0; x < TileEngine.CurrentMapChunk.MapSize.X; x++)
                {
                    int value = TileEngine.CurrentMapChunk.GetLayerValue(new MapPoint(x, y), MapLayers.CollisionLayer);

                    if (value == -1)
                        continue;

                    Rectangle destRectangle = new Rectangle(0, 0, TileEngine.CurrentMapChunk.TileSize.X, TileEngine.CurrentMapChunk.TileSize.Y);
                    destRectangle.X = (int)TileEngine.Camera.MapOffset.X + (int)TileEngine.Camera.CameraOffset.X + (x * TileEngine.CurrentMapChunk.TileSize.X);
                    destRectangle.Y = (int)TileEngine.Camera.MapOffset.Y + (int)TileEngine.Camera.CameraOffset.Y + (y * TileEngine.CurrentMapChunk.TileSize.Y);

                    spriteBatch.Draw(this.CollisionSprite, destRectangle, new Rectangle(0, 0, this.CollisionSprite.Width, this.CollisionSprite.Height), Color.White);
                }
            }
        }

        public void Update(GameTime gameTime)
        {

        }


    }
}
