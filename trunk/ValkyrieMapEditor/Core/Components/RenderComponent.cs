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
using Valkyrie.Engine.Maps;

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

			if (this.context.SceneProvider.GetCamera("camera1") != null)
			{
				var camera = this.context.SceneProvider.GetCamera("camera1");
				camera.ResizeScreen(camera.Screen.X, camera.Screen.Y, e.Width, e.Height);

			    if (MapEditorManager.CurrentMap != null)
			    {
			        // Move the X origin
					int DisplayedWidth = (MapEditorManager.CurrentMap.MapSize.X * MapEditorManager.CurrentMap.TileSize) + (int)camera.MapOffset.X;
			        if (DisplayedWidth < e.Width)
			        {
						if(MapEditorManager.CurrentMap.MapSize.X * MapEditorManager.CurrentMap.TileSize < e.Width)
							camera.CenterOriginOnPoint(0, (int)(camera.MapOffset.Y * -1));
						else
						{
							int newOffset = (e.Width - DisplayedWidth);
							camera.CenterOriginOnPoint((int)(camera.MapOffset.X * -1) - newOffset, (int)(camera.MapOffset.Y * -1));
						}
					}

			        // Move the Y origin
					int DisplayedHeight = (MapEditorManager.CurrentMap.MapSize.Y * MapEditorManager.CurrentMap.TileSize) + (int)camera.MapOffset.Y;
					if(DisplayedHeight < e.Height)
					{
						if(MapEditorManager.CurrentMap.MapSize.Y * MapEditorManager.CurrentMap.TileSize < e.Height)
							camera.CenterOriginOnPoint((int)(camera.MapOffset.X * -1), 0);
						else
						{
							int newOffset = (e.Height - DisplayedHeight);
							camera.CenterOriginOnPoint((int)(camera.MapOffset.X * -1), (int)(camera.MapOffset.Y * -1) - newOffset);
						}
					}
			    }

			}
        }

        public void OnScrolled(object sender, ScrollEventArgs e)
        {
			var camera = this.context.SceneProvider.GetCamera("camera1");
			if(camera == null) return;

			int dif = (e.NewValue - e.OldValue);

			if (e.Type == ScrollEventType.EndScroll)
			    return;

			int x = (int)(camera.MapOffset.X) * -1;
			int y = (int)(camera.MapOffset.Y) * -1;

			if(e.ScrollOrientation == ScrollOrientation.VerticalScroll)
				y += dif * MapEditorManager.CurrentMap.TileSize;
			else
				x += dif * MapEditorManager.CurrentMap.TileSize;

			camera.CenterOriginOnPoint(new Point(x, y));
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
			if (MapEditorManager.CurrentMap == null)
			    return;

			if (MapEditorManager.ViewMode == ViewMode.All)
			{
				this.context.SceneProvider.DrawCamera(spriteBatch, "camera1");
			}
			else if (MapEditorManager.ViewMode == ViewMode.Below)
			{
			    if (MapEditorManager.CurrentLayer == MapLayers.TopLayer)
			    {
					this.context.SceneProvider.DrawCamera(spriteBatch, "camera1");
			    }
				else if(MapEditorManager.CurrentLayer == MapLayers.MiddleLayer)
				{
					this.context.SceneProvider.DrawCameraLayer(spriteBatch, "camera1", MapLayers.UnderLayer);
					this.context.SceneProvider.DrawCameraLayer(spriteBatch, "camera1", MapLayers.BaseLayer);
					this.context.SceneProvider.DrawCameraLayer(spriteBatch, "camera1", MapLayers.MiddleLayer);
				}
				else if(MapEditorManager.CurrentLayer == MapLayers.BaseLayer)
				{
					this.context.SceneProvider.DrawCameraLayer(spriteBatch, "camera1", MapLayers.UnderLayer);
					this.context.SceneProvider.DrawCameraLayer(spriteBatch, "camera1", MapLayers.BaseLayer);
				}
				else
				{
					this.context.SceneProvider.DrawCameraLayer(spriteBatch, "camera1", MapLayers.UnderLayer);
				}
			}
			else
			{
			    // I only changed this because I really wanted to use a switch here
			    // because I already wrote the code and I didn't want to waste it... heh
			    // Also it looks pretty clean. :D
			    switch (MapEditorManager.CurrentLayer)
			    {
			        case MapLayers.UnderLayer:
						this.context.SceneProvider.DrawCameraLayer(spriteBatch, "camera1", MapLayers.UnderLayer);
			            break;

			        case MapLayers.BaseLayer:
						this.context.SceneProvider.DrawCameraLayer(spriteBatch, "camera1", MapLayers.BaseLayer);
						break;

					case MapLayers.MiddleLayer:
						this.context.SceneProvider.DrawCameraLayer(spriteBatch, "camera1", MapLayers.MiddleLayer);
						break;

					case MapLayers.TopLayer:
						this.context.SceneProvider.DrawCameraLayer(spriteBatch, "camera1", MapLayers.TopLayer);
						break;

					default:
						break;
				}
			}
        }

        public void Update(GameTime gameTime)
        {
        }

		public void LoadContent (GraphicsDevice graphicsDevice, IEngineContext context)
        {
			this.context = context;
        }

		private IEngineContext context = null;
    }
}
