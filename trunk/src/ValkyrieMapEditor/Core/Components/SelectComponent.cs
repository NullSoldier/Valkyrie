using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using Valkyrie.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Valkyrie.Library;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Maps;
using Valkyrie.Engine.Core.Scene;

namespace ValkyrieMapEditor.Core.Components
{
	public class SelectComponent : IEditorComponent
	{
		public void OnComponentActivated()
		{
		}

		public void OnComponentDeactivated()
		{
			var map = MapEditorManager.CurrentMap;

			if (currentgrabed != null && map != null)
				this.PutDownGrabbable(currentgrabed, map);

			this.grabbables.Clear();
			this.currentgrabed = null;
			this.isgrabbing = false;
			this.isselecting = false;
			this.startedinwindow = false;
		}

		public void OnSizeChanged(object sender, ScreenResizedEventArgs e)
		{
		}

		public void OnScrolled(object sender, ScrollEventArgs e)
		{
		}

		public void OnMouseMove(object sender, MouseEventArgs ev)
		{
			if (!MapEditorManager.IsMapLoaded || MapEditorManager.IgnoreInput)
				return;

			var camera = MapEditorManager.GameInstance.Engine.SceneProvider.Cameras["camera1"];

			if (camera == null || !ComponentHelpers.PointInBounds(camera, ev.X, ev.Y))
				return;

			if (this.isgrabbing && this.currentgrabed != null)
			{
				var mappoint = camera.ScreenSpaceToWorldSpace(new ScreenPoint(ev.X, ev.Y)).ToMapPoint();

				this.currentgrabed.Move(mappoint - this.graboffset);
			}
		}

		public void OnMouseUp(object sender, MouseEventArgs ev)
		{
			if (!MapEditorManager.IsMapLoaded || MapEditorManager.IgnoreInput)
				return;

			if (this.isselecting)
			{
				// They stop selecting, now make the region grabbable
				var region = ComponentHelpers.GetSelectionRectangle(this.startpoint.ToScreenPoint().ToPoint(), this.endpoint.ToScreenPoint().ToPoint());
				var mapregion = new Rectangle(region.X / 32, region.Y / 32, (region.Width / 32) - 1, (region.Height / 32) - 1);
				var camera = MapEditorManager.GameInstance.Engine.SceneProvider.Cameras["camera1"];
				var map = MapEditorManager.CurrentMap;

				if (camera == null)
					return;

				var grabbable = this.CreateGrabbable(mapregion, map, true);
				this.grabbables.Add(grabbable);

				this.currentgrabed = grabbable;

				this.isselecting = false;
			}
			else if (this.isgrabbing)
			{
				// Place grabbed tile at new location
				this.isgrabbing = false;
			}

			this.startedinwindow = false;
		}

		public void OnMouseClicked(object sender, MouseEventArgs ev)
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

			var grabpointmap = camera.ScreenSpaceToWorldSpace (new ScreenPoint(ev.X, ev.Y)).ToMapPoint();
			bool ingrabbed = this.IsGrabbableAtPoint(grabpointmap);

			// We are grabbing and we clicked somewhere out of the grab region
			if (currentgrabed != null && !ingrabbed)
			{
				this.PutDownGrabbable(currentgrabed, map);

				this.grabbables.Clear();
				this.currentgrabed = null;

				this.isgrabbing = false;
			}
			// We aren't grabbing and we grab a grabbable region
			else if (!this.isgrabbing && ingrabbed)
			{
				this.currentgrabed = this.GetGrabbableAtPoint(grabpointmap);
				
				var mappoint = camera.ScreenSpaceToWorldSpace(new ScreenPoint(ev.X, ev.Y)).ToMapPoint();
				this.graboffset = mappoint - new MapPoint(this.currentgrabed.GetStartPoint());

				this.isgrabbing = true;
			}
			
			// We aren't grabbing and we don't click in any grabbable regions
			if(!this.isgrabbing && !ingrabbed)
			{
				if(currentgrabed != null)
					this.PutDownGrabbable(currentgrabed, map);

				this.grabbables.Clear();
				this.currentgrabed = null;
				this.isgrabbing = false;

				// Start selecting
				this.startpoint = camera.ScreenSpaceToWorldSpace(new ScreenPoint(ev.X, ev.Y)).ToMapPoint();
				this.startpoint = this.ConstrainPoint(map, this.startpoint);
				this.endpoint = new MapPoint(startpoint.X, startpoint.Y);

				this.isselecting = true;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (startpoint == this.GetNegativeOne())
				return;

			var camera = MapEditorManager.GameInstance.Engine.SceneProvider.Cameras["camera1"];

			if(!MapEditorManager.IsMapLoaded || camera == null)
				return;

			var map = MapEditorManager.CurrentMap;
			
			if (this.isselecting)
			{
				var rect = ComponentHelpers.GetSelectionRectangle(startpoint.ToScreenPoint().ToPoint(), endpoint.ToScreenPoint().ToPoint()); // map rect
				this.PlotSelectionRect(spriteBatch, camera, rect);
			}

			foreach (Grabbable grab in grabbables)
			{
				var rect = ComponentHelpers.GetSelectionRectangle(grab.GetStartPoint().ToScreenPoint().ToPoint(), grab.GetEndPoint().ToScreenPoint().ToPoint());

				this.DrawGrabbable(spriteBatch, camera, map, grab);
				this.PlotSelectionRect(spriteBatch, camera, rect);
			}

		}

