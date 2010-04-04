using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ValkyrieWorldEditor.Forms
{
    public partial class frmNewWorld : Form
    {
        public frmNewWorld()
        {
            InitializeComponent();
        }

        public String GetString()
        {
            return this.textBox1.Text;
        }
    }
}
