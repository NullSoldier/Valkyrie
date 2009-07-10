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

namespace ValkyrieMapEditor
{
	public partial class frmMain : Form
	{
		public bool MapChanged = false;

		public frmMain()
		{
			InitializeComponent();
		}


		public IntPtr getDrawSurface()
		{
			return pctSurface.Handle;
		}

        public IntPtr getDrawTilesSurface()
        {
            return pctTileSurface.Handle;
        }

		private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
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
            Map newMap = MapManager.LoadMap(MapLocation);
            TileEngine.SetMap(newMap);

            this.LoadMapSettingsToList(newMap);

            this.btnMapProperties.Enabled = true;
        }

        private void LoadMapSettingsToList(Map map)
        {
            this.lstSettings.Items.Clear();

            this.lstSettings.Items.Add(new ListViewItem(new string[] { "Name", map.Name }));
            this.lstSettings.Items.Add(new ListViewItem(new string[] { "Tile Size", map.TileSize.ToString() }));
            this.lstSettings.Items.Add(new ListViewItem(new string[] { "Tile Set", map.TextureName }));
            this.lstSettings.Items.Add(new ListViewItem(new string[] { "Tiles Per Row", map.TilesPerRow.ToString() }));
            this.lstSettings.Items.Add(new ListViewItem(new string[] { "Map Size", map.MapSize.ToString() }));

			this.pctSurface.Size = new Size(map.MapSize.X * map.TileSize.X, map.MapSize.Y * map.TileSize.Y);

			this.pctTileSurface.Image = Image.FromFile(TileEngine.Configuration["GraphicsRoot"] + "\\" + map.TextureName);
			this.pctTileSurface.Size = this.pctTileSurface.Image.Size;
        }

		private void pctSurface_Resize(object sender, EventArgs e)
		{            
			this.ScreenResized(this, new ScreenResizedEventArgs(this.pctSurface.Size.Width, this.pctSurface.Size.Height));
		}

        //public delegate void EventHandler(object sender, EventArgs e);

		public event EventHandler<ScreenResizedEventArgs> ScreenResized;
        public event EventHandler<SurfaceClickedEventArgs> SurfaceClicked;
        public event EventHandler MapSettingsChanged;

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

        private void frmMain_Load(object sender, EventArgs e)
        {
            if( this.ScreenResized != null)
                this.ScreenResized(this, new ScreenResizedEventArgs(this.pctSurface.Size.Width, this.pctSurface.Size.Height));
        }

        private void pctSurface_MouseClick(object sender, MouseEventArgs e)
        {
            this.SurfaceClicked(this, new SurfaceClickedEventArgs(e.Button, new Point(e.X, e.Y)));
        }

        private void pctTileSurface_MouseClick(object sender, MouseEventArgs e)
        {
            int tileX = (e.X / TileEngine.Map.TileSize.X);
            int tileY = (e.Y / TileEngine.Map.TileSize.Y);

           MapManager.CurrentTile = ((16 * tileY) + tileX);
        }

        private void btnMapProperties_Click(object sender, EventArgs e)
        {
            if (TileEngine.Map != null)
            {
                frmProperty dialog = new frmProperty(TileEngine.Map, false);
                dialog.ShowDialog(this);

                this.LoadMapSettingsToList(TileEngine.Map);
				TileEngine.Map = MapManager.ApplySettings(TileEngine.Map);
                //var settingsEvent = this.MapSettingsChanged;
               // settingsEvent(this, EventArgs.Empty);
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

            newMap = MapManager.ApplySettings(newMap);
            this.LoadMapSettingsToList(newMap);

           // var settingsEvent = this.MapSettingsChanged;
            //settingsEvent(this, EventArgs.Empty);

            TileEngine.Map = newMap;
        }

		private void toolSave_Click(object sender, EventArgs e)
		{
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
			this.SaveMapAs();
		}

		private void SaveMap()
		{
			if (TileEngine.Map == null) return;

			if (!MapManager.CurrentMapLocation.Exists)
				this.SaveMapAs();
			else
				MapManager.SaveMap(TileEngine.Map, MapManager.CurrentMapLocation);
		}

		private void SaveMapAs()
		{
			if (TileEngine.Map == null) return;

			SaveFileDialog dialog = new SaveFileDialog();
			dialog.ShowDialog();

			FileInfo file = new FileInfo(dialog.FileName);

			MapManager.SaveMap(TileEngine.Map, file);
		}
	}
}
