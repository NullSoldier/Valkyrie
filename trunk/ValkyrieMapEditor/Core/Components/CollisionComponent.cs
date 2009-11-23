using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Valkyrie.Library.Core;
using Valkyrie.Library;
using Valkyrie.Engine;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Maps;

namespace ValkyrieMapEditor.Core
{
    public class CollisionComponent : IEditorComponent
    {
        private Texture2D CollisionSprite;

        public CollisionComponent()
        {

        }

        public void LoadContent(GraphicsDevice graphicsDevice, IEngineContext context)
        {
            this.CollisionSprite = Texture2D.FromFile(graphicsDevice, "Graphics/EditorCollision.png");

			this.context = context;
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
			if (MapEditorManager.CurrentMap != null)
			{
				var camera = this.context.SceneProvider.GetCamera("camera1");

			    MapPoint point = new MapPoint((e.Location.X - (int)camera.MapOffset.X) / 32, (e.Location.Y - (int)camera.MapOffset.Y) / 32);

				if(e.Button == MouseButtons.Left)
				{
					int value = MapEditorManager.CurrentMap.GetLayerValue(point, MapLayers.CollisionLayer);

					MapEditorManager.CurrentMap.SetLayerValue(point, MapLayers.CollisionLayer, value * -1);
				}
			}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
			for (int y = 0; y < MapEditorManager.CurrentMap.MapSize.Y; y++)
			{
				for(int x = 0; x < MapEditorManager.CurrentMap.MapSize.X; x++)
			    {
					int value = MapEditorManager.CurrentMap.GetLayerValue(new MapPoint(x, y), MapLayers.CollisionLayer);

			        if (value == -1)
			            continue;

					var camera = this.context.SceneProvider.GetCamera("camera1");

					Rectangle destRectangle = new Rectangle(0, 0, MapEditorManager.CurrentMap.TileSize, MapEditorManager.CurrentMap.TileSize);
					destRectangle.X = (int)camera.MapOffset.X + (int)camera.CameraOffset.X + (x * MapEditorManager.CurrentMap.TileSize);
					destRectangle.Y = (int)camera.MapOffset.Y + (int)camera.CameraOffset.Y + (y * MapEditorManager.CurrentMap.TileSize);

			        spriteBatch.Draw(this.CollisionSprite, destRectangle, new Rectangle(0, 0, this.CollisionSprite.Width, this.CollisionSprite.Height), Color.White);
			    }
			}
        }

        public void Update(GameTime gameTime)
        {

        }

		private IEngineContext context = null;
    }
}
