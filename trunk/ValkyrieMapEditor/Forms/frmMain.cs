using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Valkyrie.Library.Core;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using ValkyrieMapEditor.Properties;
using System.Threading;
using Valkyrie.Library;
using System.Reflection;
using Valkyrie.Library.Events;
using ValkyrieMapEditor.Forms;
using ValkyrieMapEditor.Core;
using Valkyrie.Engine.Maps;
using Valkyrie.Library.Providers;
using Valkyrie.Engine;
using Valkyrie.Engine.Events;

namespace ValkyrieMapEditor
{
	public partial class frmMain : Form
	{
		#region Constructors

		public frmMain()
		{
			this.Icon = Icon.FromHandle(Resources.imgLayers.GetHicon());

			InitializeComponent();

			this.pctTileSurface.Initialize();
			this.pctTileSurface.TileSelectionChanged += this.SelectionChanged;
		}

		#endregion

		#region Public Properties

		public bool MapChanged
		{
			get { return this.mapchanged; }
			set
			{
				if(value)
				{
					if(!this.Text.EndsWith("*"))
					{
						this.Text += "*";
						this.btnSave.Enabled = true;
						this.toolSave.Enabled = true;
					}
				}
				else
				{
					if(this.Text.EndsWith("*"))
					{
						this.Text = this.Text.Substring(0, this.Text.Length - 1);
						this.btnSave.Enabled = false;
						this.toolSave.Enabled = false;
					}
				}

				this.mapchanged = value;
			}
		}

		public event EventHandler<ScreenResizedEventArgs> ScreenResized;
		public event EventHandler<ScrollEventArgs> ScrolledMap;

		#endregion

		#region Public Methods

		public IntPtr getDrawSurface ()
		{
			return pctSurface.Handle;
		}

		public IntPtr getDrawTilesSurface ()
		{
			return pctTileSurface.Handle;
		}

		#endregion

		private bool mapchanged = false;
		private FileInfo currentmaplocation = null;

		#region UI Events
		
		private void frmMain_Load(object sender, EventArgs e)
		{
			this.lblVersion.Text = String.Format("Version: {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
			frmMain.EventHandlerTypes = this.LoadImplementers();

			MapEditorManager.MapChanged += this.frmMain_MapChanged;

			// Do this last
			var handler = this.ScreenResized;
			if( handler != null)
				this.ScreenResized(this, new ScreenResizedEventArgs(this.pctSurface.Size.Width, this.pctSurface.Size.Height));
		}

		private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}

		private void frmMain_Activated (object sender, EventArgs e)
		{
			MapEditorManager.IgnoreInput = false;
		}

		private void frmMain_Deactivate (object sender, EventArgs e)
		{
			MapEditorManager.IgnoreInput = true;
		}

		private void toolOpen_Click (object sender, EventArgs e)
		{
			MapEditorManager.IgnoreInput = true;

			OpenFileDialog openDialog = new OpenFileDialog();
			openDialog.DefaultExt = ".xml";
			openDialog.Filter = "Valkyrie Maps|*.xml";

			var result = openDialog.ShowDialog(this);
			if(result == DialogResult.Cancel)
				return;

			this.LoadMap(new FileInfo(openDialog.FileName));

			MapEditorManager.IgnoreInput = false;
		}

		private void toolNew_Click (object sender, EventArgs e)
		{
			MapEditorManager.IgnoreInput = true;

			frmProperty dialog = new frmProperty();
			DialogResult result = dialog.ShowDialog(this);
			if(result == DialogResult.Cancel)
				return;

			Map newMap = MapEditorManager.ApplyMapProperties(dialog.Map);
			newMap.Texture = MapEditorManager.GameInstance.Engine.TextureManager.GetTexture(newMap.TextureName);
			newMap = MapEditorManager.ResizeMap(newMap, dialog.NewMapSize);

			MapEditorManager.SetCurrentMap(newMap);

			this.DisplayMapProperties(newMap);
			this.DisplayTileSheet(newMap);

			this.currentmaplocation = null;

			MapEditorManager.IgnoreInput = false;
		}

