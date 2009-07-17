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
			

			using (Game1 game = new Game1(form.getDrawSurface(), form.getDrawTilesSurface()))
			{
				form.ScreenResized += game.SurfaceSizeChanged;
                form.SurfaceClicked += game.SurfaceClicked;
				form.ScrolledMap += game.ScrolledMap;
                form.Show();
				game.Run();
			}
		}

	}
}

