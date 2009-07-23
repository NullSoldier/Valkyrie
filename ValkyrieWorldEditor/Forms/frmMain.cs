using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ValkyrieWorldEditor.Core;
using ValkyrieLibrary.Core;
using ValkyrieLibrary.Maps;
using ValkyrieLibrary;

namespace ValkyrieWorldEditor.Forms
{
    public partial class frmMain : XNARenderForm
    {
        public event EventHandler<ScreenResizedEventArgs> ScreenResized;
        public event EventHandler<ScrollEventArgs> ScrolledMap;

        public frmMain() 
        {
            InitializeComponent();

            XNARenderControls.Add(this.pctPreview);
            XNARenderControls.Add(this.pctSurface);

            InitRenderControls();

            this.ScreenResized += this.pctSurface.Resized;
            this.ScrolledMap += this.pctSurface.Scrolled;

            this.ScreenResized += this.pctPreview.Resized;
            this.ScrolledMap += this.pctPreview.Scrolled;
        }

        public void UpdateScrollBars()
        {
            this.UpdateScrollBars(false);
        }

        private void UpdateScrollBars(bool Reset)
        {
            if (!TileEngine.IsMapLoaded || TileEngine.WorldManager.CurrentWorld == null) 
                return;

            ScreenPoint worldMap = (TileEngine.WorldManager.CurrentWorld.WorldSize*(WorldEditor.Scale)).ToScreenPoint();
            ScreenPoint TileSize = TileEngine.CurrentMapChunk.TileSize * (WorldEditor.Scale);



            this.HorizontalScroll.Visible = (worldMap.X > this.pctSurface.Size.Width);
            this.VerticalScroll.Visible = (worldMap.Y > this.pctSurface.Size.Height);

            int xTilesInView = ((this.pctSurface.Width + this.VerticalScroll.Width) / TileSize.X);
            int xUnseenAmount = worldMap.ToMapPoint().X - xTilesInView;
            this.HorizontalScroll.Maximum = xUnseenAmount + this.HorizontalScroll.LargeChange - 1;

            int yTilesInView = ((this.pctSurface.Height + this.HorizontalScroll.Height) / TileSize.Y);
            int yUnseenAmount = worldMap.ToMapPoint().Y - yTilesInView;
            this.VerticalScroll.Maximum = yUnseenAmount + this.VerticalScroll.LargeChange - 1;

            MapPoint camOff = (new ScreenPoint(TileEngine.Camera.CameraOrigin) * (WorldEditor.Scale)).ToMapPoint();

            //camOff.X -= xTilesInView;
            //camOff.Y -= yTilesInView;

            /* Don't set to 0 when they aren't visible
             * when there isnt enough map to show a scroll
             * bar the values for it are wonky!
            */

            if (this.HorizontalScroll.Visible)
                this.HorizontalScroll.Value = Math.Min(this.HorizontalScroll.Maximum, camOff.X);

            if (this.VerticalScroll.Visible)
                this.VerticalScroll.Value = Math.Min(this.VerticalScroll.Maximum, camOff.Y);
        }
        private void frmMain_Activated(object sender, EventArgs e)
        {
            WorldEditor.IgnoreInput = false;
        }

        private void frmMain_Deactivated(object sender, EventArgs e)
        {
            WorldEditor.IgnoreInput = true;
        }



        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            var result = openDialog.ShowDialog(this);
            if (result == DialogResult.Cancel)
                return;

            this.LoadUniverse(new FileInfo(openDialog.FileName));
        }

        public void LoadUniverse(FileInfo UniLocation)
        {
            TileEngine.TextureManager.ClearCache();

            WorldEditor.LoadUniverse(UniLocation);

            this.UpdateScrollBars();
        }

        public void RefreshWorldList(WorldManager worldMng)
        {
            cbCurWorld.Items.Clear();

            foreach (var world in worldMng.WorldsList)
            {
                cbCurWorld.Items.Add(world.Value.Name);
            }

            if (worldMng.WorldsList.Count() > 0)
            {
                WorldEditor.SetCurWorld(worldMng.WorldsList.First().Value.Name);
                this.cbCurWorld.Text = worldMng.WorldsList.First().Value.Name;
                this.cbCurWorld.Enabled = true;
            }
            else
            {
                this.cbCurWorld.Text = "";
                this.cbCurWorld.Enabled = false;
            }
        }

        public void RefreshWorldProp(World curWorld)
        {
            if (curWorld == null)
            {
                groupWorld.Enabled = false;
            }
            else
            {
                groupWorld.Enabled = true;

                cbDefaultSpawn.Items.Clear();

                foreach (var map in curWorld.WorldList)
                {
                    foreach (var e in map.Value.Map.EventList)
                    {
                        if (e.Type == "Load")
                            cbDefaultSpawn.Items.Add(e.Parameters["Name"]);
                    }
                }

                cbDefaultSpawn.Text = curWorld.DefaultSpawn;
            }
        }

        public void RefreshMapProp(Map curMap)
        {
            if (curMap == null)
            {
                groupBox1.Enabled = false;
            }
            else
            {
                groupBox1.Enabled = true;
                tbMapName.Text = curMap.Name;
                tbMapXPos.Text = "0";
                tbMapYPos.Text = "0";
            }
        }

        private void OnWorldSelect(object sender, EventArgs e)
        {
            WorldEditor.SetCurWorld(cbCurWorld.Text);
        }

        private void OnSpawnChange(object sender, EventArgs e)
        {
            if (WorldEditor.CurWorld != null)
                WorldEditor.CurWorld.DefaultSpawn = cbDefaultSpawn.Text;
        }

        private void OnNameChange(object sender, EventArgs e)
        {
            if (WorldEditor.CurMap != null)
                WorldEditor.CurMap.Name = tbMapName.Text;
        }

        private void pctSurface_Click(object sender, EventArgs e)
        {

        }

        private void OnClose(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void pctSurface_Resized(object sender, EventArgs e)
        {
            this.ScreenResized(this, new ScreenResizedEventArgs(this.pctSurface.Size.Width, this.pctSurface.Size.Height));
            this.UpdateScrollBars();
        }

        private void OnScaleChanged(object sender, EventArgs e)
        {
            double val = (double)Double.Parse(tscbScale.Text);
            double res =(val * 0.01);
            WorldEditor.SetScale(res);

            UpdateScrollBars(false);
        }

        private void OnSurfaceScrolled(object sender, ScrollEventArgs e)
        {
			var handler = this.ScrolledMap;
			
			if( handler != null)
				this.ScrolledMap(sender, e);
        }

    }

    #region EventArgs
    public class ScreenResizedEventArgs : EventArgs
    {
        public ScreenResizedEventArgs(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public int Width;
        public int Height;
    }

    public class SurfaceClickedEventArgs : EventArgs
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
