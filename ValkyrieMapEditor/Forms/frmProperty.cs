using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using valkyrie.Core;
using Microsoft.Xna.Framework;
using System.IO;

namespace ValkyrieMapEditor
{
    public partial class frmProperty : Form
    {
        private Map Map;
        private bool NewMap;

        public frmProperty(Map map, bool newMap)
        {
            InitializeComponent();
            this.Map = map;
            this.NewMap = newMap;

            this.LoadPropertes(newMap);            
        }

        public void LoadPropertes(bool newMap)
        {
            if (!newMap)
            {
                this.inName.Text = this.Map.Name;
                this.inTileSet.Text = this.Map.TextureName;

                this.inMapWidth.Value = this.Map.MapSize.X;
                this.inMapHeight.Value = this.Map.MapSize.Y;

                this.inTileWidth.Value = this.Map.TileSize.X;
                this.inTileHeight.Value = this.Map.TileSize.Y;
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
			// Copy it over
			FileInfo TileSet = new FileInfo(this.inTileSet.Text);
			FileInfo tmp = new FileInfo(Path.Combine(Environment.CurrentDirectory, TileEngine.Configuration["GraphicsRoot"]));

			if (!TileSet.Exists)
			{
				MessageBox.Show("Tileset graphic does not exist in the local graphic folder" + Environment.NewLine + Path.Combine(Environment.CurrentDirectory, "Graphics/"), "Error", MessageBoxButtons.OK);
				return;
			}

			if (TileSet.FullName != (tmp.FullName + TileSet.Name))
			{
				var result = MessageBox.Show("Would you like to copy this tile set to the local directory this will override any previous tilesheets with that name?", "Copy Tileset", MessageBoxButtons.YesNo);
				if (result == DialogResult.Yes)
					TileSet.CopyTo(Path.Combine(Path.Combine(Environment.CurrentDirectory, TileEngine.Configuration["GraphicsRoot"]), TileSet.Name), true);
			}

			this.Map.TextureName = TileSet.Name;

			// Other properties
            this.Map.Name = this.inName.Text;
            this.Map.MapSize = new Point((int)this.inMapWidth.Value, (int)this.inMapHeight.Value);
            this.Map.TileSize = new Point((int)this.inTileWidth.Value, (int)this.inTileHeight.Value);
			this.Map.TilesPerRow = (System.Drawing.Image.FromFile(TileEngine.Configuration["GraphicsRoot"] + "\\" + this.Map.TextureName).Width / this.Map.TileSize.X);

			this.DialogResult = DialogResult.OK;
            this.Close();
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
