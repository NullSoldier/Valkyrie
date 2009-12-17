using System;
using System.Runtime.InteropServices;

namespace Gablarski.OpenAL
{
	public static class Listener
	{
		public static float Gain
		{
			get
			{
				float gain;
				alGetListenerf (ALEnum.AL_GAIN, out gain);
				OpenAL.ErrorCheck();
				
				return gain;
			}
			
			set
			{
				alListenerf (ALEnum.AL_GAIN, value);
				OpenAL.ErrorCheck();
			}
		}
		
		[DllImport ("OpenAL32.dll")]
		static extern void alListenerf (ALEnum param, float val);
		
		[DllImport ("OpenAL32.dll")]
		static extern void alGetListenerf (ALEnum param, out float val);
	}
}