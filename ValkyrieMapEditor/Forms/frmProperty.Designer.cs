namespace ValkyrieMapEditor
{
    partial class frmProperty
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
			this.lblName = new System.Windows.Forms.Label();
			this.inName = new System.Windows.Forms.TextBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.lblWidth = new System.Windows.Forms.Label();
			this.inMapWidth = new System.Windows.Forms.NumericUpDown();
			this.lblHeight = new System.Windows.Forms.Label();
			this.inMapHeight = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.inTileSet = new System.Windows.Forms.TextBox();
			this.btnBrowseTileSet = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.numericUpDown6 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDown7 = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.inTileWidth = new System.Windows.Forms.NumericUpDown();
			this.inTileHeight = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.inMapWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.inMapHeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown7)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.inTileWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.inTileHeight)).BeginInit();
			this.SuspendLayout();
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.lblName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblName.Location = new System.Drawing.Point(15, 15);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(43, 15);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "Name:";
			// 
			// inName
			// 
			this.inName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.inName.Location = new System.Drawing.Point(122, 12);
			this.inName.Name = "inName";
			this.inName.Size = new System.Drawing.Size(260, 22);
			this.inName.TabIndex = 1;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(310, 204);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 10;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(229, 204);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 9;
			this.btnOk.Text = "&Ok";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// lblWidth
			// 
			this.lblWidth.AutoSize = true;
			this.lblWidth.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblWidth.Location = new System.Drawing.Point(15, 46);
			this.lblWidth.Name = "lblWidth";
			this.lblWidth.Size = new System.Drawing.Size(71, 15);
			this.lblWidth.TabIndex = 2;
			this.lblWidth.Text = "Map Width:";
			// 
			// inMapWidth
			// 
			this.inMapWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.inMapWidth.Location = new System.Drawing.Point(122, 45);
			this.inMapWidth.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.inMapWidth.Name = "inMapWidth";
			this.inMapWidth.Size = new System.Drawing.Size(260, 22);
			this.inMapWidth.TabIndex = 2;
			this.inMapWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblHeight
			// 
			this.lblHeight.AutoSize = true;
			this.lblHeight.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblHeight.Location = new System.Drawing.Point(15, 78);
			this.lblHeight.Name = "lblHeight";
			this.lblHeight.Size = new System.Drawing.Size(75, 15);
			this.lblHeight.TabIndex = 4;
			this.lblHeight.Text = "Map Height:";
			// 
			// inMapHeight
			// 
			this.inMapHeight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.inMapHeight.Location = new System.Drawing.Point(122, 77);
			this.inMapHeight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.inMapHeight.Name = "inMapHeight";
			this.inMapHeight.Size = new System.Drawing.Size(260, 22);
			this.inMapHeight.TabIndex = 3;
			this.inMapHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(15, 114);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(52, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "Tile Set:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(22, 10);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(35, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Width";
			// 
			// numericUpDown3
			// 
			this.numericUpDown3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDown3.Location = new System.Drawing.Point(96, 4);
			this.numericUpDown3.Name = "numericUpDown3";
			this.numericUpDown3.Size = new System.Drawing.Size(77, 20);
			this.numericUpDown3.TabIndex = 3;
			// 
			// numericUpDown4
			// 
			this.numericUpDown4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDown4.Location = new System.Drawing.Point(54, 4);
			this.numericUpDown4.Name = "numericUpDown4";
			this.numericUpDown4.Size = new System.Drawing.Size(70, 20);
			this.numericUpDown4.TabIndex = 5;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 10);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(38, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "Height";
			// 
			// inTileSet
			// 
			this.inTileSet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.inTileSet.Location = new System.Drawing.Point(122, 110);
			this.inTileSet.Name = "inTileSet";
			this.inTileSet.Size = new System.Drawing.Size(169, 22);
			this.inTileSet.TabIndex = 4;
			// 
			// btnBrowseTileSet
			// 
			this.btnBrowseTileSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseTileSet.Location = new System.Drawing.Point(297, 109);
			this.btnBrowseTileSet.Name = "btnBrowseTileSet";
			this.btnBrowseTileSet.Size = new System.Drawing.Size(85, 23);
			this.btnBrowseTileSet.TabIndex = 5;
			this.btnBrowseTileSet.Text = "Browse";
			this.btnBrowseTileSet.UseVisualStyleBackColor = true;
			this.btnBrowseTileSet.Click += new System.EventHandler(this.btnBrowseTileSet_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(18, 10);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(44, 15);
			this.label2.TabIndex = 2;
			this.label2.Text = "Width:";
			// 
			// numericUpDown6
			// 
			this.numericUpDown6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDown6.Location = new System.Drawing.Point(125, 8);
			this.numericUpDown6.Name = "numericUpDown6";
			this.numericUpDown6.Size = new System.Drawing.Size(127, 20);
			this.numericUpDown6.TabIndex = 3;
			// 
			// numericUpDown7
			// 
			this.numericUpDown7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDown7.Location = new System.Drawing.Point(54, 8);
			this.numericUpDown7.Name = "numericUpDown7";
			this.numericUpDown7.Size = new System.Drawing.Size(118, 20);
			this.numericUpDown7.TabIndex = 5;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(3, 10);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(45, 15);
			this.label5.TabIndex = 4;
			this.label5.Text = "Height";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(15, 142);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(67, 15);
			this.label6.TabIndex = 2;
			this.label6.Text = "Tile Width:";
			// 
			// inTileWidth
			// 
			this.inTileWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.inTileWidth.Location = new System.Drawing.Point(122, 142);
			this.inTileWidth.Name = "inTileWidth";
			this.inTileWidth.Size = new System.Drawing.Size(260, 22);
			this.inTileWidth.TabIndex = 7;
			this.inTileWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// inTileHeight
			// 
			this.inTileHeight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.inTileHeight.Location = new System.Drawing.Point(122, 172);
			this.inTileHeight.Name = "inTileHeight";
			this.inTileHeight.Size = new System.Drawing.Size(260, 22);
			this.inTileHeight.TabIndex = 8;
			this.inTileHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(15, 173);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(71, 15);
			this.label7.TabIndex = 4;
			this.label7.Text = "Tile Height:";
			// 
			// frmProperty
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(397, 239);
			this.Controls.Add(this.inTileHeight);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.inTileWidth);
			this.Controls.Add(this.inMapWidth);
			this.Controls.Add(this.lblWidth);
			this.Controls.Add(this.inMapHeight);
			this.Controls.Add(this.lblHeight);
			this.Controls.Add(this.btnBrowseTileSet);
			this.Controls.Add(this.inTileSet);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.inName);
			this.Controls.Add(this.lblName);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MinimumSize = new System.Drawing.Size(331, 177);
			this.Name = "frmProperty";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Map Properties";
			((System.ComponentModel.ISupportInitialize)(this.inMapWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.inMapHeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown7)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.inTileWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.inTileHeight)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox inName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.NumericUpDown inMapHeight;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.NumericUpDown inMapWidth;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox inTileSet;
		private System.Windows.Forms.Button btnBrowseTileSet;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown6;
        private System.Windows.Forms.NumericUpDown numericUpDown7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown inTileWidth;
        private System.Windows.Forms.NumericUpDown inTileHeight;
        private System.Windows.Forms.Label label7;
    }
}