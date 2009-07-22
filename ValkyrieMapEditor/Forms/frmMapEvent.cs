using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ValkyrieLibrary.Events;

namespace ValkyrieMapEditor.Forms
{
    public partial class frmMapEvent : Form
    {
		public Event Event
		{
			get { return this.pevent; }
			set { this.pevent = value; }
		}

		private Event pevent;

		public frmMapEvent() { }

        public frmMapEvent(Event e)
        {
            InitializeComponent();
        }

		private void frmMapEvent_Load(object sender, EventArgs e)
		{
			this.DisplayEvent();
		}

        public void DisplayEvent()
        {
            inType.Text = this.Event.Type;
            inDirection.Text = this.Event.Dir;
        }
    }
}
