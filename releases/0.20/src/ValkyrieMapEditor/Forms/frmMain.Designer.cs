﻿using System.Windows.Forms;
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
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.pctTileSurface = new ValkyrieMapEditor.TileBox();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.btnMapProperties = new System.Windows.Forms.Button();
			this.lstSettings = new System.Windows.Forms.ListView();
			this.colSetting = new System.Windows.Forms.ColumnHeader();
			this.colValue = new System.Windows.Forms.ColumnHeader();
			this.panel1 = new System.Windows.Forms.Panel();
			this.pctSurface = new System.Windows.Forms.PictureBox();
			this.VerticalScroll = new System.Windows.Forms.VScrollBar();
			this.HorizontalScroll = new System.Windows.Forms.HScrollBar();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.footerStatus = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblVersion = new System.Windows.Forms.ToolStripStatusLabel();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.toolProject = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolNew = new System.Windows.Forms.ToolStripMenuItem();
			this.toolOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.toolSave = new System.Windows.Forms.ToolStripMenuItem();
			this.toolSaveAs = new System.Windows.Forms.ToolStripMenuItem();
			this.toolClose = new System.Windows.Forms.ToolStripMenuItem();
			this.toolView = new System.Windows.Forms.ToolStripDropDownButton();
			this.currentLayerAndBelowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dimOtherLayersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.allLayersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnViewSelected = new System.Windows.Forms.ToolStripMenuItem();
			this.toolTool = new System.Windows.Forms.ToolStripDropDownButton();
			this.btnAnimatedTileManager = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripTools = new System.Windows.Forms.ToolStrip();
			this.btnNew = new System.Windows.Forms.ToolStripButton();
			this.btnOpen = new System.Windows.Forms.ToolStripButton();
			this.btnSave = new System.Windows.Forms.ToolStripButton();
			this.btnCut = new System.Windows.Forms.ToolStripButton();
			this.btnCopy = new System.Windows.Forms.ToolStripButton();
			this.btnPaste = new System.Windows.Forms.ToolStripButton();
			this.btnDelete = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.btnUndo = new System.Windows.Forms.ToolStripButton();
			this.btnRedo = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.btnUnderLayer = new System.Windows.Forms.ToolStripButton();
			this.btnBaseLayer = new System.Windows.Forms.ToolStripButton();
			this.btnMiddleLayer = new System.Windows.Forms.ToolStripButton();
			this.btnTopLayer = new System.Windows.Forms.ToolStripButton();
			this.btnHelp = new System.Windows.Forms.ToolStripButton();
			this.btnSelection = new System.Windows.Forms.ToolStripButton();
			this.btnPencil = new System.Windows.Forms.ToolStripButton();
			this.btnRect = new System.Windows.Forms.ToolStripButton();
			this.btnFill = new System.Windows.Forms.ToolStripButton();
			this.btnEvent = new System.Windows.Forms.ToolStripButton();
			this.btnCollisionLayer = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.btnZoomNone = new System.Windows.Forms.ToolStripButton();
			this.btnZoomMedium = new System.Windows.Forms.ToolStripButton();
			this.btnZoomFar = new System.Windows.Forms.ToolStripButton();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pctTileSurface)).BeginInit();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pctSurface)).BeginInit();
			this.footerStatus.SuspendLayout();
			this.menuStrip.SuspendLayout();
			this.toolStripTools.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer
			// 
			this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer.Location = new System.Drawing.Point(0, 53);
			this.splitContainer.Name = "splitContainer";
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.splitContainer2);
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.AutoScroll = true;
			this.splitContainer.Panel2.BackColor = System.Drawing.SystemColors.ControlDark;
			this.splitContainer.Panel2.Controls.Add(this.panel1);
			this.splitContainer.Size = new System.Drawing.Size(1008, 652);
			this.splitContainer.SplitterDistance = 311;
			this.splitContainer.SplitterWidth = 5;
			this.splitContainer.TabIndex = 2;
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
			this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.splitContainer2.Panel1.Controls.Add(this.pctTileSurface);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
			this.splitContainer2.Size = new System.Drawing.Size(307, 648);
			this.splitContainer2.SplitterDistance = 441;
			this.splitContainer2.SplitterWidth = 5;
			this.splitContainer2.TabIndex = 0;
			// 
			// pctTileSurface
			// 
			this.pctTileSurface.DisplayTileSelection = true;
			this.pctTileSurface.Location = new System.Drawing.Point(-2, 0);
			this.pctTileSurface.Name = "pctTileSurface";
			this.pctTileSurface.SelectedRect = new System.Drawing.Rectangle(0, 0, 13, 13);
			this.pctTileSurface.Size = new System.Drawing.Size(173, 134);
			this.pctTileSurface.TabIndex = 0;
			this.pctTileSurface.TabStop = false;
			this.pctTileSurface.TileSize = new System.Drawing.Point(0, 0);
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
			this.splitContainer3.Size = new System.Drawing.Size(307, 202);
			this.splitContainer3.SplitterDistance = 25;
			this.splitContainer3.TabIndex = 3;
			// 
			// btnMapProperties
			// 
			this.btnMapProperties.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnMapProperties.Enabled = false;
			this.btnMapProperties.Location = new System.Drawing.Point(0, 0);
			this.btnMapProperties.Name = "btnMapProperties";
			this.btnMapProperties.Size = new System.Drawing.Size(307, 25);
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
			this.lstSettings.Size = new System.Drawing.Size(307, 173);
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
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.Control;
			this.panel1.Controls.Add(this.pctSurface);
			this.panel1.Controls.Add(this.VerticalScroll);
			this.panel1.Controls.Add(this.HorizontalScroll);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(688, 648);
			this.panel1.TabIndex = 3;
			// 
			// pctSurface
			// 
			this.pctSurface.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pctSurface.Location = new System.Drawing.Point(0, -2);
			this.pctSurface.Name = "pctSurface";
			this.pctSurface.Size = new System.Drawing.Size(668, 633);
			this.pctSurface.TabIndex = 0;
			this.pctSurface.TabStop = false;
			this.pctSurface.Click += new System.EventHandler(this.pctSurface_Click);
			this.pctSurface.Resize += new System.EventHandler(this.pctSurface_Resize);
			// 
			// VerticalScroll
			// 
			this.VerticalScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.VerticalScroll.Location = new System.Drawing.Point(671, -2);
			this.VerticalScroll.Name = "VerticalScroll";
			this.VerticalScroll.Size = new System.Drawing.Size(17, 633);
			this.VerticalScroll.TabIndex = 2;
			this.VerticalScroll.Visible = false;
			this.VerticalScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.TileMap_Scroll);
			// 
			// HorizontalScroll
			// 
			this.HorizontalScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.HorizontalScroll.Location = new System.Drawing.Point(0, 631);
			this.HorizontalScroll.Name = "HorizontalScroll";
			this.HorizontalScroll.Size = new System.Drawing.Size(671, 17);
			this.HorizontalScroll.TabIndex = 1;
			this.HorizontalScroll.Visible = false;
			this.HorizontalScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.TileMap_Scroll);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Padding = new System.Windows.Forms.Padding(20);
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 26);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 26);
			// 
			// footerStatus
			// 
			this.footerStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.lblVersion});
			this.footerStatus.Location = new System.Drawing.Point(0, 708);
			this.footerStatus.Name = "footerStatus";
			this.footerStatus.Size = new System.Drawing.Size(1008, 22);
			this.footerStatus.TabIndex = 4;
			this.footerStatus.Text = "statusStrip1";
			// 
			// toolStripStatusLabel2
			// 
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.Size = new System.Drawing.Size(947, 17);
			this.toolStripStatusLabel2.Spring = true;
			// 
			// lblVersion
			// 
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(46, 17);
			this.lblVersion.Text = "Version";
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolProject,
            this.toolView,
            this.toolTool});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(1008, 26);
			this.menuStrip.TabIndex = 5;
			this.menuStrip.Text = "menuStrip1";
			// 
			// toolProject
			// 
			this.toolProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolProject.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolNew,
            this.toolOpen,
            this.toolSave,
            this.toolSaveAs,
            this.toolClose});
			this.toolProject.Image = ((System.Drawing.Image)(resources.GetObject("toolProject.Image")));
			this.toolProject.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolProject.Name = "toolProject";
			this.toolProject.ShowDropDownArrow = false;
			this.toolProject.Size = new System.Drawing.Size(29, 19);
			this.toolProject.Text = "File";
			// 
			// toolNew
			// 
			this.toolNew.Image = global::ValkyrieMapEditor.Properties.Resources.imgPage;
			this.toolNew.Name = "toolNew";
			this.toolNew.Size = new System.Drawing.Size(114, 22);
			this.toolNew.Text = "New";
			this.toolNew.Click += new System.EventHandler(this.toolNew_Click);
			// 
			// toolOpen
			// 
			this.toolOpen.Image = global::ValkyrieMapEditor.Properties.Resources.imgOpen;
			this.toolOpen.Name = "toolOpen";
			this.toolOpen.Size = new System.Drawing.Size(114, 22);
			this.toolOpen.Text = "Open";
			this.toolOpen.Click += new System.EventHandler(this.toolOpen_Click);
			// 
			// toolSave
			// 
			this.toolSave.Enabled = false;
			this.toolSave.Image = global::ValkyrieMapEditor.Properties.Resources.imgSave;
			this.toolSave.Name = "toolSave";
			this.toolSave.Size = new System.Drawing.Size(114, 22);
			this.toolSave.Text = "Save";
			this.toolSave.Click += new System.EventHandler(this.toolSave_Click);
			// 
			// toolSaveAs
			// 
			this.toolSaveAs.Enabled = false;
			this.toolSaveAs.Image = global::ValkyrieMapEditor.Properties.Resources.imgSaveAs;
			this.toolSaveAs.Name = "toolSaveAs";
			this.toolSaveAs.Size = new System.Drawing.Size(114, 22);
			this.toolSaveAs.Text = "Save As";
			this.toolSaveAs.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// toolClose
			// 
			this.toolClose.Image = global::ValkyrieMapEditor.Properties.Resources.imgExit;
			this.toolClose.Name = "toolClose";
			this.toolClose.Size = new System.Drawing.Size(114, 22);
			this.toolClose.Text = "Exit";
			this.toolClose.Click += new System.EventHandler(this.toolClose_Click);
			// 
			// toolView
			// 
			this.toolView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.currentLayerAndBelowToolStripMenuItem,
            this.dimOtherLayersToolStripMenuItem,
            this.allLayersToolStripMenuItem,
            this.toolStripSeparator1,
            this.btnViewSelected});
			this.toolView.Image = ((System.Drawing.Image)(resources.GetObject("toolView.Image")));
			this.toolView.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolView.Name = "toolView";
			this.toolView.Size = new System.Drawing.Size(45, 19);
			this.toolView.Text = "View";
			// 
			// currentLayerAndBelowToolStripMenuItem
			// 
			this.currentLayerAndBelowToolStripMenuItem.Name = "currentLayerAndBelowToolStripMenuItem";
			this.currentLayerAndBelowToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
			this.currentLayerAndBelowToolStripMenuItem.Text = "Current Layer and Below";
			this.currentLayerAndBelowToolStripMenuItem.Click += new System.EventHandler(this.currentLayerAndBelowToolStripMenuItem_Click);
			// 
			// dimOtherLayersToolStripMenuItem
			// 
			this.dimOtherLayersToolStripMenuItem.Name = "dimOtherLayersToolStripMenuItem";
			this.dimOtherLayersToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
			this.dimOtherLayersToolStripMenuItem.Text = "Current Layer Only";
			this.dimOtherLayersToolStripMenuItem.Click += new System.EventHandler(this.dimOtherLayersToolStripMenuItem_Click);
			// 
			// allLayersToolStripMenuItem
			// 
			this.allLayersToolStripMenuItem.Checked = true;
			this.allLayersToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.allLayersToolStripMenuItem.Name = "allLayersToolStripMenuItem";
			this.allLayersToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
			this.allLayersToolStripMenuItem.Text = "All Layers";
			this.allLayersToolStripMenuItem.Click += new System.EventHandler(this.allLayersToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(200, 6);
			// 
			// btnViewSelected
			// 
			this.btnViewSelected.Checked = true;
			this.btnViewSelected.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnViewSelected.Name = "btnViewSelected";
			this.btnViewSelected.Size = new System.Drawing.Size(203, 22);
			this.btnViewSelected.Text = "View Selected Graphic";
			this.btnViewSelected.Click += new System.EventHandler(this.btnViewSelected_Click);
			// 
			// toolTool
			// 
			this.toolTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolTool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAnimatedTileManager,
            this.optionsToolStripMenuItem});
			this.toolTool.Image = ((System.Drawing.Image)(resources.GetObject("toolTool.Image")));
			this.toolTool.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolTool.Name = "toolTool";
			this.toolTool.Size = new System.Drawing.Size(49, 19);
			this.toolTool.Text = "Tools";
			// 
			// btnAnimatedTileManager
			// 
			this.btnAnimatedTileManager.Enabled = false;
			this.btnAnimatedTileManager.Image = global::ValkyrieMapEditor.Properties.Resources.imgFilm;
			this.btnAnimatedTileManager.Name = "btnAnimatedTileManager";
			this.btnAnimatedTileManager.Size = new System.Drawing.Size(198, 22);
			this.btnAnimatedTileManager.Text = "Animated Tile Manager";
			this.btnAnimatedTileManager.Click += new System.EventHandler(this.btnAnimatedTileManager_Click);
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.Image = global::ValkyrieMapEditor.Properties.Resources.imgCog;
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.optionsToolStripMenuItem.Text = "Options";
			this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
			// 
			// toolStripTools
			// 
			this.toolStripTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnOpen,
            this.btnSave,
            this.toolStripSeparator2,
            this.btnCut,
            this.btnCopy,
            this.btnPaste,
            this.btnDelete,
            this.toolStripSeparator6,
            this.btnUndo,
            this.btnRedo,
            this.toolStripSeparator5,
            this.btnUnderLayer,
            this.btnBaseLayer,
            this.btnMiddleLayer,
            this.btnTopLayer,
            this.btnHelp,
            this.toolStripSeparator3,
            this.btnSelection,
            this.btnPencil,
            this.btnRect,
            this.btnFill,
            this.btnEvent,
            this.btnCollisionLayer,
            this.toolStripSeparator4,
            this.btnZoomNone,
            this.btnZoomMedium,
            this.btnZoomFar});
			this.toolStripTools.Location = new System.Drawing.Point(0, 26);
			this.toolStripTools.Name = "toolStripTools";
			this.toolStripTools.Size = new System.Drawing.Size(1008, 26);
			this.toolStripTools.TabIndex = 3;
			this.toolStripTools.Text = "toolStrip1";
			// 
			// btnNew
			// 
			this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnNew.Image = global::ValkyrieMapEditor.Properties.Resources.imgPage;
			this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnNew.Name = "btnNew";
			this.btnNew.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnNew.Size = new System.Drawing.Size(34, 23);
			this.btnNew.Text = "New Map";
			this.btnNew.Click += new System.EventHandler(this.toolNew_Click);
			// 
			// btnOpen
			// 
			this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnOpen.Image = global::ValkyrieMapEditor.Properties.Resources.imgOpen;
			this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnOpen.Size = new System.Drawing.Size(34, 23);
			this.btnOpen.Text = "Open Map";
			this.btnOpen.Click += new System.EventHandler(this.toolOpen_Click);
			// 
			// btnSave
			// 
			this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSave.Enabled = false;
			this.btnSave.Image = global::ValkyrieMapEditor.Properties.Resources.imgSave;
			this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSave.Name = "btnSave";
			this.btnSave.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnSave.Size = new System.Drawing.Size(34, 23);
			this.btnSave.Text = "Save Map";
			this.btnSave.Click += new System.EventHandler(this.toolSave_Click);
			// 
			// btnCut
			// 
			this.btnCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnCut.Enabled = false;
			this.btnCut.Image = global::ValkyrieMapEditor.Properties.Resources.imgCut;
			this.btnCut.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnCut.Name = "btnCut";
			this.btnCut.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnCut.Size = new System.Drawing.Size(34, 23);
			this.btnCut.Text = "Cut";
			// 
			// btnCopy
			// 
			this.btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnCopy.Enabled = false;
			this.btnCopy.Image = global::ValkyrieMapEditor.Properties.Resources.imgCopy;
			this.btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnCopy.Size = new System.Drawing.Size(34, 23);
			this.btnCopy.Text = "Copy";
			// 
			// btnPaste
			// 
			this.btnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnPaste.Enabled = false;
			this.btnPaste.Image = global::ValkyrieMapEditor.Properties.Resources.imgPaste;
			this.btnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnPaste.Name = "btnPaste";
			this.btnPaste.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnPaste.Size = new System.Drawing.Size(34, 23);
			this.btnPaste.Text = "Paste";
			// 
			// btnDelete
			// 
			this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnDelete.Enabled = false;
			this.btnDelete.Image = global::ValkyrieMapEditor.Properties.Resources.imgDelete2;
			this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnDelete.Size = new System.Drawing.Size(34, 23);
			this.btnDelete.Text = "Delete";
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(6, 26);
			// 
			// btnUndo
			// 
			this.btnUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnUndo.Enabled = false;
			this.btnUndo.Image = global::ValkyrieMapEditor.Properties.Resources.imgUndo;
			this.btnUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnUndo.Name = "btnUndo";
			this.btnUndo.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnUndo.Size = new System.Drawing.Size(34, 23);
			this.btnUndo.Text = "Undo";
			this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
			// 
			// btnRedo
			// 
			this.btnRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnRedo.Enabled = false;
			this.btnRedo.Image = global::ValkyrieMapEditor.Properties.Resources.imgRedo;
			this.btnRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRedo.Name = "btnRedo";
			this.btnRedo.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnRedo.Size = new System.Drawing.Size(34, 23);
			this.btnRedo.Text = "Redo";
			this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(6, 26);
			// 
			// btnUnderLayer
			// 
			this.btnUnderLayer.CheckOnClick = true;
			this.btnUnderLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnUnderLayer.Enabled = false;
			this.btnUnderLayer.Image = global::ValkyrieMapEditor.Properties.Resources.imgUnderLayer;
			this.btnUnderLayer.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.btnUnderLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnUnderLayer.Name = "btnUnderLayer";
			this.btnUnderLayer.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnUnderLayer.Size = new System.Drawing.Size(34, 23);
			this.btnUnderLayer.Text = "Under Layer";
			this.btnUnderLayer.Click += new System.EventHandler(this.btnUnderLayer_Click);
			// 
			// btnBaseLayer
			// 
			this.btnBaseLayer.Checked = true;
			this.btnBaseLayer.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnBaseLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnBaseLayer.Enabled = false;
			this.btnBaseLayer.Image = global::ValkyrieMapEditor.Properties.Resources.imgBaseLayer;
			this.btnBaseLayer.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.btnBaseLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnBaseLayer.Name = "btnBaseLayer";
			this.btnBaseLayer.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnBaseLayer.Size = new System.Drawing.Size(34, 23);
			this.btnBaseLayer.Text = "Base Layer";
			this.btnBaseLayer.Click += new System.EventHandler(this.btnBaseLayer_Click);
			// 
			// btnMiddleLayer
			// 
			this.btnMiddleLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnMiddleLayer.Enabled = false;
			this.btnMiddleLayer.Image = global::ValkyrieMapEditor.Properties.Resources.imgMiddleLayer;
			this.btnMiddleLayer.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.btnMiddleLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnMiddleLayer.Name = "btnMiddleLayer";
			this.btnMiddleLayer.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnMiddleLayer.Size = new System.Drawing.Size(34, 23);
			this.btnMiddleLayer.Text = "Middle Layer";
			this.btnMiddleLayer.Click += new System.EventHandler(this.btnMiddleLayer_Click);
			// 
			// btnTopLayer
			// 
			this.btnTopLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnTopLayer.Enabled = false;
			this.btnTopLayer.Image = global::ValkyrieMapEditor.Properties.Resources.imgTopLayer;
			this.btnTopLayer.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.btnTopLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnTopLayer.Name = "btnTopLayer";
			this.btnTopLayer.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnTopLayer.Size = new System.Drawing.Size(34, 23);
			this.btnTopLayer.Text = "Top Layer";
			this.btnTopLayer.Click += new System.EventHandler(this.btnTopLayer_Click);
			// 
			// btnHelp
			// 
			this.btnHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.btnHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnHelp.Image = global::ValkyrieMapEditor.Properties.Resources.imgHelp;
			this.btnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnHelp.Size = new System.Drawing.Size(34, 23);
			this.btnHelp.Text = "Help";
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// btnSelection
			// 
			this.btnSelection.Checked = true;
			this.btnSelection.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnSelection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSelection.Enabled = false;
			this.btnSelection.Image = global::ValkyrieMapEditor.Properties.Resources.imgSelect;
			this.btnSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSelection.Name = "btnSelection";
			this.btnSelection.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnSelection.Size = new System.Drawing.Size(34, 23);
			this.btnSelection.Text = "Selection";
			this.btnSelection.Click += new System.EventHandler(this.btnSelection_Click);
			// 
			// btnPencil
			// 
			this.btnPencil.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnPencil.Enabled = false;
			this.btnPencil.Image = global::ValkyrieMapEditor.Properties.Resources.imgPencil;
			this.btnPencil.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnPencil.Name = "btnPencil";
			this.btnPencil.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnPencil.Size = new System.Drawing.Size(34, 23);
			this.btnPencil.Text = "Pencil";
			this.btnPencil.Click += new System.EventHandler(this.btnPencil_Click);
			// 
			// btnRect
			// 
			this.btnRect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnRect.Enabled = false;
			this.btnRect.Image = global::ValkyrieMapEditor.Properties.Resources.imgRectangle;
			this.btnRect.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRect.Name = "btnRect";
			this.btnRect.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnRect.Size = new System.Drawing.Size(34, 23);
			this.btnRect.Text = "Rectangle";
			this.btnRect.Click += new System.EventHandler(this.btnRect_Click);
			// 
			// btnFill
			// 
			this.btnFill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnFill.Enabled = false;
			this.btnFill.Image = global::ValkyrieMapEditor.Properties.Resources.imgBucket;
			this.btnFill.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnFill.Name = "btnFill";
			this.btnFill.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnFill.Size = new System.Drawing.Size(34, 23);
			this.btnFill.Text = "Fill";
			this.btnFill.Click += new System.EventHandler(this.btnFill_Click);
			// 
			// btnEvent
			// 
			this.btnEvent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnEvent.Enabled = false;
			this.btnEvent.Image = global::ValkyrieMapEditor.Properties.Resources.imgLightning;
			this.btnEvent.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnEvent.Name = "btnEvent";
			this.btnEvent.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnEvent.Size = new System.Drawing.Size(34, 23);
			this.btnEvent.Text = "Event Mode";
			this.btnEvent.Click += new System.EventHandler(this.btnEvent_Click);
			// 
			// btnCollisionLayer
			// 
			this.btnCollisionLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnCollisionLayer.Enabled = false;
			this.btnCollisionLayer.Image = global::ValkyrieMapEditor.Properties.Resources.imgCollision;
			this.btnCollisionLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnCollisionLayer.Name = "btnCollisionLayer";
			this.btnCollisionLayer.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnCollisionLayer.Size = new System.Drawing.Size(34, 23);
			this.btnCollisionLayer.Text = "Collison Layer";
			this.btnCollisionLayer.Click += new System.EventHandler(this.btnCollisionLayer_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 26);
			// 
			// btnZoomNone
			// 
			this.btnZoomNone.Checked = true;
			this.btnZoomNone.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnZoomNone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnZoomNone.Enabled = false;
			this.btnZoomNone.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomNone.Image")));
			this.btnZoomNone.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnZoomNone.Name = "btnZoomNone";
			this.btnZoomNone.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnZoomNone.Size = new System.Drawing.Size(34, 23);
			this.btnZoomNone.Text = "No Zoom";
			this.btnZoomNone.Click += new System.EventHandler(this.btnZoomNone_Click);
			// 
			// btnZoomMedium
			// 
			this.btnZoomMedium.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnZoomMedium.Enabled = false;
			this.btnZoomMedium.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomMedium.Image")));
			this.btnZoomMedium.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnZoomMedium.Name = "btnZoomMedium";
			this.btnZoomMedium.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnZoomMedium.Size = new System.Drawing.Size(34, 23);
			this.btnZoomMedium.Text = "Medium Zoom";
			this.btnZoomMedium.Click += new System.EventHandler(this.btnZoomMedium_Click);
			// 
			// btnZoomFar
			// 
			this.btnZoomFar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnZoomFar.Enabled = false;
			this.btnZoomFar.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomFar.Image")));
			this.btnZoomFar.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnZoomFar.Name = "btnZoomFar";
			this.btnZoomFar.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.btnZoomFar.Size = new System.Drawing.Size(34, 23);
			this.btnZoomFar.Text = "Far Zoom";
			this.btnZoomFar.Click += new System.EventHandler(this.btnZoomFar_Click);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1008, 730);
			this.Controls.Add(this.footerStatus);
			this.Controls.Add(this.toolStripTools);
			this.Controls.Add(this.splitContainer);
			this.Controls.Add(this.menuStrip);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MainMenuStrip = this.menuStrip;
			this.Name = "frmMain";
			this.Text = "Griffin Map Editor";
			this.Deactivate += new System.EventHandler(this.frmMain_Deactivate);
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseWheel);
			this.Activated += new System.EventHandler(this.frmMain_Activated);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			this.splitContainer.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pctTileSurface)).EndInit();
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			this.splitContainer3.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pctSurface)).EndInit();
			this.footerStatus.ResumeLayout(false);
			this.footerStatus.PerformLayout();
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.toolStripTools.ResumeLayout(false);
			this.toolStripTools.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.PictureBox pctSurface;
		private System.Windows.Forms.ToolStripDropDownButton toolProject;
		private System.Windows.Forms.ToolStripMenuItem toolNew;
		private System.Windows.Forms.ToolStripMenuItem toolOpen;
		private System.Windows.Forms.ToolStripMenuItem toolClose;
		private System.Windows.Forms.ToolStripMenuItem toolSave;
		private System.Windows.Forms.ToolStripDropDownButton toolView;
		private System.Windows.Forms.ToolStripMenuItem currentLayerAndBelowToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem allLayersToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem dimOtherLayersToolStripMenuItem;
		private System.Windows.Forms.ToolStripDropDownButton toolTool;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView lstSettings;
        private System.Windows.Forms.ColumnHeader colSetting;
        private System.Windows.Forms.ColumnHeader colValue;
        private TileBox pctTileSurface;
        private System.Windows.Forms.SplitContainer splitContainer3;
		private System.Windows.Forms.Button btnMapProperties;
        private System.Windows.Forms.ToolStripButton btnBaseLayer;
        private System.Windows.Forms.ToolStripButton btnMiddleLayer;
        private System.Windows.Forms.ToolStripButton btnTopLayer;
        private System.Windows.Forms.ToolStripButton btnCollisionLayer;
		private System.Windows.Forms.ToolStripMenuItem toolSaveAs;
		private System.Windows.Forms.StatusStrip footerStatus;
		private System.Windows.Forms.ToolStripButton btnHelp;
		private System.Windows.Forms.ToolStripButton btnNew;
		private System.Windows.Forms.ToolStripButton btnOpen;
		private System.Windows.Forms.ToolStripButton btnSave;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton btnPencil;
		private System.Windows.Forms.ToolStripButton btnRect;
		private System.Windows.Forms.ToolStripButton btnFill;
		private System.Windows.Forms.ToolStripButton btnSelection;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStrip toolStripTools;
		private System.Windows.Forms.VScrollBar VerticalScroll;
		private System.Windows.Forms.HScrollBar HorizontalScroll;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton btnEvent;
		private System.Windows.Forms.ToolStripButton btnUnderLayer;
		private System.Windows.Forms.ToolStripMenuItem btnAnimatedTileManager;
		private System.Windows.Forms.ToolStripMenuItem btnViewSelected;
		private System.Windows.Forms.ToolStripStatusLabel lblVersion;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripButton btnZoomNone;
		private System.Windows.Forms.ToolStripButton btnZoomMedium;
		private System.Windows.Forms.ToolStripButton btnZoomFar;
		private ToolStripButton btnCut;
		private ToolStripButton btnCopy;
		private ToolStripButton btnPaste;
		private ToolStripButton btnDelete;
		private ToolStripSeparator toolStripSeparator6;
		private ToolStripButton btnUndo;
		private ToolStripButton btnRedo;
		private ToolStripSeparator toolStripSeparator5;
	}
}