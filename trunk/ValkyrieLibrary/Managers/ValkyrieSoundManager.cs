using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Managers;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Valkyrie.Engine;
using System.Media;
using System.Runtime.InteropServices;
using NAudio.Wave;
using Valkyrie.Engine.Core.Sound;
using System.Threading;

namespace Valkyrie.Library.Managers
{
	public class ValkyrieSoundManager
		: ISoundManager
	{
		#region Properties and Methods

		public string SoundRoot
		{
			get { return this.soundroot; }
			set { this.soundroot = value; }
		}

		public void Load (string soundroot)
		{
			this.SoundRoot = soundroot;
		}

		public void AddSound (string Name, AudioSource newSound)
		{
			this.Resources.Add(Name, newSound);
		}

		public void AddSound (string FileName)
		{
			FileInfo file = new FileInfo (Path.Combine(Environment.CurrentDirectory,
				Path.Combine(this.context.Configuration[EngineConfigurationName.SoundsRoot], FileName)));
			if(!file.Exists)
				return;

			WaveStream readerstream = new WaveFileReader (file.FullName);

			readerstream = new BlockAlignReductionStream (readerstream);

			if(readerstream.WaveFormat.Encoding != WaveFormatEncoding.Pcm)
			{
				readerstream = WaveFormatConversionStream.CreatePcmStream (readerstream);
			}

			if(readerstream.WaveFormat.BitsPerSample != 16)
			{
				var format = new WaveFormat (readerstream.WaveFormat.SampleRate, 16,
					readerstream.WaveFormat.Channels);

				readerstream = new WaveFormatConversionStream (format, readerstream);
			}

			byte[] data = new byte[readerstream.Length];

			int position = 0;
			while(true)
			{
				int result = readerstream.ReadByte ();
				if(result == -1)
					break;

				data[position] = Convert.ToByte (result);
				position++;
			}
			
			this.AddSound (file.Name, new AudioSource(data, (uint)readerstream.WaveFormat.SampleRate, 1.0f, readerstream.WaveFormat.Channels));
		}

		public AudioSource GetSound (string fileName)
		{
			if(!this.Resources.ContainsKey (fileName))
				this.AddSound (fileName);
			
			// If we couldn't find it
			if(!this.Resources.ContainsKey (fileName))
				return null;

			return this.Resources[fileName];
		}

		public void GetSoundAsync (string filename, Action<SoundLoadedEventArgs> callback)
		{
			Thread thread = new Thread (this.GetSound);
			thread.IsBackground = true;
			thread.Name = "Sound Loader Loading " + filename;
			thread.Start (new object[] {filename, callback});
		}

		public bool ContainsSound (string FileName)
		{
			return (this.Resources.ContainsKey(FileName));
		}

		public void ClearCache ()
		{
			this.Resources.Clear();
		}

		public void ClearFromCache (string resourcename)
		{
			if(this.Resources.Keys.Contains(resourcename))
			{
				this.Resources.Remove(resourcename);
			}
			else
				throw new ArgumentException("Resource does not exist in the cache"); // uneccessary?
		}

		#endregion

		private Dictionary<string, AudioSource> Resources = new Dictionary<string, AudioSource> ();
		private IEngineContext context = null;
		private string soundroot = string.Empty;
		private bool isloaded = false;

		private void GetSound (object args)
		{
			var objargs = (object[]) args;
			var filename = (string) objargs[0];

			this.GetSound (filename);

			var handler = (Action<SoundLoadedEventArgs>)objargs[1];
			if(handler != null)
				handler (new SoundLoadedEventArgs (filename));
		}

		#region IEngineProvider Members

		public void LoadEngineContext (IEngineContext context)
		{
			this.context = context;
			this.Load(this.context.Configuration[EngineConfigurationName.SoundsRoot]);

			this.isloaded = true;
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		#endregion
	}
}
