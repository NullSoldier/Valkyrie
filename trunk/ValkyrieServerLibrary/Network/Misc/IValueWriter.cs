using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gablarski
{
	public interface IValueWriter
		: IDisposable
	{
		void WriteBytes (byte[] value);

		void WriteSByte (SByte value);
		void WriteInt16 (Int16 value);
		void WriteInt32 (Int32 value);
		void WriteInt64 (Int64 value);
		
		void WriteByte (Byte value);
		void WriteUInt16 (UInt16 value);
		void WriteUInt32 (UInt32 value);
		void WriteUInt64 (UInt64 value);

		void WriteString (string value);

		void Flush();
	}

	public static class ValueWriterExtensions
	{
		public static void WriteBool (this IValueWriter writer, bool value)
		{
			writer.WriteByte ((byte)((value) ? 1 : 0));
		}

		public static void WriteVersion (this IValueWriter writer, Version version)
		{
			writer.WriteInt32 (version.Major);
			writer.WriteInt32 (version.Minor);
			writer.WriteInt32 (version.Build);
			writer.WriteInt32 (version.Revision);
		}

		public static void WriteType (this IValueWriter writer, Type value)
		{
			writer.WriteString (value.AssemblyQualifiedName);
		}

		public static void WriteGenericResult (this IValueWriter writer, GenericResult result)
		{
			writer.WriteByte ((byte)result);
		}
	}
}