		private void toolSave_Click (object sender, EventArgs e)
		{
			this.SaveMap();
		}

		private void toolClose_Click (object sender, EventArgs e)
		{
			if(this.MapChanged)
			{
				DialogResult result = MessageBox.Show("Do you want to save changes to" + Environment.NewLine, "Save Changes", MessageBoxButtons.YesNoCancel);

				if(result == DialogResult.Yes)
					this.SaveMap();
				else if(result == DialogResult.Cancel)
					return;
			}

			this.Close();
		}

		private void btnMapProperties_Click (object sender, EventArgs e)
		{
			if(MapEditorManager.CurrentMap != null)
			{
				frmProperty dialog = new frmProperty(MapEditorManager.CurrentMap);
				dialog.ShowDialog(this);

				MapEditorManager.ApplyMapProperties(MapEditorManager.CurrentMap);
				if(dialog.NewMapSize != dialog.LastMapSize)
				{
					var tmpmap = MapEditorManager.ResizeMap(MapEditorManager.CurrentMap, dialog.NewMapSize);
					MapEditorManager.SetCurrentMap(tmpmap);
				}

				this.DisplayMapProperties(MapEditorManager.CurrentMap);

				this.UpdateScrollBars();
			}
		}

		private void btnUnderLayer_Click (object sender, EventArgs e)
		{
			this.btnUnderLayer.Checked = true;

			this.btnBaseLayer.Checked = false;
			this.btnMiddleLayer.Checked = false;
			this.btnTopLayer.Checked = false;
			this.btnCollisionLayer.Checked = false;

			MapEditorManager.CurrentLayer = MapLayers.UnderLayer;
			MapEditorManager.GameInstance.SwitchToComponent(ComponentID.Draw);
		}

		private void btnBaseLayer_Click (object sender, EventArgs e)
		{
			this.btnBaseLayer.Checked = true;

			this.btnUnderLayer.Checked = false;
			this.btnMiddleLayer.Checked = false;
			this.btnTopLayer.Checked = false;
			this.btnCollisionLayer.Checked = false;

			MapEditorManager.CurrentLayer = MapLayers.BaseLayer;
			MapEditorManager.GameInstance.SwitchToComponent(ComponentID.Draw);
		}

		private void btnMiddleLayer_Click (object sender, EventArgs e)
		{
			this.btnMiddleLayer.Checked = true;

			this.btnUnderLayer.Checked = false;
			this.btnBaseLayer.Checked = false;
			this.btnTopLayer.Checked = false;
			this.btnCollisionLayer.Checked = false;

			MapEditorManager.CurrentLayer = MapLayers.MiddleLayer;
			MapEditorManager.GameInstance.SwitchToComponent(ComponentID.Draw);
		}

		private void btnTopLayer_Click (object sender, EventArgs e)
		{
			this.btnTopLayer.Checked = true;

			this.btnUnderLayer.Checked = false;
			this.btnBaseLayer.Checked = false;
			this.btnMiddleLayer.Checked = false;
			this.btnCollisionLayer.Checked = false;

			MapEditorManager.CurrentLayer = MapLayers.TopLayer;
			MapEditorManager.GameInstance.SwitchToComponent(ComponentID.Draw);
		}

		private void saveAsToolStripMenuItem_Click (object sender, EventArgs e)
		{
			this.SaveMapAs();
		}

		private void SelectionChanged (object sender, TileSelectionChangedEventArgs ev)
		{
			MapEditorManager.SelectedTilesRectangle = new Microsoft.Xna.Framework.Rectangle(
				ev.Selection.X,
				ev.Selection.Y,
				ev.Selection.Width,
				ev.Selection.Height);
		}

		private void btnHelp_Click (object sender, EventArgs e)
		{
			foreach(ToolStripItem item in this.toolStripTools.Items)
			{
				if(item.DisplayStyle == ToolStripItemDisplayStyle.Image)
					item.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
				else
					item.DisplayStyle = ToolStripItemDisplayStyle.Image;
			}
		}

