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
using System.Security;
using Gablarski.OpenAL;

namespace Gablarski.OpenAL
{
	[SuppressUnmanagedCodeSecurity]
	public static class OpenAL
	{
		static OpenAL ()
		{
			#if DEBUG
			OpenAL.ErrorChecking = true;
			#endif

			IsCaptureSupported = GetIsExtensionPresent ("ALC_EXT_CAPTURE");
		}

		/// <summary>
		/// Sets the distance model for OpenAL.
		/// </summary>
		public static DistanceModel DistanceModel
		{
			set
			{
				alDistanceModel (value);
				OpenAL.ErrorCheck ();
			}
		}

		/// <summary>
		/// Sets the speed of sound for OpenAL.
		/// </summary>
		public static float SpeedOfSound
		{
			set
			{
				alSpeedOfSound (value);
				OpenAL.ErrorCheck ();
			}
		}

		/// <summary>
		/// Sets the doppler factor for OpenAL.
		/// </summary>
		public static float DopplerFactor
		{
			set
			{
				alDopplerFactor (value);
				OpenAL.ErrorCheck ();
			}
		}

		/// <summary>
		/// Gets whether capture support is available.
		/// </summary>
		public static bool IsCaptureSupported
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets whether error checking is enabled.
		/// </summary>
		public static bool ErrorChecking
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the underlying OpenAL provider version.
		/// </summary>
		public static string Version
		{
			get { return Marshal.PtrToStringAnsi (alGetString (AL_VERSION)); }
		}

		public static PlaybackDevice GetDefaultPlaybackDevice()
		{
			PlaybackDevice defaultDevice;
			GetPlaybackDevices (out defaultDevice);
			return defaultDevice;
		}

		public static IEnumerable<PlaybackDevice> GetPlaybackDevices()
		{
			PlaybackDevice defaultDevice;
			return GetPlaybackDevices (out defaultDevice);
		}

		public static IEnumerable<PlaybackDevice> GetPlaybackDevices (out PlaybackDevice defaultDevice)
		{
			defaultDevice = null;

			string defaultName = null;
			string[] strings = new string[0];
			if (GetIsExtensionPresent ("ALC_ENUMERATE_ALL_EXT"))
			{
				defaultName = Marshal.PtrToStringAnsi (alcGetString (IntPtr.Zero, ALC_DEFAULT_ALL_DEVICES_SPECIFIER));
				strings = ReadStringsFromMemory (alcGetString (IntPtr.Zero, ALC_ALL_DEVICES_SPECIFIER));
			}
			else if (GetIsExtensionPresent ("ALC_ENUMERATION_EXT"))
			{
				defaultName = Marshal.PtrToStringAnsi (alcGetString (IntPtr.Zero, ALC_DEFAULT_DEVICE_SPECIFIER));
				strings = ReadStringsFromMemory (alcGetString (IntPtr.Zero, ALC_DEVICE_SPECIFIER));
			}
			
			PlaybackDevice[] devices = new PlaybackDevice[strings.Length];
			for (int i = 0; i < strings.Length; ++i)
			{
				string s = strings[i];
				devices[i] = new PlaybackDevice (s);

				if (s == defaultName)
					defaultDevice = devices[i];
			}

			return devices;
		}

		public static CaptureDevice GetDefaultCaptureDevice()
		{
			if (!IsCaptureSupported)
				throw new NotSupportedException();

			CaptureDevice defaultDevice;
			GetCaptureDevices (out defaultDevice);
			return defaultDevice;
		}

		public static IEnumerable<CaptureDevice> GetCaptureDevices()
		{
			CaptureDevice def;
			return GetCaptureDevices (out def);
		}

