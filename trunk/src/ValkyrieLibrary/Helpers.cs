using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Valkyrie.Engine.Core;
using Valkyrie.Engine;

namespace Valkyrie.Library
{
	public static class Helpers
	{
		public static int Clamp (this int value, int min, int max)
		{
			if(value < min) return min;
			if(value > max) return max;

			return value;
		}

		public static float Clamp (this float value, float min, float max)
		{
			if(value < min) return min;
			if(value > max) return max;

			return value;
		}

		public static string MD5 (string originalPassword)
		{
			MD5CryptoServiceProvider x = new MD5CryptoServiceProvider ();

			byte[] bs = System.Text.Encoding.UTF8.GetBytes (originalPassword);
			bs = x.ComputeHash (bs);

			StringBuilder s = new StringBuilder ();
			foreach(byte b in bs)
			{
				s.Append (b.ToString ("x2").ToLower ());
			}

			return s.ToString ();
		}

		public static ScreenPoint ScreenSpaceToWorldSpace (this BaseCamera self, ScreenPoint point)
		{
			return (point + self.Origin) / self.Zoom;
		}
	}
}
