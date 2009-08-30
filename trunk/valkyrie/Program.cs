using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ValkyrieLibrary
{
    static class Program
    {
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
			if(!Debugger.IsAttached)
				AppDomain.CurrentDomain.UnhandledException += Program.CurrentDomain_UnhandledException;

            using (PokeGame game = new PokeGame())
            {
                game.Run();
            }
        }

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs ev)
		{
			MessageBox(new IntPtr(0), ((Exception)ev.ExceptionObject).Message + Environment.NewLine + Environment.NewLine + ((Exception)ev.ExceptionObject).StackTrace, "Error!", 0);
		}
    }
}

