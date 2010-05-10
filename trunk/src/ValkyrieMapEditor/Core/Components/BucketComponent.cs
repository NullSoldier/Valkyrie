using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Maps;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;

namespace ValkyrieMapEditor.Core.Components
{
	public class BucketComponent : IEditorComponent
	{
		#region IEditorComponent Members

		public void OnSizeChanged(object sender, ScreenResizedEventArgs e)
		{
			throw new NotImplementedException();
		}

		public void OnScrolled(object sender, ScrollEventArgs e)
		{
			throw new NotImplementedException();
		}

		public void OnMouseDown(object sender, MouseEventArgs ev)
		{
			var camera = this.context.SceneProvider.Cameras["camera1"];

			MapPoint tileLocation = new MapPoint((ev.X - (int)camera.Location.X) / 32, (ev.Y - (int)camera.Location.Y) / 32);
			MapPoint tilesheetPoint = new MapPoint(MapEditorManager.SelectedTilesRectangle.X, MapEditorManager.SelectedTilesRectangle.Y);

			int oldvalue = MapEditorManager.CurrentMap.GetLayerValue(tileLocation, MapEditorManager.CurrentLayer);
			int newvalue = MapEditorManager.CurrentMap.GetTileSetValue(tilesheetPoint);

			this.FloodFill(tileLocation.IntX, tileLocation.IntY, oldvalue, newvalue);

			MapEditorManager.OnMapChanged();
		}

		public void OnMouseMove(object sender, MouseEventArgs ev)
		{
			throw new NotImplementedException();
		}

		public void OnMouseUp(object sender, MouseEventArgs ev)
		{
			throw new NotImplementedException();
		}

		public void OnMouseClicked(object sender, MouseEventArgs ev)
		{
			throw new NotImplementedException();
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			#region Bucket
			pos = new Rectangle((mouseState.X / 32) * 32, (mouseState.Y / 32) * 32, 32, 32);
			newLoc = new Point(pos.X, pos.Y);
			selectbox = EditorXNA.CreateSelectRectangle(pos.Width, pos.Height);
			#endregion
		}

		public void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			throw new NotImplementedException();
		}

		public void LoadContent(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, Valkyrie.Engine.IEngineContext context)
		{
			throw new NotImplementedException();
		}

		#endregion

		private void FloodFill(Map map, MapLayers layer, int x, int y, int targetid, int replacementid)
		{
			int value = map.GetLayerValue(new MapPoint(x, y), layer);

			if (value != targetid || value == replacementid)
				return;

			map.SetLayerValue(new MapPoint(x, y), layer, replacementid);

			if (x + 1 < MapEditorManager.CurrentMap.MapSize.X)
				this.FloodFill(map, layer, x + 1, y, targetid, replacementid);

			if (x - 1 >= 0)
				this.FloodFill(map, layer, x - 1, y, targetid, replacementid);

			if (y + 1 < MapEditorManager.CurrentMap.MapSize.Y)
				this.FloodFill(map, layer, x, y + 1, targetid, replacementid);

			if (y - 1 >= 0)
				this.FloodFill(map, layer, x, y - 1, targetid, replacementid);
		}
	}
}
