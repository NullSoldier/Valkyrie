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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Gablarski.Audio;

namespace Gablarski.OpenAL
{
	public abstract class Device
		: IAudioDevice
	{
		protected Device (string deviceName)
		{
			this.Name = deviceName;
		}

		/// <summary>
		/// Gets the name of the device
		/// </summary>
		public string Name
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets whether the device is open or not
		/// </summary>
		public bool IsOpen
		{
			get { return (this.Handle != IntPtr.Zero); }
		}

		/// <summary>
		/// Gets the refresh rate of the device.
		/// </summary>
		public int Refresh
		{
			get
			{
				ThrowIfDisposed();

				int refresh;
				OpenAL.alcGetIntegerv (this.Handle, ALCEnum.ALC_REFRESH, 1, out refresh);
				return refresh;
			}
		}

		public override string ToString ()
		{
			return this.Name;
		}

		public static bool operator==(Device d1, Device d2)
		{
			bool oneNull = Object.ReferenceEquals (d1, null);
			bool twoNull = Object.ReferenceEquals (d2, null);
			if (oneNull && twoNull)
				return true;
			else if (!twoNull)
				return false;

			return d1.Equals (d2);
		}

		public static bool operator!=(Device d1, Device d2)
		{
			return !(d1 == d2);
		}

		public override bool Equals (object obj)
		{
			var d = (obj as Device);
			if (d == null)
				return false;

			return (d.Name == this.Name);
		}

		public override int GetHashCode ()
		{
			return this.Name.GetHashCode();
		}

		internal IntPtr Handle;
		protected bool disposed;

		#region Imports
		[DllImport ("OpenAL32.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr alcOpenDevice (string deviceName);

		[DllImport ("OpenAL32.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr alcCloseDevice (IntPtr handle);
		#endregion

		#region IDisposable Members

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected abstract void Dispose (bool disposing);

		~Device ()
		{
			this.Dispose (false);
		}

		[Conditional ("DEBUG")]
		protected void ThrowIfDisposed()
		{
			if (this.disposed)
				throw new ObjectDisposedException ("Device");
		}
		#endregion
	}
}