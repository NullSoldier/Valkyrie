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
		public static string ValkyrieGameInstallationAssemblyPath = @"C:\Users\NullSoldier\Documents\Code Work Area\Project Valkyrie\trunk\valkyrie\bin\x86\Debug\valkyrie.exe";

        public event EventHandler<ScreenResizedEventArgs> ScreenResized;
        public event EventHandler<ScrollEventArgs> ScrolledMap;

        private bool ignoreTextChanges = false;
        private bool ignoreCheckEvent = false;

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

            this.pctPreview.Cursor = System.Windows.Forms.Cursors.Hand;
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


        public void UpdateSelectedMap()
        {
            this.tbMapName.Text = "";
            this.ignoreTextChanges = true;

            if (WorldEditor.SelectedMaps.Count() == 0)
            {
                this.groupBox1.Enabled = false;
                this.tbMapName.Enabled = false;
            }
            else
            {
                this.groupBox1.Enabled = true;

                if (WorldEditor.SelectedMaps.Count() == 1)
                {
                    this.tbMapName.Enabled = true;
                    this.tbMapName.Text = WorldEditor.SelectedMaps[0].Map.Name;
                    this.tbMapXPos.Text = WorldEditor.SelectedMaps[0].MapLocation.X.ToString();
                    this.tbMapYPos.Text = WorldEditor.SelectedMaps[0].MapLocation.Y.ToString();
                }
                else
                {
                    this.tbMapName.Enabled = false;

                    MapPoint loc = new MapPoint(9999, 9999);

                    foreach (var mh in WorldEditor.SelectedMaps)
                    {
                        if (mh.MapLocation.X < loc.X)
                            loc.X = mh.MapLocation.X;

                        if (mh.MapLocation.Y < loc.Y)
                            loc.Y = mh.MapLocation.Y;
                    }

                    this.tbMapXPos.Text = loc.X.ToString();
                    this.tbMapYPos.Text = loc.Y.ToString();
                }
            }

            ignoreCheckEvent = true;
            for (int x = 0; x < listMapList.Items.Count; x++)
            {
                bool check = false;

                foreach (var map in WorldEditor.SelectedMaps)
                {
                    if (listMapList.Items[x].ToString() == map.MapName)
                    {
                        check = true;
                        break;
                    }
                }

                listMapList.SetItemChecked(x, check);
            }
            ignoreCheckEvent = false;


            this.ignoreTextChanges = false;
        }

        public void SetActiveComponent(ComponentID compId)
        {
            this.pctSurface.SetActiveComponent(compId);

            switch (compId)
            {
                case ComponentID.Hand:
                    this.pctSurface.Cursor = System.Windows.Forms.Cursors.Hand;

                    this.btnSelect.Checked = false;
                    this.btnMove.Checked = false;
                    this.btnHand.Checked = true;
                    break;

                case ComponentID.Move:
                    this.pctSurface.Cursor = System.Windows.Forms.Cursors.NoMove2D;

                    this.btnSelect.Checked = false;
                    this.btnMove.Checked = true;
                    this.btnHand.Checked = false;
                    break;

                case ComponentID.Select:
                    this.pctSurface.Cursor = System.Windows.Forms.Cursors.Arrow;

                    this.btnSelect.Checked = true;
                    this.btnMove.Checked = false;
                    this.btnHand.Checked = false;
                    break;
            }
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
            SetActiveComponent(ComponentID.Hand);
            EnableButtons();
        }

        private void EnableButtons()
        {
            this.btnSave.Enabled = true;
            this.btnHand.Enabled = true;
            this.btnMove.Enabled = true;
            this.btnSelect.Enabled = true;
            this.btnExport.Enabled = true;
            this.btnAddMap.Enabled = true;
            this.btnAddWorld.Enabled = true;
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
                listMapList.Items.Clear();

                foreach (var map in curWorld.MapList)
                {
                    listMapList.Items.Add( map.Value.MapName );

                    foreach (var e in map.Value.Map.EventList)
                    {
                        if (e.GetType() == "EntryPoint")
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

        private void OnAddWorldClicked(object sender, EventArgs e)
        {
            frmNewWorld newWorld = new frmNewWorld();
            DialogResult res = newWorld.ShowDialog();

            if (res == DialogResult.OK)
                WorldEditor.AddWorld(newWorld.GetString());
        }

        private void OnAddMapClicked(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            var result = openDialog.ShowDialog(this);
            if (result == DialogResult.Cancel)
                return;

            WorldEditor.AddMap(new FileInfo(openDialog.FileName));       
        }

        private void OnHandClicked(object sender, EventArgs e)
        {
            SetActiveComponent(ComponentID.Hand);
        }

        private void OnSelectClicked(object sender, EventArgs e)
        {
            SetActiveComponent(ComponentID.Select);
        }

        private void OnMoveClicked(object sender, EventArgs e)
        {
            SetActiveComponent(ComponentID.Move);
        }

        private void OnNewClicked(object sender, EventArgs e)
        {
            WorldEditor.NewUniverse("Main");
            EnableButtons();
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            TileEngine.WorldManager.Save();
        }

        private void OnExportClicked(object sender, EventArgs e)
        {
            DialogResult res = fbdExport.ShowDialog();

            if (res == DialogResult.OK)
                TileEngine.WorldManager.Export(fbdExport.SelectedPath);
        }

        private void OnMapXPosChange(object sender, EventArgs e)
        {
            if (ignoreTextChanges || this.tbMapXPos.Text.Length == 0)
                return;

            if (WorldEditor.SelectedMaps.Count() == 1)
            {
                WorldEditor.SelectedMaps[0].MapLocation.X = Int32.Parse(this.tbMapXPos.Text);
            }
            else
            {
                MapPoint loc = new MapPoint(9999, 9999);

                foreach (var mh in WorldEditor.SelectedMaps)
                {
                    if (mh.MapLocation.X < loc.X)
                        loc.X = mh.MapLocation.X;
                }

                foreach (var mh in WorldEditor.SelectedMaps)
                {
                    mh.MapLocation.X += Int32.Parse(this.tbMapXPos.Text) - loc.X;
                }
            }
        }

        private void OnMapYPosChange(object sender, EventArgs e)
        {
            if (ignoreTextChanges || this.tbMapYPos.Text.Length == 0)
                return;

            if (WorldEditor.SelectedMaps.Count() == 1)
            {
                WorldEditor.SelectedMaps[0].MapLocation.Y = Int32.Parse(this.tbMapYPos.Text);
            }
            else
            {
                MapPoint loc = new MapPoint(9999, 9999);

                foreach (var mh in WorldEditor.SelectedMaps)
                {
                    if (mh.MapLocation.Y < loc.Y)
                        loc.Y = mh.MapLocation.Y;
                }

                foreach (var mh in WorldEditor.SelectedMaps)
                {
                    mh.MapLocation.Y += Int32.Parse(this.tbMapYPos.Text) - loc.Y;
                }
            }
        }

        private void OnMapItemChecked(object sender, ItemCheckEventArgs e)
        {
            if (ignoreCheckEvent)
                return;

            foreach (var map in TileEngine.WorldManager.CurrentWorld.MapList)
            {
                if (listMapList.Items[e.Index].ToString() == map.Value.MapName)
                {
                    if (e.CurrentValue == CheckState.Unchecked)
                    {
                        WorldEditor.SelectedMaps.Add(map.Value);
                        UpdateSelectedMap();
                        break;
                    }
                    else
                    {
                        WorldEditor.SelectedMaps.Remove(map.Value);
                        UpdateSelectedMap();
                        break;
                    }
                }
            }
        }

        private void OnDeleteMapClicked(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are you sure you want to remove the selected maps?", "WorldEditor", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            
            if (res == DialogResult.Yes)
            {
                foreach (var map in WorldEditor.SelectedMaps)
                {
                    TileEngine.WorldManager.CurrentWorld.MapList.Remove(map.MapName);
                }

                WorldEditor.SelectedMaps.Clear();
                RefreshWorldProp(WorldEditor.CurWorld);
                UpdateSelectedMap();
                WorldEditor.CurWorld.CalcWorldSize();
            }
        }

        private void OnDeleteWorldClicked(object sender, EventArgs e)
        {
            if (WorldEditor.CurWorld.Name == "Main")
            {
                MessageBox.Show("Cant delete default world!", "WorldEditor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult res = MessageBox.Show("Are you sure you want to remove the current world?", "WorldEditor", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (res == DialogResult.Yes)
            {
                TileEngine.WorldManager.WorldsList.Remove(WorldEditor.CurWorld.Name);
                WorldEditor.SetCurWorld("Main");
                RefreshWorldList(TileEngine.WorldManager);
            }
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