		public static IEnumerable<CaptureDevice> GetCaptureDevices (out CaptureDevice defaultDevice)
		{
			if (!IsCaptureSupported)
				throw new NotSupportedException();

			defaultDevice = null;

			string defaultName = Marshal.PtrToStringAnsi (alcGetString (IntPtr.Zero, ALC_CAPTURE_DEFAULT_DEVICE_SPECIFIER));

			var strings = ReadStringsFromMemory (alcGetString (IntPtr.Zero, ALC_CAPTURE_DEVICE_SPECIFIER));
			CaptureDevice[] devices = new CaptureDevice[strings.Length];
			for (int i = 0; i < strings.Length; ++i)
			{
				string s = strings[i];
				devices[i] = new CaptureDevice (s);

				if (s == defaultName)
					defaultDevice = devices[i];
			}

			return devices;
		}

		#region AudioFormat Extensions
		public static uint GetBytesPerSample (this AudioFormat self)
		{
			switch (self)
			{
				default:
				case AudioFormat.Mono8Bit:
					return 1;

				case AudioFormat.Mono16Bit:
				case AudioFormat.Stereo8Bit:
					return 2;

				case AudioFormat.Stereo16Bit:
					return 4;
			}
		}

		public static uint GetBytes (this AudioFormat self, uint samples)
		{
			return self.GetBytesPerSample () * samples;
		}

		public static uint GetSamplesPerSecond (this AudioFormat self, uint frequency)
		{
			switch (self)
			{
				default:
				case AudioFormat.Mono8Bit:
				case AudioFormat.Mono16Bit:
					return frequency;

				case AudioFormat.Stereo8Bit:
				case AudioFormat.Stereo16Bit:
					return (frequency * 2);
			}
		}
		#endregion

		internal static IntPtr NullDevice = IntPtr.Zero;

