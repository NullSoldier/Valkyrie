using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Valkyrie.Library.Core;
using Microsoft.Xna.Framework;
using System.IO;
using System.Drawing;
using Valkyrie.Library;
using Valkyrie.Engine.Maps;
using Valkyrie.Engine;
using Valkyrie.Engine.Core;

namespace ValkyrieMapEditor
{
	public partial class frmProperty : Form
	{
		#region Constructors

		public frmProperty(Map map)
		{
			InitializeComponent();

			this.Map = map;
			this.LastMapSize = new MapPoint(map.MapSize.X, map.MapSize.Y);
			this.IsNewMap = false;

			this.LoadPropertes(this.IsNewMap);
		}

		public frmProperty()
		{
			this.InitializeComponent();

			this.Map = new Map();
			this.IsNewMap = true;

			this.LoadPropertes(this.IsNewMap);
		}

		#endregion

		public Map Map
		{
			get { return this.map; }
			set { this.map = value; }
		}

		public MapPoint LastMapSize
		{
			get { return this.lastmapsize; }
			set { this.lastmapsize = value; }
		}

		public MapPoint NewMapSize
		{
			get { return this.newmapsize; }
			set { this.newmapsize = value; }
		}

		private Map map;
		private MapPoint lastmapsize = MapPoint.Zero;
		private MapPoint newmapsize = MapPoint.Zero;
		private bool IsNewMap = true;

		private void LoadPropertes(bool newMap)
		{
			if (!newMap)
			{
				this.inName.Text = this.map.Name;

				this.inTileSet.Text = Path.Combine(Environment.CurrentDirectory, Path.Combine(MapEditorManager.GameInstance.Engine.Configuration[EngineConfigurationName.GraphicsRoot], map.TextureName));

				this.inMapWidth.Value = this.map.MapSize.IntX;
				this.inMapHeight.Value = this.map.MapSize.IntY;

				this.inTileSize.Value = this.map.TileSize;
			}
			else
			{
				this.inName.Text = "New Map";
				this.inMapWidth.Value = 20;
				this.inMapHeight.Value = 20;

				this.inTileSize.Value = 32;
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (!ValidateForm())
				return;

			// Copy it over
			FileInfo TileSet = new FileInfo(this.inTileSet.Text);
			string graphicsdir = Path.Combine(Environment.CurrentDirectory, MapEditorManager.GameInstance.Engine.Configuration[EngineConfigurationName.GraphicsRoot]);
			FileInfo tmp = new FileInfo(graphicsdir);

			if (TileSet.FullName != (tmp.FullName + TileSet.Name))
			{
				try
				{
					var result = MessageBox.Show("Would you like to copy this tile set to the local directory this will override any previous tilesheets with that name?", "Copy Tileset", MessageBoxButtons.YesNo);
					if (result == DialogResult.Yes)
						TileSet.CopyTo(Path.Combine(graphicsdir, TileSet.Name), true);
				}
				catch (IOException)
				{
					MessageBox.Show(String.Format("Could not copy the image {0} to the target directory.", TileSet.Name), "Error", MessageBoxButtons.OK);
				}
			}

			this.map.Name = this.inName.Text;
			this.map.TextureName = TileSet.Name;
			this.map.TileSize = (int)this.inTileSize.Value;
			//if(this.IsNewMap)
			//	this.Map.MapSize = new MapPoint((int)this.inMapWidth.Value, (int)this.inMapHeight.Value);
			//else
			this.NewMapSize = new MapPoint((int)this.inMapWidth.Value, (int)this.inMapHeight.Value);

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private bool ValidateForm()
		{
			bool Error = false;

			this.inTileSet.BackColor = Color.FromKnownColor(KnownColor.Control);
			this.inName.BackColor = Color.FromKnownColor(KnownColor.Control);
			this.inMapWidth.BackColor = Color.FromKnownColor(KnownColor.Control);
			this.inMapHeight.BackColor = Color.FromKnownColor(KnownColor.Control);
			this.inTileSize.BackColor = Color.FromKnownColor(KnownColor.Control);

			/* TileSet */
			if (string.IsNullOrEmpty(this.inTileSet.Text))
			{
				this.inTileSet.BackColor = Color.Red;
				new ToolTip().Show("Value cannot be null", this,
					this.inTileSet.Location.X + this.inTileSet.Width,
					this.inTileSet.Location.Y + this.inTileSet.Height, 3000);
				Error = true;
			}
			else
			{
				FileInfo TileSet = new FileInfo(this.inTileSet.Text);

				if (!TileSet.Exists)
				{
					this.inTileSet.BackColor = Color.Red;
					new ToolTip().Show("Tileset does not exist.", this,
					this.inTileSet.Location.X + this.inTileSet.Width,
					this.inTileSet.Location.Y + this.inTileSet.Height, 3000);
					Error = true;
				}
			}

			/* Map Properties */
			if (string.IsNullOrEmpty(this.inName.Text))
			{
				this.inName.BackColor = Color.Red;

				Error = true;
			}

			if (string.IsNullOrEmpty(this.inMapWidth.Text))
			{
				this.inMapWidth.BackColor = Color.Red;
				Error = true;
			}

			if (string.IsNullOrEmpty(this.inMapHeight.Text))
			{
				this.inMapHeight.BackColor = Color.Red;
				Error = true;
			}

			if (string.IsNullOrEmpty(this.inTileSize.Text))
			{
				this.inTileSize.BackColor = Color.Red;
				Error = true;
			}

			return !(Error);
		}

		private void btnBrowseTileSet_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "Compatable Valkyrie Format (*.png)|*.png";

			DialogResult result = dialog.ShowDialog(this);

			if (result == DialogResult.Cancel || result == DialogResult.None)
				return;

			var fileInfo = new FileInfo(dialog.FileName);

			this.inTileSet.Text = fileInfo.FullName;
		}

	}
}
