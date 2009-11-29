using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Valkyrie.Library;
using Valkyrie.Engine.Animation;
using Valkyrie.Engine.Maps;

namespace ValkyrieMapEditor.Forms
{
	public partial class frmAnimatedTileManager : Form
	{
		#region Constructors

		public frmAnimatedTileManager(Map map, Image tilesheet)
		{
			InitializeComponent();

			this.map = map;
			this.tilesheet = tilesheet;
		}

		#endregion

		private Map map;
		private Image tilesheet;

		private void frmAnimatedTileManager_Load(object sender, EventArgs e)
		{
			this.BuildTileList();
		}

		private void lnkAdd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			frmAnimatedTile dialog = new frmAnimatedTile(this.map, this.tilesheet);
			DialogResult result = dialog.ShowDialog(this);

			if(result == DialogResult.OK)
			{
			    int tileID = ((dialog.Tile.InitialFrameRect.Y / this.map.TileSize) * this.map.TilesPerRow + dialog.Tile.InitialFrameRect.X);

			    this.map.AnimatedTiles.Add(tileID, dialog.Tile);
			}

			this.BuildTileList();
		}

		private void AddTileToList(FrameAnimation tile)
		{
			ListViewItem item = new ListViewItem(new string[] { tile.InitialFrameRect.ToString(), tile.FrameCount.ToString() });
			item.Tag = tile;

			this.lstAnimatedTiles.Items.Add(item);
		}

		private void lstAnimatedTiles_ItemActivate(object sender, EventArgs e)
		{
			ListViewItem item = this.lstAnimatedTiles.SelectedItems[0];
			
			var tile = (FrameAnimation)item.Tag;
			int tileID = ((tile.InitialFrameRect.Y / this.map.TileSize) * this.map.TilesPerRow + tile.InitialFrameRect.X);

			frmAnimatedTile dialog = new frmAnimatedTile(this.map, this.tilesheet, tile);
			dialog.ShowDialog(this);

			int newtileID = ((tile.InitialFrameRect.Y / this.map.TileSize) * this.map.TilesPerRow + tile.InitialFrameRect.X);

			//// Remove old, add new. Solves problem of automatically creating new dictionary items
			//// If the key doesn't exist
			this.map.AnimatedTiles.Remove(tileID);
			this.map.AnimatedTiles.Add(newtileID, dialog.Tile);

			this.BuildTileList();
		}

		private void lstAnimatedTiles_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if(e.KeyData == Keys.Delete
				&& this.lstAnimatedTiles.SelectedItems.Count > 0)
			{
				DialogResult result = MessageBox.Show("You are about to delete animated tiles.", "Confirm", MessageBoxButtons.OKCancel);

				if(result != DialogResult.OK)
					return;

				foreach(ListViewItem item in lstAnimatedTiles.SelectedItems)
				{
					var tile = (FrameAnimation)item.Tag;

					int tileID = ((tile.InitialFrameRect.Y / this.map.TileSize) * this.map.TilesPerRow + tile.InitialFrameRect.X);
					this.map.AnimatedTiles.Remove(tileID);
				}

				this.BuildTileList();
			}
		}

		private void BuildTileList()
		{
			this.lstAnimatedTiles.Items.Clear();

			foreach (FrameAnimation tile in map.AnimatedTiles.Values)
			    this.AddTileToList(tile);
		}


	}
}
