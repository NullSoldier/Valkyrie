using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Maps;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using Valkyrie.Library;
using Valkyrie.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ValkyrieMapEditor.Core.Components
{
	public class BucketComponent : IEditorComponent
	{
		public void OnSizeChanged(object sender, ScreenResizedEventArgs e)
		{
		}

		public void OnScrolled(object sender, ScrollEventArgs e)
		{
		}

		public void OnMouseDown(object sender, MouseEventArgs ev)
		{
			if (!MapEditorManager.IsMapLoaded
				|| MapEditorManager.IgnoreInput)
				return;

			var map = MapEditorManager.CurrentMap;
			var camera = this.context.SceneProvider.Cameras["camera1"];
			var tilesheetPoint = new MapPoint(MapEditorManager.SelectedTilesRectangle.X, MapEditorManager.SelectedTilesRectangle.Y);

			if(camera == null)
				return;

			var mappoint = camera.ScreenSpaceToWorldSpace(new ScreenPoint(ev.X, ev.Y)).ToMapPoint();

			int oldvalue = MapEditorManager.CurrentMap.GetLayerValue(mappoint, MapEditorManager.CurrentLayer);
			int newvalue = MapEditorManager.CurrentMap.GetTileSetValue(tilesheetPoint);

			this.FloodFill (map, MapEditorManager.CurrentLayer, mappoint.IntX, mappoint.IntY, oldvalue, newvalue);

			MapEditorManager.OnMapChanged();
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
			if (!MapEditorManager.IsMapLoaded
				|| MapEditorManager.IgnoreInput)
				return;

			var camera = MapEditorManager.GameInstance.Engine.SceneProvider.Cameras["camera1"];
			var mouse = Mouse.GetState();
			var map = MapEditorManager.CurrentMap;

			if (camera == null)
				return;

			var startpoint = camera.ScreenSpaceToWorldSpace(new ScreenPoint(mouse.X, mouse.Y)).ToMapPoint();
			var texture = EditorXNA.CreateSelectRectangle(32, 32);
			var drawloc = new Vector2(startpoint.X * 32, startpoint.Y * 32);

			if (!ComponentHelpers.PointInMap(map, startpoint))
				return;

			spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState, camera.TransformMatrix);
			spriteBatch.Draw(texture, drawloc, Color.White);
			spriteBatch.End();
		}

		public void Update(GameTime gameTime)
		{
		}

		public void LoadContent(GraphicsDevice graphicsDevice, IEngineContext context)
		{
			this.context = context;
		}

		private IEngineContext context;

		/// <summary>
		/// Performs a flood fill beginning on the target square
		/// </summary>
		/// <param name="map">The map to perform the operation on</param>
		/// <param name="layer">The layer to perform on</param>
		/// <param name="x">The x location to begin filling</param>
		/// <param name="y">The y location to begin filling</param>
		/// <param name="oldtileid">The ID of the tile to begin replacing.</param>
		/// <param name="newtileid">The ID of the replacement tile.</param>
		private void FloodFill(Map map, MapLayers layer, int x, int y, int oldtileid, int newtileid)
		{
			int value = map.GetLayerValue(new MapPoint(x, y), layer);

			if (value != oldtileid || value == newtileid)
				return;

			map.SetLayerValue(new MapPoint(x, y), layer, newtileid);

			if (x + 1 < MapEditorManager.CurrentMap.MapSize.X)
				this.FloodFill(map, layer, x + 1, y, oldtileid, newtileid);

			if (x - 1 >= 0)
				this.FloodFill(map, layer, x - 1, y, oldtileid, newtileid);

			if (y + 1 < MapEditorManager.CurrentMap.MapSize.Y)
				this.FloodFill(map, layer, x, y + 1, oldtileid, newtileid);

			if (y - 1 >= 0)
				this.FloodFill(map, layer, x, y - 1, oldtileid, newtileid);
		}
	}
}
