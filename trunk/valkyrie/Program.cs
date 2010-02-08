using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace Valkyrie
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
			//if(!Debugger.IsAttached)
				AppDomain.CurrentDomain.UnhandledException += Program.CurrentDomain_UnhandledException;

            using (PokeGame game = new PokeGame())
            {
                game.Run();
            }
        }

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs ev)
		{
			try
			{
				string[] array = new string[] { ((Exception) ev.ExceptionObject).Message, ((Exception) ev.ExceptionObject).StackTrace };

				StringBuilder builder = new StringBuilder ("http://www.pokeworldonline.com/?v=exception&salt=FjvJxY6S9G7M&exception=");
				builder.Append (JsonConvert.SerializeObject (array));

				var request = WebRequest.Create (builder.ToString ());
				request.GetResponse ();

				MessageBox (new IntPtr (0), ((Exception) ev.ExceptionObject).Message + Environment.NewLine + Environment.NewLine + ((Exception) ev.ExceptionObject).StackTrace, "Error!", 0);
			}
			catch { }
		}
    }
}

