namespace ValkyrieMapEditor.Forms
{
    partial class frmMapEvent
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
			this.lblType = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.inType = new System.Windows.Forms.ComboBox();
			this.inDirection = new System.Windows.Forms.ComboBox();
			this.btnDelete = new System.Windows.Forms.Button();
			this.flowParameters = new System.Windows.Forms.FlowLayoutPanel();
			this.lblDirection = new System.Windows.Forms.Label();
			this.lnkAddParameter = new System.Windows.Forms.LinkLabel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblType
			// 
			this.lblType.AutoSize = true;
			this.lblType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblType.Location = new System.Drawing.Point(12, 13);
			this.lblType.Name = "lblType";
			this.lblType.Size = new System.Drawing.Size(37, 15);
			this.lblType.TabIndex = 1;
			this.lblType.Text = "Type:";
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(358, 163);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(87, 27);
			this.btnOk.TabIndex = 4;
			this.btnOk.Text = "Ok";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(451, 163);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(87, 27);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// inType
			// 
			this.inType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.inType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.inType.FormattingEnabled = true;
			this.inType.Location = new System.Drawing.Point(99, 10);
			this.inType.Name = "inType";
			this.inType.Size = new System.Drawing.Size(439, 23);
			this.inType.Sorted = true;
			this.inType.TabIndex = 7;
			// 
			// inDirection
			// 
			this.inDirection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.inDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.inDirection.FormattingEnabled = true;
			this.inDirection.Location = new System.Drawing.Point(99, 46);
			this.inDirection.Name = "inDirection";
			this.inDirection.Size = new System.Drawing.Size(439, 23);
			this.inDirection.Sorted = true;
			this.inDirection.TabIndex = 8;
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this.btnDelete.Location = new System.Drawing.Point(15, 163);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(87, 27);
			this.btnDelete.TabIndex = 9;
			this.btnDelete.Text = "Delete";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// flowParameters
			// 
			this.flowParameters.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowParameters.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.flowParameters.Location = new System.Drawing.Point(3, 19);
			this.flowParameters.Name = "flowParameters";
			this.flowParameters.Size = new System.Drawing.Size(517, 29);
			this.flowParameters.TabIndex = 10;
			// 
			// lblDirection
			// 
			this.lblDirection.AutoSize = true;
			this.lblDirection.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDirection.Location = new System.Drawing.Point(12, 50);
			this.lblDirection.Name = "lblDirection";
			this.lblDirection.Size = new System.Drawing.Size(62, 15);
			this.lblDirection.TabIndex = 11;
			this.lblDirection.Text = "Direction:";
			// 
			// lnkAddParameter
			// 
			this.lnkAddParameter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lnkAddParameter.AutoSize = true;
			this.lnkAddParameter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lnkAddParameter.Location = new System.Drawing.Point(447, 134);
			this.lnkAddParameter.Name = "lnkAddParameter";
			this.lnkAddParameter.Size = new System.Drawing.Size(91, 15);
			this.lnkAddParameter.TabIndex = 12;
			this.lnkAddParameter.TabStop = true;
			this.lnkAddParameter.Text = "Add Parameter";
			this.lnkAddParameter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAddParameter_LinkClicked);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.flowParameters);
			this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(15, 80);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(523, 51);
			this.groupBox1.TabIndex = 13;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Parameters";
			// 
			// frmMapEvent
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(545, 202);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.lnkAddParameter);
			this.Controls.Add(this.lblDirection);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.inDirection);
			this.Controls.Add(this.inType);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.lblType);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(313, 230);
			this.Name = "frmMapEvent";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Map Event";
			this.Load += new System.EventHandler(this.frmMapEvent_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.ComboBox inType;
        public System.Windows.Forms.ComboBox inDirection;
        private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.FlowLayoutPanel flowParameters;
		private System.Windows.Forms.Label lblDirection;
		private System.Windows.Forms.LinkLabel lnkAddParameter;
		private System.Windows.Forms.GroupBox groupBox1;
    }
}