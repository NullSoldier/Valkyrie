using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gablarski
{
	public interface IValueReader
		: IDisposable
	{
		byte[] ReadBytes ();

		SByte ReadSByte ();
		Int16 ReadInt16 ();
		Int32 ReadInt32 ();
		Int64 ReadInt64 ();

		Byte ReadByte ();
		UInt16 ReadUInt16 ();
		UInt32 ReadUInt32 ();
		UInt64 ReadUInt64 ();

		string ReadString ();
	}

	public static class ValueReaderExtensions
	{
		public static bool ReadBool (this IValueReader reader)
		{
			return (reader.ReadByte () == 1);
		}

		public static Version ReadVersion (this IValueReader reader)
		{
			return new Version (reader.ReadInt32 (), reader.ReadInt32 (), reader.ReadInt32 (), reader.ReadInt32 ());
		}

		public static Type ReadType (this IValueReader reader)
		{
			return Type.GetType (reader.ReadString ());
		}

		public static GenericResult ReadGenericResult (this IValueReader reader)
		{
			return (GenericResult)reader.ReadByte ();
		}
	}
}