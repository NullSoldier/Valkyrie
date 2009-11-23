namespace ValkyrieMapEditor.Forms
{
	partial class frmOptions
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.listProfiles = new System.Windows.Forms.ListView();
			this.ColProfile = new System.Windows.Forms.ColumnHeader();
			this.grpProperties = new System.Windows.Forms.GroupBox();
			this.lnkAddAssembly = new System.Windows.Forms.LinkLabel();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.inAssemblyPath = new System.Windows.Forms.TextBox();
			this.lblAssemblies = new System.Windows.Forms.Label();
			this.listAssemblies = new System.Windows.Forms.ListBox();
			this.inName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.lnkAddProfile = new System.Windows.Forms.LinkLabel();
			this.btnClose = new System.Windows.Forms.Button();
			this.grpProperties.SuspendLayout();
			this.SuspendLayout();
			// 
			// listProfiles
			// 
			this.listProfiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.listProfiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColProfile});
			this.listProfiles.FullRowSelect = true;
			this.listProfiles.HideSelection = false;
			this.listProfiles.Location = new System.Drawing.Point(12, 12);
			this.listProfiles.MultiSelect = false;
			this.listProfiles.Name = "listProfiles";
			this.listProfiles.Size = new System.Drawing.Size(175, 202);
			this.listProfiles.TabIndex = 0;
			this.listProfiles.UseCompatibleStateImageBehavior = false;
			this.listProfiles.View = System.Windows.Forms.View.Details;
			this.listProfiles.SelectedIndexChanged += new System.EventHandler(this.listProfiles_SelectedIndexChanged);
			// 
			// ColProfile
			// 
			this.ColProfile.Text = "Profile Name";
			this.ColProfile.Width = 149;
			// 
			// grpProperties
			// 
			this.grpProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpProperties.Controls.Add(this.lnkAddAssembly);
			this.grpProperties.Controls.Add(this.btnBrowse);
			this.grpProperties.Controls.Add(this.inAssemblyPath);
			this.grpProperties.Controls.Add(this.lblAssemblies);
			this.grpProperties.Controls.Add(this.listAssemblies);
			this.grpProperties.Controls.Add(this.inName);
			this.grpProperties.Controls.Add(this.lblName);
			this.grpProperties.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.grpProperties.Location = new System.Drawing.Point(193, 12);
			this.grpProperties.Name = "grpProperties";
			this.grpProperties.Size = new System.Drawing.Size(414, 202);
			this.grpProperties.TabIndex = 1;
			this.grpProperties.TabStop = false;
			this.grpProperties.Text = "Properties";
			// 
			// lnkAddAssembly
			// 
			this.lnkAddAssembly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lnkAddAssembly.AutoSize = true;
			this.lnkAddAssembly.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lnkAddAssembly.Location = new System.Drawing.Point(358, 170);
			this.lnkAddAssembly.Name = "lnkAddAssembly";
			this.lnkAddAssembly.Size = new System.Drawing.Size(34, 13);
			this.lnkAddAssembly.TabIndex = 6;
			this.lnkAddAssembly.TabStop = true;
			this.lnkAddAssembly.Text = "Add..";
			this.lnkAddAssembly.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAddAssembly_LinkClicked);
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnBrowse.Location = new System.Drawing.Point(311, 165);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(41, 23);
			this.btnBrowse.TabIndex = 5;
			this.btnBrowse.Text = "...";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// inAssemblyPath
			// 
			this.inAssemblyPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.inAssemblyPath.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.inAssemblyPath.Location = new System.Drawing.Point(101, 167);
			this.inAssemblyPath.Name = "inAssemblyPath";
			this.inAssemblyPath.Size = new System.Drawing.Size(204, 22);
			this.inAssemblyPath.TabIndex = 4;
			// 
			// lblAssemblies
			// 
			this.lblAssemblies.AutoSize = true;
			this.lblAssemblies.Location = new System.Drawing.Point(16, 75);
			this.lblAssemblies.Name = "lblAssemblies";
			this.lblAssemblies.Size = new System.Drawing.Size(73, 13);
			this.lblAssemblies.TabIndex = 3;
			this.lblAssemblies.Text = "Assemblies:";
			// 
			// listAssemblies
			// 
			this.listAssemblies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.listAssemblies.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listAssemblies.FormattingEnabled = true;
			this.listAssemblies.Location = new System.Drawing.Point(101, 75);
			this.listAssemblies.Name = "listAssemblies";
			this.listAssemblies.Size = new System.Drawing.Size(294, 82);
			this.listAssemblies.TabIndex = 2;
			// 
			// inName
			// 
			this.inName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.inName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.inName.Location = new System.Drawing.Point(101, 34);
			this.inName.Name = "inName";
			this.inName.Size = new System.Drawing.Size(294, 22);
			this.inName.TabIndex = 1;
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(16, 37);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(43, 13);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "Name:";
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(451, 226);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			// 
			// lnkAddProfile
			// 
			this.lnkAddProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lnkAddProfile.AutoSize = true;
			this.lnkAddProfile.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lnkAddProfile.Location = new System.Drawing.Point(12, 226);
			this.lnkAddProfile.Name = "lnkAddProfile";
			this.lnkAddProfile.Size = new System.Drawing.Size(70, 13);
			this.lnkAddProfile.TabIndex = 3;
			this.lnkAddProfile.TabStop = true;
			this.lnkAddProfile.Text = "Add Profile..";
			this.lnkAddProfile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAddProfile_LinkClicked);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.Location = new System.Drawing.Point(532, 226);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 4;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// frmOptions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(619, 261);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.lnkAddProfile);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.grpProperties);
			this.Controls.Add(this.listProfiles);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "frmOptions";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Options";
			this.Load += new System.EventHandler(this.frmOptions_Load);
			this.grpProperties.ResumeLayout(false);
			this.grpProperties.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView listProfiles;
		private System.Windows.Forms.GroupBox grpProperties;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.ColumnHeader ColProfile;
		private System.Windows.Forms.LinkLabel lnkAddProfile;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.TextBox inName;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.LinkLabel lnkAddAssembly;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.TextBox inAssemblyPath;
		private System.Windows.Forms.Label lblAssemblies;
		private System.Windows.Forms.ListBox listAssemblies;
	}
}