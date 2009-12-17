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
using System.Threading;
using System.Security;
using Gablarski.OpenAL;

namespace Gablarski.OpenAL
{
	[SuppressUnmanagedCodeSecurity]
	public class CaptureDevice
		: Device
	{
		internal CaptureDevice (string deviceName)
			: base (deviceName)
		{
			//this.listenerThread = new Thread (this.SampleListener)
			//{
			//    Name = "OpenAL Sample Listener",
			//    IsBackground = true
			//};
		}

		/// <summary>
		/// Gets or sets the minimum samples needed before the <c>SamplesAvailable</c> event is triggered.
		/// </summary>
		public int MinimumSamples
		{
			get { return this.minimumSamples; }
			set { this.minimumSamples = value; }
		}

		/// <summary>
		/// Gets the current format of the capture device.
		/// </summary>
		public AudioFormat Format
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the current frequency of the capture device.
		/// </summary>
		public uint Frequency
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the number of available samples.
		/// </summary>
		public int AvailableSamples
		{
			get { return GetSamplesAvailable (); }
		}

		/// <summary>
		/// Opens the capture device with the specified <paramref name="frequency"/> and <paramref name="format"/>.
		/// </summary>
		/// <param name="frequency">The frequency to open the capture device with.</param>
		/// <param name="format">The audio format to open the device with.</param>
		/// <returns>Returns <c>this</c>.</returns>
		public unsafe CaptureDevice Open (uint frequency, AudioFormat format)
		{
			ThrowIfDisposed();

			this.Format = format;
			this.Frequency = frequency;

			uint bufferSize = format.GetBytes (format.GetSamplesPerSecond (frequency)) * 2;

			this.Handle = alcCaptureOpenDevice (this.Name, frequency, format, (int)bufferSize);
			OpenAL.ErrorCheck (this);

			pcm = new byte[bufferSize];
			fixed (byte* bppcm = pcm)
				pcmPtr = new IntPtr ((void*)bppcm);

			return this;
		}

		public void Close ()
		{
			Dispose (true);
		}

		/// <summary>
		/// Starts capturing.
		/// </summary>
		public void StartCapture ()
		{
			ThrowIfDisposed();
			
			this.capturing = true;
			alcCaptureStart (this.Handle);
			OpenAL.ErrorCheck (this);
		}

		/// <summary>
		/// Stops capturing.
		/// </summary>
		public void StopCapture ()
		{
			ThrowIfDisposed();
			
			this.capturing = false;
			alcCaptureStop (this.Handle);
			OpenAL.ErrorCheck (this);
		}

		/// <summary>
		/// Gets the available samples.
		/// </summary>
		/// <returns>The available PCM samples.</returns>
		public byte[] GetSamples ()
		{
			return GetSamples (AvailableSamples, false);
		}

		/// <summary>
		/// Gets the available samples and provides the number of samples.
		/// </summary>
		/// <param name="numSamples">The number of samples returned.</param>
		/// <returns>The available PCM samples.</returns>
		public byte[] GetSamples (out int numSamples)
		{
			numSamples = GetSamplesAvailable ();
			return GetSamples (numSamples, false);
		}

		/// <summary>
		/// Gets the specified number of samples.
		/// </summary>
		/// <param name="numSamples">The number of samples to return.</param>
		/// <returns></returns>
		public byte[] GetSamples (int numSamples)
		{
			return GetSamples (numSamples, true);
		}

		protected override void Dispose (bool disposing)
		{
			if (this.Handle == IntPtr.Zero)
				return;

			alcCaptureCloseDevice (this.Handle);
			this.Handle = IntPtr.Zero;
			this.pcm = null;
			
			this.disposed = true;
		}

		#region Imports
		[DllImport ("OpenAL32.dll", CallingConvention=CallingConvention.Cdecl)]
		private static extern void alcCaptureStart (IntPtr device);

		[DllImport ("OpenAL32.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void alcCaptureStop (IntPtr device);

		[DllImport ("OpenAL32.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void alcCaptureSamples (IntPtr device, IntPtr buffer, int numSamples);

		[DllImport ("OpenAL32.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr alcCaptureOpenDevice (string deviceName, uint frequency, AudioFormat format, int bufferSize);

		[DllImport ("OpenAL32.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void alcCaptureCloseDevice (IntPtr device);
		#endregion

		private readonly ManualResetEvent mre = new ManualResetEvent (true);
		private volatile bool capturing;
		private int minimumSamples = 1;
		private byte[] pcm;
		private IntPtr pcmPtr;

		private byte[] GetSamples (int numSamples, bool block)
		{
			ThrowIfDisposed();

			byte[] samples = new byte[numSamples * 2];

			while (this.capturing && block && this.AvailableSamples < numSamples)
				Thread.Sleep (0);

			int diff = numSamples - this.AvailableSamples;
			if (diff > 0)
			{
				numSamples -= diff;
				for (int i = diff * 2; i < samples.Length; i++)
				{
					samples[i] = 0;
					samples[++i] = 0;
				}
			}

			alcCaptureSamples (this.Handle, pcmPtr, numSamples);
			OpenAL.ErrorCheck (this);
			Array.Copy (pcm, samples, samples.Length);

			return samples;
		}

		private int GetSamplesAvailable ()
		{
			ThrowIfDisposed();

			int samples;
			OpenAL.alcGetIntegerv (this.Handle, ALCEnum.ALC_CAPTURE_SAMPLES, 4, out samples);
			OpenAL.ErrorCheck (this);
			return samples;
		}
	}
}