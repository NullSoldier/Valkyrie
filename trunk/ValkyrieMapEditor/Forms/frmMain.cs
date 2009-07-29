using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ValkyrieLibrary.Core;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using ValkyrieMapEditor.Properties;
using System.Threading;
using ValkyrieLibrary.Maps;
using ValkyrieLibrary;
using System.Reflection;
using ValkyrieLibrary.Events;
using ValkyrieMapEditor.Forms;

namespace ValkyrieMapEditor
{
	public partial class frmMain : Form
	{
		public static Image TileSheetImage;
		public bool MapChanged = false;
		public event EventHandler<ScreenResizedEventArgs> ScreenResized;
		public event EventHandler<ScrollEventArgs> ScrolledMap;

		public static string ValkyrieGameInstallationAssemblyPath = @"C:\Users\NullSoldier\Documents\Code Work Area\Project Valkyrie\valkyrie\bin\x86\Debug\valkyrie.exe";
		public static List<Type> EventHandlerTypes;

		public frmMain()
		{
			InitializeComponent();

			this.Icon = Icon.FromHandle(Resources.imgLayers.GetHicon());

			this.pctTileSurface.Initialize();
			this.pctTileSurface.TileSelectionChanged += this.SelectionChanged;

			//this.CollectEventHandlers(Assembly.LoadFile(this.ValkyrieGameInstallationAssemblyPath));
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

		public void CollectEventHandlers(Assembly assembly)
		{
			var Types = assembly.GetTypes().Where(p => p.IsSubclassOf(typeof(BaseMapEvent)));

			List<Type> ehandlers = new List<Type>();

			foreach (var handler in Types)
				ehandlers.Add(handler);

			frmMain.EventHandlerTypes = ehandlers;
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
			TileEngine.EventManager.ClearEventCache();

			var map = MapEditorManager.LoadMap(MapLocation);
			MapEditorManager.SetWorldMap(map);

			this.Text = String.Format("{0} - {1}",
				MapLocation.Name.Substring(0, MapLocation.Name.Length - MapLocation.Extension.Length).Replace('_', ' '),
				"Valkyrie Map Editor");

			this.RefreshMapProperties(map);

			this.UpdateScrollBars();
        }

        private void RefreshMapProperties(Map map)
        {
            this.lstSettings.Items.Clear();

            this.lstSettings.Items.Add(new ListViewItem(new string[] { "Name", map.Name }));
            this.lstSettings.Items.Add(new ListViewItem(new string[] { "Tile Size", map.TileSize.ToString() }));
            this.lstSettings.Items.Add(new ListViewItem(new string[] { "Tile Set", map.TextureName }));
            this.lstSettings.Items.Add(new ListViewItem(new string[] { "Tiles Per Row", map.TilesPerRow.ToString() }));
            this.lstSettings.Items.Add(new ListViewItem(new string[] { "Map Size", map.MapSize.ToString() }));

			this.pctTileSurface.Image = frmMain.TileSheetImage = Image.FromFile(TileEngine.Configuration[TileEngineConfigurationName.GraphicsRoot] + "\\" + map.TextureName);
			this.pctTileSurface.Size = this.pctTileSurface.Image.Size;
			this.pctTileSurface.TileSize = new Point(map.TileSize.X, map.TileSize.Y);
			this.pctTileSurface.Invalidate();

			this.UpdateScrollBars(true);

			this.btnAnimatedTileManager.Enabled = true;
			this.btnMapProperties.Enabled = true;
			this.toolSave.Enabled = true;
			this.toolSaveAs.Enabled = true;
			this.btnUnderLayer.Enabled = true;
			this.btnBaseLayer.Enabled = true;
			this.btnMiddleLayer.Enabled = true;
			this.btnTopLayer.Enabled = true;
			this.btnCollisionLayer.Enabled = true;
			this.btnSave.Enabled = true;
			this.btnPencil.Enabled = true;
			this.btnRect.Enabled = true;
			this.btnFill.Enabled = true;
            this.btnEvent.Enabled = true;
        }

		private void pctSurface_Resize(object sender, EventArgs e)
		{
			this.ScreenResized(this, new ScreenResizedEventArgs(this.pctSurface.Size.Width, this.pctSurface.Size.Height));
			this.UpdateScrollBars();
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
			 * bar the values for it are crazy!
			*/
			if (Reset && this.HorizontalScroll.Visible)
				this.HorizontalScroll.Value = 0;

			if (Reset && this.VerticalScroll.Visible)
				this.VerticalScroll.Value = 0;
		}

        private void btnMapProperties_Click(object sender, EventArgs e)
        {
            if (TileEngine.IsMapLoaded)
            {
				frmProperty dialog = new frmProperty(TileEngine.CurrentMapChunk, false);
                dialog.ShowDialog(this);

                this.RefreshMapProperties(TileEngine.CurrentMapChunk);
                
                MapEditorManager.SetWorldMap(MapEditorManager.ApplySettings(TileEngine.CurrentMapChunk));

				this.UpdateScrollBars();
            }
        }

		private void btnUnderLayer_Click(object sender, EventArgs e)
		{
			this.btnUnderLayer.Checked = true;

			this.btnBaseLayer.Checked = false;
			this.btnMiddleLayer.Checked = false;
			this.btnTopLayer.Checked = false;
			this.btnEvent.Checked = false;
			this.btnCollisionLayer.Checked = false;

			MapEditorManager.CurrentLayer = MapLayers.UnderLayer;
			MapEditorManager.GameInstance.SwitchTo(ComponentID.Draw);
		}

        private void btnBaseLayer_Click(object sender, EventArgs e)
        {
			this.btnBaseLayer.Checked = true;

			this.btnUnderLayer.Checked = false;
            this.btnMiddleLayer.Checked = false;
            this.btnTopLayer.Checked = false;
            this.btnEvent.Checked = false;
            this.btnCollisionLayer.Checked = false;

            MapEditorManager.CurrentLayer = MapLayers.BaseLayer;
            MapEditorManager.GameInstance.SwitchTo(ComponentID.Draw);
        }

        private void btnMiddleLayer_Click(object sender, EventArgs e)
        {
			this.btnMiddleLayer.Checked = true;

			this.btnUnderLayer.Checked = false;
            this.btnBaseLayer.Checked = false;
            this.btnTopLayer.Checked = false;
            this.btnEvent.Checked = false;
            this.btnCollisionLayer.Checked = false;

            MapEditorManager.CurrentLayer = MapLayers.MiddleLayer;
            MapEditorManager.GameInstance.SwitchTo(ComponentID.Draw);
        }

        private void btnTopLayer_Click(object sender, EventArgs e)
        {
			this.btnTopLayer.Checked = true;

			this.btnUnderLayer.Checked = false;
            this.btnBaseLayer.Checked = false;
            this.btnMiddleLayer.Checked = false;
            this.btnEvent.Checked = false;
            this.btnCollisionLayer.Checked = false;

            MapEditorManager.CurrentLayer = MapLayers.TopLayer;
            MapEditorManager.GameInstance.SwitchTo(ComponentID.Draw);
        }

        private void toolNew_Click(object sender, EventArgs e)
        {
            Map newMap = new Map();

            frmProperty dialog = new frmProperty(newMap, true);
            DialogResult result = dialog.ShowDialog(this);
			if (result == DialogResult.Cancel)
				return;

			newMap = MapEditorManager.ApplySettings(dialog.Map);

			this.RefreshMapProperties(newMap);

			MapEditorManager.SetWorldMap(newMap);		
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

			if (MapEditorManager.CurrentMapLocation == null || !MapEditorManager.CurrentMapLocation.Exists)
				this.SaveMapAs();
			else
				MapEditorManager.SaveMap(TileEngine.CurrentMapChunk, MapEditorManager.CurrentMapLocation);
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

				MapEditorManager.SaveMap(TileEngine.CurrentMapChunk, file);
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

		public void SelectionChanged(object sender, TileSelectionChangedEventArgs ev)
		{
			MapEditorManager.SelectedTilesRect = new Microsoft.Xna.Framework.Rectangle(
				ev.Selection.X,
				ev.Selection.Y,
				ev.Selection.Width,
				ev.Selection.Height);
		}

		private void pctTileSurface_MouseClick(object sender, MouseEventArgs e)
		{

		}

		private void frmMain_Activated(object sender, EventArgs e)
		{
			MapEditorManager.IgnoreInput = false;
		}

		private void frmMain_Deactivate(object sender, EventArgs e)
		{
			MapEditorManager.IgnoreInput = true;
		}

		private void TileMap_Scroll(object sender, ScrollEventArgs e)
		{
			var handler = this.ScrolledMap;
			
			if( handler != null)
				this.ScrolledMap(sender, e);
		}

        private void btnEvent_Click(object sender, EventArgs e)
        {
			this.btnEvent.Checked = true;

            this.btnBaseLayer.Checked = false;
            this.btnMiddleLayer.Checked = false;
            this.btnTopLayer.Checked = false;
            this.btnCollisionLayer.Checked = false;

            if (this.btnEvent.Checked)
                MapEditorManager.GameInstance.SwitchTo(ComponentID.Events);
            else
                MapEditorManager.GameInstance.SwitchTo(ComponentID.Draw);
        }

        private void btnCollisionLayer_Click(object sender, EventArgs e)
        {
			this.btnCollisionLayer.Checked = true;

            this.btnBaseLayer.Checked = false;
            this.btnMiddleLayer.Checked = false;
            this.btnTopLayer.Checked = false;
            this.btnEvent.Checked = false;

            if (this.btnCollisionLayer.Checked)
                MapEditorManager.GameInstance.SwitchTo(ComponentID.Collsion);
            else
                MapEditorManager.GameInstance.SwitchTo(ComponentID.Draw);
        }

        private void allLayersToolStripMenuItem_Click(object sender, EventArgs e)
        {
			this.allLayersToolStripMenuItem.Checked = true;

            currentLayerAndBelowToolStripMenuItem.Checked = false;
            dimOtherLayersToolStripMenuItem.Checked = false;
            MapEditorManager.ViewMode = ViewMode.All;
        }

        private void currentLayerAndBelowToolStripMenuItem_Click(object sender, EventArgs e)
        {
			this.currentLayerAndBelowToolStripMenuItem.Checked = true;

            allLayersToolStripMenuItem.Checked = false;
            dimOtherLayersToolStripMenuItem.Checked = false;

            MapEditorManager.ViewMode = ViewMode.Below;
        }

        private void dimOtherLayersToolStripMenuItem_Click(object sender, EventArgs e)
        {
			this.dimOtherLayersToolStripMenuItem.Checked = true;

            allLayersToolStripMenuItem.Checked = false;
            currentLayerAndBelowToolStripMenuItem.Checked = false;
            MapEditorManager.ViewMode = ViewMode.Dim;
        }

		private void btnViewSelected_Click(object sender, EventArgs e)
		{
			this.btnViewSelected.Checked = !this.btnViewSelected.Checked;
			this.pctTileSurface.DisplayTileSelection = this.btnViewSelected.Checked;

			this.pctTileSurface.Invalidate();
		}

		private void btnAnimatedTileManager_Click(object sender, EventArgs e)
		{
			frmAnimatedTileManager dialog = new frmAnimatedTileManager(TileEngine.CurrentMapChunk);
			dialog.ShowDialog(this);
		}

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			frmOptions dialog = new frmOptions();
			dialog.ShowDialog(this);
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
