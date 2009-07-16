using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using valkyrie.Core;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using ValkyrieMapEditor.Properties;
using System.Threading;

namespace ValkyrieMapEditor
{
	public partial class frmMain : Form
	{
		public bool MapChanged = false;
		public event EventHandler<ScreenResizedEventArgs> ScreenResized;
		public event EventHandler<SurfaceClickedEventArgs> SurfaceClicked;

		public frmMain()
		{
			InitializeComponent();

			this.pctTileSurface.Initialize();
			this.pctTileSurface.TileSelectionChanged += this.SelectionChanged;
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			if (this.ScreenResized != null)
				this.ScreenResized(this, new ScreenResizedEventArgs(this.pctSurface.Size.Width, this.pctSurface.Size.Height));
		}

		private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}

		public IntPtr getDrawSurface()
		{
			return pctSurface.Handle;
		}

        public IntPtr getDrawTilesSurface()
        {
            return pctTileSurface.Handle;
        }

		private void toolOpen_Click(object sender, EventArgs e)
		{
			OpenFileDialog openDialog = new OpenFileDialog();
			
            var result = openDialog.ShowDialog(this);
            if (result == DialogResult.Cancel)
                return;

            this.LoadMap(new FileInfo(openDialog.FileName));
        }

        public void LoadMap(FileInfo MapLocation)
        {
			TileEngine.TextureManager.ClearCache();
            TileEngine.SetMap(MapManager.LoadMap(MapLocation));

            this.RefreshMapProperties(TileEngine.Map);
        }

        private void RefreshMapProperties(Map map)
        {
            this.lstSettings.Items.Clear();

            this.lstSettings.Items.Add(new ListViewItem(new string[] { "Name", map.Name }));
            this.lstSettings.Items.Add(new ListViewItem(new string[] { "Tile Size", map.TileSize.ToString() }));
            this.lstSettings.Items.Add(new ListViewItem(new string[] { "Tile Set", map.TextureName }));
            this.lstSettings.Items.Add(new ListViewItem(new string[] { "Tiles Per Row", map.TilesPerRow.ToString() }));
            this.lstSettings.Items.Add(new ListViewItem(new string[] { "Map Size", map.MapSize.ToString() }));

			this.pctTileSurface.Image = Image.FromFile(TileEngine.Configuration["GraphicsRoot"] + "\\" + map.TextureName);
			this.pctTileSurface.Size = this.pctTileSurface.Image.Size;
			this.pctTileSurface.TileSize = new Point(TileEngine.Map.TileSize.X, TileEngine.Map.TileSize.Y);
			this.pctTileSurface.Invalidate();

			this.btnMapProperties.Enabled = true;
			this.toolSave.Enabled = true;
			this.toolSaveAs.Enabled = true;
			this.btnBaseLayer.Enabled = true;
			this.btnMiddleLayer.Enabled = true;
			this.btnTopLayer.Enabled = true;
			this.btnCollisionLayer.Enabled = true;
			this.btnSave.Enabled = true;
			this.btnPencil.Enabled = true;
			this.btnRect.Enabled = true;
			this.btnFill.Enabled = true;
        }

		private void pctSurface_Resize(object sender, EventArgs e)
		{            
			this.ScreenResized(this, new ScreenResizedEventArgs(this.pctSurface.Size.Width, this.pctSurface.Size.Height));
		}

        private void pctSurface_MouseClick(object sender, MouseEventArgs e)
        {
            this.SurfaceClicked(this, new SurfaceClickedEventArgs(e.Button, new Point(e.X, e.Y)));
        }

        private void btnMapProperties_Click(object sender, EventArgs e)
        {
            if (TileEngine.IsMapLoaded)
            {
                frmProperty dialog = new frmProperty(TileEngine.Map, false);
                dialog.ShowDialog(this);

                this.RefreshMapProperties(TileEngine.Map);
				TileEngine.Map = MapManager.ApplySettings(TileEngine.Map);
            }
        }

        private void btnBaseLayer_Click(object sender, EventArgs e)
        {
            this.btnMiddleLayer.Checked = false;
            this.btnTopLayer.Checked = false;
            this.btnCollisionLayer.Checked = false;

            MapManager.CurrentLayer = MapLayer.BaseLayer;
        }

