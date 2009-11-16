using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ValkyrieMapEditor
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		///

		[STAThreadAttribute]
		static void Main(string[] args)
		{
			frmMain form = new frmMain();

			if (!Debugger.IsAttached)
				AppDomain.CurrentDomain.UnhandledException += Program.Program_UnhandledException;

			using (EditorXNA game = new EditorXNA(form.getDrawSurface(), form.getDrawTilesSurface()))
			{
                form.ScreenResized += game.Resized;
                form.ScrolledMap += game.Scrolled;

                game.EnlistEvents(form.pctSurface);

                MapEditorManager.GameInstance = game;

                form.Show();
				game.Run(); 
			}
		}

		static void Program_UnhandledException(object sender, UnhandledExceptionEventArgs ev)
		{
			MessageBox.Show(((Exception)ev.ExceptionObject).Message + Environment.NewLine + Environment.NewLine + ((Exception)ev.ExceptionObject).StackTrace, "Error!", MessageBoxButtons.OK);
		}

	}
}

