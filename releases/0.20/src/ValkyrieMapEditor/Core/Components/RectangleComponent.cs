using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using Valkyrie.Engine;
using Valkyrie.Engine.Core;
using Valkyrie.Library;
using Valkyrie.Engine.Maps;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using ValkyrieMapEditor.Core.Actions;


namespace ValkyrieMapEditor.Core.Components
{
	public class RectangleComponent : IEditorComponent
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
				|| MapEditorManager.IgnoreInput
				|| ev.Button != MouseButtons.Left)
				return;

			var camera = MapEditorManager.GameInstance.Engine.SceneProvider.Cameras["camera1"];
			var map = MapEditorManager.CurrentMap;

			if (!ComponentHelpers.PointInBounds(camera, ev.X, ev.Y) || camera == null)
				return;

			// Start selecting
			this.startpoint = camera.ScreenSpaceToWorldSpace(new ScreenPoint(ev.X, ev.Y)).ToMapPoint();
			this.startpoint = this.ConstrainPoint(map, this.startpoint);
			this.endpoint = new MapPoint(startpoint.X, startpoint.Y);

			this.isselecting = true;
			this.startedinwindow = true;
		}

		public void OnMouseMove(object sender, MouseEventArgs ev)
		{
		}

		public void OnMouseUp(object sender, MouseEventArgs ev)
		{
			var rect = ComponentHelpers.GetSelectionRectangleTiles (this.startpoint.ToPoint(), this.endpoint.ToPoint());
			this.PlotRectangleChunk(rect);

			this.startpoint = this.GetNegativeOne();
			this.endpoint = this.GetNegativeOne();

			this.isselecting = false;
			this.startedinwindow = false;
		}

		public void OnMouseClicked(object sender, MouseEventArgs ev)
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

		public void Draw(SpriteBatch spriteBatch)
		{
			if (!MapEditorManager.IsMapLoaded
				|| MapEditorManager.IgnoreInput
				|| !isselecting)
				return;

			var camera = this.context.SceneProvider.Cameras["camera1"];
			var mouse = Mouse.GetState();
			var map = MapEditorManager.CurrentMap;

			if (!ComponentHelpers.PointInBounds(camera, mouse.X, mouse.Y) || camera == null)
				return;

			var rect = ComponentHelpers.GetSelectionRectangle(startpoint.ToScreenPoint().ToPoint(), endpoint.ToScreenPoint().ToPoint());

			this.PlotSelectionRect(spriteBatch, camera, rect);
		}

		public void Update(GameTime gameTime)
		{
			if (!MapEditorManager.IsMapLoaded
				|| MapEditorManager.IgnoreInput
				|| !startedinwindow)
				return;

			var mouse = Mouse.GetState();
			var camera = MapEditorManager.GameInstance.Engine.SceneProvider.Cameras["camera1"];
			var map = MapEditorManager.CurrentMap;

			if (camera == null
				|| map == null)
				return;

			if (isselecting && mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
			{
				this.endpoint = camera.ScreenSpaceToWorldSpace(new ScreenPoint(mouse.X, mouse.Y)).ToMapPoint();
				this.endpoint = this.ConstrainPoint(map, this.endpoint);
			}
		}

		public void LoadContent(GraphicsDevice graphicsDevice, IEngineContext context)
		{
			this.context = context;

			this.startpoint = GetNegativeOne();
			this.endpoint = GetNegativeOne();
		}

		private IEngineContext context;
		private MapPoint startpoint;
		private MapPoint endpoint;
		private bool startedinwindow = false;
		private bool isselecting = false;
		private Color bordercolor = new Color(93, 134, 212, 255);
		private Color fillcolor = new Color(160, 190, 234, 100);

		private MapPoint GetNegativeOne()
		{
			return new MapPoint(-1, -1);
		}

		private MapPoint ConstrainPoint(Map map, MapPoint mpoint)
		{
			mpoint.IntX = mpoint.IntX.Clamp(0, map.MapSize.IntX - 1);
			mpoint.IntY = mpoint.IntY.Clamp(0, map.MapSize.IntY - 1);

			return mpoint;
		}

		/// <summary>
		/// Plots a chunk of tiles within a specified rectangle
		/// </summary>
		/// <param name="rect">The MapPoint based selection rectangle</param>
		private void PlotRectangleChunk(Rectangle rect)
		{
			var camera = this.context.SceneProvider.Cameras["camera1"];
			var map = MapEditorManager.CurrentMap;
			var tilerect = MapEditorManager.SelectedTilesRectangle;
			var plotstart = new MapPoint(rect.X, rect.Y);

			ActionBatchAction<PlaceTileAction> batchactions = new ActionBatchAction<PlaceTileAction>();

			// Keep track of where in the tile sheet we are
			int tilex = 0;
			int totalx = 0;
			int tiley = 0;

			for (int i = 0; i < rect.Width * rect.Height; i++)
			{
				int y = (i / rect.Width);
				int x = (i - (y * rect.Width));

				MapPoint tilepoint = new MapPoint(plotstart.X + x, plotstart.Y + y);
				MapPoint sheetpoint = new MapPoint(tilerect.X + tilex, tilerect.Y + tiley);

				if (ComponentHelpers.PointInMap(map, tilepoint))
				{
					batchactions.Add(new PlaceTileAction(tilepoint.IntX, tilepoint.IntY,MapEditorManager.CurrentLayer,map.GetTileSetValue(sheetpoint)));
				}

				tilex++;
				totalx++;
				if (tilex >= tilerect.Width || totalx >= rect.Width)
				{
					if (totalx >= rect.Width)
					{
						tiley++;
						
						totalx = 0;
						tilex = 0;
					}

					tilex = 0;
				}

				if (tiley >= tilerect.Height)
					tiley = 0;
			}

			MapEditorManager.ActionManager.PerformAction(batchactions);
			MapEditorManager.OnMapChanged();
		}

		private void PlotSelectionRect(SpriteBatch spritebatch, BaseCamera camera, Rectangle rect)
		{
			var selecttexture = EditorXNA.CreateSelectRectangle(rect.Width, rect.Height);
			var startplot = new Vector2(rect.X, rect.Y);

			spritebatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState, camera.TransformMatrix);
			spritebatch.Draw(selecttexture, startplot, Color.White);
			spritebatch.End();
		}
	}
}
