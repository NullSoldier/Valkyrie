using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ValkyrieLibrary.Animation;
using ValkyrieLibrary;

namespace ValkyrieMapEditor.Forms
{
	public partial class frmAnimatedTile : Form
	{
		public FrameAnimation Tile;

		public frmAnimatedTile()
			: this(new FrameAnimation(0, 0, 0, 0, 0)) { }

		public frmAnimatedTile(FrameAnimation animation)
		{
			InitializeComponent();

			this.Tile = animation;

			this.inTilePane.Initialize(); // Set it up
			this.inTilePane.Image = frmMain.TileSheetImage;
			this.inTilePane.Size = this.inTilePane.Image.Size;
			this.inTilePane.TileSize = new Point(TileEngine.TileSize, TileEngine.TileSize);
			this.inTilePane.SelectedRect = new Rectangle(
				this.Tile.InitialFrameRect.X / TileEngine.TileSize,
				this.Tile.InitialFrameRect.Y / TileEngine.TileSize,
				((this.Tile.InitialFrameRect.Width / TileEngine.TileSize) * this.Tile.FrameCount) - 1,
				(this.Tile.InitialFrameRect.Height / TileEngine.TileSize) - 1);
			this.inTilePane.Invalidate();

			// Size correction
			int newx = this.Size.Width;
			int newy = this.splitTileManager.Panel1.Height;

			if (this.Size.Width > this.inTilePane.Image.Size.Width)
				newx = this.inTilePane.Image.Size.Width + 6;

			if (this.splitTileManager.Panel2.Height > this.inTilePane.Image.Size.Height)
				newy = this.inTilePane.Image.Size.Height;

			this.Height = newy + this.splitTileManager.Panel2.Height;
			this.Width = newx + 10;

			// Max size is the box's max size
			this.MaximumSize = new Size(this.Width, this.splitTileManager.Panel1.Height + this.inTilePane.Image.Height);

			this.inTilePane.TileSelectionChanged += this.OnSelectionChanged;
		}

		private void OnSelectionChanged(object sender, TileSelectionChangedEventArgs ev)
		{
			this.Tile.FrameLength = 0.2f;
			this.Tile.FrameCount = ev.Selection.Width;
			this.Tile.InitialFrameRect = new Microsoft.Xna.Framework.Rectangle(
				ev.Selection.X * TileEngine.TileSize,
				ev.Selection.Y * TileEngine.TileSize,
				TileEngine.TileSize,
				TileEngine.TileSize);
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

	}
}
