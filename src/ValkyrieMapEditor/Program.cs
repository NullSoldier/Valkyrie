using System;
using System.Diagnostics;
using System.Windows.Forms;
using ValkyrieMapEditor.Core;
using ValkyrieMapEditor.Core.Actions;

namespace ValkyrieMapEditor
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		///
		public static SettingsManager Settings = new SettingsManager();

		[STAThreadAttribute]
		static void Main(string[] args)
		{
			if(!Debugger.IsAttached)
				AppDomain.CurrentDomain.UnhandledException += Program.Program_UnhandledException;

			Program.Settings.Initialize();
			

			frmMain form = new frmMain();

			MapEditorManager.GameInstance = new EditorXNA(form.getDrawSurface(), form.getDrawTilesSurface());
			MapEditorManager.GameInstance.EnlistEvents(form.pctSurface);

            form.ScreenResized += MapEditorManager.GameInstance.Resized;
			form.ScrolledMap += MapEditorManager.GameInstance.Scrolled;
			form.Show();

			MapEditorManager.GameInstance.Run();
		}

		static void Program_UnhandledException(object sender, UnhandledExceptionEventArgs ev)
		{
			MessageBox.Show(((Exception)ev.ExceptionObject).Message + Environment.NewLine + Environment.NewLine + ((Exception)ev.ExceptionObject).StackTrace, "Error!", MessageBoxButtons.OK);
		}

	}
}

