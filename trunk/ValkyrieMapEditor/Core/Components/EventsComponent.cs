using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using Valkyrie.Library;
using System.Windows.Forms;
using ValkyrieMapEditor.Forms;
using Valkyrie.Library.Events;
using Valkyrie.Engine;
using Valkyrie.Engine.Events;
using Valkyrie.Engine.Core;

namespace ValkyrieMapEditor.Core
{
    public class EventsComponent : IEditorComponent
    {
		public Point SelectedPoint
		{
			get { return this.selectedpoint; }
			set { this.selectedpoint = value; }
		}

		public Point EndSelectedPoint
		{
			get { return this.endselectedpoint; }
			set { this.endselectedpoint = value; }
		}

		public void LoadContent (GraphicsDevice device, IEngineContext context)
        {
            this.EventSprite = Texture2D.FromFile(device, "Graphics/EditorEvent.png");
            this.SelectionSprite = Texture2D.FromFile(device, "Graphics/EditorSelection.png");

			this.context = context;
        }

        public void CreateOrEditEvent(Rectangle selectedRect)
        {
			MapEditorManager.IgnoreInput = true;

			var camera = this.context.SceneProvider.GetCamera("camera1");
			
			int xOffset = (int)camera.MapOffset.X / 32 + (int)camera.CameraOffset.X;
			int yOffset = (int)camera.MapOffset.Y / 32 + (int)camera.CameraOffset.Y;

			IMapEvent mapevent = this.context.EventProvider.GetMapsEvents(MapEditorManager.CurrentMap.Name).Where(e => e.Rectangle.Intersects(selectedRect)).FirstOrDefault();

			bool newEvent = false;
			if(mapevent != null)
			{
				SelectedPoint = new Point(mapevent.Rectangle.X * 32, mapevent.Rectangle.Y * 32);
				EndSelectedPoint = new Point((mapevent.Rectangle.X * 32) + ((mapevent.Rectangle.Width - 1) * 32), (mapevent.Rectangle.Y * 32) + ((mapevent.Rectangle.Height - 1) * 32));
			}
			else
			{
				newEvent = true;
			}

			MapPoint size = new MapPoint(((this.EndSelectedPoint.X - this.SelectedPoint.X) / 32) + 1, ((this.EndSelectedPoint.Y - this.SelectedPoint.Y) / 32) + 1);

			frmMapEvent dialog = new frmMapEvent(mapevent);
			DialogResult result = dialog.ShowDialog();

			mapevent = dialog.Event;
			if(mapevent != null)
			{
				if(!newEvent)
					mapevent.Rectangle = new Rectangle(mapevent.Rectangle.X, mapevent.Rectangle.Y, size.X, size.Y);
				else
					mapevent.Rectangle = new Rectangle(selectedRect.X, selectedRect.Y, size.X, size.Y);
			}

			if(result == DialogResult.OK)
			{
				this.context.EventProvider.ReferenceSetOrAdd(MapEditorManager.CurrentMap.Name, mapevent);
				MapEditorManager.OnMapChanged();
			}
			else if(result == DialogResult.Abort)
			{
				this.context.EventProvider.Remove(MapEditorManager.CurrentMap.Name, mapevent);
				MapEditorManager.OnMapChanged();
			}

			MapEditorManager.IgnoreInput = false;
        }

        public void OnMouseDown(object sender, MouseEventArgs ev)
        {
			if(MapEditorManager.IgnoreInput) return;

			lock(this.pointlock)
			{
				this.SelectedPoint = new Point((ev.X / 32) * 32, (ev.Y / 32) * 32);
				this.EndSelectedPoint = new Point((ev.X / 32) * 32, (ev.Y / 32) * 32);
			}
        }

