using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gablarski
{
	public class ByteArrayValueReader
		: IValueReader
	{
		public ByteArrayValueReader (byte[] data)
		{
			if (data == null)
				throw new ArgumentNullException ("data");

			this.array = data;
		}

		public ByteArrayValueReader (byte[] data, int position)
			: this (data)
		{
			if (position < 0)
				throw new ArgumentOutOfRangeException ("position");

			this.position = position;
		}

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
		}

		#endregion

		#region Implementation of IValueReader

		public byte[] ReadBytes()
		{
			int len = this.array.ReadInt32 (ref this.position);

			byte[] value = new byte[len];
			Array.Copy (this.array, this.position, value, 0, len);

			return value;
		}

		public sbyte ReadSByte()
		{
			return (sbyte)this.array[this.position++];
		}

		public short ReadInt16()
		{
			return this.array.ReadInt16 (ref this.position);
		}

		public int ReadInt32()
		{
			return this.array.ReadInt32 (ref this.position);
		}

		public long ReadInt64()
		{
			return this.array.ReadInt64 (ref this.position);
		}

		public byte ReadByte()
		{
			return this.array[this.position++];
		}

		public ushort ReadUInt16()
		{
			return this.array.ReadUInt16 (ref this.position);
		}

		public uint ReadUInt32()
		{
			return this.array.ReadUInt32 (ref this.position);
		}

		public ulong ReadUInt64()
		{
			return this.array.ReadUInt64 (ref this.position);
		}

		public string ReadString()
		{
			byte[] buffer = this.array;
			int pos = this.position;

			string str = String.Empty;
			while (true)
			{
				string tstr = Encoding.UTF8.GetString (buffer, pos++, 1);
				if (tstr == "\0")
					break;

				str += tstr;
			}

			this.position = pos;
			return str;
		}

		#endregion

		private readonly byte[] array;
		private int position;
	}
}