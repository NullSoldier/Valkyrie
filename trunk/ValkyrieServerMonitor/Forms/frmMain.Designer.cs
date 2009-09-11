namespace ValkyrieServerMonitor
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
			this.listView1 = new System.Windows.Forms.ListView();
			this.colServer = new System.Windows.Forms.ColumnHeader();
			this.colStatus = new System.Windows.Forms.ColumnHeader();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.btnEnter = new System.Windows.Forms.Button();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.btnConnect = new System.Windows.Forms.ToolStripButton();
			this.btnOptions = new System.Windows.Forms.ToolStripButton();
			this.Console = new ValkyrieServerMonitor.Console();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// listView1
			// 
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colServer,
            this.colStatus});
			this.listView1.Location = new System.Drawing.Point(12, 27);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(466, 97);
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// colServer
			// 
			this.colServer.Text = "Server";
			this.colServer.Width = 295;
			// 
			// colStatus
			// 
			this.colStatus.Text = "Status";
			this.colStatus.Width = 109;
			// 
			// textBox2
			// 
			this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBox2.Enabled = false;
			this.textBox2.HideSelection = false;
			this.textBox2.Location = new System.Drawing.Point(12, 344);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(385, 23);
			this.textBox2.TabIndex = 2;
			// 
			// btnEnter
			// 
			this.btnEnter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEnter.Enabled = false;
			this.btnEnter.Location = new System.Drawing.Point(403, 343);
			this.btnEnter.Name = "btnEnter";
			this.btnEnter.Size = new System.Drawing.Size(75, 23);
			this.btnEnter.TabIndex = 3;
			this.btnEnter.Text = "Enter";
			this.btnEnter.UseVisualStyleBackColor = true;
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnConnect,
            this.btnOptions});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(490, 25);
			this.toolStrip1.TabIndex = 4;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// btnConnect
			// 
			this.btnConnect.Image = global::ValkyrieServerMonitor.Properties.Resources.imgConnect;
			this.btnConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(72, 22);
			this.btnConnect.Text = "Connect";
			this.btnConnect.ToolTipText = "Connect to the servers";
			// 
			// btnOptions
			// 
			this.btnOptions.Image = global::ValkyrieServerMonitor.Properties.Resources.imgOptions;
			this.btnOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnOptions.Name = "btnOptions";
			this.btnOptions.Size = new System.Drawing.Size(69, 22);
			this.btnOptions.Text = "Options";
			this.btnOptions.ToolTipText = "change the server monitor\'s options";
			this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
			// 
			// Console
			// 
			this.Console.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.Console.Location = new System.Drawing.Point(12, 130);
			this.Console.Name = "Console";
			this.Console.Size = new System.Drawing.Size(466, 207);
			this.Console.TabIndex = 5;
			// 
			// frmMain
			// 
			this.AcceptButton = this.btnEnter;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(490, 379);
			this.Controls.Add(this.Console);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.btnEnter);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.listView1);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Server Monitor";
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader colServer;
		private System.Windows.Forms.ColumnHeader colStatus;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Button btnEnter;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton btnConnect;
		private Console Console;
		private System.Windows.Forms.ToolStripButton btnOptions;
	}
}