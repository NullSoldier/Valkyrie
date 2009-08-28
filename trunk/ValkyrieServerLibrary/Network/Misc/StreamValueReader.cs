using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Gablarski
{
	public class StreamValueReader
		: IValueReader
	{
		public StreamValueReader (Stream baseStream)
		{
			this.baseStream = baseStream;
		}

		#region IValueReader Members
		public byte[] ReadBytes ()
		{
			int length = ReadInt32 ();
			return baseStream.ReadBytes (length);
		}

		public sbyte ReadSByte ()
		{
			return (sbyte)this.baseStream.ReadByte ();
		}

		public short ReadInt16 ()
		{
			return BitConverter.ToInt16 (this.baseStream.ReadBytes (2), 0);
		}

		public int ReadInt32 ()
		{
			return BitConverter.ToInt32 (this.baseStream.ReadBytes (4), 0);
		}

		public long ReadInt64 ()
		{
			return BitConverter.ToInt64 (this.baseStream.ReadBytes (8), 0);
		}

		public byte ReadByte ()
		{
			return (byte)this.baseStream.ReadByte ();
		}

		public ushort ReadUInt16 ()
		{
			return BitConverter.ToUInt16 (this.baseStream.ReadBytes (2), 0);
		}

		public uint ReadUInt32 ()
		{
			return BitConverter.ToUInt32 (this.baseStream.ReadBytes (4), 0);
		}

		public ulong ReadUInt64 ()
		{
			return BitConverter.ToUInt64 (this.baseStream.ReadBytes (8), 0);
		}

		public string ReadString ()
		{
			byte[] buffer = new byte[1];
			string str = String.Empty;
			while (true)
			{
				if (this.baseStream.Read (buffer, 0, 1) == 0)
					break;

				string tstr = Encoding.UTF8.GetString (buffer);
				if (tstr == "\0")
					break;

				str += tstr;
			}

			return str;
		}

		#endregion

		private readonly Stream baseStream;

		#region IDisposable Members

		public void Dispose ()
		{
			if (baseStream != null)
				baseStream.Close();
		}

		#endregion
	}
}