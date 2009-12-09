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
using System.IO;
using System.Net;
using System.Net.Sockets;
using Gablarski.Messages;

namespace Gablarski.Network
{
	public class NetworkServerConnection
		: IConnection
	{
		internal NetworkServerConnection (NetworkServerConnectionProvider provider, uint nid, TcpClient tcp, IPEndPoint endPoint, IValueWriter uwriter)
		{
			this.NetworkId = nid;
			this.Tcp = tcp;
			this.ReliableStream = tcp.GetStream ();
			this.provider = provider;
			this.EndPoint = endPoint;
			this.UnreliableWriter = uwriter;
			this.ReliableReader = new StreamValueReader (ReliableStream);
			this.ReliableWriter = new StreamValueWriter (ReliableStream);
		}

		public event EventHandler<MessageReceivedEventArgs> MessageReceived;
		public event EventHandler<ConnectionEventArgs> Disconnected;
		
		public bool IsConnected
		{
			get { return this.Tcp.Connected; }
		}

		public void Send (MessageBase message)
		{
			provider.EnqueueToSend (this, message);
		}

		public void Disconnect ()
		{
			this.Tcp.Close ();
			provider.Disconnect (this);

			OnDisconnected ();
		}

		internal readonly uint NetworkId;
		internal readonly TcpClient Tcp;
		internal readonly Stream ReliableStream;
		internal readonly IPEndPoint EndPoint;
		internal readonly IValueWriter UnreliableWriter;
		internal readonly IValueReader ReliableReader;
		internal readonly IValueWriter ReliableWriter;

		internal void Receive (MessageBase message)
		{
			var received = this.MessageReceived;
			if (received != null)
				received (this, new MessageReceivedEventArgs (this, message));
		}

		private readonly NetworkServerConnectionProvider provider;		

		private void OnDisconnected ()
		{
			var dced = this.Disconnected;
			if (dced != null)
				dced (this, new ConnectionEventArgs (this));
		}
	}
}