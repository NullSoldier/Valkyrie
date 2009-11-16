using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gablarski
{
	public static class ByteArrayExtensions
	{
		public static Int16 ReadInt16 (this byte[] self, ref int position)
		{
			Int16 value = BitConverter.ToInt16 (self, position);
			position += sizeof(Int16);

			return value;
		}

		public static Int32 ReadInt32 (this byte[] self, ref int position)
		{
			Int32 value = BitConverter.ToInt32 (self, position);
			position += sizeof(Int32);

			return value;
		}

		public static Int64 ReadInt64 (this byte[] self, ref int position)
		{
			Int64 value = BitConverter.ToInt16 (self, position);
			position += sizeof(Int64);

			return value;
		}

		public static UInt16 ReadUInt16 (this byte[] self, ref int position)
		{
			UInt16 value = BitConverter.ToUInt16 (self, position);
			position += sizeof(UInt16);

			return value;
		}

		public static UInt32 ReadUInt32 (this byte[] self, ref int position)
		{
			UInt32 value = BitConverter.ToUInt32 (self, position);
			position += sizeof(UInt32);

			return value;
		}

		public static UInt64 ReadUInt64 (this byte[] self, ref int position)
		{
			UInt64 value = BitConverter.ToUInt16 (self, position);
			position += sizeof(UInt64);

			return value;
		}
	}
}