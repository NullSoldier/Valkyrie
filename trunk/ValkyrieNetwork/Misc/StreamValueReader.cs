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
using System.IO;

namespace Gablarski
{
	public class StreamValueReader
		: IValueReader
	{
		public StreamValueReader (Stream baseStream)
		{
			this.baseStream = baseStream;
		}

		#region IValueReader Members
		public byte[] ReadBytes ()
		{
			int length = ReadInt32 ();
			return baseStream.ReadBytes (length);
		}

		public sbyte ReadSByte ()
		{
			return (sbyte)this.baseStream.ReadByte ();
		}

		public short ReadInt16 ()
		{
			return BitConverter.ToInt16 (this.baseStream.ReadBytes (2), 0);
		}

		public int ReadInt32 ()
		{
			return BitConverter.ToInt32 (this.baseStream.ReadBytes (4), 0);
		}

		public long ReadInt64 ()
		{
			return BitConverter.ToInt64 (this.baseStream.ReadBytes (8), 0);
		}

		public byte ReadByte ()
		{
			return (byte)this.baseStream.ReadByte ();
		}

		public ushort ReadUInt16 ()
		{
			return BitConverter.ToUInt16 (this.baseStream.ReadBytes (2), 0);
		}

		public uint ReadUInt32 ()
		{
			return BitConverter.ToUInt32 (this.baseStream.ReadBytes (4), 0);
		}

		public ulong ReadUInt64 ()
		{
			return BitConverter.ToUInt64 (this.baseStream.ReadBytes (8), 0);
		}

		public string ReadString ()
		{
			byte[] buffer = new byte[1];
			string str = String.Empty;
			while (true)
			{
				if (this.baseStream.Read (buffer, 0, 1) == 0)
					break;

				string tstr = Encoding.UTF8.GetString (buffer);
				if (tstr == "\0")
					break;

				str += tstr;
			}

			return str;
		}

		#endregion

		private readonly Stream baseStream;

		#region IDisposable Members

		public void Dispose ()
		{
			if (baseStream != null)
				baseStream.Close();
		}

		#endregion
	}
}