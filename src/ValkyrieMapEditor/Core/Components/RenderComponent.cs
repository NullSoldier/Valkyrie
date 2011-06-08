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
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Core.Scene;
using Valkyrie.Engine.Providers;

namespace ValkyrieMapEditor.Core
{
	public class RenderComponent : IEditorComponent
	{
		public RenderComponent()
		{

		}

		public void OnComponentActivated()
		{
		}

		public void OnComponentDeactivated()
		{
		}

		public void OnCut()
		{
		}

		public void OnCopy()
		{
		}

		public void OnPaste()
		{
		}

		public void OnDelete()
		{
		}

		public void OnSizeChanged(object sender, ScreenResizedEventArgs e)
		{
			EditorXNA.graphics.PreferredBackBufferWidth = e.Width;
			EditorXNA.graphics.PreferredBackBufferHeight = e.Height;
			EditorXNA.graphics.ApplyChanges();
			// Throws exception

			if (!this.isloaded) return;

			if (this.context.SceneProvider.Cameras.ContainsKey("camera1"))
			{
				var camera = this.context.SceneProvider.Cameras["camera1"];
				camera.ResizeScreen(camera.Screen.X, camera.Screen.Y, e.Width, e.Height);

				if (MapEditorManager.CurrentMap != null)
				{
					// Move the X origin
					int DisplayedWidth = (MapEditorManager.CurrentMap.MapSize.IntX * MapEditorManager.CurrentMap.TileSize) + (int)camera.Origin.IntX;
					if (DisplayedWidth < e.Width)
					{
						if (MapEditorManager.CurrentMap.MapSize.X * MapEditorManager.CurrentMap.TileSize < e.Width)
							camera.CenterOriginOnPoint(0, (int)(camera.Origin.Y));
						else
						{
							int newOffset = (e.Width - DisplayedWidth);
							camera.CenterOriginOnPoint((int)(camera.Origin.X * -1) - newOffset, (int)(camera.Origin.Y * -1));
						}
					}

					// Move the Y origin
					int DisplayedHeight = (MapEditorManager.CurrentMap.MapSize.IntY * MapEditorManager.CurrentMap.TileSize) + (int)camera.Origin.IntY;
					if (DisplayedHeight < e.Height)
					{
						if (MapEditorManager.CurrentMap.MapSize.Y * MapEditorManager.CurrentMap.TileSize < e.Height)
							camera.CenterOriginOnPoint((int)(camera.Origin.X), 0);
						else
						{
							int newOffset = (e.Height - DisplayedHeight);
							camera.CenterOriginOnPoint((int)(camera.Origin.X * -1), (int)(camera.Origin.Y * -1) - newOffset);
						}
					}
				}
			}
		}

		public void OnScrolled(object sender, ScrollEventArgs e)
		{
			var camera = this.context.SceneProvider.Cameras["camera1"];
			if (camera == null) return;

			int dif = (e.NewValue - e.OldValue);

			if (e.Type == ScrollEventType.EndScroll)
				return;

			int x = camera.Origin.IntX;
			int y = camera.Origin.IntY;

			if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
				y += dif;
			else
				x += dif;

			camera.CenterOriginOnPoint(new ScreenPoint(x, y));
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

			scene.BeginScene("camera1");

			if (MapEditorManager.ViewMode == ViewMode.All)
			{
				scene.Draw(RenderFlags.NoPlayers);
			}
			else if (MapEditorManager.ViewMode == ViewMode.Below)
			{
				if (MapEditorManager.CurrentLayer == MapLayers.TopLayer)
				{
					scene.Draw(RenderFlags.NoPlayers);
				}
				else if (MapEditorManager.CurrentLayer == MapLayers.MiddleLayer)
				{
					scene.DrawLayer(MapLayers.UnderLayer);
					scene.DrawLayer(MapLayers.BaseLayer);
					scene.DrawLayer(MapLayers.MiddleLayer);
				}
				else if (MapEditorManager.CurrentLayer == MapLayers.BaseLayer)
				{
					scene.DrawLayer(MapLayers.UnderLayer);
					scene.DrawLayer(MapLayers.BaseLayer);
				}
				else
				{
					scene.DrawLayer(MapLayers.UnderLayer);
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
						scene.DrawLayer(MapLayers.UnderLayer);
						break;

					case MapLayers.BaseLayer:
						scene.DrawLayer(MapLayers.BaseLayer);
						break;

					case MapLayers.MiddleLayer:
						scene.DrawLayer(MapLayers.MiddleLayer);
						break;

					case MapLayers.TopLayer:
						scene.DrawLayer(MapLayers.TopLayer);
						break;

					default:
						break;
				}
			}

			this.context.SceneProvider.EndScene();
		}

		public void Update(GameTime gameTime)
		{
		}

		public void LoadContent(GraphicsDevice graphicsDevice, IEngineContext context)
		{
			this.context = context;
			this.scene = context.SceneProvider;

			this.isloaded = true;
		}

		private bool isloaded = false;
		private IEngineContext context = null;
		private ISceneProvider scene = null;
	}
}
