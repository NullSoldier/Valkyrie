namespace ValkyrieMapEditor
{
	partial class frmMain
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.ToolBar = new System.Windows.Forms.ToolStrip();
			this.toolFile = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolNew = new System.Windows.Forms.ToolStripMenuItem();
			this.toolOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.toolClose = new System.Windows.Forms.ToolStripMenuItem();
			this.toolSave = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
			this.currentLayerAndBelowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.allLayersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.dimOtherLayersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripDropDownButton3 = new System.Windows.Forms.ToolStripDropDownButton();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.btnBaseLayer = new System.Windows.Forms.ToolStripButton();
			this.btnMiddleLayer = new System.Windows.Forms.ToolStripButton();
			this.btnTopLayer = new System.Windows.Forms.ToolStripButton();
			this.btnCollisionLayer = new System.Windows.Forms.ToolStripButton();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.pctTileSurface = new System.Windows.Forms.PictureBox();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.btnMapProperties = new System.Windows.Forms.Button();
			this.lstSettings = new System.Windows.Forms.ListView();
			this.colSetting = new System.Windows.Forms.ColumnHeader();
			this.colValue = new System.Windows.Forms.ColumnHeader();
			this.pctSurface = new System.Windows.Forms.PictureBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.ToolBar.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pctTileSurface)).BeginInit();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pctSurface)).BeginInit();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// ToolBar
			// 
			this.ToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolFile,
            this.toolStripDropDownButton2,
            this.toolStripDropDownButton3,
            this.toolStripComboBox1,
            this.toolStripSeparator2,
            this.btnBaseLayer,
            this.btnMiddleLayer,
            this.btnTopLayer,
            this.btnCollisionLayer});
			this.ToolBar.Location = new System.Drawing.Point(0, 0);
			this.ToolBar.Name = "ToolBar";
			this.ToolBar.Size = new System.Drawing.Size(1010, 25);
			this.ToolBar.TabIndex = 1;
			this.ToolBar.Text = "toolStrip1";
			// 
			// toolFile
			// 
			this.toolFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolNew,
            this.toolOpen,
            this.toolClose,
            this.toolSave});
			this.toolFile.Image = ((System.Drawing.Image)(resources.GetObject("toolFile.Image")));
			this.toolFile.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolFile.Name = "toolFile";
			this.toolFile.Size = new System.Drawing.Size(38, 22);
			this.toolFile.Text = "File";
			// 
			// toolNew
			// 
			this.toolNew.Name = "toolNew";
			this.toolNew.Size = new System.Drawing.Size(152, 22);
			this.toolNew.Text = "New";
			this.toolNew.Click += new System.EventHandler(this.toolNew_Click);
			// 
			// toolOpen
			// 
			this.toolOpen.Name = "toolOpen";
			this.toolOpen.Size = new System.Drawing.Size(152, 22);
			this.toolOpen.Text = "Open";
			this.toolOpen.Click += new System.EventHandler(this.toolOpen_Click);
			// 
			// toolClose
			// 
			this.toolClose.Name = "toolClose";
			this.toolClose.Size = new System.Drawing.Size(152, 22);
			this.toolClose.Text = "Close";
			// 
			// toolSave
			// 
			this.toolSave.Name = "toolSave";
			this.toolSave.Size = new System.Drawing.Size(152, 22);
			this.toolSave.Text = "Save";
			this.toolSave.Click += new System.EventHandler(this.toolSave_Click);
			// 
			// toolStripDropDownButton2
			// 
			this.toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.currentLayerAndBelowToolStripMenuItem,
            this.allLayersToolStripMenuItem,
            this.toolStripSeparator1,
            this.dimOtherLayersToolStripMenuItem});
			this.toolStripDropDownButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton2.Image")));
			this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
			this.toolStripDropDownButton2.Size = new System.Drawing.Size(45, 22);
			this.toolStripDropDownButton2.Text = "View";
			// 
			// currentLayerAndBelowToolStripMenuItem
			// 
			this.currentLayerAndBelowToolStripMenuItem.Name = "currentLayerAndBelowToolStripMenuItem";
			this.currentLayerAndBelowToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
			this.currentLayerAndBelowToolStripMenuItem.Text = "Current Layer and Below";
			// 
			// allLayersToolStripMenuItem
			// 
			this.allLayersToolStripMenuItem.Name = "allLayersToolStripMenuItem";
			this.allLayersToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
			this.allLayersToolStripMenuItem.Text = "All Layers";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(200, 6);
			// 
			// dimOtherLayersToolStripMenuItem
			// 
			this.dimOtherLayersToolStripMenuItem.Name = "dimOtherLayersToolStripMenuItem";
			this.dimOtherLayersToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
			this.dimOtherLayersToolStripMenuItem.Text = "Dim Other Layers";
			// 
			// toolStripDropDownButton3
			// 
			this.toolStripDropDownButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripDropDownButton3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
			this.toolStripDropDownButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton3.Image")));
			this.toolStripDropDownButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton3.Name = "toolStripDropDownButton3";
			this.toolStripDropDownButton3.Size = new System.Drawing.Size(49, 22);
			this.toolStripDropDownButton3.Text = "Tools";
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
			this.optionsToolStripMenuItem.Text = "Options";
			// 
			// toolStripComboBox1
			// 
			this.toolStripComboBox1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.toolStripComboBox1.DropDownWidth = 121;
			this.toolStripComboBox1.MaxDropDownItems = 4;
			this.toolStripComboBox1.Name = "toolStripComboBox1";
			this.toolStripComboBox1.Size = new System.Drawing.Size(200, 25);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// btnBaseLayer
			// 
			this.btnBaseLayer.Checked = true;
			this.btnBaseLayer.CheckOnClick = true;
			this.btnBaseLayer.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnBaseLayer.Image = ((System.Drawing.Image)(resources.GetObject("btnBaseLayer.Image")));
			this.btnBaseLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnBaseLayer.Name = "btnBaseLayer";
			this.btnBaseLayer.Size = new System.Drawing.Size(82, 22);
			this.btnBaseLayer.Text = "Base Layer";
			this.btnBaseLayer.Click += new System.EventHandler(this.btnBaseLayer_Click);
			// 
			// btnMiddleLayer
			// 
			this.btnMiddleLayer.CheckOnClick = true;
			this.btnMiddleLayer.Image = ((System.Drawing.Image)(resources.GetObject("btnMiddleLayer.Image")));
			this.btnMiddleLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnMiddleLayer.Name = "btnMiddleLayer";
			this.btnMiddleLayer.Size = new System.Drawing.Size(95, 22);
			this.btnMiddleLayer.Text = "Middle Layer";
			this.btnMiddleLayer.Click += new System.EventHandler(this.btnMiddleLayer_Click);
			// 
			// btnTopLayer
			// 
			this.btnTopLayer.CheckOnClick = true;
			this.btnTopLayer.Image = ((System.Drawing.Image)(resources.GetObject("btnTopLayer.Image")));
			this.btnTopLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnTopLayer.Name = "btnTopLayer";
			this.btnTopLayer.Size = new System.Drawing.Size(79, 22);
			this.btnTopLayer.Text = "Top Layer";
			this.btnTopLayer.Click += new System.EventHandler(this.btnTopLayer_Click);
			// 
			// btnCollisionLayer
			// 
			this.btnCollisionLayer.CheckOnClick = true;
			this.btnCollisionLayer.Image = ((System.Drawing.Image)(resources.GetObject("btnCollisionLayer.Image")));
			this.btnCollisionLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnCollisionLayer.Name = "btnCollisionLayer";
			this.btnCollisionLayer.Size = new System.Drawing.Size(101, 22);
			this.btnCollisionLayer.Text = "Collison Layer";
			this.btnCollisionLayer.Click += new System.EventHandler(this.btnCollisionLayer_Click);
			// 
			// splitContainer1
			// 
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(0, 25);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.AutoScroll = true;
			this.splitContainer1.Panel2.Controls.Add(this.pctSurface);
			this.splitContainer1.Size = new System.Drawing.Size(1010, 563);
			this.splitContainer1.SplitterDistance = 280;
			this.splitContainer1.SplitterWidth = 5;
			this.splitContainer1.TabIndex = 2;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.AutoScroll = true;
			this.splitContainer2.Panel1.Controls.Add(this.pctTileSurface);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
			this.splitContainer2.Size = new System.Drawing.Size(276, 559);
			this.splitContainer2.SplitterDistance = 341;
			this.splitContainer2.SplitterWidth = 5;
			this.splitContainer2.TabIndex = 0;
			// 
			// pctTileSurface
			// 
			this.pctTileSurface.Location = new System.Drawing.Point(-2, 0);
			this.pctTileSurface.Name = "pctTileSurface";
			this.pctTileSurface.Size = new System.Drawing.Size(226, 170);
			this.pctTileSurface.TabIndex = 0;
			this.pctTileSurface.TabStop = false;
			this.pctTileSurface.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pctTileSurface_MouseClick);
			// 
			// splitContainer3
			// 
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.IsSplitterFixed = true;
			this.splitContainer3.Location = new System.Drawing.Point(0, 0);
			this.splitContainer3.Name = "splitContainer3";
			this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer3.Panel1
			// 
			this.splitContainer3.Panel1.Controls.Add(this.btnMapProperties);
			// 
			// splitContainer3.Panel2
			// 
			this.splitContainer3.Panel2.Controls.Add(this.lstSettings);
			this.splitContainer3.Size = new System.Drawing.Size(276, 213);
			this.splitContainer3.SplitterDistance = 26;
			this.splitContainer3.TabIndex = 3;
			// 
			// btnMapProperties
			// 
			this.btnMapProperties.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnMapProperties.Enabled = false;
			this.btnMapProperties.Location = new System.Drawing.Point(0, 0);
			this.btnMapProperties.Name = "btnMapProperties";
			this.btnMapProperties.Size = new System.Drawing.Size(276, 26);
			this.btnMapProperties.TabIndex = 1;
			this.btnMapProperties.Text = "Map Properties";
			this.btnMapProperties.UseVisualStyleBackColor = true;
			this.btnMapProperties.Click += new System.EventHandler(this.btnMapProperties_Click);
			// 
			// lstSettings
			// 
			this.lstSettings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSetting,
            this.colValue});
			this.lstSettings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstSettings.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstSettings.FullRowSelect = true;
			this.lstSettings.HideSelection = false;
			this.lstSettings.Location = new System.Drawing.Point(0, 0);
			this.lstSettings.Name = "lstSettings";
			this.lstSettings.Size = new System.Drawing.Size(276, 183);
			this.lstSettings.TabIndex = 0;
			this.lstSettings.UseCompatibleStateImageBehavior = false;
			this.lstSettings.View = System.Windows.Forms.View.Details;
			// 
			// colSetting
			// 
			this.colSetting.Text = "Setting";
			this.colSetting.Width = 99;
			// 
			// colValue
			// 
			this.colValue.Text = "Value";
			this.colValue.Width = 155;
			// 
			// pctSurface
			// 
			this.pctSurface.Location = new System.Drawing.Point(0, 1);
			this.pctSurface.Name = "pctSurface";
			this.pctSurface.Size = new System.Drawing.Size(1, 1);
			this.pctSurface.TabIndex = 0;
			this.pctSurface.TabStop = false;
			this.pctSurface.Resize += new System.EventHandler(this.pctSurface_Resize);
			this.pctSurface.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pctSurface_MouseClick);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 564);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
			this.statusStrip1.Size = new System.Drawing.Size(1010, 24);
			this.statusStrip1.TabIndex = 3;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.toolStripStatusLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(92, 19);
			this.toolStripStatusLabel1.Text = "mouseLoc (0,0)";
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1010, 588);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.ToolBar);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "frmMain";
			this.Text = "Valkryie Map Editor";
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
			this.ToolBar.ResumeLayout(false);
			this.ToolBar.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pctTileSurface)).EndInit();
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			this.splitContainer3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pctSurface)).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pctSurface;
		private System.Windows.Forms.ToolStrip ToolBar;
		private System.Windows.Forms.ToolStripDropDownButton toolFile;
		private System.Windows.Forms.ToolStripMenuItem toolNew;
		private System.Windows.Forms.ToolStripMenuItem toolOpen;
		private System.Windows.Forms.ToolStripMenuItem toolClose;
		private System.Windows.Forms.ToolStripMenuItem toolSave;
		private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
		private System.Windows.Forms.ToolStripMenuItem currentLayerAndBelowToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem allLayersToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem dimOtherLayersToolStripMenuItem;
		private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
		private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton3;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView lstSettings;
        private System.Windows.Forms.ColumnHeader colSetting;
        private System.Windows.Forms.ColumnHeader colValue;
        private System.Windows.Forms.PictureBox pctTileSurface;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Button btnMapProperties;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnBaseLayer;
        private System.Windows.Forms.ToolStripButton btnMiddleLayer;
        private System.Windows.Forms.ToolStripButton btnTopLayer;
        private System.Windows.Forms.ToolStripButton btnCollisionLayer;
	}
}