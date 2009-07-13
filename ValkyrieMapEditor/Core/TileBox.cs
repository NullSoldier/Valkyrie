﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ValkyrieMapEditor.Properties;

namespace ValkyrieMapEditor
{
	public class TileBox
		: PictureBox
	{
		public Size TileSize { get; set; }
		public Image SelectionImage { get; set; }

		public Point SelectedPoint { get; set; }

		public Image OriginalImage
		{
			get { return this.originalimage; }
			set
			{
				this.Image = value;
				this.originalimage = value;
			}
		}

		private Image originalimage;
		private Point selectionpoint;

		public void Initialize()
		{
			this.SelectedPoint = new Point(0, 0);
			this.MouseClick += this.MouseClicked;
		}

		public void MouseClicked(object sender, MouseEventArgs ev)
		{
			if (this.Image == null)
				return;

			this.SelectedPoint = new Point(ev.X / 32, ev.Y / 32);

			this.DrawSelection();
		}

		public void DrawSelection()
		{
			this.Image = this.OriginalImage;

			this.DrawTo(new Bitmap(this.SelectionImage), 
				new Rectangle(this.SelectedPoint.X, this.SelectedPoint.Y, 32, 32));
		}

		public void DrawTo(Bitmap bitmap, Rectangle dest)
		{
			Bitmap newBitmap = new Bitmap(this.Image);

			for (int x = 0; x < bitmap.Size.Width; x++)
			{
				for (int y = 0; y < bitmap.Size.Height; y++)
				{
					int argb = bitmap.GetPixel(x, y).ToArgb();

					if (argb == 0)
						continue;
					else
						newBitmap.SetPixel(x + (dest.X * dest.Width), y + (dest.Y * dest.Height),
							Color.FromArgb(argb));
				}
			}

			this.Image = Image.FromHbitmap(newBitmap.GetHbitmap());
			this.Update();
		}


	}
}
