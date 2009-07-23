using System;
using System.Windows.Forms;

using ValkyrieWorldEditor.Forms;
using ValkyrieWorldEditor;
using ValkyrieWorldEditor.Core;

namespace ValkyrieWorldEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThreadAttribute]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            frmMain main = new frmMain();
            WorldEditor.MainForm = main;

            Application.Run(main);
        }
    }
}

