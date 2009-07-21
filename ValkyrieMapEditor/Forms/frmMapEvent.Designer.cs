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
            this.label1 = new System.Windows.Forms.Label();
            this.tbArgOne = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbArgTwo = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.cbDir = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Type:";
            // 
            // tbArgOne
            // 
            this.tbArgOne.Location = new System.Drawing.Point(52, 36);
            this.tbArgOne.Name = "tbArgOne";
            this.tbArgOne.Size = new System.Drawing.Size(241, 20);
            this.tbArgOne.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Arg 1:";
            // 
            // tbArgTwo
            // 
            this.tbArgTwo.Location = new System.Drawing.Point(52, 62);
            this.tbArgTwo.Multiline = true;
            this.tbArgTwo.Name = "tbArgTwo";
            this.tbArgTwo.Size = new System.Drawing.Size(242, 88);
            this.tbArgTwo.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(57, 156);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(138, 156);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Arg 2:";
            // 
            // cbType
            // 
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "Jump",
            "SignPost"});
            this.cbType.Location = new System.Drawing.Point(52, 9);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(161, 21);
            this.cbType.Sorted = true;
            this.cbType.TabIndex = 7;
            this.cbType.Text = "Null Event";
            // 
            // cbDir
            // 
            this.cbDir.FormattingEnabled = true;
            this.cbDir.Items.AddRange(new object[] {
            "All",
            "East",
            "North",
            "South",
            "West"});
            this.cbDir.Location = new System.Drawing.Point(219, 9);
            this.cbDir.Name = "cbDir";
            this.cbDir.Size = new System.Drawing.Size(72, 21);
            this.cbDir.Sorted = true;
            this.cbDir.TabIndex = 8;
            this.cbDir.Text = "All";
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.button3.Location = new System.Drawing.Point(219, 156);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "Delete";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // frmMapEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 187);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.cbDir);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbArgTwo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbArgOne);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMapEvent";
            this.Text = "Map Event";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;

        public System.Windows.Forms.TextBox tbArgOne;
        public System.Windows.Forms.TextBox tbArgTwo;
        public System.Windows.Forms.ComboBox cbType;
        public System.Windows.Forms.ComboBox cbDir;
        private System.Windows.Forms.Button button3;
    }
}