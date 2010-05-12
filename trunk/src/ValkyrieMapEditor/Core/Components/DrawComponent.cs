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
using ValkyrieMapEditor.Core.Components;

namespace ValkyrieMapEditor.Core
{
	public class DrawComponent : IEditorComponent
	{
		public DrawComponent()
		{

		}

		public void OnComponentActivated()
		{
		}

		public void OnComponentDeactivated()
		{
		}

		public void OnSizeChanged(object sender, ScreenResizedEventArgs e) { }

		public void OnScrolled(object sender, ScrollEventArgs e) { }

		public void OnMouseDown(object sender, MouseEventArgs ev)
		{
			var camera = this.context.SceneProvider.Cameras["camera1"];

			if(!MapEditorManager.IsMapLoaded || camera == null)
				return;

			if (ComponentHelpers.PointInBounds(camera, ev.X, ev.Y))
				startedinwindow = true;
		}

		public void OnMouseMove(object sender, MouseEventArgs ev)
		{
		}

		public void OnMouseUp(object sender, MouseEventArgs ev)
		{
			startedinwindow = false;
		}

		public void OnMouseClicked(object sender, MouseEventArgs e)
		{

		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (!MapEditorManager.IsMapLoaded)
				return;

			var camera = this.context.SceneProvider.Cameras["camera1"];
			var mouse = Mouse.GetState();
			var rect = MapEditorManager.SelectedTilesRectangle;
			var map = MapEditorManager.CurrentMap;

			if(!ComponentHelpers.PointInBounds(camera, mouse.X, mouse.Y) || camera == null)
				return;

			var startpoint = camera.ScreenSpaceToWorldSpace(new ScreenPoint(mouse.X, mouse.Y)).ToMapPoint();
			var selecttexture = EditorXNA.CreateSelectRectangle(rect.Width * 32, rect.Height * 32);
			var drawloc = new Vector2(startpoint.X  * 32, startpoint.Y * 32);

			if (!ComponentHelpers.PointInMap(map, startpoint))
				return;

			spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState, camera.TransformMatrix);
			spriteBatch.Draw(selecttexture, drawloc, Color.White);
			spriteBatch.End();
		}

		public void Update(GameTime gameTime)
		{
			var mouse = Mouse.GetState();

			if (mouse.LeftButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed
				|| MapEditorManager.IgnoreInput
				|| !MapEditorManager.IsMapLoaded
				|| !startedinwindow)
				return;

			this.PlotTiles(mouse.X, mouse.Y);
		}

		public void LoadContent(GraphicsDevice graphicsDevice, IEngineContext context)
		{
			this.context = context;
		}

		private IEngineContext context = null;
		private bool startedinwindow = false;

		private void PlotTiles(int mousex, int mousey)
		{
			var camera = this.context.SceneProvider.Cameras["camera1"];
			var map = MapEditorManager.CurrentMap;
			var rect = MapEditorManager.SelectedTilesRectangle;

			if (!ComponentHelpers.PointInBounds(camera, mousex, mousey))
				return;

			MapPoint plotstart = camera.ScreenSpaceToWorldSpace(new ScreenPoint(mousex, mousey)).ToMapPoint();

			for (int i = 0; i < rect.Width * rect.Height; i++)
			{
				int y = (i / rect.Width);
				int x = (i - (y * rect.Width));

				MapPoint tilepoint = new MapPoint(plotstart.X + x, plotstart.Y + y);
				MapPoint sheetpoint = new MapPoint(rect.X + x, rect.Y + y);

				if (ComponentHelpers.PointInMap(map, tilepoint))
					map.SetLayerValue(tilepoint, MapEditorManager.CurrentLayer, map.GetTileSetValue(sheetpoint));

			}

			MapEditorManager.OnMapChanged();
		}
	}
}