		#region Imports
		// ReSharper disable InconsistentNaming
		[DllImport ("OpenAL32.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void alDopplerFactor (float value);

		[DllImport ("OpenAL32.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void alSpeedOfSound (float value);

		[DllImport ("OpenAL32.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void alcGetIntegerv (IntPtr device, ALCEnum param, int size, out int data);

		[DllImport ("OpenAL32.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr alGetString (int name);

		[DllImport ("OpenAL32.dll", CallingConvention=CallingConvention.Cdecl)]
		internal static extern IntPtr alcGetString ([In] IntPtr device, int name);

		[DllImport ("OpenAL32.dll", CallingConvention=CallingConvention.Cdecl)]
		internal static extern int alGetError ();

		[DllImport ("OpenAL32.dll", CallingConvention=CallingConvention.Cdecl)]
		internal static extern int alcGetError ([In] IntPtr device);

		[DllImport ("OpenAL32.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern sbyte alcIsExtensionPresent ([In] IntPtr device, string extensionName);

		[DllImport ("OpenAL32.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern sbyte alIsExtensionPresent (string extensionName);

		[DllImport ("OpenAL32.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void alDistanceModel (DistanceModel model);
		// ReSharper restore InconsistentNaming
		#endregion

		internal static bool GetIsExtensionPresent (string extension)
		{
			sbyte result;
			if (extension.StartsWith ("ALC"))
			{
				result = alcIsExtensionPresent (IntPtr.Zero, extension);
			}
			else
			{
				result = alIsExtensionPresent (extension);
				OpenAL.ErrorCheck ();
			}
	
			return (result == 1);
		}

		internal static bool GetIsExtensionPresent (Device device, string extension)
		{
			sbyte result;
			if (extension.StartsWith ("ALC"))
			{
				result = alcIsExtensionPresent (device.Handle, extension);
				OpenAL.ErrorCheck (device);
			}
			else
			{
				result = alIsExtensionPresent (extension);
				OpenAL.ErrorCheck ();
			}

			return (result == 1);
		}

		internal static string[] ReadStringsFromMemory (IntPtr location)
		{
			List<string> strings = new List<string> ();

			bool lastNull = false;
			int i = -1;
			byte c;
			while (!((c = Marshal.ReadByte (location, ++i)) == '\0' && lastNull))
			{
				if (c == '\0')
				{
					lastNull = true;

					strings.Add (Marshal.PtrToStringAnsi (location, i));
					location = new IntPtr ((long)location + i + 1);
					i = -1;
				}
				else
					lastNull = false;
			}

			return strings.ToArray();
		}

		[DebuggerStepThrough]
		internal static void ErrorCheck ()
		{
			if (!ErrorChecking)
				return;

			int err = alGetError ();
			switch ((ALError)err)
			{
				case ALError.AL_NO_ERROR:
					return;

				case ALError.AL_OUT_OF_MEMORY:
					throw new OutOfMemoryException ("OpenAL (AL) out of memory.");

				case ALError.AL_INVALID_ENUM:
					throw new ArgumentException ("Invalid enum");

				case ALError.AL_INVALID_NAME:
					throw new ArgumentException ("Invalid name");

				case ALError.AL_INVALID_VALUE:
					throw new ArgumentException ("Invalid value");

				case ALError.AL_INVALID_OPERATION:
					throw new InvalidOperationException ();
			}
		}

		[DebuggerStepThrough]
		internal static void ErrorCheck (Device device)
		{
			int error = alcGetError (device.Handle);
			switch ((ALCError)error)
			{
				case ALCError.ALC_NO_ERROR:
					return;

				case ALCError.ALC_INVALID_ENUM:
					throw new ArgumentException ("Invalid enum");

				case ALCError.ALC_INVALID_VALUE:
					throw new ArgumentException ("Invalid value");

				case ALCError.ALC_INVALID_CONTEXT:
					throw new ArgumentException ("Invalid context");

				case ALCError.ALC_INVALID_DEVICE:
					throw new ArgumentException ("Invalid device");

				case ALCError.ALC_OUT_OF_MEMORY:
					throw new OutOfMemoryException ("OpenAL (ALC) out of memory.");
			}
		}

		// ReSharper disable InconsistentNaming
		internal const int AL_VERSION = 0xB002;
		internal const int ALC_DEFAULT_DEVICE_SPECIFIER = 0x1004;
		internal const int ALC_DEVICE_SPECIFIER = 0x1005;
		internal const int ALC_CAPTURE_DEVICE_SPECIFIER = 0x310;
		internal const int ALC_CAPTURE_DEFAULT_DEVICE_SPECIFIER = 0x311;
		internal const int ALC_ALL_DEVICES_SPECIFIER = 0x1013;
		internal const int ALC_DEFAULT_ALL_DEVICES_SPECIFIER = 0x1012;
		// ReSharper restore InconsistentNaming
	}

	// ReSharper disable InconsistentNaming
	internal enum ALError
	{
		AL_NO_ERROR = 0,
		AL_INVALID_NAME = 0xA001,
		AL_INVALID_ENUM = 0xA002,
		AL_INVALID_VALUE = 0xA003,
		AL_INVALID_OPERATION = 0xA004,
		AL_OUT_OF_MEMORY = 0xA005,
	}

	internal enum ALCError
	{
		ALC_NO_ERROR		= 0,
		ALC_INVALID_DEVICE	= 0xA001,
		ALC_INVALID_CONTEXT	= 0xA002,
		ALC_INVALID_ENUM	= 0xA003,
		ALC_INVALID_VALUE	= 0xA004,
		ALC_OUT_OF_MEMORY	= 0xA005
	}
	
	internal enum ALEnum
	{
		AL_GAIN
	}

	internal enum ALCEnum
	{
		ALC_MAJOR_VERSION	= 0x1000,
		ALC_MINOR_VERSION	= 0x1001,
		ALC_ATTRIBUTES_SIZE	= 0x1002,
		ALC_ALL_ATTRIBUTES	= 0x1003,
		ALC_CAPTURE_SAMPLES	= 0x312,
		ALC_FREQUENCY		= 0x1007,
		ALC_REFRESH			= 0x1008,
		ALC_SYNC			= 0x1009,
		ALC_MONO_SOURCES	= 0x1010,
		ALC_STEREO_SOURCES	= 0x1011,
	}

	public enum DistanceModel
	{
		None = 0
	}

	public enum AudioFormat
	{
		Mono8Bit = 0x1100,
		Mono16Bit = 0x1101,
		Stereo8Bit = 0x1102,
		Stereo16Bit = 0x1103
	}
	// ReSharper restore InconsistentNaming
}