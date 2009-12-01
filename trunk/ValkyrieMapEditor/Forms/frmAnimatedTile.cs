using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Valkyrie.Library;
using ValkyrieMapEditor.Properties;
using Valkyrie.Engine.Animation;
using Valkyrie.Engine.Maps;

namespace ValkyrieMapEditor.Forms
{
	public partial class frmAnimatedTile : Form
	{
		public FrameAnimation Tile;

		public frmAnimatedTile(Map map, Image tilesheet)
			: this(map, tilesheet, new FrameAnimation(0, 0, 0, 0, 0)) { }

		public frmAnimatedTile(Map map, Image tilesheetimage, FrameAnimation animation)
		{
			InitializeComponent();

			this.Icon = Icon.FromHandle(Resources.imgFilm.GetHicon());

			this.Tile = animation;
			this.map = map;
			this.tilesheetimage = tilesheetimage;

			this.DisplayTile();
		}

		private Map map = null;
		private Image tilesheetimage = null;

		private void DisplayTile()
		{
			this.inTilePane.Initialize();

			this.inTilePane.MaximumSize = new Size(this.map.MapSize.X, 0);
			this.inTilePane.EnforceSize = true;
			this.inTilePane.Image = this.tilesheetimage;
			this.inTilePane.Size = this.inTilePane.Image.Size;
			this.inTilePane.TileSize = new Point(this.map.TileSize, this.map.TileSize);

			this.inTilePane.SelectedRect = new Rectangle(
				this.Tile.InitialFrameRect.X / this.map.TileSize,
				this.Tile.InitialFrameRect.Y / this.map.TileSize,
				((this.Tile.InitialFrameRect.Width / this.map.TileSize) * this.Tile.FrameCount) - 1,
				(this.Tile.InitialFrameRect.Height / this.map.TileSize) - 1);

			this.inTilePane.Invalidate();

			// Change the size of the window to fit the image
			int newx = this.Size.Width;
			int newy = this.splitTileManager.Panel1.Height;

			if(this.Size.Width > this.inTilePane.Image.Size.Width)
			    newx = this.inTilePane.Image.Size.Width + 6;

			if(this.splitTileManager.Panel2.Height > this.inTilePane.Image.Size.Height)
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
				ev.Selection.X * this.map.TileSize,
				ev.Selection.Y * this.map.TileSize,
				this.map.TileSize,
				this.map.TileSize);
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

	}
}
