using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace NAudio.Midi
{
	/// <summary>
	/// Represents a MIDI in device
	/// </summary>
	public class MidiIn : IDisposable 
	{
		private IntPtr hMidiIn = IntPtr.Zero;
		private bool disposed = false;
        private MidiInterop.MidiInCallback callback;

        /// <summary>
        /// Called when a MIDI message is received
        /// </summary>
        public event EventHandler<MidiInMessageEventArgs> MessageReceived;

        /// <summary>
        /// An invalid MIDI message
        /// </summary>
        public event EventHandler<MidiInMessageEventArgs> ErrorReceived;

		/// <summary>
		/// Gets the number of MIDI input devices available in the system
		/// </summary>
		public static int NumberOfDevices 
		{
			get 
			{
				return MidiInterop.midiInGetNumDevs();
			}
		}
		
		/// <summary>
		/// Opens a specified MIDI in device
		/// </summary>
		/// <param name="deviceNo">The device number</param>
		public MidiIn(int deviceNo) 
		{
            this.callback = new MidiInterop.MidiInCallback(Callback);
			MmException.Try(MidiInterop.midiInOpen(out hMidiIn,deviceNo,this.callback,0,MidiInterop.CALLBACK_FUNCTION),"midiInOpen");
		}
		
		/// <summary>
		/// Closes this MIDI in device
		/// </summary>
		public void Close() 
		{
			Dispose();
		}

		/// <summary>
		/// Closes this MIDI in device
		/// </summary>
		public void Dispose() 
		{
            GC.KeepAlive(callback);
			Dispose(true);
			GC.SuppressFinalize(this);
		}

        /// <summary>
        /// Start the MIDI in device
        /// </summary>
        public void Start()
        {
            MmException.Try(MidiInterop.midiInStart(hMidiIn), "midiInStart");
        }

        /// <summary>
        /// Stop the MIDI in device
        /// </summary>
        public void Stop()
        {
            MmException.Try(MidiInterop.midiInStop(hMidiIn), "midiInStop");
        }

        /// <summary>
        /// Reset the MIDI in device
        /// </summary>
        public void Reset()
        {
            MmException.Try(MidiInterop.midiInReset(hMidiIn), "midiInReset");
        }
        
        private void Callback(IntPtr midiInHandle, MidiInterop.MidiInMessage message, IntPtr userData, IntPtr messageParameter1, IntPtr messageParameter2)
        {
            switch(message)
            {
                case MidiInterop.MidiInMessage.Open:
                    // message Parameter 1 & 2 are not used
                    break;
                case MidiInterop.MidiInMessage.Data:
                    // parameter 1 is packed MIDI message
                    // parameter 2 is milliseconds since MidiInStart
                    if (MessageReceived != null)
                    {
                        MessageReceived(this, new MidiInMessageEventArgs(messageParameter1.ToInt32(), messageParameter2.ToInt32()));
                    }
                    break;
                case MidiInterop.MidiInMessage.Error:
                    // parameter 1 is invalid MIDI message
                    if (ErrorReceived != null)
                    {
                        ErrorReceived(this, new MidiInMessageEventArgs(messageParameter1.ToInt32(), messageParameter2.ToInt32()));
                    } 
                    break;
                case MidiInterop.MidiInMessage.Close:
                    // message Parameter 1 & 2 are not used
                    break;
                case MidiInterop.MidiInMessage.LongData:
                    // parameter 1 is pointer to MIDI header
                    // parameter 2 is milliseconds since MidiInStart
                    break;
                case MidiInterop.MidiInMessage.LongError:
                    // parameter 1 is pointer to MIDI header
                    // parameter 2 is milliseconds since MidiInStart
                    break;
                case MidiInterop.MidiInMessage.MoreData:
                    // parameter 1 is packed MIDI message
                    // parameter 2 is milliseconds since MidiInStart
                    break;
            }
        }

		/// <summary>
		/// Gets the MIDI in device info
		/// </summary>
		public static MidiInCapabilities DeviceInfo(int midiInDeviceNumber)
		{
            MidiInCapabilities caps = new MidiInCapabilities();
            int structSize = Marshal.SizeOf(caps);
			MmException.Try(MidiInterop.midiInGetDevCaps(midiInDeviceNumber,out caps,structSize),"midiInGetDevCaps");
			return caps;
		}

		/// <summary>
		/// Closes the MIDI out device
		/// </summary>
		/// <param name="disposing">True if called from Dispose</param>
		protected virtual void Dispose(bool disposing) 
		{
			if(!this.disposed) 
			{
				//if(disposing) Components.Dispose();
				MidiInterop.midiInClose(hMidiIn);
			}
			disposed = true;         
		}

		/// <summary>
		/// Cleanup
		/// </summary>
		~MidiIn()
		{
			System.Diagnostics.Debug.Assert(false,"MIDI In was not finalised");
			Dispose(false);
		}	
	}

    /// <summary>
    /// MIDI In Message Information
    /// </summary>
    public class MidiInMessageEventArgs : EventArgs
    {
        int message;
        MidiEvent midiEvent;
        int timestamp;

        /// <summary>
        /// Create a new MIDI In Message EventArgs
        /// </summary>
        /// <param name="message"></param>
        /// <param name="timestamp"></param>
        public MidiInMessageEventArgs(int message, int timestamp)
        {
            this.message = message;
            this.timestamp = timestamp;
            try
            {
                this.midiEvent = MidiEvent.FromRawMessage(message);
            }
            catch (Exception)
            {
                // don't worry too much - might be an invalid message
            }
        }

        /// <summary>
        /// The Raw message received from the MIDI In API
        /// </summary>
        public int RawMessage
        {
            get
            {
                return message;
            }
        }

        /// <summary>
        /// The raw message interpreted as a MidiEvent
        /// </summary>
        public MidiEvent MidiEvent
        {
            get
            {
                return midiEvent;
            }
        }

        /// <summary>
        /// The timestamp in milliseconds for this message
        /// </summary>
        public int Timestamp
        {
            get
            {
                return timestamp;
            }
        }
    }
}