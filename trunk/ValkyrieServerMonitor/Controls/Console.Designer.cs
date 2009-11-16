namespace ValkyrieServerMonitor
{
	partial class Console
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.inText = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// inText
			// 
			this.inText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.inText.Location = new System.Drawing.Point(0, 0);
			this.inText.Multiline = true;
			this.inText.Name = "inText";
			this.inText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.inText.Size = new System.Drawing.Size(346, 242);
			this.inText.TabIndex = 0;
			// 
			// Console
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.inText);
			this.Name = "Console";
			this.Size = new System.Drawing.Size(346, 242);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox inText;
	}
}
