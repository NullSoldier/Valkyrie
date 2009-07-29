using System;

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

	}
}

