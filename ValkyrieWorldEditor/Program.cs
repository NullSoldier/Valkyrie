using System;

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
			frmMain form = new frmMain();

            using (EditorXNA game = new EditorXNA(form.getDrawSurface(), form.getPreviewSurface()))
			{
                //form.ScreenResized += game.Resized;
                //form.ScrolledMap += game.Scrolled;

                //game.EnlistEvents(form.pctSurface);

                //MapEditorManager.GameInstance = game;

                WorldEditor.Game = game;
                WorldEditor.MainForm = form;

                form.Show();
				game.Run(); 
			} 
        }
    }
}

