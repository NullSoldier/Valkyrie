namespace ValkyrieMapEditor.Forms
{
	partial class frmAnimatedTile
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
			this.grpProperties = new System.Windows.Forms.GroupBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.inSpeed = new System.Windows.Forms.NumericUpDown();
			this.lblSpeed = new System.Windows.Forms.Label();
			this.splitTileManager = new System.Windows.Forms.SplitContainer();
			this.inTilePane = new ValkyrieMapEditor.TileBox();
			this.grpProperties.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.inSpeed)).BeginInit();
			this.splitTileManager.Panel1.SuspendLayout();
			this.splitTileManager.Panel2.SuspendLayout();
			this.splitTileManager.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.inTilePane)).BeginInit();
			this.SuspendLayout();
			// 
			// grpProperties
			// 
			this.grpProperties.Controls.Add(this.btnOk);
			this.grpProperties.Controls.Add(this.inSpeed);
			this.grpProperties.Controls.Add(this.lblSpeed);
			this.grpProperties.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.grpProperties.Location = new System.Drawing.Point(0, 0);
			this.grpProperties.Name = "grpProperties";
			this.grpProperties.Size = new System.Drawing.Size(480, 97);
			this.grpProperties.TabIndex = 1;
			this.grpProperties.TabStop = false;
			this.grpProperties.Text = "Properties";
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnOk.Location = new System.Drawing.Point(393, 68);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 2;
			this.btnOk.Text = "&Ok";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// inSpeed
			// 
			this.inSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.inSpeed.DecimalPlaces = 2;
			this.inSpeed.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.inSpeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.inSpeed.Location = new System.Drawing.Point(59, 31);
			this.inSpeed.Name = "inSpeed";
			this.inSpeed.Size = new System.Drawing.Size(409, 23);
			this.inSpeed.TabIndex = 1;
			this.inSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.inSpeed.Value = new decimal(new int[] {
            2,
            0,
            0,
            131072});
			// 
			// lblSpeed
			// 
			this.lblSpeed.AutoSize = true;
			this.lblSpeed.Location = new System.Drawing.Point(7, 33);
			this.lblSpeed.Name = "lblSpeed";
			this.lblSpeed.Size = new System.Drawing.Size(45, 15);
			this.lblSpeed.TabIndex = 0;
			this.lblSpeed.Text = "Speed:";
			// 
			// splitTileManager
			// 
			this.splitTileManager.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitTileManager.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitTileManager.IsSplitterFixed = true;
			this.splitTileManager.Location = new System.Drawing.Point(0, 0);
			this.splitTileManager.Name = "splitTileManager";
			this.splitTileManager.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitTileManager.Panel1
			// 
			this.splitTileManager.Panel1.Controls.Add(this.grpProperties);
			// 
			// splitTileManager.Panel2
			// 
			this.splitTileManager.Panel2.AutoScroll = true;
			this.splitTileManager.Panel2.Controls.Add(this.inTilePane);
			this.splitTileManager.Size = new System.Drawing.Size(480, 562);
			this.splitTileManager.SplitterDistance = 97;
			this.splitTileManager.TabIndex = 2;
			// 
			// inTilePane
			// 
			this.inTilePane.DisplayTileSelection = false;
			this.inTilePane.EndSelectedPoint = new System.Drawing.Point(0, 0);
			this.inTilePane.Location = new System.Drawing.Point(1, 0);
			this.inTilePane.Name = "inTilePane";
			this.inTilePane.SelectedPoint = new System.Drawing.Point(0, 0);
			this.inTilePane.Size = new System.Drawing.Size(377, 411);
			this.inTilePane.TabIndex = 0;
			this.inTilePane.TabStop = false;
			this.inTilePane.TileSize = new System.Drawing.Point(0, 0);
			// 
			// frmAnimatedTile
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(480, 562);
			this.Controls.Add(this.splitTileManager);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "frmAnimatedTile";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Animated Tile";
			this.grpProperties.ResumeLayout(false);
			this.grpProperties.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.inSpeed)).EndInit();
			this.splitTileManager.Panel1.ResumeLayout(false);
			this.splitTileManager.Panel2.ResumeLayout(false);
			this.splitTileManager.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.inTilePane)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private TileBox inTilePane;
		private System.Windows.Forms.GroupBox grpProperties;
		private System.Windows.Forms.SplitContainer splitTileManager;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.NumericUpDown inSpeed;
		private System.Windows.Forms.Label lblSpeed;
	}
}