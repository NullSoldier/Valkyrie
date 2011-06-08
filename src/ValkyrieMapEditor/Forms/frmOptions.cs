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
		#region Constructor

		public frmOptions()
		{
			InitializeComponent();

			this.Icon = Icon.FromHandle(Resources.imgCog.GetHicon());
		}

		#endregion

		public bool Changed
		{
			get { return this.changed; }
			set
			{
				if(value)
					this.Text = "Options *";
				else
					this.Text = "Options";

				this.btnSave.Enabled = value;
				this.changed = value;
			}
		}

		private bool changed = false;
		private bool ignorechanged = true;

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
			this.listProfiles.Items.Clear();

			int currentprofileid = Program.Settings.CurrentProfileID;

			foreach(var profile in Program.Settings.GetProfiles())
			{
				ListViewItem item = new ListViewItem(profile.Value);
				item.Tag = profile.Key;

				if(currentprofileid == profile.Key)
				{
					item.Selected = true;
					this.DisplayProfile(profile.Key, profile.Value);
				}

				listProfiles.Items.Add(item);
			}
		}

		private void lnkAddProfile_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.RemindSave();

			this.Cursor = Cursors.WaitCursor;

			Program.Settings.AddProfile("New Profile");
			this.DisplayProfiles();

			this.Cursor = Cursors.Default;
		}

		private void listProfiles_SelectedIndexChanged (object sender, EventArgs e)
		{
			//this.RemindSave();

			if(this.listProfiles.SelectedItems.Count <= 0 ||
				this.listProfiles.SelectedItems.Count >= 2)
			{
				this.ignorechanged = true;
				this.inName.Text = string.Empty;
				this.ignorechanged = false;
				this.grpProperties.Enabled = false;

				this.listAssemblies.Items.Clear();
				return;
			}

			this.grpProperties.Enabled = true;
			var item = this.listProfiles.SelectedItems[0];
			this.DisplayProfile((int)item.Tag, item.Text);
		}

		private void RemindSave ()
		{
			if(!this.Changed)
				return;

			var result = MessageBox.Show("Save unsaved changes to the current profile?", "Save", MessageBoxButtons.YesNo);
			if(result == DialogResult.Yes)
				this.SaveCurrentProfile();
		}

		private void DisplayProfile (int profileid, string name)
		{
			this.listAssemblies.Items.Clear();

			Program.Settings.SetCurrentProfile(profileid);

			this.ignorechanged = true;
			this.inName.Text = name;
			this.ignorechanged = false;

			var valuenullable = Program.Settings.GetSetting("AssemblyDirectories");
			if(valuenullable != null)
			{
				var values = valuenullable.Split(';');

				for(int i = 0; i < values.Length; i++)
				{
					if(!string.IsNullOrEmpty(values[i]))
						this.listAssemblies.Items.Add(values[i]);
				}
			}
		}

		private void lnkAddAssembly_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			frmAddAssembly dialog = new frmAddAssembly();
			var result = dialog.ShowDialog(this);
			if(result == DialogResult.OK)
			{
				this.listAssemblies.Items.Add(dialog.Assembly);

				this.Changed = true;
			}
		}

		private void btnSave_Click (object sender, EventArgs e)
		{
			this.SaveCurrentProfile();
		}

		private void SaveCurrentProfile ()
		{
			if(this.listProfiles.SelectedItems[0].ToString() != this.inName.Text)
			{
				Program.Settings.RenameCurrentProfile(this.inName.Text);

				this.listProfiles.SelectedItems[0].Text = this.inName.Text;
			}

			StringBuilder assemblies = new StringBuilder();

			for(int i = 0; i < this.listAssemblies.Items.Count; i++)
			{
				assemblies.Append(this.listAssemblies.Items[i].ToString());

				if((i + 1) < this.listAssemblies.Items.Count)
					assemblies.Append(';');
			}

			Program.Settings.SetSetting("AssemblyDirectories", assemblies.ToString());

			this.Changed = false;
		}

		private void listAssemblies_PreviewKeyDown (object sender, PreviewKeyDownEventArgs e)
		{
			if(e.KeyCode == Keys.Delete && this.listAssemblies.SelectedItems.Count > 0)
			{
				var result = MessageBox.Show(string.Format("Are you sure you want to delete the {0} selected assemblies?", this.listAssemblies.SelectedItems.Count), "Delete", MessageBoxButtons.YesNo);
				if(result == DialogResult.Yes)
				{
					while(listAssemblies.SelectedItems.Count > 0)
					{
						this.listAssemblies.Items.Remove(listAssemblies.SelectedItems[0]);
					}

					this.Changed = true;
				}
			}
		}

		private void inName_TextChanged (object sender, EventArgs e)
		{
			if(!ignorechanged)
				this.Changed = true;
		}

		private void listProfiles_PreviewKeyDown (object sender, PreviewKeyDownEventArgs e)
		{
			if(e.KeyCode == Keys.Delete && this.listProfiles.SelectedItems.Count > 0)
			{
				if(this.listProfiles.Items.Count - this.listProfiles.SelectedItems.Count <= 0)
				{
					MessageBox.Show("You cannot delete all profiles.", "Delete");
					return;
				}

				var result = MessageBox.Show(string.Format("Are you sure you want to delete the {0} selected profiles?", this.listProfiles.SelectedItems.Count), "Delete", MessageBoxButtons.YesNo);
				if(result == DialogResult.Yes)
				{
					this.Cursor = Cursors.WaitCursor;

					while(listProfiles.SelectedItems.Count > 0)
					{
						Program.Settings.RemoveProfile(Convert.ToInt32(listProfiles.SelectedItems[0].Tag));

						this.listProfiles.Items.Remove(listProfiles.SelectedItems[0]);
					}

					this.Cursor = Cursors.Default;
				}
			}
		}

		private void frmOptions_FormClosing (object sender, FormClosingEventArgs e)
		{
			this.RemindSave();

			if(this.listProfiles.SelectedItems.Count <= 0 )
				Program.Settings.SetCurrentProfile(Convert.ToInt32(this.listProfiles.Items[0].Tag));
		}
	}
}
