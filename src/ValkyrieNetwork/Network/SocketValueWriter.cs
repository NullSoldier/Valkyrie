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
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Valkyrie.Network
{
	public class SocketValueWriter
		: IValueWriter
	{
		public SocketValueWriter (Socket uclient)
			: this (uclient, 1280)
		{
		}

		public SocketValueWriter (Socket uclient, int length)
		{
			this.client = uclient;
			this.EnsureCapacity (length);
		}

		public SocketValueWriter (Socket uclient, EndPoint sendTo)
			: this (uclient)
		{
			this.endpoint = sendTo;
		}

		public EndPoint EndPoint
		{
			get { return this.endpoint; }
			set { this.endpoint = value; }
		}

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
		}

		#endregion

		#region Implementation of IValueWriter

		public void WriteBytes (byte[] value)
		{
			WriteInt32 (value.Length);

			EnsureCapacity (this.size + value.Length);
			Array.Copy (value, 0, this.buffer, this.size, value.Length);
			this.size += value.Length;
		}

		public void WriteSByte (sbyte value)
		{
			EnsureCapacity (this.size + sizeof(sbyte));
			this.buffer[this.size++] = (byte)value;
		}

		public void WriteInt16 (short value)
		{
			EnsureCapacity (this.size + sizeof (Int16));
			
			Array.Copy (BitConverter.GetBytes (value), 0, this.buffer, this.size, sizeof (Int16));
			this.size += sizeof (Int16);
		}

		public void WriteInt32 (int value)
		{
			EnsureCapacity (this.size + sizeof (Int32));
			
			Array.Copy (BitConverter.GetBytes (value), 0, this.buffer, this.size, sizeof (Int32));
			this.size += sizeof (Int32);
		}

		public void WriteInt64 (long value)
		{
			EnsureCapacity (this.size + sizeof (Int32));
			
			Array.Copy (BitConverter.GetBytes (value), 0, this.buffer, this.size, sizeof (Int32));
			this.size += sizeof (Int32);
		}

		public void WriteByte (byte value)
		{
			EnsureCapacity (this.size + 1);
			this.buffer[this.size++] = value;
		}

		public void WriteUInt16(ushort value)
		{
			EnsureCapacity (this.size + sizeof (Int16));
			
			Array.Copy (BitConverter.GetBytes (value), 0, this.buffer, this.size, sizeof (Int16));
			this.size += sizeof (Int16);
		}

		public void WriteUInt32(uint value)
		{
			EnsureCapacity (this.size + sizeof (Int32));
			
			Array.Copy (BitConverter.GetBytes (value), 0, this.buffer, this.size, sizeof (Int32));
			this.size += sizeof (Int32);
		}

		public void WriteUInt64(ulong value)
		{
			EnsureCapacity (this.size + sizeof (Int64));
			
			Array.Copy (BitConverter.GetBytes (value), 0, this.buffer, this.size, sizeof (Int64));
			this.size += sizeof (Int64);
		}

		public void WriteString (string value)
		{
			value = (value ?? String.Empty) + '\0';
			byte[] str = this.encoding.GetBytes (value);
			EnsureCapacity (this.size + str.Length);

			Array.Copy (str, 0, this.buffer, this.size, str.Length);
			this.size += str.Length;
		}

		public void Flush()
		{
			try
			{
				lock (client)
				{
					client.SendTo (this.buffer, this.size, SocketFlags.None, this.endpoint);
				}
			}
			catch (SocketException)
			{
			}
			finally
			{
				this.size = 0;
			}
		}

		#endregion

		private readonly Socket client;
		private readonly Encoding encoding = Encoding.UTF8;
		private byte[] buffer;
		private int size;
		private EndPoint endpoint;

		private void EnsureCapacity (int min)
		{
			byte[] tbuffer = this.buffer;
			if (tbuffer == null)
			{
				this.buffer = new byte[min];
				return;
			}

			if (tbuffer.Length >= min)
				return;

			int nlen = tbuffer.Length*2;
			if (nlen < min)
				nlen = min;

			byte[] nbuffer = new byte[nlen];
			if (this.size > 0)
				Array.Copy (tbuffer, 0, nbuffer, 0, this.size);

			this.buffer = nbuffer;
		}
	}
}