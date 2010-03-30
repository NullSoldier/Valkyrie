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
			if(MapEditorManager.IgnoreInput || ev.Button != MouseButtons.Left) return;

			if(MapEditorManager.CurrentTool == Tools.Rectangle)
			{
				lock(this.pointlock)
				{
					if(ev.Button == MouseButtons.Left)
						this.startpoint = new Point((ev.X / 32) * 32, (ev.Y / 32) * 32);
				}
			}
			else if(MapEditorManager.CurrentTool == Tools.Select)
			{
				lock(this.pointlock)
				{
					if(ev.Button == MouseButtons.Left)
					{
						// See if we clicked on the selected area
						// If we did set selectedgrabbed to true
						// 
						this.isdragging = true;
						this.startpoint = new Point((ev.X / 32) * 32, (ev.Y / 32) * 32);
					}
				}
			}
			else if(MapEditorManager.CurrentTool == Tools.Bucket)
			{
				var camera = this.context.SceneProvider.GetCamera("camera1");

				MapPoint tileLocation = new MapPoint((ev.X - (int)camera.Location.X) / 32, (ev.Y - (int)camera.Location.Y) / 32);
				MapPoint tilesheetPoint = new MapPoint(MapEditorManager.SelectedTilesRectangle.X, MapEditorManager.SelectedTilesRectangle.Y);

				int oldvalue = MapEditorManager.CurrentMap.GetLayerValue(tileLocation, MapEditorManager.CurrentLayer);
				int newvalue = MapEditorManager.CurrentMap.GetTileSetValue(tilesheetPoint);

				this.FloodFill(tileLocation.X, tileLocation.Y, oldvalue, newvalue);

				MapEditorManager.OnMapChanged();
			}
        }

        public void OnMouseMove(object sender, MouseEventArgs ev)
        {
        }

        public void OnMouseUp(object sender, MouseEventArgs ev)
        {
			if(MapEditorManager.IgnoreInput || ev.Button != MouseButtons.Left) return;

			this.isdragging = false;

			if(MapEditorManager.CurrentTool == Tools.Rectangle)
			{
				lock(this.pointlock)
				{
					if(this.startpoint == this.GetNegativeOne()) return;

					this.endpoint = new Point((ev.X / 32) * 32, (ev.Y / 32) * 32);

					Rectangle selectrect = this.GetSelectionRectangle(this.startpoint, this.endpoint);

					// Process
					int xtile = 0;
					int ytile = 0;
					var camera = MapEditorManager.GameInstance.Engine.SceneProvider.GetCamera("camera1");

					for(int y = 0; y < selectrect.Height / 32; y++)
					{
						for(int x = 0; x < selectrect.Width / 32; x++)
						{
							// Figure out location on map
							// Set it to proper tile
							MapPoint tileLocation = new MapPoint((selectrect.X - (int)camera.MapOffset.X) / 32, (selectrect.Y - (int)camera.MapOffset.Y) / 32);
							MapPoint tilesheetPoint = new MapPoint(MapEditorManager.SelectedTilesRectangle.X + xtile, MapEditorManager.SelectedTilesRectangle.Y + ytile);
							MapPoint point = new MapPoint(tileLocation.X + x, tileLocation.Y + y);

							if(point.X < MapEditorManager.CurrentMap.MapSize.X && point.Y < MapEditorManager.CurrentMap.MapSize.Y)
								MapEditorManager.CurrentMap.SetLayerValue(point, MapEditorManager.CurrentLayer, MapEditorManager.CurrentMap.GetTileSetValue(tilesheetPoint));

							xtile++;
							if(xtile > MapEditorManager.SelectedTilesRectangle.Width)
								xtile = 0;
						}

						ytile++;
						if(ytile > MapEditorManager.SelectedTilesRectangle.Height)
							ytile = 0;
					}

					this.endpoint = this.GetNegativeOne();
					this.startpoint = this.GetNegativeOne();

					MapEditorManager.OnMapChanged();
				}
			}
			else if(MapEditorManager.CurrentTool == Tools.Select)
			{
				if(this.endpoint == this.startpoint)
				{
					this.endpoint = this.GetNegativeOne();
					this.startpoint = this.GetNegativeOne();
				}

			}
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
			var camera = this.context.SceneProvider.GetCamera("camera1");

			// Common properties
			Texture2D selectbox = null;
			Rectangle pos = Rectangle.Empty;
			Point newLoc = Point.Zero;

			if(mouseState.X > 0 && mouseState.Y > 0 &&
				mouseState.X < this.context.SceneProvider.GetCamera("camera1").Screen.Width &&
				mouseState.Y < this.context.SceneProvider.GetCamera("camera1").Screen.Height)
			{
				if(MapEditorManager.CurrentTool == Tools.Pencil)
				{
					#region Pencil
					Point tileLocation = new Point(mouseState.X / 32, mouseState.Y / 32);
					Vector2 cLoc = new Vector2(tileLocation.X * 32, tileLocation.Y * 32);
					pos = new Rectangle((int)cLoc.X, (int)cLoc.Y, MapEditorManager.SelectedTilesRectangle.Width * 32 + 32, MapEditorManager.SelectedTilesRectangle.Height * 32 + 32);
					newLoc = new Point(pos.X, pos.Y);
					selectbox = EditorXNA.CreateSelectRectangle(pos.Width, pos.Height);
					#endregion
				}
				else if(MapEditorManager.CurrentTool == Tools.Rectangle)
				{
					lock(this.pointlock)
					{
						#region Rectangle
						if(startpoint != this.GetNegativeOne())
						{
							pos = this.GetSelectionRectangle(this.startpoint, this.endpoint);
							newLoc = new Point(pos.X, pos.Y);
							selectbox = EditorXNA.CreateSelectRectangle(pos.Width, pos.Height);
						}
						#endregion
					}
				}
				else if(MapEditorManager.CurrentTool == Tools.Bucket)
				{
					#region Bucket
					pos = new Rectangle((mouseState.X / 32) * 32, (mouseState.Y / 32) * 32, 32, 32);
					newLoc = new Point(pos.X, pos.Y);
					selectbox = EditorXNA.CreateSelectRectangle(pos.Width, pos.Height);
					#endregion
				}
			}


			if(MapEditorManager.CurrentTool == Tools.Select)
			{
				#region Select
				if(startpoint != this.GetNegativeOne())
				{
					pos = this.GetSelectionRectangle(this.startpoint, this.endpoint);
					
					newLoc = new Point((int)camera.MapOffset.X + (int)camera.CameraOffset.X + pos.X,
					 (int)camera.MapOffset.Y + (int)camera.CameraOffset.Y + pos.Y);

					selectbox = EditorXNA.CreateSelectRectangleFilled(pos.Width, pos.Height, new Color(93, 134, 212, 255), new Color(160, 190, 234, 160));
				}
				#endregion
			}

		    if (selectbox != null)
				spriteBatch.Draw(selectbox, new Vector2(newLoc.X, newLoc.Y), new Rectangle(0, 0, selectbox.Width, selectbox.Height), Color.White);
        }

        public void Update(GameTime gameTime)
        {
			if (MapEditorManager.IgnoreInput) return;

			//// TODO: Add your update logic here
			KeyboardState keyState = Keyboard.GetState();

			if(MapEditorManager.CurrentMap == null)
				return;

			// Get the mouse state for analyzing it's current state
			var mouseState = Mouse.GetState();

			// Is the left mouse pressed and in the map's range?
			if(mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && mouseState.X > 0 && mouseState.Y > 0)
			{
				var camera = this.context.SceneProvider.GetCamera("camera1");

				if(MapEditorManager.CurrentTool == Tools.Pencil)
				{
					MapPoint tileLocation = new MapPoint((mouseState.X - (int)camera.MapOffset.X) / 32, (mouseState.Y - (int)camera.MapOffset.Y) / 32);
					MapHeader header = this.context.WorldManager.GetWorld("Default").Maps[MapEditorManager.CurrentMap.Name];

					if(header.TilePointInMapLocal(tileLocation))
					{
						for(int y = 0; y <= MapEditorManager.SelectedTilesRectangle.Height; y++)
						{
							for(int x = 0; x <= MapEditorManager.SelectedTilesRectangle.Width; x++)
							{
								MapPoint tilesheetPoint = new MapPoint(MapEditorManager.SelectedTilesRectangle.X + x, MapEditorManager.SelectedTilesRectangle.Y + y);
								MapPoint point = new MapPoint(tileLocation.X + x, tileLocation.Y + y);

								if(header.TilePointInMapLocal(point))
									MapEditorManager.CurrentMap.SetLayerValue(point, MapEditorManager.CurrentLayer, MapEditorManager.CurrentMap.GetTileSetValue(tilesheetPoint));
							}
						}

						MapEditorManager.OnMapChanged();
					}
				}
				else if(MapEditorManager.CurrentTool == Tools.Rectangle || MapEditorManager.CurrentTool == Tools.Select)
				{
					lock(this.pointlock)
					{
						this.endpoint = new Point((mouseState.X / 32) * 32, (mouseState.Y / 32) * 32);
					}
				}
			}
        }

		public void LoadContent (GraphicsDevice graphicsDevice, IEngineContext context)
        {
			this.context = context;

			this.startpoint = this.GetNegativeOne();
			this.endpoint = this.GetNegativeOne();
        }

		private IEngineContext context = null;
		private Point startpoint;
		private Point endpoint;
		private object pointlock = new object();
		private bool isdragging = false;

		private void FloodFill (int x, int y, int targetid, int replacementid)
		{
			int value = MapEditorManager.CurrentMap.GetLayerValue(new MapPoint(x, y), MapEditorManager.CurrentLayer);

			if(value != targetid || value == replacementid)
				return;

			MapEditorManager.CurrentMap.SetLayerValue(new MapPoint(x, y), MapEditorManager.CurrentLayer, replacementid);

			if(x + 1 < MapEditorManager.CurrentMap.MapSize.X)
				this.FloodFill(x + 1, y, targetid, replacementid);

			if(x - 1 >= 0)
				this.FloodFill(x - 1, y, targetid, replacementid);

			if(y + 1 < MapEditorManager.CurrentMap.MapSize.Y)
				this.FloodFill(x, y + 1, targetid, replacementid);

			if(y - 1 >= 0)
				this.FloodFill(x, y - 1, targetid, replacementid);
		}

		private Rectangle GetSelectionRectangle (Point spoint, Point epoint)
		{
			int x = -1;
			int y = -1;
			int width = -1;
			int height = -1;

			if(spoint.X <= epoint.X)
			{
				x = spoint.X;
				width = (epoint.X - spoint.X) + 32;
			}
			else
			{
				x = epoint.X;
				width = ((spoint.X + 32) - epoint.X);
			}

			if(spoint.Y <= epoint.Y)
			{
				y = spoint.Y;
				height = (epoint.Y - spoint.Y) + 32;
			}
			else
			{
				y = epoint.Y;
				height = ((spoint.Y + 32) - epoint.Y);
			}

			return new Rectangle(x, y, width, height);
		}

		private Point GetNegativeOne ()
		{
			return new Point(-1, -1);
		}
    }
}