        public void OnMouseUp(object sender, MouseEventArgs ev)
        {
			if(MapEditorManager.IgnoreInput) return;

			lock(this.pointlock)
			{
				this.EndSelectedPoint = new Point((ev.X / 32) * 32, (ev.Y / 32) * 32);

				var tmpRect = this.GetSelectionRectangle(this.SelectedPoint, this.EndSelectedPoint);

				this.CreateOrEditEvent(new Rectangle(tmpRect.X / 32, tmpRect.Y / 32, tmpRect.Width / 32, tmpRect.Height / 32));

				this.SelectedPoint = new Point(-1, -1);
				this.EndSelectedPoint = new Point(-1, -1);
			}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
			var camera = this.context.SceneProvider.GetCamera("camera1");

			// Render all of the events
			foreach (IMapEvent mapevent in this.context.EventProvider.GetMapsEvents(MapEditorManager.CurrentMap.Name))
			{
				// Where on the screen?\
				Point newLoc = new Point((int)camera.MapOffset.X + (int)camera.CameraOffset.X + (mapevent.Rectangle.X * MapEditorManager.CurrentMap.TileSize),
					 (int)camera.MapOffset.Y + (int)camera.CameraOffset.Y + (mapevent.Rectangle.Y * MapEditorManager.CurrentMap.TileSize));

				Texture2D border = EditorXNA.CreateSelectRectangleFilled(mapevent.Rectangle.Width * 32, mapevent.Rectangle.Height * 32, new Color(136, 0, 21, 255), new Color(255, 128, 128, 160));

				Rectangle destRectangle = new Rectangle(newLoc.X, newLoc.Y, mapevent.Rectangle.Width * 32, mapevent.Rectangle.Height * 32);

				if(border != null)
					spriteBatch.Draw(border, destRectangle, new Rectangle(0, 0, border.Width, border.Height), Color.White);

				// If this event is the one we've currently selected
				if(mapevent == this.currentmapevent)
				{
					Texture2D selection = EditorXNA.CreateSelectRectangle(mapevent.Rectangle.Width * 32, mapevent.Rectangle.Height * 32);

					if(selection != null)
						spriteBatch.Draw(selection, destRectangle, new Rectangle(0, 0, border.Width, border.Height), Color.White);
				}

				spriteBatch.DrawString(EditorXNA.font, mapevent.GetStringType(), new Vector2(newLoc.X + 10, newLoc.Y + 10), Color.AliceBlue);
			}

			// Only if there is no selected event and there is a valid selection.
			// -1 is used to denote an invalid selection
			if(this.currentmapevent == null && SelectedPoint.X != -1 && SelectedPoint.Y != -1)
			{
				var rectangle = this.GetSelectionRectangle(this.SelectedPoint, this.EndSelectedPoint);

				Texture2D texture = EditorXNA.CreateSelectRectangle(rectangle.Width, rectangle.Height);

				if(texture == null) // If we couldn't generate a texture use the loaded texture
					texture = this.SelectionSprite;

				spriteBatch.Draw(texture, rectangle, new Rectangle(0, 0, texture.Width, texture.Height), Color.White);
			}
        }

        public void OnSizeChanged(object sender, ScreenResizedEventArgs e)
        {
        }

        public void OnScrolled(object sender, ScrollEventArgs e)
        {
        }

		public void OnMouseClicked (object sender, MouseEventArgs e)
		{
		}

		public void OnMouseMove (object sender, MouseEventArgs e)
		{
		}

        public void Update(GameTime gameTime)
        {
			if(MapEditorManager.IgnoreInput) return;

			var mouseState = Mouse.GetState();

			if(mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
			{
				lock(this.pointlock)
				{
					this.EndSelectedPoint = new Point((mouseState.X / 32) * 32, (mouseState.Y / 32) * 32);
				}
			}
        }

		private IEngineContext context = null;
		private Texture2D EventSprite = null;
		private Texture2D SelectionSprite = null;
		private IMapEvent currentmapevent = null;
		public Point selectedpoint = new Point(-1, -1);
		public Point endselectedpoint = new Point(-1, -1);
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
    }
}
