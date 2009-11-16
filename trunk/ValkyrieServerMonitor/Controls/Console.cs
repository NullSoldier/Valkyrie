using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ValkyrieServerMonitor
{
	public partial class Console : UserControl
	{
		public string TimeFormat
		{
			get { return this.timeformat; }
			set { this.timeformat = value; }
		}

		public Console()
		{
			InitializeComponent();

			this.inText.Font = this.Font;
			this.inText.ForeColor = this.ForeColor;
		}

		public void Log(string text)
		{
			this.inText.Text += string.Format("{0}: {1}{2}", DateTime.Now.ToString(this.TimeFormat), text, Environment.NewLine);

			this.inText.SelectionStart = this.inText.Text.Length;
			this.inText.ScrollToCaret();
		}

		private string timeformat = "t";
	}
}
