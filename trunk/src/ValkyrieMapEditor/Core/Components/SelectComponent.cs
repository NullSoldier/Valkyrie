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

				Grabbable grabbable = new Grabbable(mapregion)
				{
					Texture = this.GetSelectionTexture(camera, map, mapregion)
				};

				this.grabbables.Add(grabbable);

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
			if (this.isgrabbing && !ingrabbed)
			{
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
			else
			{
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
				var scaledtilesize = (int)(map.TileSize * camera.Zoom);

				var rect = ComponentHelpers.GetSelectionRectangle(grab.GetStartPoint().ToScreenPoint().ToPoint(), grab.GetEndPoint().ToScreenPoint().ToPoint());
				var loc = new Vector2((rect.X * camera.Zoom) + camera.Offset.X, (rect.Y * camera.Zoom) + camera.Offset.Y);

				spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);
				//spriteBatch.Draw(grab.Texture, new Vector2(rect.X * camera.Zoom, rect.Y * camera.Zoom), Color.White);
				spriteBatch.Draw(grab.Texture, loc, null, Color.White, 0f, Vector2.Zero, camera.Zoom, SpriteEffects.None, 0f);
				spriteBatch.End();

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
			return this.grabbables.Where(g => g.ContainsPoint(mpoint)).FirstOrDefault();
		}

		private void PlotSelectionRect(SpriteBatch spritebatch, BaseCamera camera, Rectangle rect)
		{
			var selecttexture = EditorXNA.CreateSelectRectangleFilled(rect.Width, rect.Height, bordercolor, fillcolor);
			var startplot = new Vector2(rect.X, rect.Y);

			spritebatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState, camera.TransformMatrix);
			spritebatch.Draw(selecttexture, startplot, Color.White);
			spritebatch.End();
		}

		private MapPoint ConstrainPoint(Map map, MapPoint mpoint)
		{
			mpoint.IntX = mpoint.IntX.Clamp(0, map.MapSize.IntX - 1);
			mpoint.IntY = mpoint.IntY.Clamp(0, map.MapSize.IntY - 1);

			return mpoint;
		}

		private Texture2D GetSelectionTexture(BaseCamera camera, Map map, Rectangle rect)
		{
			float oldzoom = camera.Zoom;
			camera.Scale(1);

			int scaledtilesize = (int)(map.TileSize * camera.Zoom);

			Rectangle getrect = new Rectangle((rect.X * scaledtilesize) + camera.Origin.IntX,
										(rect.Y * scaledtilesize) + camera.Origin.IntY,
										(rect.Width * scaledtilesize) + scaledtilesize,
										(rect.Height * scaledtilesize) + scaledtilesize);
			
			getrect.Width = Helpers.Clamp(getrect.Width, 0, camera.Screen.Width - getrect.X);
			getrect.Height = Helpers.Clamp(getrect.Height, 0, camera.Screen.Height - getrect.Y);

			Texture2D texture = new Texture2D(this.device, getrect.Width, getrect.Height);

			device.SetRenderTarget(0, camera.Buffer);
			context.SceneProvider.BeginScene(camera);
			context.SceneProvider.Draw(RenderFlags.NoPlayers & RenderFlags.NoFog);
			context.SceneProvider.EndScene();
			device.SetRenderTarget(0, null);

			camera.Scale(oldzoom);

			Color[] data = new Color[getrect.Width * getrect.Height];
			camera.Buffer.GetTexture().GetData<Color> (0, getrect, data, 0, data.Length);
			texture.SetData<Color> (data);

			return texture;
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

		public Texture2D Texture
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
