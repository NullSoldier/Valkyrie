using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ValkyrieServerMonitor
{
	public partial class frmOptions : Form
	{
		public MonitorConfiguration Configuration
		{
			get { return this.configuration; }
			set { this.configuration = value; }
		}

		private MonitorConfiguration configuration;

		public frmOptions(MonitorConfiguration configuration)
		{
			InitializeComponent();

			this.Configuration = configuration;

			this.DisplayConfiguration();
		}

		private void DisplayConfiguration()
		{
			this.inRetryTime.Value = Convert.ToDecimal(this.Configuration[MonitorConfigurationName.RetryTime]);
			this.inTimeFormat.Text = this.Configuration[MonitorConfigurationName.TimeStampFormat];

			this.inGameAddress.Text = this.Configuration[MonitorConfigurationName.GameServerAddress];
			this.inChatAddress.Text = this.Configuration[MonitorConfigurationName.ChatServerAddress];
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (!this.Validate())
				return;

			this.Configuration[MonitorConfigurationName.RetryTime] = Convert.ToString(this.inRetryTime.Text);
			this.Configuration[MonitorConfigurationName.TimeStampFormat] = this.inTimeFormat.Text;

			this.Configuration[MonitorConfigurationName.GameServerAddress] = this.inGameAddress.Text;
			this.Configuration[MonitorConfigurationName.ChatServerAddress] = this.inChatAddress.Text;

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private bool Validate()
		{
			bool validateOK = true;

			if (string.IsNullOrEmpty(this.inTimeFormat.Text))
				validateOK = false;

			if (string.IsNullOrEmpty(this.inGameAddress.Text))
				validateOK = false;
			
			if (string.IsNullOrEmpty (this.inChatAddress.Text))
				validateOK = false;

			return validateOK;
		}
	}
}
