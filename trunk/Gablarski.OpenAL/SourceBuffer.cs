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
using System.Runtime.InteropServices;
using System.Security;

namespace Gablarski.OpenAL
{
	[SuppressUnmanagedCodeSecurity]
	public class SourceBuffer
		: IDisposable
	{
		internal SourceBuffer (uint bufferID)
		{
			this.bufferID = bufferID;
		}

		public void Buffer (byte[] data, AudioFormat format, uint frequency)
		{
			alBufferData (this.bufferID, format, data, data.Length, frequency);
			OpenAL.ErrorCheck ();
		}

		#region IDisposable Members
		private bool disposed;

		public void Dispose ()
		{
			this.Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected virtual void Dispose (bool disposing)
		{
			lock (lck)
			{
				if (this.disposed)
					return;

				alDeleteBuffers (1, new[] { this.bufferID });
				Buffers.Remove (this.bufferID);
				
				this.disposed = true;
			}
		}

		~SourceBuffer()
		{
			Dispose (false);
		}

		#endregion

		internal readonly uint bufferID;

		public static SourceBuffer Generate ()
		{
			return Generate (1)[0];
		}

		public static SourceBuffer[] Generate (int count)
		{			
			lock (lck)
			{
				if (Buffers == null)
					Buffers = new Dictionary<uint, SourceBuffer> (count);

				SourceBuffer[] buffers = new SourceBuffer[count];

				uint[] bufferIDs = new uint[count];
				alGenBuffers (count, bufferIDs);
				OpenAL.ErrorCheck ();

				for (int i = 0; i < count; ++i)
				{
					buffers[i] = new SourceBuffer (bufferIDs[i]);
					Buffers.Add (buffers[i].bufferID, buffers[i]);
				}

				return buffers;
			}
		}

		//public static void Delete (this IEnumerable<SourceBuffer> self)
		//{
		//    uint[] bufferIDs = self.Select (b => b.bufferID).ToArray ();
		//    alDeleteBuffers (bufferIDs.Length, bufferIDs);
		//    OpenAL.ErrorCheck ();
		//}

		#region Imports
		[DllImport ("OpenAL32.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void alGenBuffers (int count, uint[] bufferIDs);

		[DllImport ("OpenAL32.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void alBufferData (uint bufferID, AudioFormat format, byte[] data, int byteSize, uint frequency);

		[DllImport ("OpenAL32.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void alDeleteBuffers (int numBuffers, uint[] bufferIDs);
		#endregion

		private static object lck = new object ();

		private static Dictionary<uint, SourceBuffer> Buffers
		{
			get;
			set;
		}

		internal static SourceBuffer GetBuffer (uint bufferID)
		{
			lock (lck)
			{
				return Buffers[bufferID];
			}
		}
	}
}