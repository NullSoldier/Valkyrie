using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using Valkyrie.Library.Core;
using Valkyrie.Library.Network;
using Valkyrie.Library.Core.Messages;
using Valkyrie.Library;
using ValkyrieServerLibrary.Network.Messages.Valkyrie;
using ValkyrieServerLibrary.Network;
using System.Net;
using ValkyrieServerLibrary.Core;
using ValkyrieServerMonitor.Properties;
using ValkyrieServerMonitor.Core;

namespace ValkyrieServerMonitor
{
	public partial class frmMain : Form
	{
		private MonitorConfiguration Configuration
		{
			get;
			set;
		}

		public frmMain()
		{
			this.InitializeComponent();
			this.Icon = Icon.FromHandle(Resources.imgServerChart.GetHicon());

			this.Configuration = ConfigManager.LoadConfig("MonitorConfiguration.xml");
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
		    this.Console.Log("Server monitor version " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " initialized.");
		}

		private void btnOptions_Click(object sender, EventArgs e)
		{
			frmOptions options = new frmOptions(this.Configuration);
			var result = options.ShowDialog(this);

			if(result == DialogResult.OK)
			{
				this.Configuration = options.Configuration;
				
				ConfigManager.SaveConfig("MonitorConfiguration.xml", this.Configuration);
				// Disconnect and reconnect to new servers
			}
			
		}
	}
}