		private void pctSurface_Resize (object sender, EventArgs e)
		{
			this.ScreenResized(this, new ScreenResizedEventArgs(this.pctSurface.Size.Width, this.pctSurface.Size.Height));
			this.UpdateScrollBars();
		}

		private void TileMap_Scroll (object sender, ScrollEventArgs e)
		{
			var handler = this.ScrolledMap;

			if(handler != null)
				this.ScrolledMap(sender, e);
		}

		private void btnEvent_Click (object sender, EventArgs e)
		{
			this.btnEvent.Checked = true;

			this.btnPencil.Checked = false;
			this.btnFill.Checked = false;
			this.btnSelection.Checked = false;
			this.btnRect.Checked = false;

			MapEditorManager.GameInstance.SwitchToComponent(ComponentID.Events);
		}

		private void btnCollisionLayer_Click (object sender, EventArgs e)
		{
			this.btnCollisionLayer.Checked = true;

			this.btnBaseLayer.Checked = false;
			this.btnMiddleLayer.Checked = false;
			this.btnTopLayer.Checked = false;

			MapEditorManager.GameInstance.SwitchToComponent(ComponentID.Collsion);
		}

		private void allLayersToolStripMenuItem_Click (object sender, EventArgs e)
		{
			this.allLayersToolStripMenuItem.Checked = true;

			currentLayerAndBelowToolStripMenuItem.Checked = false;
			dimOtherLayersToolStripMenuItem.Checked = false;
			MapEditorManager.ViewMode = ViewMode.All;
		}

		private void currentLayerAndBelowToolStripMenuItem_Click (object sender, EventArgs e)
		{
			this.currentLayerAndBelowToolStripMenuItem.Checked = true;

			allLayersToolStripMenuItem.Checked = false;
			dimOtherLayersToolStripMenuItem.Checked = false;

			MapEditorManager.ViewMode = ViewMode.Below;
		}

		private void dimOtherLayersToolStripMenuItem_Click (object sender, EventArgs e)
		{
			this.dimOtherLayersToolStripMenuItem.Checked = true;

			allLayersToolStripMenuItem.Checked = false;
			currentLayerAndBelowToolStripMenuItem.Checked = false;
			MapEditorManager.ViewMode = ViewMode.Dim;
		}

		private void btnViewSelected_Click (object sender, EventArgs e)
		{
			this.btnViewSelected.Checked = !this.btnViewSelected.Checked;
			this.pctTileSurface.DisplayTileSelection = this.btnViewSelected.Checked;

			this.pctTileSurface.Invalidate();
		}

		private void btnAnimatedTileManager_Click (object sender, EventArgs e)
		{
			MapEditorManager.IgnoreInput = true;

			frmAnimatedTileManager dialog = new frmAnimatedTileManager(MapEditorManager.CurrentMap, this.pctTileSurface.Image);
			dialog.ShowDialog(this);

			MapEditorManager.IgnoreInput = false;
		}

		private void optionsToolStripMenuItem_Click (object sender, EventArgs e)
		{
			MapEditorManager.IgnoreInput = true;

			frmOptions dialog = new frmOptions();
			dialog.ShowDialog(this);

			MapEditorManager.IgnoreInput = false;
		}

		#endregion

		#region Helper Methods

		private void LoadMap(FileInfo MapLocation)
        {
			this.currentmaplocation = MapLocation;

			MapEditorManager.GameInstance.Engine.TextureManager.ClearCache();
			MapEditorManager.GameInstance.Engine.EventProvider.ClearEvents();

			Map map = MapEditorManager.LoadMap(MapLocation.FullName, new GriffinMapProvider(this.GetAssemblies()));
			MapEditorManager.SetCurrentMap(map);

			this.Text = String.Format("{0} - {1}",
				MapLocation.Name.Substring(0, MapLocation.Name.Length - MapLocation.Extension.Length).Replace('_', ' '),
				"Valkyrie Map Editor");

			this.DisplayMapProperties(map);
			this.DisplayTileSheet(map);
			this.UpdateScrollBars();

			this.splitContainer2.Panel1.VerticalScroll.SmallChange = MapEditorManager.CurrentMap.TileSize;
			this.splitContainer2.Panel1.HorizontalScroll.SmallChange = MapEditorManager.CurrentMap.TileSize;

			if(MapEditorManager.NoEvents || 
				frmMain.EventHandlerTypes.Count() <= 0 )
			{
				this.btnEvent.Enabled = false;
			}
        }

