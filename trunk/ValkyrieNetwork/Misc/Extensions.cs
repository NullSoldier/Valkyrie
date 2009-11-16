using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.IO;

namespace Gablarski
{
	public static class Extensions
	{
		public static byte[] ReadBytes (this Stream stream, int size)
		{
			byte[] buffer = new byte[size];

			int i = 0;
			int bytes = 0;
			while (i < buffer.Length && (bytes = stream.Read (buffer, i, size)) > 0)
			{
				i += bytes;
				size -= bytes;
			}

			return buffer;
		}

		public static bool IsEmpty (this string self)
		{
			return (String.IsNullOrEmpty (self) || self.Trim () == String.Empty);
		}

		public static T Trim<T> (this T self, T maximum)
			where T : struct, IComparable<T>
		{
			return (self.CompareTo (maximum) >= 1) ? maximum : self;
		}

		public static T Trim<T> (this T self, T minimum, T maximum)
			where T : struct, IComparable<T>
		{
			if (self.CompareTo (maximum) >= 1)
				return maximum;
			else if (self.CompareTo (minimum) <= -1)
				return minimum;
			else
				return self;
		}


		private static readonly Dictionary<Type, object> CachedValues = new Dictionary<Type, object>();
		public static object GetDefaultValue (this Type self)
        {
			if (!self.IsValueType)
				return null;

			lock (CachedValues)
			{
				if (!CachedValues.ContainsKey (self))
					CachedValues.Add (self, Activator.CreateInstance (self));

				return CachedValues[self];
			}
        } 
	}
}