using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ValkyrieLibrary.Animation;

namespace ValkyrieMapEditor.Forms
{
	public partial class frmAnimatedTile : Form
	{
		public FrameAnimation tile;

		public frmAnimatedTile()
			: this(new FrameAnimation(0, 0, 0, 0, 0)) { }

		public frmAnimatedTile(FrameAnimation tile)
		{
			InitializeComponent();

			this.tile = tile;
			this.inTilePane.TileSelectionChanged += this.OnSelectionChanged;
		}

		private void frmAnimatedTile_Load(object sender, EventArgs e)
		{

		}

		private void OnSelectionChanged(object sender, TileSelectionChangedEventArgs ev)
		{
			
		}
	}
}
