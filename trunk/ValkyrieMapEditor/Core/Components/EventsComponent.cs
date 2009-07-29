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
using ValkyrieLibrary.Core;
using System.IO;
using ValkyrieLibrary;
using System.Windows.Forms;
using ValkyrieLibrary.Maps;
using ValkyrieMapEditor.Forms;
using ValkyrieLibrary.Events;

namespace ValkyrieMapEditor.Core
{
    public class EventsComponent : IEditorComponent
    {
        private Texture2D EventSprite;
        private Texture2D SelectionSprite;

        private bool Cancel { get; set; }

        public Point SelectedPoint {get; set; }
        public Point EndSelectedPoint { get; set; }


        public bool IsStartAfterEnd()
        {
            return (SelectedPoint.X > EndSelectedPoint.X || SelectedPoint.Y > EndSelectedPoint.Y);
        }

        public void LoadContent(GraphicsDevice device)
        {
            this.EventSprite = Texture2D.FromFile(device, "Graphics/EditorEvent.png");
            this.SelectionSprite = Texture2D.FromFile(device, "Graphics/EditorSelection.png");
        }

        public EventsComponent()
        {
            this.SelectedPoint = new Point(0, 0);
            this.EndSelectedPoint = new Point(0, 0);
        }

        private void CancelEvent()
        {
            Cancel = true;
            this.SelectedPoint = new Point(0, 0);
            this.EndSelectedPoint = new Point(0, 0);
        }

        public void CreateOrEditEvent()
        {
            if (Cancel)
                return;

            if (IsStartAfterEnd())
            {
                Point temp = SelectedPoint;
                SelectedPoint = EndSelectedPoint;
                EndSelectedPoint = temp;
            }

            int xOffset = (int)TileEngine.Camera.MapOffset.X / 32 + (int)TileEngine.Camera.CameraOffset.X;
            int yOffset = (int)TileEngine.Camera.MapOffset.Y / 32 + (int)TileEngine.Camera.CameraOffset.Y;

            MapPoint point = new MapPoint(SelectedPoint.X - xOffset, SelectedPoint.Y - yOffset);
            MapPoint size = new MapPoint(this.EndSelectedPoint.X - this.SelectedPoint.X, this.EndSelectedPoint.Y - this.SelectedPoint.Y);

            BaseMapEvent e = TileEngine.EventManager.GetEventInRect(point, size);

			if (e != null)
			{
				SelectedPoint = e.Location.ToPoint();
				EndSelectedPoint = (e.Location + e.Size).ToPoint();
			}

			frmMapEvent dialog = new frmMapEvent(e);
			DialogResult result = dialog.ShowDialog();

			e = dialog.Event;
			if( e != null)
				e.Rectangle = new Rectangle(point.X, point.Y, size.X, size.Y);

			if (result == DialogResult.OK)
                TileEngine.EventManager.SetOrAddEvent(e);

			else if (result == DialogResult.Abort)
                TileEngine.EventManager.DelEvent(e);
        }

        public void OnMouseDown(object sender, MouseEventArgs ev)
        {
            Cancel = false;
            //TileEngine.CurrentMapChunk.TilePointInMapLocal(tileLocation)
            this.SelectedPoint = new Point(ev.X / 32, ev.Y / 32);
            this.EndSelectedPoint = new Point(ev.X / 32 + 1, ev.Y / 32 + 1);
        }

        public void OnMouseMove(object sender, MouseEventArgs ev)
        {
            if (ev.Button == MouseButtons.Left)
            {
                if (!IsStartAfterEnd())
                    this.EndSelectedPoint = new Point(ev.X / 32 + 1, ev.Y / 32 + 1);
                else
                    CancelEvent();
            }
        }

        public void OnMouseUp(object sender, MouseEventArgs ev)
        {
            if (!IsStartAfterEnd())
            {
                this.EndSelectedPoint = new Point(ev.X / 32 + 1, ev.Y / 32 + 1);
                this.CreateOrEditEvent();
            }
            else
            {
                CancelEvent();
            }
        }

        public void OnMouseClicked(object sender, MouseEventArgs ev)
        {
            //this.SelectedPoint = new Point(ev.X / 32, ev.Y / 32);
            //this.EndSelectedPoint = new Point(ev.X / 32 + 1, ev.Y / 32 + 1);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (BaseMapEvent e in TileEngine.CurrentMapChunk.EventList)
            {

                Point newLoc = new Point((int)TileEngine.Camera.MapOffset.X + (int)TileEngine.Camera.CameraOffset.X + (e.Location.X * TileEngine.CurrentMapChunk.TileSize.X),
                     (int)TileEngine.Camera.MapOffset.Y + (int)TileEngine.Camera.CameraOffset.Y + (e.Location.Y * TileEngine.CurrentMapChunk.TileSize.Y));

  
                Texture2D border = CreateBorderRectangle(e.Size.X * 32, e.Size.Y * 32);

                if (border != null)
                {
                    Rectangle destRectangle = new Rectangle(newLoc.X, newLoc.Y, e.Size.X * 32, e.Size.Y * 32);
                    spriteBatch.Draw(border, destRectangle, new Rectangle(0, 0, border.Width, border.Height), Color.White);
                }

                spriteBatch.DrawString(EditorXNA.font, e.GetType(), new Vector2(newLoc.X + 10, newLoc.Y + 10), Color.AliceBlue);
            }

            if (!Cancel)
            {
                Point sel = SelectedPoint;
                Point end = EndSelectedPoint;

                Rectangle tileSelection = new Rectangle(sel.X * 32,
                    sel.Y * 32,
                    end.X * 32 - sel.X * 32,
                    end.Y * 32 - sel.Y * 32);

                Texture2D text = EditorXNA.CreateSelectRectangle(tileSelection.Width, tileSelection.Height);

                if (text == null)
                    text = this.SelectionSprite;

                spriteBatch.Draw(text, tileSelection, new Rectangle(0, 0, text.Width, text.Height), Color.White);
            }
        }

        static public Texture2D CreateBorderRectangle(int width, int height)
        {
            if (width <= 2 || height <=  2)
                return null;

            // create the rectangle texture, ,but it will have no color! lets fix that
            Texture2D rectangleTexture = new Texture2D(EditorXNA.graphicsDevice, width, height, 1, TextureUsage.None, SurfaceFormat.Color);
            Color[] color = new Color[width * height];//set the color to the amount of pixels

            //loop through all the colors setting them to whatever values we want
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    color[x + (y * width)] = new Color(255, 0, 0, 125);
                }
            }

            //outer four
            for (int y = 0; y < height; y++)
                color[0 + (y * width)] = new Color(0, 0, 0, 255);

            for (int y = 0; y < height; y++)
                color[width - 1 + (y * width)] = new Color(0, 0, 0, 255);

            for (int x = 0; x < width; x++)
                color[x + (0 * width)] = new Color(0, 0, 0, 255);

            for (int x = 0; x < width; x++)
                color[x + ((height - 1) * width)] = new Color(0, 0, 0, 255);


            rectangleTexture.SetData(color);//set the color data on the texture
            return rectangleTexture;
        }

        public void OnSizeChanged(object sender, ScreenResizedEventArgs e)
        {
        }

        public void OnScrolled(object sender, ScrollEventArgs e)
        {
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
