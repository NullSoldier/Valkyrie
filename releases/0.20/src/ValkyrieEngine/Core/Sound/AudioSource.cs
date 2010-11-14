using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Valkyrie.Engine.Core.Sound
{
	public class AudioSource
	{
		public AudioSource ()
		{
		}

		public AudioSource (byte[] pcm, uint freq, float gain, int channels)
		{
			this.PCM = pcm;
			this.Frequency = freq;
			this.Gain = gain;
			this.Channels = channels;
		}

		public byte[] PCM
		{
			get { return this.pcm; }
			set { this.pcm = value; }
		}

		public uint Frequency
		{
			get { return frequency; }
			set { frequency = value; }
		}

		public float Gain
		{
			get { return this.gain; }
			set { this.gain = value; }
		}

		public int Channels
		{
			get { return this.channels; }
			set { this.channels = value; }
		}

		private byte[] pcm;
		private uint frequency = 0;
		private float gain = 0;
		private int channels = 0;
	}
}
