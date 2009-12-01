using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ValkyrieMapEditor.Forms
{
	public partial class frmAddAssembly : Form
	{
		public frmAddAssembly ()
			: this(string.Empty)
		{
			
		}

		public frmAddAssembly (string assembly)
		{
			InitializeComponent();

			this.assembly = assembly;
		}

		public string Assembly
		{
			get { return this.assembly; }
		}

		private string assembly = string.Empty;

		private void btnOk_Click (object sender, EventArgs e)
		{
			if(!this.IsValid())
				return;

			this.assembly = this.inAssemblyPath.Text;
			
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void btnCancel_Click (object sender, EventArgs e)
		{
			this.Close();
		}

		private void frmAddAssembly_Load(object sender, EventArgs e)
		{
			this.DisplayAssembly();
		}

		private void DisplayAssembly ()
		{
			this.inAssemblyPath.Text = this.assembly;
		}

		private bool IsValid ()
		{
			bool iserror = false;

			FileInfo info = new FileInfo(this.inAssemblyPath.Text);
			if(!info.Exists)
			{
				iserror = true;
				this.inAssemblyPath.BackColor = Color.Red;
			}

			return !iserror;
		}

		private void btnBrowse_Click (object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "Valkyrie .NET Assembly|*.dll;*.exe";
			var result = dialog.ShowDialog(this);

			if(result == DialogResult.OK)
				this.inAssemblyPath.Text = dialog.FileName;
		}
	}
}
