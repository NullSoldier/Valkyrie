using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ValkyrieLibrary.Core;
using Microsoft.Xna.Framework;
using System.IO;
using System.Drawing;
using ValkyrieLibrary.Maps;
using ValkyrieLibrary;

namespace ValkyrieMapEditor
{
    public partial class frmProperty : Form
    {
		public Map Map
		{
			get { return this.map; }
			set { this.map = value; } 
		}

        private Map map;
        private bool IsNewMap;

        public frmProperty(Map map, bool newMap)
        {
            InitializeComponent();
            this.Map = map;
            this.IsNewMap = newMap;

			this.LoadPropertes(newMap);            
        }

        public void LoadPropertes(bool newMap)
        {
            if (!newMap)
            {
                this.inName.Text = this.map.Name;

				this.inTileSet.Text = MapEditorManager.CurrentTileSetLocation.FullName;

                this.inMapWidth.Value = this.map.MapSize.X;
                this.inMapHeight.Value = this.map.MapSize.Y;

                this.inTileWidth.Value = this.map.TileSize.X;
                this.inTileHeight.Value = this.map.TileSize.Y;
            }
            else
            {
                this.inName.Text = "New Map";
                this.inMapWidth.Value = 20;
                this.inMapHeight.Value = 20;

                this.inTileWidth.Value = 32;
                this.inTileHeight.Value = 32;
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
			FileInfo tmp = new FileInfo(Path.Combine(Environment.CurrentDirectory, TileEngine.Configuration[TileEngineConfigurationName.GraphicsRoot]));			

			if (TileSet.FullName != (tmp.FullName + TileSet.Name))
			{
				try
				{
					var result = MessageBox.Show("Would you like to copy this tile set to the local directory this will override any previous tilesheets with that name?", "Copy Tileset", MessageBoxButtons.YesNo);
					if (result == DialogResult.Yes)
						TileSet.CopyTo(Path.Combine(Path.Combine(Environment.CurrentDirectory, TileEngine.Configuration[TileEngineConfigurationName.GraphicsRoot]), TileSet.Name), true);
				}
				catch (IOException)
				{
					MessageBox.Show(String.Format("Could not copy the image {0} to the target directory.", TileSet.Name), "Error", MessageBoxButtons.OK);
				}
			}

			this.map.TextureName = TileSet.Name;

			// Other properties
            this.map.Name = this.inName.Text;
            this.map.MapSize = new MapPoint((int)this.inMapWidth.Value, (int)this.inMapHeight.Value);
            this.map.TileSize = new ScreenPoint((int)this.inTileWidth.Value, (int)this.inTileHeight.Value);

			this.DialogResult = DialogResult.OK;
            this.Close();
        }

		public bool ValidateForm()
		{
			bool Error = false;

			this.inTileSet.BackColor = Color.FromKnownColor(KnownColor.Control);
			this.inName.BackColor = Color.FromKnownColor(KnownColor.Control);
			this.inMapWidth.BackColor = Color.FromKnownColor(KnownColor.Control);
			this.inMapHeight.BackColor = Color.FromKnownColor(KnownColor.Control);
			this.inTileWidth.BackColor = Color.FromKnownColor(KnownColor.Control);
			this.inTileHeight.BackColor = Color.FromKnownColor(KnownColor.Control);

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

			if (string.IsNullOrEmpty(this.inTileWidth.Text))
			{
				this.inTileWidth.BackColor = Color.Red;
				Error = true;
			}

			if (string.IsNullOrEmpty(this.inTileHeight.Text))
			{
				this.inTileHeight.BackColor = Color.Red;
				Error = true;
			}

			return !(Error);
		}

        private void btnBrowseTileSet_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog(this);
			
			if (result == DialogResult.Cancel || result == DialogResult.None)
				return;

            var fileInfo = new FileInfo(dialog.FileName);

            this.inTileSet.Text = fileInfo.FullName;
        }

    }
}