		public void Update(GameTime gameTime)
		{
			if(!MapEditorManager.IsMapLoaded
				|| MapEditorManager.IgnoreInput)
				return;

			var mouse = Mouse.GetState();
			var camera = MapEditorManager.GameInstance.Engine.SceneProvider.Cameras["camera1"];
			var map = MapEditorManager.CurrentMap;

			if (camera == null)
				return;

			if (isselecting)
			{
				this.endpoint = camera.ScreenSpaceToWorldSpace(new ScreenPoint(mouse.X, mouse.Y)).ToMapPoint();
				this.endpoint = this.ConstrainPoint(map, this.endpoint);
			}
		}

		public void LoadContent(GraphicsDevice graphicsDevice, IEngineContext context)
		{
			this.device = graphicsDevice;
			this.context = context;

			startpoint = GetNegativeOne();
			endpoint = GetNegativeOne();
			graboffset = GetNegativeOne();
		}

		private GraphicsDevice device;
		private IEngineContext context;
		private MapPoint startpoint;
		private MapPoint endpoint;
		private bool startedinwindow = false;
		private bool isselecting = false;
		private bool isgrabbing = false; 
		private Color bordercolor = new Color(93, 134, 212, 255);
		private Color fillcolor = new Color(160, 190, 234, 100);
		private List<Grabbable> grabbables = new List<Grabbable>();
		private Grabbable currentgrabed = null;
		private MapPoint graboffset;

		private MapPoint GetNegativeOne()
		{
			return new MapPoint(-1, -1);
		}

		private bool IsGrabbableAtPoint(MapPoint mpoint)
		{
			return (this.GetGrabbableAtPoint(mpoint) != default(Grabbable));
		}
		
		private Grabbable GetGrabbableAtPoint(MapPoint mpoint)
		{
			Check.NullArgument<MapPoint>(mpoint, "mpoint");

			return this.grabbables.Where(g => g.ContainsPoint(mpoint)).FirstOrDefault();
		}

		private MapPoint ConstrainPoint(Map map, MapPoint mpoint)
		{
			Check.NullArgument<Map>(map, "map");
			Check.NullArgument<MapPoint>(mpoint, "mpoint");

			mpoint.IntX = mpoint.IntX.Clamp(0, map.MapSize.IntX - 1);
			mpoint.IntY = mpoint.IntY.Clamp(0, map.MapSize.IntY - 1);

			return mpoint;
		}

		private void PlotSelectionRect(SpriteBatch spritebatch, BaseCamera camera, Rectangle rect)
		{
			Check.NullArgument<SpriteBatch>(spritebatch, "spritebatch");
			Check.NullArgument<BaseCamera>(camera, "camera");

			var selecttexture = EditorXNA.CreateSelectRectangleFilled(rect.Width, rect.Height, bordercolor, fillcolor);
			var startplot = new Vector2(rect.X, rect.Y);

			spritebatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState, camera.TransformMatrix);
			spritebatch.Draw(selecttexture, startplot, Color.White);
			spritebatch.End();
		}

		private void DrawGrabbable (SpriteBatch spritebatch, BaseCamera camera, Map map, Grabbable grabbable)
		{
			Check.NullArgument<SpriteBatch>(spritebatch, "spritebatch");
			Check.NullArgument<BaseCamera>(camera, "camera");
			Check.NullArgument<Map>(map, "map");
			Check.NullArgument<Grabbable>(grabbable, "grabbable");

			var startpoint = grabbable.GetStartPoint();

			spritebatch.Begin( SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, camera.TransformMatrix);
			foreach (var tile in grabbable.Tiles)
			{
				Rectangle destrect = new Rectangle((startpoint.IntX + tile.Offset.IntX ) * map.TileSize, (startpoint.IntY + tile.Offset.IntY) * map.TileSize, map.TileSize, map.TileSize);
				MapPoint mappoint = new MapPoint(startpoint.IntX + tile.Offset.IntX, startpoint.IntY + tile.Offset.IntY);

				if (!ComponentHelpers.PointInMap(map, mappoint))
					continue;

				PlotLayerTile(spritebatch, map, destrect, tile.Layer1);
				PlotLayerTile(spritebatch, map, destrect, tile.Layer2);
				PlotLayerTile(spritebatch, map, destrect, tile.Layer3);
				PlotLayerTile(spritebatch, map, destrect, tile.Layer4);
				
			}
			spritebatch.End();
		}

