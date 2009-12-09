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

namespace Gablarski
{
	public static class ByteArrayExtensions
	{
		public static Int16 ReadInt16 (this byte[] self, ref int position)
		{
			Int16 value = BitConverter.ToInt16 (self, position);
			position += sizeof(Int16);

			return value;
		}

		public static Int32 ReadInt32 (this byte[] self, ref int position)
		{
			Int32 value = BitConverter.ToInt32 (self, position);
			position += sizeof(Int32);

			return value;
		}

		public static Int64 ReadInt64 (this byte[] self, ref int position)
		{
			Int64 value = BitConverter.ToInt16 (self, position);
			position += sizeof(Int64);

			return value;
		}

		public static UInt16 ReadUInt16 (this byte[] self, ref int position)
		{
			UInt16 value = BitConverter.ToUInt16 (self, position);
			position += sizeof(UInt16);

			return value;
		}

		public static UInt32 ReadUInt32 (this byte[] self, ref int position)
		{
			UInt32 value = BitConverter.ToUInt32 (self, position);
			position += sizeof(UInt32);

			return value;
		}

		public static UInt64 ReadUInt64 (this byte[] self, ref int position)
		{
			UInt64 value = BitConverter.ToUInt16 (self, position);
			position += sizeof(UInt64);

			return value;
		}
	}
}