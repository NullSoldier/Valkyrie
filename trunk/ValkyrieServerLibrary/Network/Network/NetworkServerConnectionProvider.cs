using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Gablarski.Messages;

namespace Gablarski.Network
{
	public class NetworkServerConnectionProvider
		: IConnectionProvider
	{
		public int Port
		{
			get { return this.port; }
			set { this.port = value; }
		}

		#region Implementation of IConnectionProvider

		/// <summary>
		/// A connectionless message was received.
		/// </summary>
		public event EventHandler<MessageReceivedEventArgs> ConnectionlessMessageReceived;

		/// <summary>
		/// A connection was made.
		/// </summary>
		public event EventHandler<ConnectionEventArgs> ConnectionMade;

		/// <summary>
		/// Starts listening for connections and connectionless messages.
		/// </summary>
		public void StartListening()
		{
			this.listening = true;
			
			Trace.WriteLineIf (VerboseTracing, "[Network] Listening on port " + port);

			var localEP = new IPEndPoint (IPAddress.Any, port);

			udp = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			udp.Bind (localEP);

			tcpListener = new TcpListener (localEP);
			tcpListener.Start ();

			this.listenerThread = new Thread (this.Listener) { Name = "Network Provider Listener" };
			this.listenerThread.Start();
		}

		/// <summary>
		/// Stops listening for connections and connectionless messages.
		/// </summary>
		public void StopListening()
		{
			this.listening = false;

			Trace.WriteLineIf (VerboseTracing, "[Network] Stopped listening to port " + port);

			udp.Close();
			tcpListener.Stop();

			if (this.listenerThread != null)
			{
				this.listenerThread.Join();
				this.listenerThread = null;
			}

			udp = null;
			tcpListener = null;
		}

		/// <summary>
		/// Whether to output detailed tracing.
		/// </summary>
		public bool VerboseTracing
		{
			get; set;
		}
		#endregion

		private Thread listenerThread;
		private volatile bool waiting;
		private volatile bool listening;
		private TcpListener tcpListener;
		private Socket udp;
		private int port = 6112;
		private volatile bool accepting;

		private readonly Dictionary<uint, NetworkServerConnection> connections = new Dictionary<uint, NetworkServerConnection>();

		private void UnreliableReceive (IAsyncResult result)
		{
			try
			{
				var ipendpoint = new IPEndPoint (IPAddress.Any, 0);
				var endpoint = (EndPoint)ipendpoint;
				if (udp.EndReceiveFrom (result, ref endpoint) == 0)
				{
					Trace.WriteLineIf (VerboseTracing, "[Network] UDP EndReceiveFrom returned nothing.");
					return;
				}

				byte[] buffer = (byte[])result.AsyncState;

				if (buffer[0] != 0x2A)
				{
					if (buffer[0] == 24)
						udp.SendTo (new byte[] { 24, 24 }, endpoint);

					Trace.WriteLineIf (VerboseTracing, "[Network] Unreliable message failed sanity check.");
					return;
				}

				IValueReader reader = new ByteArrayValueReader (buffer, 1);

				uint nid = reader.ReadUInt32();

				NetworkServerConnection connection;
				lock (connections)
				{
					connections.TryGetValue (nid, out connection);
				}

				ushort mtype = reader.ReadUInt16();

				MessageBase msg;
				if (!MessageBase.GetMessage (mtype, out msg))
				{
					Trace.WriteLineIf (VerboseTracing, "[Network] Message type " + mtype + " not found from " + connection);
					return;
				}
				else
				{
					msg.ReadPayload (reader);

					if (connection == null)
					{
						Trace.WriteLineIf (VerboseTracing, "[Network] Connectionless message received: " + msg.MessageTypeCode);
						OnConnectionlessMessageReceived (new MessageReceivedEventArgs (null, msg));
					}
					else
					{
						Trace.WriteLineIf (VerboseTracing, "[Network] Unreliable message received: " + msg.MessageTypeCode);
						connection.Receive (msg);
					}
				}
			}
			catch (SocketException)
			{
			}
			catch (ObjectDisposedException)
			{
			}
			finally
			{
				this.waiting = false;
			}
		}

		protected void OnConnectionlessMessageReceived (MessageReceivedEventArgs e)
		{
			var received = this.ConnectionlessMessageReceived;
			if (received != null)
				received (this, e);
		}

		private void AcceptConnection (object result)
		{
			try
			{
				var listener = (result as TcpListener);
				#if DEBUG
				if (listener == null)
					throw new ArgumentException ("result");
				#endif

				TcpClient client = listener.AcceptTcpClient();
				client.NoDelay = true;
				
				var stream = client.GetStream();
				var tendpoint = (IPEndPoint)client.Client.RemoteEndPoint;
				Trace.WriteLine ("[Server] Accepted TCP Connection from " + tendpoint);

				uint nid = 0;
				NetworkServerConnection connection;
				lock (connections)
				{
					while (connections.ContainsKey (++nid)) ;

					connection = new NetworkServerConnection (nid, tendpoint, client, new SocketValueWriter (this.udp, tendpoint));
					connections.Add (nid, connection);
				}

				stream.Write (BitConverter.GetBytes (nid), 0, sizeof (uint));		

				OnConnectionMade (new ConnectionEventArgs (connection));
			}
			catch (SocketException sex)
			{
				Trace.WriteLine ("[Server] Failed to accept connection: " + sex.Message);
			}
			finally
			{
				this.accepting = false;
			}
		}

		protected void OnConnectionMade (ConnectionEventArgs e)
		{
			var connection = this.ConnectionMade;
			if (connection != null)
				connection (this, e);
		}

		private void Listener()
		{
			const uint maxLoops = UInt32.MaxValue;
			uint loops = 0;
			bool singleCore = (Environment.ProcessorCount == 1);

			while (this.listening)
			{
				if (!this.waiting && udp.Available > 3)
				{
					this.waiting = true;
					var ipEndpoint = new IPEndPoint (IPAddress.Any, 0);
					var tendpoint = (EndPoint)ipEndpoint;
					byte[] buffer = new byte[5120];

					try
					{
						udp.BeginReceiveFrom (buffer, 0, 5120, SocketFlags.None, ref tendpoint, UnreliableReceive, buffer);
					}
					catch (SocketException)
					{
					}
				}

				if (!this.accepting && tcpListener.Pending())
				{
					this.accepting = true;
					ThreadPool.QueueUserWorkItem (AcceptConnection, tcpListener);
				}

				if (singleCore || (++loops % 100) == 0)
					Thread.Sleep (1);
				else
					Thread.SpinWait (20);

				if (loops == maxLoops)
					loops = 0;
			}
		}
	}
}