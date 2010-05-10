using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using Valkyrie.Engine;


namespace ValkyrieMapEditor.Core.Components
{
	class RectangleComponent : IEditorComponent
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
			throw new NotImplementedException();
		}

		public void OnMouseMove(object sender, MouseEventArgs ev)
		{
			throw new NotImplementedException();
		}

		public void OnMouseUp(object sender, MouseEventArgs ev)
		{
			lock (this.pointlock)
			{
				if (ev.Button == MouseButtons.Left)
					this.startpoint = new Point((ev.X / 32) * 32, (ev.Y / 32) * 32);
			}

			lock (this.pointlock)
			{
				if (this.startpoint == this.GetNegativeOne()) return;

				this.endpoint = new Point((ev.X / 32) * 32, (ev.Y / 32) * 32);

				Rectangle selectrect = this.GetSelectionRectangle(this.startpoint, this.endpoint);

				// Process
				int xtile = 0;
				int ytile = 0;
				var camera = MapEditorManager.GameInstance.Engine.SceneProvider.Cameras["camera1"];

				for (int y = 0; y < selectrect.Height / 32; y++)
				{
					for (int x = 0; x < selectrect.Width / 32; x++)
					{
						// Figure out location on map
						// Set it to proper tile
						MapPoint tileLocation = new MapPoint((selectrect.X - (int)camera.Offset.IntX) / 32, (selectrect.Y - (int)camera.Offset.IntY) / 32);
						MapPoint tilesheetPoint = new MapPoint(MapEditorManager.SelectedTilesRectangle.X + xtile, MapEditorManager.SelectedTilesRectangle.Y + ytile);
						MapPoint point = new MapPoint(tileLocation.X + x, tileLocation.Y + y);

						if (point.X < MapEditorManager.CurrentMap.MapSize.X && point.Y < MapEditorManager.CurrentMap.MapSize.Y)
							MapEditorManager.CurrentMap.SetLayerValue(point, MapEditorManager.CurrentLayer, MapEditorManager.CurrentMap.GetTileSetValue(tilesheetPoint));

						xtile++;
						if (xtile > MapEditorManager.SelectedTilesRectangle.Width)
							xtile = 0;
					}

					ytile++;
					if (ytile > MapEditorManager.SelectedTilesRectangle.Height)
						ytile = 0;
				}

				this.endpoint = this.GetNegativeOne();
				this.startpoint = this.GetNegativeOne();

				MapEditorManager.OnMapChanged();
			}
		}

		public void OnMouseClicked(object sender, MouseEventArgs ev)
		{
			throw new NotImplementedException();
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			lock (this.pointlock)
			{
				#region Rectangle
				if (startpoint != this.GetNegativeOne())
				{
					pos = this.GetSelectionRectangle(this.startpoint, this.endpoint);
					newLoc = new Point(pos.X, pos.Y);
					selectbox = EditorXNA.CreateSelectRectangle(pos.Width, pos.Height);
				}
				#endregion
			}
		}

		public void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			lock (this.pointlock)
			{
				this.endpoint = new Point((mouseState.X / 32) * 32, (mouseState.Y / 32) * 32);
			}
		}

		public void LoadContent(GraphicsDevice graphicsDevice, IEngineContext context)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
