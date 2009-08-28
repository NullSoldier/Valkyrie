using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ValkyrieLibrary.Events;
using ValkyrieLibrary.Characters;
using ValkyrieMapEditor.Properties;
using ValkyrieLibrary;

namespace ValkyrieMapEditor.Forms
{
    public partial class frmMapEvent : Form
    {
		public IMapEvent Event
		{
			get { return this.pevent; }
			set { this.pevent = value; }
		}

		private int InitialHeight = 0;
		private IMapEvent pevent;

		// For new events
		public frmMapEvent() { }

		public frmMapEvent(IMapEvent e)
        {
            InitializeComponent();

			this.Event = e;

			this.Icon = Icon.FromHandle(Resources.imgLightning.GetHicon());
			this.InitialHeight = this.Height - this.flowParameters.Height;
        }

		private void frmMapEvent_Load(object sender, EventArgs e)
		{
			// Event Handler types
			foreach (var type in frmMain.EventHandlerTypes)
				this.inType.Items.Add(type);

			this.inType.DisplayMember = "Name";
			this.inType.SelectedIndex = 0;

			// Directions
			var values = Enum.GetNames(typeof(Directions));

			for (int i = 0; i < values.Length; i++)
				this.inDirection.Items.Add(values[i]);

			this.inDirection.SelectedIndex = 0;

			// Activation
			values = Enum.GetNames(typeof(ActivationTypes));

			for (int i = 0; i < values.Length; i++)
				this.inActivation.Items.Add(values[i]);

			this.inActivation.SelectedIndex = 0;

			// Display events
			this.DisplayEvent();
		}

        public void DisplayEvent()
        {
			if (this.Event != null)
			{
				this.inType.SelectedItem = this.Event.GetType();

				// Wtf won't set selected value?? Do it manually
				for (int i = 0; i < this.inDirection.Items.Count; i++)
				{
					if (inDirection.Items[i].ToString() == this.Event.Direction.ToString())
						this.inDirection.SelectedIndex = i;
				}

				this.ClearParameters();

				foreach (var Parameter in this.Event.Parameters)
					this.AddParameter(Parameter.Key, Parameter.Value, true);
				
				this.ResizeWindow();
			}

			this.inType.SelectedIndexChanged += inType_SelectedIndexChanged;

			if( this.Event == null )
				this.inType_SelectedIndexChanged(this, EventArgs.Empty);

			this.ResizeWindow();
        }

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if( this.Event == null)
				this.Event = (IMapEvent)Activator.CreateInstance((Type)this.inType.SelectedItem);

			this.Event.Direction = (Directions)Enum.Parse(typeof(Directions), this.inDirection.Text);
			this.Event.Activation = (ActivationTypes)Enum.Parse(typeof(ActivationTypes), this.inActivation.SelectedItem.ToString());

			this.Event.Parameters = new Dictionary<string, string>();
			foreach (var Parameter in this.GetParameters())
			{
				if(!String.IsNullOrEmpty(Parameter.Key))
					this.Event.Parameters.Add(Parameter.Key, Parameter.Value);
			}

			this.Close();
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Abort;
		}

		private void AddParameter(string initialname, string initialvalue, bool nameReadOnly)
		{
			FlowLayoutPanel panel = new FlowLayoutPanel();
			panel.Width = flowParameters.Width;
			panel.Height = 20;
			panel.FlowDirection = FlowDirection.LeftToRight;

			LinkLabel label = new LinkLabel();
			label.Text = "Remove";
			label.Font = new Font(label.Font, FontStyle.Bold);
			label.Click += this.Remove_LinkClicked;

			TextBox nameText = new TextBox();
			nameText.Text = initialname;
			nameText.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			nameText.Tag = "Name";
			if (nameReadOnly)
			{
				nameText.ReadOnly = true;
				nameText.Width = (panel.Width / 2);
			}
			else
				nameText.Width = (panel.Width - (label.Width + 20)) / 3;

			TextBox valueText = new TextBox();
			valueText.Text = initialvalue;
			valueText.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			valueText.Tag = "Value";

			panel.Controls.Add(nameText);
			panel.Controls.Add(valueText);

			if (!nameReadOnly)
			{
				valueText.Width = (panel.Width / 2);
				panel.Controls.Add(label);
			}
			else
				valueText.Width = (panel.Width - (label.Width + 20)) / 2;

			this.flowParameters.Controls.Add(panel);
		}

		private void lnkAddParameter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.AddParameter(string.Empty, String.Empty, false);

			this.ResizeWindow();
		}

		private void ResizeWindow()
		{
			int height = (this.flowParameters.Controls.Count * 27);
			this.Height = this.InitialHeight + height;
		}

		private void Remove_LinkClicked(object sender, EventArgs e)
		{
			this.flowParameters.Controls.Remove((Control)((LinkLabel)sender).Parent);

			this.ResizeWindow();
		}

		public IEnumerable<KeyValuePair<string, string>> GetParameters()
		{
			foreach (Control control in flowParameters.Controls)
			{
				string Key = string.Empty;
				string Value = string.Empty;

				foreach (Control subcontrol in control.Controls)
				{
					if (subcontrol is TextBox && (string)subcontrol.Tag == "Name")
						Key = ((TextBox)subcontrol).Text.Trim();
					else if (subcontrol is TextBox && (string)subcontrol.Tag == "Value")
						Value = ((TextBox)subcontrol).Text.Trim();
				}

				yield return new KeyValuePair<string, string>(Key, Value);
			}
		}

		private void inType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.inType.SelectedItem == null)
				return;

			this.ClearParameters();

			Type type =  (Type)this.inType.SelectedItem;

			var mapevent = (IMapEvent)Activator.CreateInstance(type);

			var parameters = mapevent.GetParameterNames();
			if (parameters.Count() > 0)
			{
				foreach (String param in mapevent.GetParameterNames())
					this.AddParameter(param, string.Empty, true);
			}
			else
			{
				this.AddParameter(string.Empty, string.Empty, false);
			}

			this.ResizeWindow();

		}

		private void ClearParameters()
		{
			this.flowParameters.Controls.Clear();
		}
    }
}
