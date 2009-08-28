using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Gablarski.Network
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