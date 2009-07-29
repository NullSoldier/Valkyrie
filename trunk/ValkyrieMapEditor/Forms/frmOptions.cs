using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ValkyrieMapEditor.Properties;

namespace ValkyrieMapEditor.Forms
{
	public partial class frmOptions : Form
	{
		public frmOptions()
		{
			InitializeComponent();

			this.Icon = Icon.FromHandle(Resources.imgCog.GetHicon());
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
