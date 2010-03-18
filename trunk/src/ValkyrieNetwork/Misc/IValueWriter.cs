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
	public interface IValueWriter
		: IDisposable
	{
		void WriteBytes (byte[] value);

		void WriteSByte (SByte value);
		void WriteInt16 (Int16 value);
		void WriteInt32 (Int32 value);
		void WriteInt64 (Int64 value);
		
		void WriteByte (Byte value);
		void WriteUInt16 (UInt16 value);
		void WriteUInt32 (UInt32 value);
		void WriteUInt64 (UInt64 value);

		void WriteString (string value);

		void Flush();
	}

	public static class ValueWriterExtensions
	{
		public static void WriteBool (this IValueWriter writer, bool value)
		{
			writer.WriteByte ((byte)((value) ? 1 : 0));
		}

		public static void WriteVersion (this IValueWriter writer, Version version)
		{
			writer.WriteInt32 (version.Major);
			writer.WriteInt32 (version.Minor);
			writer.WriteInt32 (version.Build);
			writer.WriteInt32 (version.Revision);
		}

		public static void WriteType (this IValueWriter writer, Type value)
		{
			writer.WriteString (value.AssemblyQualifiedName);
		}

		public static void WriteGenericResult (this IValueWriter writer, GenericResult result)
		{
			writer.WriteByte ((byte)result);
		}
	}
}