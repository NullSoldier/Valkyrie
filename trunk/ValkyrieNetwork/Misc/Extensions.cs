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
using System.Reflection.Emit;
using System.Text;
using System.IO;

namespace Gablarski
{
	public static class Extensions
	{
		public static byte[] ReadBytes (this Stream stream, int size)
		{
			byte[] buffer = new byte[size];

			int i = 0;
			int bytes = 0;
			while (i < buffer.Length && (bytes = stream.Read (buffer, i, size)) > 0)
			{
				i += bytes;
				size -= bytes;
			}

			return buffer;
		}

		public static T Trim<T> (this T self, T maximum)
			where T : struct, IComparable<T>
		{
			return (self.CompareTo (maximum) >= 1) ? maximum : self;
		}

		public static T Trim<T> (this T self, T minimum, T maximum)
			where T : struct, IComparable<T>
		{
			if (self.CompareTo (maximum) >= 1)
				return maximum;
			else if (self.CompareTo (minimum) <= -1)
				return minimum;
			else
				return self;
		}

		private static readonly Dictionary<Type, object> CachedValues = new Dictionary<Type, object>();
		public static object GetDefaultValue (this Type self)
        {
			if (!self.IsValueType)
				return null;

			lock (CachedValues)
			{
				if (!CachedValues.ContainsKey (self))
					CachedValues.Add (self, Activator.CreateInstance (self));

				return CachedValues[self];
			}
        }
	}
}