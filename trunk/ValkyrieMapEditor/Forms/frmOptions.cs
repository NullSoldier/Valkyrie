using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ValkyrieMapEditor.Properties;
using System.IO;

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

		private void frmOptions_Load (object sender, EventArgs e)
		{
			this.DisplayProfiles();
		}

		private void DisplayProfiles ()
		{
			int currentprofileid = Program.Settings.CurrentProfileID;

			foreach(var profile in Program.Settings.GetProfiles())
			{
				ListViewItem item = new ListViewItem(profile.Value);

				if(currentprofileid == profile.Key)
				{
					item.Selected = true;
					this.DisplayProfile(profile.Value);
				}

				listProfiles.Items.Add(item);
			}

		}

		private void lnkAddProfile_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			Program.Settings.AddProfile("New Profile");
			this.DisplayProfiles();
		}

		private void listProfiles_SelectedIndexChanged (object sender, EventArgs e)
		{
			if(this.listProfiles.SelectedItems.Count <= 0)
			{
				this.inName.Text = string.Empty;
				this.listAssemblies.Items.Clear();
			}

			Program.Settings.SetCurrentProfile(this.listProfiles.SelectedItems[0].Text);
			this.DisplayProfile(this.listProfiles.SelectedItems[0].Text);
		}

		private void DisplayProfile (string name)
		{
			this.listAssemblies.Items.Clear();

			Program.Settings.SetCurrentProfile(name);

			this.inName.Text = name;
			
			var values = Program.Settings.GetSetting("AssemblyDirectories").Split(';');

			for(int i = 0; i < values.Length; i++)
			{
				if(!string.IsNullOrEmpty(values[i]))
					this.listAssemblies.Items.Add(values[i]);
			}
		}

		private void btnBrowse_Click (object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "Valkyrie .NET Assembly|*.dll;*.exe";
			var result = dialog.ShowDialog(this);

			if(result == DialogResult.OK)
				this.inAssemblyPath.Text = dialog.FileName;
		}

		private void lnkAddAssembly_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			FileInfo info = new FileInfo(this.inAssemblyPath.Text);
			if(info.Exists)
			{
				this.listAssemblies.Items.Add(info.FullName);
				this.inAssemblyPath.BackColor = Color.FromKnownColor(KnownColor.Control);
			}
			else
			{
				this.inAssemblyPath.BackColor = Color.Red;
			}

		}
	}
}
