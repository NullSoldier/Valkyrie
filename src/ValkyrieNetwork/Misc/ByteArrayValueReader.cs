// Copyright (c) 2009, Eric Maupin
// All rights reserved.
//
// Redistribution and use in source and binary forms, with
// or without modification, are permitted provided that
// the following conditions are met:
//
// - Redistributions of source code must retain the above 
//   copyright notice, this list of conditions and the
//   following disclaimer.
//
// - Redistributions in binary form must reproduce the above
//   copyright notice, this list of conditions and the
//   following disclaimer in the documentation and/or other
//   materials provided with the distribution.
//
// - Neither the name of Gablarski nor the names of its
//   contributors may be used to endorse or promote products
//   or services derived from this software without specific
//   prior written permission.
//
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
using System.Text;

namespace Valkyrie
{
	public class ByteArrayValueReader
		: IValueReader
	{
		public ByteArrayValueReader (byte[] data)
		{
			if (data == null)
				throw new ArgumentNullException ("data");

			this.array = data;
		}

		public ByteArrayValueReader (byte[] data, int position)
			: this (data)
		{
			if (position < 0)
				throw new ArgumentOutOfRangeException ("position");

			this.position = position;
		}

		public int Position
		{
			get { return this.position; }
			set { this.position = value; }
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

		#region Implementation of IValueReader

		public byte[] ReadBytes()
		{
			int len = this.array.ReadInt32 (ref this.position);

			byte[] value = new byte[len];
			Array.Copy (this.array, this.position, value, 0, len);

			return value;
		}

		public sbyte ReadSByte()
		{
			return (sbyte)this.array[this.position++];
		}

		public short ReadInt16()
		{
			return this.array.ReadInt16 (ref this.position);
		}

		public int ReadInt32()
		{
			return this.array.ReadInt32 (ref this.position);
		}

		public long ReadInt64()
		{
			return this.array.ReadInt64 (ref this.position);
		}

		public byte ReadByte()
		{
			return this.array[this.position++];
		}

		public ushort ReadUInt16()
		{
			return this.array.ReadUInt16 (ref this.position);
		}

		public uint ReadUInt32()
		{
			return this.array.ReadUInt32 (ref this.position);
		}

		public ulong ReadUInt64()
		{
			return this.array.ReadUInt64 (ref this.position);
		}

		public string ReadString()
		{
			byte[] buffer = this.array;
			int pos = this.position;

			string str = String.Empty;
			while (true)
			{
				string tstr = Encoding.UTF8.GetString (buffer, pos++, 1);
				if (tstr == "\0")
					break;

				str += tstr;
			}

			this.position = pos;
			return str;
		}

		#endregion

		private readonly byte[] array;
		private int position;
	}
}