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
				form.ScreenResized += game.SurfaceSizeChanged;
                form.SurfaceClicked += game.SurfaceClicked;
				form.ScrolledMap += game.ScrolledMap;

                form.pctSurface.MouseDown += new System.Windows.Forms.MouseEventHandler(game.MouseDown);
                form.pctSurface.MouseUp += new System.Windows.Forms.MouseEventHandler(game.MouseUp);
                form.pctSurface.MouseMove += new System.Windows.Forms.MouseEventHandler(game.MouseMove);
                form.pctSurface.MouseClick += new System.Windows.Forms.MouseEventHandler(game.MouseClicked);

                form.Show();
				game.Run();
			}
		}

	}
}