		private void PlotLayerTile(SpriteBatch spritebatch, Map map, Rectangle destrect, int tilevalue)
		{
			Check.NullArgument<SpriteBatch>(spritebatch, "spritebatch");
			Check.NullArgument<Map>(map, "map");

			Rectangle sourcerect = this.GetSourceRect(map, tilevalue);
				
			spritebatch.Draw (map.Texture, destrect, sourcerect, Color.White);
		}

		private Rectangle GetSourceRect(Map map, int value)
		{
			Check.NullArgument<Map>(map, "map");

			return new Rectangle(
					(value % map.TilesPerRow) * map.TileSize,
					(value / map.TilesPerRow) * map.TileSize,
					map.TileSize - 1, map.TileSize - 1);
		}

		private Grabbable CreateGrabbable(Rectangle maprect, Map map, bool clearout)
		{
			Check.NullArgument<Map>(map, "map");

			Grabbable grab = new Grabbable(maprect);

			int width = maprect.Width + 1;
			int height = maprect.Height + 1;

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					var mappoint = new MapPoint(x + maprect.X, y + maprect.Y);

					if (!ComponentHelpers.PointInMap(map, mappoint))
						continue;

					GrabbedTile tile = new GrabbedTile(new MapPoint(x, y))
					{
						Layer1 = map.GetLayerValue(mappoint, MapLayers.UnderLayer),
						Layer2 = map.GetLayerValue(mappoint, MapLayers.BaseLayer),
						Layer3 = map.GetLayerValue(mappoint, MapLayers.MiddleLayer),
						Layer4 = map.GetLayerValue(mappoint, MapLayers.TopLayer)
					};

					if (clearout)
					{
						map.SetLayerValue(mappoint, MapLayers.UnderLayer, -1);
						map.SetLayerValue(mappoint, MapLayers.BaseLayer, -1);
						map.SetLayerValue(mappoint, MapLayers.MiddleLayer, -1);
						map.SetLayerValue(mappoint, MapLayers.TopLayer, -1);
					}

					grab.Tiles.Add(tile);
				}
			}

			return grab;
		}

		private void PutDownGrabbable(Grabbable grabbable, Map map)
		{
			Check.NullArgument<Grabbable>(grabbable, "grabbable");
			Check.NullArgument<Map>(map, "map");

			var startpoint = grabbable.GetStartPoint();

			foreach (var tile in grabbable.Tiles)
			{
				var mappoint = new MapPoint(startpoint.X + tile.Offset.X, startpoint.Y + tile.Offset.Y);

				if (!ComponentHelpers.PointInMap(map, mappoint))
					continue;

				map.SetLayerValue(mappoint, MapLayers.UnderLayer, tile.Layer1);
				map.SetLayerValue(mappoint, MapLayers.BaseLayer, tile.Layer2);
				map.SetLayerValue(mappoint, MapLayers.MiddleLayer, tile.Layer3);
				map.SetLayerValue(mappoint, MapLayers.TopLayer, tile.Layer4);
			}
		}
	}

	public class Grabbable
	{
		public Grabbable(int x, int y, int width, int height)
			: this (new Rectangle(x, y, width, height))
		{
		}

		public Grabbable(Rectangle rect)
		{
			this.rect = rect;
			this.Tiles = new List<GrabbedTile>();
		}

		public List<GrabbedTile> Tiles
		{
			get;
			set;
		}

		public bool ContainsPoint(MapPoint point)
		{
			return new Rectangle(rect.X, rect.Y, rect.Width + 1, rect.Height + 1)
				.Contains(point.ToPoint());
		}

		public MapPoint GetStartPoint()
		{
			return new MapPoint(rect.X, rect.Y);
		}

		public MapPoint GetEndPoint()
		{
			return this.GetStartPoint() + new MapPoint(rect.Width, rect.Height);
		}

		public void Move(MapPoint point)
		{
			this.Move(point.IntX, point.IntY);
		}

		public void Move(int x, int y)
		{
			rect.X = x;
			rect.Y = y;
		}

		private Rectangle rect = Rectangle.Empty;
	}

	public class GrabbedTile
	{
		public GrabbedTile ()
		{
		}

		public GrabbedTile (MapPoint offset)
		{
			this.Offset = offset;
		}

		public GrabbedTile(MapPoint offset, int layer1, int layer2, int layer3, int layer4)
		{
			Offset = offset;
			Layer1 = layer1;
			Layer2 = layer2;
			Layer3 = layer3;
			Layer4 = layer3;
		}

		public int Layer1 = -1;
		public int Layer2 = -1;
		public int Layer3 = -1;
		public int Layer4 = -1;

		public MapPoint Offset;
	}

}
