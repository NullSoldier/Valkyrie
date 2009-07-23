namespace ValkyrieWorldEditor.Forms
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAddWorld = new System.Windows.Forms.ToolStripButton();
            this.btnAddMap = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnHand = new System.Windows.Forms.ToolStripButton();
            this.btnSelect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tscbScale = new System.Windows.Forms.ToolStripComboBox();
            this.VerticalScroll = new System.Windows.Forms.VScrollBar();
            this.HorizontalScroll = new System.Windows.Forms.HScrollBar();
            this.groupWorld = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbDefaultSpawn = new System.Windows.Forms.ComboBox();
            this.cbCurWorld = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbMapYPos = new System.Windows.Forms.TextBox();
            this.tbMapXPos = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbMapName = new System.Windows.Forms.TextBox();
            this.pctPreview = new System.Windows.Forms.PictureBox();
            this.pctSurface = new System.Windows.Forms.PictureBox();
            this.toolStrip1.SuspendLayout();
            this.groupWorld.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctSurface)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnOpen,
            this.btnSave,
            this.btnExport,
            this.toolStripSeparator1,
            this.btnAddWorld,
            this.btnAddMap,
            this.toolStripSeparator2,
            this.btnHand,
            this.btnSelect,
            this.toolStripSeparator3,
            this.tscbScale});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(959, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnNew
            // 
            this.btnNew.AutoSize = false;
            this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(32, 22);
            this.btnNew.Text = "toolStripButton1";
            this.btnNew.ToolTipText = "New";
            // 
            // btnOpen
            // 
            this.btnOpen.AutoSize = false;
            this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
            this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(32, 22);
            this.btnOpen.Text = "Open";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.AutoSize = false;
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Enabled = false;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(32, 22);
            this.btnSave.Text = "toolStripButton2";
            this.btnSave.ToolTipText = "Save";
            // 
            // btnExport
            // 
            this.btnExport.AutoSize = false;
            this.btnExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExport.Enabled = false;
            this.btnExport.Image = ((System.Drawing.Image)(resources.GetObject("btnExport.Image")));
            this.btnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(32, 22);
            this.btnExport.Text = "toolStripButton3";
            this.btnExport.ToolTipText = "Export";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAddWorld
            // 
            this.btnAddWorld.AutoSize = false;
            this.btnAddWorld.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddWorld.Enabled = false;
            this.btnAddWorld.Image = ((System.Drawing.Image)(resources.GetObject("btnAddWorld.Image")));
            this.btnAddWorld.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddWorld.Name = "btnAddWorld";
            this.btnAddWorld.Size = new System.Drawing.Size(32, 22);
            this.btnAddWorld.Text = "toolStripButton4";
            this.btnAddWorld.ToolTipText = "Add World";
            // 
            // btnAddMap
            // 
            this.btnAddMap.AutoSize = false;
            this.btnAddMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddMap.Enabled = false;
            this.btnAddMap.Image = ((System.Drawing.Image)(resources.GetObject("btnAddMap.Image")));
            this.btnAddMap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddMap.Name = "btnAddMap";
            this.btnAddMap.Size = new System.Drawing.Size(32, 22);
            this.btnAddMap.Text = "toolStripButton5";
            this.btnAddMap.ToolTipText = "Add map";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnHand
            // 
            this.btnHand.AutoSize = false;
            this.btnHand.CheckOnClick = true;
            this.btnHand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnHand.Image = ((System.Drawing.Image)(resources.GetObject("btnHand.Image")));
            this.btnHand.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHand.Name = "btnHand";
            this.btnHand.Size = new System.Drawing.Size(32, 22);
            this.btnHand.Text = "toolStripButton1";
            this.btnHand.ToolTipText = "Hand Tool";
            // 
            // btnSelect
            // 
            this.btnSelect.AutoSize = false;
            this.btnSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSelect.Image = ((System.Drawing.Image)(resources.GetObject("btnSelect.Image")));
            this.btnSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(32, 22);
            this.btnSelect.Text = "Select Map";
            this.btnSelect.ToolTipText = "Select Tool";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tscbScale
            // 
            this.tscbScale.Items.AddRange(new object[] {
            "10",
            "25",
            "50",
            "75",
            "100"});
            this.tscbScale.Name = "tscbScale";
            this.tscbScale.Size = new System.Drawing.Size(75, 25);
            this.tscbScale.Text = "100";
            this.tscbScale.ToolTipText = "Scale";
            this.tscbScale.TextChanged += new System.EventHandler(this.OnScaleChanged);
            // 
            // VerticalScroll
            // 
            this.VerticalScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.VerticalScroll.Location = new System.Drawing.Point(942, 25);
            this.VerticalScroll.Name = "VerticalScroll";
            this.VerticalScroll.Size = new System.Drawing.Size(17, 582);
            this.VerticalScroll.TabIndex = 4;
            this.VerticalScroll.Visible = false;
            this.VerticalScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.OnSurfaceScrolled);
            // 
            // HorizontalScroll
            // 
            this.HorizontalScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.HorizontalScroll.Location = new System.Drawing.Point(199, 612);
            this.HorizontalScroll.Name = "HorizontalScroll";
            this.HorizontalScroll.Size = new System.Drawing.Size(740, 18);
            this.HorizontalScroll.TabIndex = 3;
            this.HorizontalScroll.Visible = false;
            this.HorizontalScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.OnSurfaceScrolled);
            // 
            // groupWorld
            // 
            this.groupWorld.Controls.Add(this.label1);
            this.groupWorld.Controls.Add(this.cbDefaultSpawn);
            this.groupWorld.Enabled = false;
            this.groupWorld.Location = new System.Drawing.Point(6, 274);
            this.groupWorld.Name = "groupWorld";
            this.groupWorld.Size = new System.Drawing.Size(187, 80);
            this.groupWorld.TabIndex = 5;
            this.groupWorld.TabStop = false;
            this.groupWorld.Text = "World Properties";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Default Spawn";
            // 
            // cbDefaultSpawn
            // 
            this.cbDefaultSpawn.FormattingEnabled = true;
            this.cbDefaultSpawn.Location = new System.Drawing.Point(6, 39);
            this.cbDefaultSpawn.Name = "cbDefaultSpawn";
            this.cbDefaultSpawn.Size = new System.Drawing.Size(175, 21);
            this.cbDefaultSpawn.Sorted = true;
            this.cbDefaultSpawn.TabIndex = 0;
            this.cbDefaultSpawn.SelectedIndexChanged += new System.EventHandler(this.OnSpawnChange);
            // 
            // cbCurWorld
            // 
            this.cbCurWorld.Enabled = false;
            this.cbCurWorld.FormattingEnabled = true;
            this.cbCurWorld.Location = new System.Drawing.Point(6, 247);
            this.cbCurWorld.Name = "cbCurWorld";
            this.cbCurWorld.Size = new System.Drawing.Size(187, 21);
            this.cbCurWorld.Sorted = true;
            this.cbCurWorld.TabIndex = 6;
            this.cbCurWorld.SelectedIndexChanged += new System.EventHandler(this.OnWorldSelect);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 230);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Current World";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbMapYPos);
            this.groupBox1.Controls.Add(this.tbMapXPos);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbMapName);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(6, 360);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(187, 125);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Map Properties";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(58, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Y Pos";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(58, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "X Pos";
            // 
            // tbMapYPos
            // 
            this.tbMapYPos.Location = new System.Drawing.Point(99, 91);
            this.tbMapYPos.Name = "tbMapYPos";
            this.tbMapYPos.Size = new System.Drawing.Size(82, 20);
            this.tbMapYPos.TabIndex = 2;
            // 
            // tbMapXPos
            // 
            this.tbMapXPos.Location = new System.Drawing.Point(99, 65);
            this.tbMapXPos.Name = "tbMapXPos";
            this.tbMapXPos.Size = new System.Drawing.Size(82, 20);
            this.tbMapXPos.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Map Name:";
            // 
            // tbMapName
            // 
            this.tbMapName.Location = new System.Drawing.Point(6, 39);
            this.tbMapName.Name = "tbMapName";
            this.tbMapName.Size = new System.Drawing.Size(175, 20);
            this.tbMapName.TabIndex = 0;
            this.tbMapName.TextChanged += new System.EventHandler(this.OnNameChange);
            // 
            // pctPreview
            // 
            this.pctPreview.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pctPreview.Location = new System.Drawing.Point(6, 28);
            this.pctPreview.Name = "pctPreview";
            this.pctPreview.Size = new System.Drawing.Size(187, 187);
            this.pctPreview.TabIndex = 9;
            this.pctPreview.TabStop = false;
            // 
            // pctSurface
            // 
            this.pctSurface.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pctSurface.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pctSurface.Location = new System.Drawing.Point(199, 28);
            this.pctSurface.Name = "pctSurface";
            this.pctSurface.Size = new System.Drawing.Size(740, 581);
            this.pctSurface.TabIndex = 1;
            this.pctSurface.TabStop = false;
            this.pctSurface.Click += new System.EventHandler(this.pctSurface_Click);
            this.pctSurface.Resize += new System.EventHandler(this.pctSurface_Resized);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 630);
            this.Controls.Add(this.pctPreview);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbCurWorld);
            this.Controls.Add(this.groupWorld);
            this.Controls.Add(this.VerticalScroll);
            this.Controls.Add(this.HorizontalScroll);
            this.Controls.Add(this.pctSurface);
            this.Controls.Add(this.toolStrip1);
            this.Name = "frmMain";
            this.Text = "Valkyrie World Editor";
            this.Deactivate += new System.EventHandler(this.frmMain_Deactivated);
            this.Activated += new System.EventHandler(this.frmMain_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnClose);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupWorld.ResumeLayout(false);
            this.groupWorld.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctSurface)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnExport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnAddWorld;
        private System.Windows.Forms.ToolStripButton btnAddMap;
        private System.Windows.Forms.VScrollBar VerticalScroll;
        private System.Windows.Forms.HScrollBar HorizontalScroll;
        private System.Windows.Forms.GroupBox groupWorld;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbDefaultSpawn;
        private System.Windows.Forms.ComboBox cbCurWorld;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbMapYPos;
        private System.Windows.Forms.TextBox tbMapXPos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbMapName;
        private System.Windows.Forms.PictureBox pctPreview;
        private System.Windows.Forms.ToolStripButton btnOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripComboBox tscbScale;
        private System.Windows.Forms.ToolStripButton btnHand;
        private System.Windows.Forms.ToolStripButton btnSelect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        public System.Windows.Forms.PictureBox pctSurface;
    }
}