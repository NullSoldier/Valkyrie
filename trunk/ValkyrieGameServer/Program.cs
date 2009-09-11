using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieServerLibrary.Core;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ValkyrieGameServerConsole
{
	class Program
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

		private static ValkyrieGameServer server;
		private static bool Running = true;
		private static Thread ServerThread;

		static void Main (string[] args)
		{
			if(!Debugger.IsAttached)
				AppDomain.CurrentDomain.UnhandledException += Program.Program_UnhandledException;

			Program.ServerThread = new Thread(Program.StartServerA);
			Program.ServerThread.Name = "Server Thread";
			Program.ServerThread.IsBackground = true;
			Program.ServerThread.Start();

			AppDomain.CurrentDomain.ProcessExit += Program.CurrentDomain_ProcessExiting;

			while (Program.Running)
			{
				Thread.Sleep(10);
			}
		}

		public static void CurrentDomain_ProcessExiting(object sender, EventArgs ev)
		{
			if(server != null)
				server.Stop();

			Program.ServerThread.Abort();

			Program.Running = false;
		}

		public static void StartServerA()
		{
			Program.server = new ValkyrieGameServer(new DefaultGameServerSettings());
			Program.server.Start();
			Console.WriteLine("Server started successfully.");
		}

		private static void Program_UnhandledException(object sender, UnhandledExceptionEventArgs ev)
		{
			MessageBox(new IntPtr(0), ((Exception)ev.ExceptionObject).Message + Environment.NewLine + Environment.NewLine + ((Exception)ev.ExceptionObject).StackTrace, "Error!", 0);
		}
	}
}
