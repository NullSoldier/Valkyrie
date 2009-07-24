using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ValkyrieLibrary.Maps;
using ValkyrieLibrary.Animation;

namespace ValkyrieMapEditor.Forms
{
	public partial class frmAnimatedTileManager : Form
	{
		private Map map;

		public frmAnimatedTileManager(Map map)
		{
			InitializeComponent();

			this.map = map;
		}

		private void frmAnimatedTileManager_Load(object sender, EventArgs e)
		{
			foreach (FrameAnimation tile in map.AnimatedTiles.Values)
			{
				this.lstAnimatedTiles.Items.Add(new ListViewItem(new string[] {tile.InitialFrameRect.ToString(), tile.FrameCount.ToString()}));
			}
		}


	}
}
