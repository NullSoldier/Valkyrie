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
			this.inTilePane = new ValkyrieMapEditor.TileBox();
			((System.ComponentModel.ISupportInitialize)(this.inTilePane)).BeginInit();
			this.SuspendLayout();
			// 
			// inTilePane
			// 
			this.inTilePane.DisplayTileSelection = false;
			this.inTilePane.EndSelectedPoint = new System.Drawing.Point(0, 0);
			this.inTilePane.Location = new System.Drawing.Point(-2, 1);
			this.inTilePane.Name = "inTilePane";
			this.inTilePane.SelectedPoint = new System.Drawing.Point(0, 0);
			this.inTilePane.Size = new System.Drawing.Size(117, 58);
			this.inTilePane.TabIndex = 0;
			this.inTilePane.TabStop = false;
			this.inTilePane.TileSize = new System.Drawing.Point(0, 0);
			// 
			// frmAnimatedTile
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(331, 302);
			this.Controls.Add(this.inTilePane);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "frmAnimatedTile";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Animated Tile";
			this.Load += new System.EventHandler(this.frmAnimatedTile_Load);
			((System.ComponentModel.ISupportInitialize)(this.inTilePane)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private TileBox inTilePane;
	}
}