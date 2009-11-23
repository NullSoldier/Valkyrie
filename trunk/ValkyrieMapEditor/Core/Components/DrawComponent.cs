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
using Valkyrie.Library;
using Valkyrie.Engine;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Maps;

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
			if(MapEditorManager.CurrentMap == null)
				return;

			var mouseState = Mouse.GetState();
			if (mouseState.X > 0 && mouseState.Y > 0)
			{
			    Point tileLocation = new Point(mouseState.X / 32, mouseState.Y / 32);
			    Vector2 cLoc = new Vector2(tileLocation.X * 32, tileLocation.Y * 32);
			    Rectangle pos = new Rectangle((int)cLoc.X, (int)cLoc.Y, MapEditorManager.SelectedTilesRectangle.Width * 32 + 32, MapEditorManager.SelectedTilesRectangle.Height * 32 + 32);

			    Texture2D selectbox = EditorXNA.CreateSelectRectangle(pos.Width, pos.Height);

			    if (selectbox != null)
			        spriteBatch.Draw(selectbox, pos, new Rectangle(0, 0, selectbox.Width, selectbox.Height), Color.White);
			}
        }

        public void Update(GameTime gameTime)
        {
			if (MapEditorManager.IgnoreInput) return;

			//// TODO: Add your update logic here
			KeyboardState keyState = Keyboard.GetState();

			// Only do this if your using something other than the pencil
			if (MapEditorManager.CurrentMap != null && MapEditorManager.CurrentTool == Tools.Pencil)
			{
				var camera = this.context.SceneProvider.GetCamera("camera1");
			    var mouseState = Mouse.GetState();

			    if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && mouseState.X > 0 && mouseState.Y > 0)
			    {
			        MapPoint tileLocation = new MapPoint((mouseState.X - (int)camera.MapOffset.X) / 32, (mouseState.Y - (int)camera.MapOffset.Y) / 32);
					MapHeader header = this.context.WorldManager.GetWorld("Default").Maps[MapEditorManager.CurrentMap.Name];

			        if (header.TilePointInMapLocal(tileLocation))
			        {
			            for (int y = 0; y <= MapEditorManager.SelectedTilesRectangle.Height; y++)
			            {
			                for (int x = 0; x <= MapEditorManager.SelectedTilesRectangle.Width; x++)
			                {
			                    MapPoint tilesheetPoint = new MapPoint(MapEditorManager.SelectedTilesRectangle.X + x, MapEditorManager.SelectedTilesRectangle.Y + y);
			                    MapPoint point = new MapPoint(tileLocation.X + x, tileLocation.Y + y);

			                    if (header.TilePointInMapLocal(point))
			                        MapEditorManager.CurrentMap.SetLayerValue(point, MapEditorManager.CurrentLayer, MapEditorManager.CurrentMap.GetTileSetValue(tilesheetPoint));
			                }
			            }
			        }
			    }
			}
        }

		public void LoadContent (GraphicsDevice graphicsDevice, IEngineContext context)
        {
			this.context = context;
        }

		private IEngineContext context = null;
    }
}
