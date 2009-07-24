namespace ValkyrieMapEditor.Forms
{
	partial class frmAnimatedTileManager
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
			this.lstAnimatedTiles = new System.Windows.Forms.ListView();
			this.lnkAdd = new System.Windows.Forms.LinkLabel();
			this.colFrameCount = new System.Windows.Forms.ColumnHeader();
			this.colTileRect = new System.Windows.Forms.ColumnHeader();
			this.colSpeed = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// lstAnimatedTiles
			// 
			this.lstAnimatedTiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstAnimatedTiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTileRect,
            this.colFrameCount,
            this.colSpeed});
			this.lstAnimatedTiles.Location = new System.Drawing.Point(0, 2);
			this.lstAnimatedTiles.Name = "lstAnimatedTiles";
			this.lstAnimatedTiles.Size = new System.Drawing.Size(430, 241);
			this.lstAnimatedTiles.TabIndex = 1;
			this.lstAnimatedTiles.UseCompatibleStateImageBehavior = false;
			this.lstAnimatedTiles.View = System.Windows.Forms.View.Details;
			// 
			// lnkAdd
			// 
			this.lnkAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lnkAdd.AutoSize = true;
			this.lnkAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lnkAdd.Location = new System.Drawing.Point(304, 246);
			this.lnkAdd.Name = "lnkAdd";
			this.lnkAdd.Size = new System.Drawing.Size(115, 15);
			this.lnkAdd.TabIndex = 2;
			this.lnkAdd.TabStop = true;
			this.lnkAdd.Text = "Add Animated Tile..";
			// 
			// colFrameCount
			// 
			this.colFrameCount.Text = "Frame Count";
			this.colFrameCount.Width = 109;
			// 
			// colTileRect
			// 
			this.colTileRect.Text = "Tile Rectangle";
			this.colTileRect.Width = 168;
			// 
			// colSpeed
			// 
			this.colSpeed.Text = "Speed";
			this.colSpeed.Width = 92;
			// 
			// frmAnimatedTileManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(431, 270);
			this.Controls.Add(this.lnkAdd);
			this.Controls.Add(this.lstAnimatedTiles);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "frmAnimatedTileManager";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Animated Tile Manager";
			this.Load += new System.EventHandler(this.frmAnimatedTileManager_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView lstAnimatedTiles;
		private System.Windows.Forms.LinkLabel lnkAdd;
		private System.Windows.Forms.ColumnHeader colFrameCount;
		private System.Windows.Forms.ColumnHeader colTileRect;
		private System.Windows.Forms.ColumnHeader colSpeed;
	}
}