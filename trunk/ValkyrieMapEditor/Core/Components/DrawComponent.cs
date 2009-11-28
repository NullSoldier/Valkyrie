﻿using System;
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
			if(MapEditorManager.IgnoreInput) return;

			lock(this.pointlock)
			{
				if(ev.Button == MouseButtons.Left)
					this.startpoint = new Point((ev.X / 32) * 32, (ev.Y / 32) * 32);
			}
        }

        public void OnMouseMove(object sender, MouseEventArgs ev)
        {
        }

        public void OnMouseUp(object sender, MouseEventArgs ev)
        {
			lock(this.pointlock)
			{
				if(MapEditorManager.IgnoreInput || this.startpoint == this.GetNegativeOne()) return;

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

			if (mouseState.X > 0 && mouseState.Y > 0 &&
				mouseState.X < this.context.SceneProvider.GetCamera("camera1").Screen.Width &&
				mouseState.Y < this.context.SceneProvider.GetCamera("camera1").Screen.Height)
			{
				// Common properties
				Texture2D selectbox = null;
				Rectangle pos = Rectangle.Empty;

				if(MapEditorManager.CurrentTool == Tools.Pencil)
				{
					#region Pencil
					Point tileLocation = new Point(mouseState.X / 32, mouseState.Y / 32);
					Vector2 cLoc = new Vector2(tileLocation.X * 32, tileLocation.Y * 32);
					pos = new Rectangle((int)cLoc.X, (int)cLoc.Y, MapEditorManager.SelectedTilesRectangle.Width * 32 + 32, MapEditorManager.SelectedTilesRectangle.Height * 32 + 32);
					
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
							this.endpoint = new Point((mouseState.X / 32) * 32, (mouseState.Y / 32) * 32);

							pos = this.GetSelectionRectangle(this.startpoint, this.endpoint);
							selectbox = EditorXNA.CreateSelectRectangle(pos.Width, pos.Height);
						}
						#endregion
					}
				}

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

			this.startpoint = this.GetNegativeOne();
			this.endpoint = this.GetNegativeOne();
        }

		private IEngineContext context = null;
		private Point startpoint;
		private Point endpoint;
		private object pointlock = new object();


		private Rectangle GetSelectionRectangle (Point spoint, Point epoint)
		{
			int x = -1;
			int y = -1;
			int width = -1;
			int height = -1;

			if(spoint.X <= epoint.X)
			{
				x = spoint.X;
				width = (endpoint.X - spoint.X) + 32;
			}
			else
			{
				x = epoint.X;
				width = ((spoint.X + 32) - epoint.X);
			}

			if(spoint.Y <= endpoint.Y)
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
