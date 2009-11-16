namespace ValkyrieServerMonitor
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
			this.label1 = new System.Windows.Forms.Label();
			this.grpAddress = new System.Windows.Forms.GroupBox();
			this.inChatAddress = new System.Windows.Forms.TextBox();
			this.inGameAddress = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.lblGameServer = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.inRetryTime = new System.Windows.Forms.NumericUpDown();
			this.lblTimeFormat = new System.Windows.Forms.Label();
			this.inTimeFormat = new System.Windows.Forms.TextBox();
			this.grpAddress.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.inRetryTime)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(14, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(105, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Retry Time (in sec):";
			// 
			// grpAddress
			// 
			this.grpAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpAddress.Controls.Add(this.inChatAddress);
			this.grpAddress.Controls.Add(this.inGameAddress);
			this.grpAddress.Controls.Add(this.label2);
			this.grpAddress.Controls.Add(this.lblGameServer);
			this.grpAddress.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.grpAddress.Location = new System.Drawing.Point(17, 82);
			this.grpAddress.Name = "grpAddress";
			this.grpAddress.Size = new System.Drawing.Size(391, 100);
			this.grpAddress.TabIndex = 2;
			this.grpAddress.TabStop = false;
			this.grpAddress.Text = "Server Address\'s";
			// 
			// inChatAddress
			// 
			this.inChatAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.inChatAddress.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.inChatAddress.Location = new System.Drawing.Point(135, 63);
			this.inChatAddress.Name = "inChatAddress";
			this.inChatAddress.Size = new System.Drawing.Size(236, 23);
			this.inChatAddress.TabIndex = 4;
			// 
			// inGameAddress
			// 
			this.inGameAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.inGameAddress.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.inGameAddress.Location = new System.Drawing.Point(135, 31);
			this.inGameAddress.Name = "inGameAddress";
			this.inGameAddress.Size = new System.Drawing.Size(236, 23);
			this.inGameAddress.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(18, 66);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(76, 15);
			this.label2.TabIndex = 3;
			this.label2.Text = "Chat Server:";
			// 
			// lblGameServer
			// 
			this.lblGameServer.AutoSize = true;
			this.lblGameServer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblGameServer.Location = new System.Drawing.Point(18, 34);
			this.lblGameServer.Name = "lblGameServer";
			this.lblGameServer.Size = new System.Drawing.Size(84, 15);
			this.lblGameServer.TabIndex = 2;
			this.lblGameServer.Text = "Game Server:";
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(321, 188);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(87, 27);
			this.btnOk.TabIndex = 6;
			this.btnOk.Text = "&Ok";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(226, 188);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(87, 27);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// inRetryTime
			// 
			this.inRetryTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.inRetryTime.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.inRetryTime.Location = new System.Drawing.Point(152, 12);
			this.inRetryTime.Maximum = new decimal(new int[] {
            18000,
            0,
            0,
            0});
			this.inRetryTime.Name = "inRetryTime";
			this.inRetryTime.Size = new System.Drawing.Size(256, 23);
			this.inRetryTime.TabIndex = 1;
			this.inRetryTime.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
			// 
			// lblTimeFormat
			// 
			this.lblTimeFormat.AutoSize = true;
			this.lblTimeFormat.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTimeFormat.Location = new System.Drawing.Point(14, 48);
			this.lblTimeFormat.Name = "lblTimeFormat";
			this.lblTimeFormat.Size = new System.Drawing.Size(115, 15);
			this.lblTimeFormat.TabIndex = 6;
			this.lblTimeFormat.Text = "Timestamp Format:";
			// 
			// inTimeFormat
			// 
			this.inTimeFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.inTimeFormat.Location = new System.Drawing.Point(152, 45);
			this.inTimeFormat.Name = "inTimeFormat";
			this.inTimeFormat.Size = new System.Drawing.Size(256, 23);
			this.inTimeFormat.TabIndex = 2;
			this.inTimeFormat.Text = "t";
			// 
			// frmOptions
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(422, 228);
			this.Controls.Add(this.inTimeFormat);
			this.Controls.Add(this.lblTimeFormat);
			this.Controls.Add(this.inRetryTime);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.grpAddress);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmOptions";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Options";
			this.grpAddress.ResumeLayout(false);
			this.grpAddress.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.inRetryTime)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox grpAddress;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.NumericUpDown inRetryTime;
		private System.Windows.Forms.TextBox inChatAddress;
		private System.Windows.Forms.TextBox inGameAddress;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblGameServer;
		private System.Windows.Forms.Label lblTimeFormat;
		private System.Windows.Forms.TextBox inTimeFormat;
	}
}