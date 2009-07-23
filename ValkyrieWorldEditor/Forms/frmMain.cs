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
    public partial class frmMain : Form
    {
        public event EventHandler<ScreenResizedEventArgs> ScreenResized;
        public event EventHandler<ScrollEventArgs> ScrolledMap;

        public frmMain()
        {
            InitializeComponent();
        }

        private void UpdateScrollBars()
        {
            this.UpdateScrollBars(false);
        }

        private void UpdateScrollBars(bool Reset)
        {
            if (!TileEngine.IsMapLoaded) return;

            this.HorizontalScroll.Visible = (TileEngine.CurrentMapChunk.MapSize.X * TileEngine.CurrentMapChunk.TileSize.X > this.pctSurface.Size.Width);
            this.VerticalScroll.Visible = (TileEngine.CurrentMapChunk.MapSize.Y * TileEngine.CurrentMapChunk.TileSize.Y > this.pctSurface.Size.Height);

            int xTilesInView = ((this.pctSurface.Width + this.VerticalScroll.Width) / TileEngine.CurrentMapChunk.TileSize.X);
            int xUnseenAmount = TileEngine.CurrentMapChunk.MapSize.X - xTilesInView;
            this.HorizontalScroll.Maximum = xUnseenAmount + this.HorizontalScroll.LargeChange - 1;

            int yTilesInView = ((this.pctSurface.Height + this.HorizontalScroll.Height) / TileEngine.CurrentMapChunk.TileSize.Y);
            int yUnseenAmount = TileEngine.CurrentMapChunk.MapSize.Y - yTilesInView;
            this.VerticalScroll.Maximum = yUnseenAmount + this.VerticalScroll.LargeChange - 1;

            /* Don't set to 0 when they aren't visible
             * when there isnt enough map to show a scroll
             * bar the values for it are wonky!
            */
            if (Reset && this.HorizontalScroll.Visible)
                this.HorizontalScroll.Value = 0;

            if (Reset && this.VerticalScroll.Visible)
                this.VerticalScroll.Value = 0;
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

            //this.UpdateScrollBars();
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
                            cbDefaultSpawn.Items.Add(e.Parms["Name"]);
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

        public IntPtr getDrawSurface()
        {
            return this.pctSurface.Handle;
        }

        public IntPtr getPreviewSurface()
        {
            return this.pctPreview.Handle;
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
