using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ValkyrieMapEditor.Properties;

namespace ValkyrieMapEditor
{
	public class TileBox : PictureBox
	{
		public event EventHandler<TileSelectionChangedEventArgs> TileSelectionChanged;
		public Size MaximumSize = new Size(5, 0);
		public bool EnforceSize = true;

		public Point TileSize
		{
			get { return this.tilesize; }
			set { this.tilesize = value; }
		}

		public Rectangle SelectedRect
		{
			get
			{
				return new Rectangle(this.SelectedPoint.X, this.SelectedPoint.Y,
					this.EndSelectedPoint.X - this.SelectedPoint.X,
					this.EndSelectedPoint.Y - this.SelectedPoint.Y);
			}
			set
			{
				this.SelectedPoint = new Point(value.X, value.Y);
				this.EndSelectedPoint = new Point(value.Width + value.X, value.Height + value.Y);
			}
		}

		public Point SelectedPoint
		{
			get { return this.selectionpoint; }
			set
			{
				this.selectionpoint = value;
				this.Invalidate();
			}
		}

		public Point EndSelectedPoint
		{
			get { return this.endselectionpoint; }
			set
			{
				this.endselectionpoint = value;
				this.Invalidate();
			}
		}

		public bool DisplayTileSelection
		{
			get { return this.displaytileselection; }
			set
			{
				this.displaytileselection = value;
				this.Invalidate();
			}
		}

		private Point tilesize;
		private Point selectionpoint;
		private Point endselectionpoint;
		private bool displaytileselection;
		

		public void Initialize()
		{
			this.SelectedPoint = new Point(0, 0);
			this.EndSelectedPoint = new Point(0, 0);

			this.MouseDown += this.Tile_MouseDown;
			this.MouseMove += this.Tile_MouseMove;
			this.MouseUp += this.Tile_MouseUp;

			//this.MouseClick += this.MouseClicked;
		}

		public void Tile_MouseDown(object sender, MouseEventArgs ev)
		{
			if (this.Image == null) 
                return;

			this.SelectedPoint = new Point(ev.X / 32, ev.Y / 32);
			this.EndSelectedPoint = new Point(ev.X / 32, ev.Y / 32);
		}

		public void Tile_MouseMove(object sender, MouseEventArgs ev)
		{
			if (this.Image == null) 
                return;

			if( ev.Button == MouseButtons.Left)
				this.EndSelectedPoint = new Point(ev.X / 32, ev.Y / 32);

			if (this.EnforceSize && this.SelectedRect.Width > this.MaximumSize.Width)
				this.SelectedRect = new Rectangle(this.SelectedRect.X, this.SelectedRect.Y, this.MaximumSize.Width, this.SelectedRect.Height);
			if (this.EnforceSize && this.SelectedRect.Height > this.MaximumSize.Height)
				this.SelectedRect = new Rectangle(this.SelectedRect.X, this.SelectedRect.Y, this.SelectedRect.Width, this.MaximumSize.Height);


		}

		public void Tile_MouseUp(object sender, MouseEventArgs ev)
		{
			if (this.Image == null) 
                return;

			this.EndSelectedPoint = new Point(ev.X / 32, ev.Y / 32);

			// Call the event to say tile selection has changed
			Rectangle tileSelection = new Rectangle(this.SelectedPoint.X,
				this.SelectedPoint.Y,
				this.EndSelectedPoint.X - this.SelectedPoint.X,
				this.EndSelectedPoint.Y - this.SelectedPoint.Y);

			var handler = this.TileSelectionChanged;
			
			if(handler != null)
				handler(this, new TileSelectionChangedEventArgs(tileSelection));
		}

		public void MouseClicked(object sender, MouseEventArgs ev)
		{
			if (this.Image == null) 
                return;

			this.SelectedPoint = new Point(ev.X / 32, ev.Y / 32);
			this.Invalidate();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			if (this.Image == null)
				return;

			pe.Graphics.DrawImage(this.Image, new Rectangle(0, 0, this.Image.Size.Width, this.Image.Size.Height));

			Brush currentBrush = Brushes.Black;
			int limit = 4;

			for (int i = 0; i <= limit; i++)
			{
				// Filled rectangle using primtiives
				currentBrush = ((i == 0 || i == limit) ? Brushes.Black : Brushes.White);

				pe.Graphics.DrawRectangle(new Pen(currentBrush, 1),
					new Rectangle(
						(this.SelectedPoint.X * this.TileSize.X) + i,
						(this.SelectedPoint.Y * this.TileSize.Y) + i,
						(((this.EndSelectedPoint.X * this.TileSize.X) - (this.SelectedPoint.X * this.TileSize.X)) - (i * 2)) + this.TileSize.X,
						(((this.EndSelectedPoint.Y * this.TileSize.Y) - (this.SelectedPoint.Y * this.TileSize.Y)) - (i * 2)) + this.TileSize.Y
						));
			}
		}
	}

	public class TileSelectionChangedEventArgs
		: EventArgs
	{
		public TileSelectionChangedEventArgs(Rectangle TileSelection)
		{
			this.Selection = TileSelection;
		}

		public Rectangle Selection
		{
			get { return this.selection; }
			set { this.selection = value; }
		}

		private Rectangle selection;
	}
}