		private void DisplayTileSheet(Map map)
		{
			this.pctTileSurface.Image = Image.FromFile(Path.Combine(MapEditorManager.GameInstance.Engine.Configuration[EngineConfigurationName.GraphicsRoot], map.TextureName));
			this.pctTileSurface.Size = this.pctTileSurface.Image.Size;
			this.pctTileSurface.TileSize = new Point(map.TileSize, map.TileSize);
			this.pctTileSurface.Invalidate();
		}

        private void DisplayMapProperties(Map map)
        {
			this.lstSettings.Items.Clear();

			this.lstSettings.Items.Add(new ListViewItem(new string[] { "Name", map.Name }));
			this.lstSettings.Items.Add(new ListViewItem(new string[] { "Tile Size", map.TileSize.ToString() }));
			this.lstSettings.Items.Add(new ListViewItem(new string[] { "Tile Set", map.TextureName }));
			this.lstSettings.Items.Add(new ListViewItem(new string[] { "Tiles Per Row", map.TilesPerRow.ToString() }));
			this.lstSettings.Items.Add(new ListViewItem(new string[] { "Map Size", map.MapSize.ToString() }));

			this.UpdateScrollBars(true);

			this.EnableToolButtons(true);
        }

		private void EnableToolButtons (bool enabled)
		{
			this.btnAnimatedTileManager.Enabled = enabled;
			this.btnMapProperties.Enabled = enabled;
			this.toolSave.Enabled = enabled;
			this.toolSaveAs.Enabled = enabled;
			this.btnUnderLayer.Enabled = enabled;
			this.btnBaseLayer.Enabled = enabled;
			this.btnMiddleLayer.Enabled = enabled;
			this.btnTopLayer.Enabled = enabled;
			this.btnCollisionLayer.Enabled = enabled;
			this.btnEvent.Enabled = enabled;
			this.btnSave.Enabled = enabled;
			this.btnPencil.Enabled = enabled;
			this.btnRect.Enabled = enabled;
			this.btnFill.Enabled = enabled;
			this.btnSelection.Enabled = enabled;
		}

		private IEnumerable<Assembly> GetAssemblies ()
		{
			List<Assembly> assemblies = new List<Assembly>();

			var results = Program.Settings.GetSetting("AssemblyDirectories").Split(';');
			foreach(var result in results)
			{
                if (string.IsNullOrEmpty(result))
                    continue;

                try
                {
                    FileInfo info = new FileInfo(result);
                    if (!info.Exists)
                        continue;


                    assemblies.Add(Assembly.LoadFile(result));
                }
                catch
                {
                    continue; // Must be an invalid assembly path
                }
			}

			return assemblies;
		}

		private void UpdateScrollBars()
		{
			this.UpdateScrollBars(false);
		}

		private void UpdateScrollBars(bool Reset)
		{
			if (MapEditorManager.CurrentMap == null) return;

			this.HorizontalScroll.Visible = (MapEditorManager.CurrentMap.MapSize.X * MapEditorManager.CurrentMap.TileSize > this.pctSurface.Size.Width);
			this.VerticalScroll.Visible = (MapEditorManager.CurrentMap.MapSize.Y * MapEditorManager.CurrentMap.TileSize > this.pctSurface.Size.Height);

			int xTilesInView = ((this.pctSurface.Width + this.VerticalScroll.Width) / MapEditorManager.CurrentMap.TileSize);
			int xUnseenAmount = MapEditorManager.CurrentMap.MapSize.X - xTilesInView;
			this.HorizontalScroll.Maximum = xUnseenAmount + this.HorizontalScroll.LargeChange - 1;

			int yTilesInView = ((this.pctSurface.Height + this.HorizontalScroll.Height) / MapEditorManager.CurrentMap.TileSize);
			int yUnseenAmount = MapEditorManager.CurrentMap.MapSize.Y - yTilesInView;
			this.VerticalScroll.Maximum = yUnseenAmount + this.VerticalScroll.LargeChange - 1;

			///* Don't set to 0 when they aren't visible
			// * when there isnt enough map to show a scroll
			// * bar the values for it are crazy!
			//*/
			if (Reset && this.HorizontalScroll.Visible)
			    this.HorizontalScroll.Value = 0;

			if (Reset && this.VerticalScroll.Visible)
			    this.VerticalScroll.Value = 0;
		}

