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
		private static ValkyrieGameServer server;
		private static bool Running = true;
		private static Thread ServerThread;

		static void Main (string[] args)
		{
			if(!Debugger.IsAttached)
				AppDomain.CurrentDomain.UnhandledException += Program.Program_UnhandledException;

			Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

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
			Program.server.UserLoggedIn += Server_UserLoggedIn;
			Program.server.UserLoggedOut += Server_UserLoggedOut;

			Program.server.Start();
			Console.WriteLine("Server started successfully.");
		}

		private static void Program_UnhandledException(object sender, UnhandledExceptionEventArgs ev)
		{
			Trace.WriteLine(Environment.NewLine + ((Exception)ev.ExceptionObject).Message + Environment.NewLine + Environment.NewLine + ((Exception)ev.ExceptionObject).StackTrace, "Error!");
		}

		public static void Server_UserLoggedIn (object sender, UserEventArgs ev)
		{
			if(ev.Player != null)
				Trace.WriteLine(string.Format("User {0} has logged in.", ev.Player.Character.Name));
			else
				Trace.WriteLine(string.Format("NULL User has logged in.", ev.Player.Character.Name));			
		}

		public static void Server_UserLoggedOut (object sender, UserEventArgs ev)
		{
			if(ev.Player != null)
				Trace.WriteLine(string.Format("User {0} has logged out.", ev.Player.Character.Name));
			else
				Trace.WriteLine(string.Format("NULL User has logged out.", ev.Player.Character.Name));
		}
	}
}
