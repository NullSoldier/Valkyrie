// Copyright (c) 2009, Eric Maupin
// All rights reserved.

// Redistribution and use in source and binary forms, with
// or without modification, are permitted provided that
// the following conditions are met:

// - Redistributions of source code must retain the above 
//   copyright notice, this list of conditions and the
//   following disclaimer.

// - Redistributions in binary form must reproduce the above
//   copyright notice, this list of conditions and the
//   following disclaimer in the documentation and/or other
//   materials provided with the distribution.

// - Neither the name of Gablarski nor the names of its
//   contributors may be used to endorse or promote products
//   derived from this software without specific prior
//   written permission.

// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS
// AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
// WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
// INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH
// DAMAGE.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Cadenza.Collections;
using Valkyrie;
using Valkyrie.Messages;
using Cadenza.Collections;
using Cadenza;

namespace Valkyrie.Network
{
	public class NetworkServerConnectionProvider
		: IConnectionProvider
	{
		public event EventHandler<ConnectionlessMessageReceivedEventArgs> ConnectionlessMessageReceived;
		public event EventHandler<ConnectionEventArgs> ConnectionMade;

		public int Port
		{
			get { return this.port; }
			set { this.port = value; }
		}

		public void SendConnectionlessMessage (MessageBase message, EndPoint endpoint)
		{
			throw new NotImplementedException ();
		}

		public void StartListening ()
		{
			this.listening = true;

			var localEp = new IPEndPoint (IPAddress.Any, port);

			udp = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			udp.EnableBroadcast = true;
			udp.Bind (localEp);

			tcpListener = new TcpListener (localEp);
			tcpListener.Start ();
			tcpListener.BeginAcceptSocket (AcceptConnection, null);

			(incomingThread = new Thread (Incoming)
			{
				Name = "Incoming",
				IsBackground = true
			}).Start ();

			(outgoingThread = new Thread (Outgoing)
			{
				Name = "Outgoing",
				IsBackground = true
			}).Start ();
		}

		public void StopListening ()
		{
			this.listening = false;

			tcpListener.Stop ();
			udp.Close ();

			if (this.outgoingThread != null)
			{
				if (outgoingWait != null)
					outgoingWait.Set ();

				this.outgoingThread.Join ();
				this.outgoingThread = null;
			}

			if (this.incomingThread != null)
			{
				this.incomingThread.Join();
				this.incomingThread = null;
			}

			udp = null;
			tcpListener = null;
		}

		internal void EnqueueToSend (IConnection connection, MessageBase message)
		{
			var sc = connection as NetworkServerConnection;

			lock (outgoing)
			{
				outgoing.Enqueue (new KeyValuePair<NetworkServerConnection, MessageBase> (sc, message));
			}

			outgoingWait.Set ();
		}

		internal void Disconnect (NetworkServerConnection connection)
		{
			lock (connections)
			{
				connections.Remove (connection.NetworkId);
			}
		}

		private volatile bool listening;
		private int port = 6112;
		private Socket udp;
		private SocketValueWriter clWriter;
		private TcpListener tcpListener;
		private readonly Dictionary<uint, NetworkServerConnection> connections = new Dictionary<uint, NetworkServerConnection> (20);

		private readonly AutoResetEvent outgoingWait = new AutoResetEvent (false);
		private readonly Queue<KeyValuePair<NetworkServerConnection, MessageBase>> outgoing = new Queue<KeyValuePair<NetworkServerConnection, MessageBase>> (100);

		private Thread outgoingThread;
		private Thread incomingThread;		

		private void Incoming ()
		{
			var cs = this.connections;
			var tl = this.tcpListener;

			byte[] buffer = new byte[5120];
			var reader = new ByteArrayValueReader (buffer);

			var ipEndPoint = new IPEndPoint (IPAddress.Any, 0);
			var tendpoint = (EndPoint)ipEndPoint;

			while (this.listening)
			{
				try
				{
					if (udp == null || udp.ReceiveFrom (buffer, ref tendpoint) == 0)
					{
						Trace.WriteLine ("[Network] UDP ReceiveFrom returned nothing");
						return;
					}

					if (buffer[0] != 0x2A)
					{
						if (buffer[0] == 24)
							udp.SendTo (new byte[] { 24, 24 }, tendpoint);

						continue;
					}

					reader.Position = 1;

					uint nid = reader.ReadUInt32 ();

					NetworkServerConnection connection;
					lock (connections)
					{
						connections.TryGetValue (nid, out connection);
					}

					ushort mtype = reader.ReadUInt16 ();

					MessageBase msg;
					if (!MessageBase.GetMessage (mtype, out msg))
					{
						Trace.WriteLine ("[Network] Message type " + mtype + " not found from " + connection);
						return;
					}
					else
					{
						msg.ReadPayload (reader);

						if (connection == null)
						{
							//Trace.WriteLineIf (VerboseTracing, "[Network] Connectionless message received: " + msg.MessageTypeCode);
							//OnConnectionlessMessageReceived (new ConnectionlessMessageReceivedEventArgs (this, msg, endpoint));
						}
						else
						{
							//Trace.WriteLine ("[Network] Unreliable message received: " + msg.MessageTypeCode);
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
			}
		}

		private void Outgoing ()
		{
			var o = this.outgoing;
			var ow = this.outgoingWait;

			while (this.listening)
			{
				if (o.Count == 0)
					ow.WaitOne ();

				while (o.Count > 0)
				{
					KeyValuePair<NetworkServerConnection, MessageBase> m;
					lock (o)
					{
						m = o.Dequeue ();
					}

					Send (m.Key, m.Value);
				}
			}
		}

		private void AcceptConnection (IAsyncResult result)
		{
			try
			{
				TcpClient client = tcpListener.EndAcceptTcpClient (result);
				client.NoDelay = true;

				var stream = client.GetStream ();
				var tendpoint = (IPEndPoint)client.Client.RemoteEndPoint;
				Trace.WriteLine ("[Server] Accepted TCP Connection from " + tendpoint);

				uint nid = 0;
				NetworkServerConnection connection;
				lock (connections)
				{
					while (connections.ContainsKey (++nid)) ;

					connection = new NetworkServerConnection (this, nid, client, tendpoint, new SocketValueWriter (this.udp, tendpoint));
					connections.Add (nid, connection);
				}

				stream.Write (BitConverter.GetBytes (nid), 0, sizeof (uint));

				OnConnectionMade (new ConnectionEventArgs (connection));

				byte[] buffer = new byte[1];
				client.GetStream ().BeginRead (buffer, 0, 1, ReliableReceive, new Tuple<NetworkServerConnection, byte[]> (connection, buffer));

				tcpListener.BeginAcceptTcpClient (AcceptConnection, null);
			}
			catch (SocketException sex)
			{
				Trace.WriteLine ("[Server] Failed to accept connection: " + sex.Message);
			}
			catch (ObjectDisposedException)
			{
			}
		}

		private void ReliableReceive (IAsyncResult result)
		{
			NetworkServerConnection connection = null;

			try
			{
				var state = (Tuple<NetworkServerConnection, byte[]>)result.AsyncState;
				connection = state.Item1;
				var stream = connection.ReliableStream;

				if (stream.EndRead (result) == 0)
				{
					connection.Disconnect ();
					return;
				}

				if (state.Item2[0] == 0x2A)
				{
					ushort type = connection.ReliableReader.ReadUInt16 ();
					MessageBase msg;
					if (MessageBase.GetMessage (type, out msg))
					{
						msg.ReadPayload (connection.ReliableReader);

						connection.Receive (msg);
					}
				}

				if (connection.IsConnected)
					stream.BeginRead (state.Item2, 0, 1, ReliableReceive, state);
				else
					connection.Disconnect ();
			}
			catch (Exception)
			{
				connection.Disconnect ();
			}
		}

		private void Send (NetworkServerConnection connection, MessageBase message)
		{
			try
			{
				IValueWriter iwriter;
				if (connection != null)
					iwriter = (!message.Reliable) ? connection.UnreliableWriter : connection.ReliableWriter;
				else
					iwriter = clWriter;

				iwriter.WriteByte (42);
				iwriter.WriteUInt16 (message.MessageTypeCode);

				message.WritePayload (iwriter);
				iwriter.Flush ();
			}
			catch (Exception)
			{
				connection.Disconnect ();
			}
		}

		private void OnConnectionMade (ConnectionEventArgs e)
		{
			var connection = this.ConnectionMade;
			if (connection != null)
				connection (this, e);
		}
	}
}