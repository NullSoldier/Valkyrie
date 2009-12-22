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

namespace Valkyrie
{
	public class StreamValueWriter
		: IValueWriter
	{
		public StreamValueWriter (Stream baseStream)
		{
			this.baseStream = baseStream;
		}

		#region IValueWriter Members
		public void WriteBytes (byte[] value)
		{
			WriteInt32 (value.Length);
			Write (value);
		}

		public void WriteSByte (sbyte value)
		{
			baseStream.WriteByte ((byte)value);
		}

		public void WriteInt16 (short value)
		{
			Write (BitConverter.GetBytes (value));
		}

		public void WriteInt32 (int value)
		{
			Write (BitConverter.GetBytes (value));
		}

		public void WriteInt64 (long value)
		{
			Write (BitConverter.GetBytes (value));
		}

		public void WriteByte (byte value)
		{
			baseStream.WriteByte (value);
		}

		public void WriteUInt16 (ushort value)
		{
			Write (BitConverter.GetBytes (value));
		}

		public void WriteUInt32 (uint value)
		{
			Write (BitConverter.GetBytes (value));
		}

		public void WriteUInt64 (ulong value)
		{
			Write (BitConverter.GetBytes (value));
		}

		public void WriteString (string value)
		{
			value = (value ?? String.Empty) + '\0';
			Write (Encoding.UTF8.GetBytes (value));
		}

		public void Flush()
		{
			baseStream.Flush();
		}

		#endregion

		private readonly Stream baseStream;

		private void Write (byte[] buffer)
		{
			baseStream.Write (buffer, 0, buffer.Length);
		}

		#region IDisposable Members

		public void Dispose ()
		{
			if (baseStream != null)
				baseStream.Close();
		}

		#endregion
	}
}