        private void btnMiddleLayer_Click(object sender, EventArgs e)
        {
            this.btnBaseLayer.Checked = false;
            this.btnTopLayer.Checked = false;
            this.btnCollisionLayer.Checked = false;

            MapManager.CurrentLayer = MapLayer.MiddleLayer;
        }

        private void btnTopLayer_Click(object sender, EventArgs e)
        {
            this.btnBaseLayer.Checked = false;
            this.btnMiddleLayer.Checked = false;
            this.btnCollisionLayer.Checked = false;

            MapManager.CurrentLayer = MapLayer.TopLayer;
        }

        private void btnCollisionLayer_Click(object sender, EventArgs e)
        {
            this.btnBaseLayer.Checked = false;
            this.btnMiddleLayer.Checked = false;
            this.btnTopLayer.Checked = false;

            MapManager.CurrentLayer = MapLayer.CollisionLayer;
        }

        private void toolNew_Click(object sender, EventArgs e)
        {
            Map newMap = new Map();

            frmProperty dialog = new frmProperty(newMap, true);
            DialogResult result = dialog.ShowDialog(this);
			if (result == DialogResult.Cancel)
				return;

            TileEngine.Map = MapManager.ApplySettings(newMap);
            this.RefreshMapProperties(TileEngine.Map);
        }

		private void toolSave_Click(object sender, EventArgs e)
		{
			if (TileEngine.IsMapLoaded)
				this.SaveMap();
		}

		private void toolClose_Click(object sender, EventArgs e)
		{
			if(this.MapChanged)
			{
				DialogResult result = MessageBox.Show("Do you want to save changes to" + Environment.NewLine, "Save Changes", MessageBoxButtons.YesNoCancel);

				if (result == DialogResult.Yes)
					this.SaveMap();
				else if (result == DialogResult.Cancel)
					return;
			}

			this.Close();
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (TileEngine.IsMapLoaded)
				this.SaveMapAs();
		}

		private void SaveMap()
		{
			if (!TileEngine.IsMapLoaded) return;

			if (MapManager.CurrentMapLocation == null || !MapManager.CurrentMapLocation.Exists)
				this.SaveMapAs();
			else
				MapManager.SaveMap(TileEngine.Map, MapManager.CurrentMapLocation);
		}

		private void SaveMapAs()
		{
			if (TileEngine.IsMapLoaded)
			{
				SaveFileDialog dialog = new SaveFileDialog();
				var result = dialog.ShowDialog();

				if (result == DialogResult.None || result == DialogResult.Cancel)
					return;

				FileInfo file = new FileInfo(dialog.FileName);

				MapManager.SaveMap(TileEngine.Map, file);
			}
		}

		private void btnHelp_Click(object sender, EventArgs e)
		{
			foreach (ToolStripItem item in this.toolStripTools.Items)
			{
				if (item.DisplayStyle == ToolStripItemDisplayStyle.Image)
					item.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
				else
					item.DisplayStyle = ToolStripItemDisplayStyle.Image;
			}
		}

		private void btnViewSelected_Click(object sender, EventArgs e)
		{
			this.btnViewSelected.Checked = !this.btnViewSelected.Checked;
			this.pctTileSurface.DisplayTileSelection = this.btnViewSelected.Checked;
		}

		public void SelectionChanged(object sender, TileSelectionChangedEventArgs ev)
		{
			MapManager.SelectedTilesRect = new Microsoft.Xna.Framework.Rectangle(
				ev.Selection.X,
				ev.Selection.Y,
				ev.Selection.Width,
				ev.Selection.Height);
		}

	}

	#region EventArgs
	public class ScreenResizedEventArgs
			: EventArgs
	{
		public ScreenResizedEventArgs(int width, int height)
		{
			this.Width = width;
			this.Height = height;
		}

		public int Width;
		public int Height;
	}

	public class SurfaceClickedEventArgs
		: EventArgs
	{
		public SurfaceClickedEventArgs(MouseButtons button, Point location)
		{
			this.Location = location;
			this.Button = button;
		}

		public Point Location;
		public MouseButtons Button;
	}
	#endregion
}
