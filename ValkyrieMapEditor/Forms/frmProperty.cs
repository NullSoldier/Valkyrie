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

                this.inTilesPerRow.Value = this.Map.TilesPerRow;
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
            this.Map.Name = this.inName.Text;
            this.Map.TextureName = this.inTileSet.Text;
            this.Map.MapSize = new Point((int)this.inMapWidth.Value, (int)this.inMapHeight.Value);
            this.Map.TileSize = new Point((int)this.inTileWidth.Value, (int)this.inTileHeight.Value);
            this.Map.TilesPerRow = (int)this.inTilesPerRow.Value;

			this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnBrowseTileSet_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog(this);

            var fileInfo = new FileInfo(dialog.FileName);

            this.inTileSet.Text = fileInfo.Name;
        }

    }
}