		private void SaveMap()
		{
			if(MapEditorManager.CurrentMap == null) return;

			if(this.currentmaplocation == null || !this.currentmaplocation.Exists)
			    this.SaveMapAs();
			else
				MapEditorManager.SaveMap(MapEditorManager.CurrentMap, this.currentmaplocation);

			this.MapChanged = false;
		}

		private void SaveMapAs()
		{
			if(MapEditorManager.CurrentMap == null) return;

			SaveFileDialog dialog = new SaveFileDialog();
			dialog.DefaultExt = ".xml";
			dialog.Filter = "Valkyrie Maps|*.xml";

			var result = dialog.ShowDialog();

			if(result == DialogResult.None || result == DialogResult.Cancel)
				return;

			FileInfo file = new FileInfo(dialog.FileName);

			MapEditorManager.SaveMap(MapEditorManager.CurrentMap, file);

			this.MapChanged = false;
		}

		#endregion

		private void btnRect_Click (object sender, EventArgs e)
		{
			this.btnRect.Checked = true;

			this.btnPencil.Checked = false;
			this.btnFill.Checked = false;
			this.btnSelection.Checked = false;
			this.btnEvent.Checked = false;

			MapEditorManager.CurrentTool = Tools.Rectangle;
			MapEditorManager.GameInstance.SwitchToComponent(ComponentID.Draw);
		}

		private void btnPencil_Click (object sender, EventArgs e)
		{
			this.btnPencil.Checked = true;

			this.btnFill.Checked = false;
			this.btnRect.Checked = false;
			this.btnSelection.Checked = false;
			this.btnEvent.Checked = false;

			MapEditorManager.CurrentTool = Tools.Pencil;
			MapEditorManager.GameInstance.SwitchToComponent(ComponentID.Draw);
		}

		private void btnFill_Click (object sender, EventArgs e)
		{
			this.btnFill.Checked = true;

			this.btnPencil.Checked = false;
			this.btnRect.Checked = false;
			this.btnSelection.Checked = false;
			this.btnEvent.Checked = false;

			MapEditorManager.CurrentTool = Tools.Bucket;
			MapEditorManager.GameInstance.SwitchToComponent(ComponentID.Draw);
		}

		private void btnSelection_Click (object sender, EventArgs e)
		{
			this.btnSelection.Checked = true;

			this.btnPencil.Checked = false;
			this.btnRect.Checked = false;
			this.btnFill.Checked = false;
			this.btnEvent.Checked = false;

			MapEditorManager.CurrentTool = Tools.Select;
			MapEditorManager.GameInstance.SwitchToComponent(ComponentID.Draw);
		}

		private IEnumerable<Type> LoadImplementers ()
		{
			return this.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => typeof(IMapEvent).IsAssignableFrom(t) && !t.IsInterface);
		}

		private void frmMain_MapChanged (object sender, EventArgs ev)
		{
			this.MapChanged = true;
		}

		#region Statics and Internals

		public static IEnumerable<Type> EventHandlerTypes = new List<Type>();

		#endregion

		private void frmMain_FormClosing (object sender, FormClosingEventArgs e)
		{
			if(this.MapChanged)
			{
				var result = MessageBox.Show(string.Format("Do you want to save changes to {0}?", MapEditorManager.CurrentMap.Name), "Griffin", MessageBoxButtons.YesNo);
				if(result == DialogResult.Yes)
					this.SaveMap();
			}
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
