using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ValkyrieMapEditor.Properties;
using Valkyrie.Library;

namespace ValkyrieMapEditor
{
	public class TileBox : PictureBox
	{
		public event EventHandler<TileSelectionChangedEventArgs> TileSelectionChanged;
		public Size MaximumSize = new Size(0, 0);
		public bool EnforceSize = false;

		public Point TileSize
		{
			get { return this.tilesize; }
			set { this.tilesize = value; }
		}

		public Rectangle SelectedRect
		{
			get
			{
				int x, y, width, height;

				if(this.SelectedPoint.X <= this.EndSelectedPoint.X)
				{
					x = this.SelectedPoint.X;
					width = (this.EndSelectedPoint.X + 1) - this.SelectedPoint.X;
				}
				else
				{
					x = this.EndSelectedPoint.X;
					width = (this.SelectedPoint.X + 1) - this.EndSelectedPoint.X;
				}

				if(this.SelectedPoint.Y <= this.EndSelectedPoint.Y)
				{
					y = this.SelectedPoint.Y;
					height = (this.EndSelectedPoint.Y + 1) - this.SelectedPoint.Y;
				}
				else
				{
					y = this.EndSelectedPoint.Y;
					height = (this.SelectedPoint.Y + 1) - this.EndSelectedPoint.Y;
				}

				return new Rectangle(x, y, width, height);

				#region Swap experiment
				Point startpoint = this.SelectedPoint;
				Point endpoint = this.EndSelectedPoint;

				// Swap them if the end is before the start
				if(SelectedPoint.X > EndSelectedPoint.X || SelectedPoint.Y > EndSelectedPoint.Y)
				{
					startpoint = this.EndSelectedPoint;
					endpoint = this.SelectedPoint;
				}

				return new Rectangle(startpoint.X, startpoint.Y,
					endpoint.X - startpoint.X,
					endpoint.Y - startpoint.Y);
				#endregion
			}
			set
			{
				if (value.Width <= 0 || value.Height <= 0)
					throw new ArgumentOutOfRangeException("The width or height cannot be less than 1");

				this.SelectedPoint = new Point(value.X, value.Y);
				this.EndSelectedPoint = new Point(value.Width - 1, value.Height - 1);

				OnTileSelectedChanged();
			}
		}

		private Point SelectedPoint
		{
			get { return this.selectionpoint; }
			set
			{
				this.selectionpoint = value;
				this.Invalidate();
			}
		}

		private Point EndSelectedPoint
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

		public void Initialize()
		{
			this.SelectedPoint = new Point(0, 0);
			this.EndSelectedPoint = new Point(0, 0);

			this.MouseDown += this.Tile_MouseDown;
			this.MouseMove += this.Tile_MouseMove;
			this.MouseUp += this.Tile_MouseUp;

			this.DisplayTileSelection = true;

			//this.MouseClick += this.MouseClicked;
		}

		public void SelectTiles (int x, int y, int width, int height)
		{
			this.SelectedRect = new Rectangle(x, y, width, height);
		}

		private Point tilesize;
		private Point selectionpoint;
		private Point endselectionpoint;
		private bool displaytileselection;

		private void Tile_MouseDown(object sender, MouseEventArgs ev)
		{
			if (this.Image == null || ev.Button != MouseButtons.Left) 
                return;

			this.SelectedPoint = new Point(ev.X / 32, ev.Y / 32);
			this.EndSelectedPoint = new Point(ev.X / 32, ev.Y / 32);
		}

		private void Tile_MouseMove(object sender, MouseEventArgs ev)
		{
			if (this.Image == null) 
                return;

			if( ev.Button == MouseButtons.Left)
				this.EndSelectedPoint = new Point(ev.X / 32, ev.Y / 32);

			if(this.EnforceSize)
			{
				if(this.EndSelectedPoint.X - this.SelectedPoint.X > this.MaximumSize.Width)
				{
					this.EndSelectedPoint = new Point(this.EndSelectedPoint.X + this.MaximumSize.Width, this.SelectedPoint.Y);
				}
				else if(this.SelectedPoint.X - this.EndSelectedPoint.X > this.MaximumSize.Width)
				{
					this.EndSelectedPoint = new Point(this.EndSelectedPoint.X - this.MaximumSize.Width, this.SelectedPoint.Y);
				}

				if (this.EndSelectedPoint.Y - this.SelectedPoint.Y > this.MaximumSize.Height)
				{
					this.EndSelectedPoint = new Point(this.EndSelectedPoint.X, this.SelectedPoint.Y + this.MaximumSize.Height);
				}
				else if(this.SelectedPoint.Y - this.EndSelectedPoint.Y > this.MaximumSize.Height)
				{
					this.EndSelectedPoint = new Point(this.EndSelectedPoint.X, this.SelectedPoint.Y - this.MaximumSize.Height);
				}
			}

			this.EnforceEdgeLimits();
		}

		private void EnforceEdgeLimits()
		{
			int maxendx = this.Width / this.tilesize.X;
			int maxendy = this.Height / this.tilesize.Y;

			int x = this.EndSelectedPoint.X;
			int y = this.EndSelectedPoint.Y;

			x = x.Clamp(0, maxendx - 1);
			y = y.Clamp(0, maxendy - 1);

			this.EndSelectedPoint = new Point(x, y);
		}

		private void Tile_MouseUp(object sender, MouseEventArgs ev)
		{
			if (this.Image == null || ev.Button != MouseButtons.Left) 
                return;

			this.EndSelectedPoint = new Point(ev.X / 32, ev.Y / 32);

			this.EnforceEdgeLimits();

			OnTileSelectedChanged();
		}

		private void MouseClicked(object sender, MouseEventArgs ev)
		{
			if (this.Image == null) 
                return;

			this.SelectedPoint = new Point(ev.X / 32, ev.Y / 32);
			this.Invalidate();
		}

		private void OnTileSelectedChanged()
		{
			// Call the event to say tile selection has changed
			Rectangle tileSelection = new Rectangle(this.SelectedRect.X,
				this.SelectedRect.Y,
				this.SelectedRect.Width, // Because the width does not start at 0
				this.SelectedRect.Height);

			var handler = this.TileSelectionChanged;

			if (handler != null)
				handler(this, new TileSelectionChangedEventArgs(tileSelection));
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			if (this.Image == null)
				return;

			pe.Graphics.DrawImage(this.Image, new Rectangle(0, 0, this.Image.Size.Width, this.Image.Size.Height));

			if (!this.DisplayTileSelection)
				return;

			var rectangle = this.SelectedRect;
			this.DrawSelection(pe.Graphics, rectangle.X * 32, rectangle.Y * 32, rectangle.Width * 32, rectangle.Height * 32);
		}

		protected void DrawSelection(Graphics gfx, int x, int y, int width, int height)
		{
			//outer four
			gfx.DrawRectangle(new Pen(Brushes.Black, 1),
				new Rectangle(x, y, width, height));

			gfx.DrawRectangle(new Pen(Brushes.White, 3),
				new Rectangle(x + 2, y + 2, width - 4, height - 4));

			gfx.DrawRectangle(new Pen(Brushes.Black, 1),
				new Rectangle(x + 4, y + 4, width - 8, height - 8));
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