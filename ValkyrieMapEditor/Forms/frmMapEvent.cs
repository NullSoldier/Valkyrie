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
        public frmMapEvent()
        {
            InitializeComponent();
        }

        public void LoadEvent(Event e)
        {
            //this.tbArgOne.Text = e.ParmOne;
            //this.tbArgTwo.Text = e.ParmTwo;

            cbType.Text = e.Type;
            cbDir.Text = e.Dir;
        }
    }
}
