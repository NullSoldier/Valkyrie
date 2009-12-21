using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Valkyrie.Library
{
	public static class Helpers
	{
		public static int Clamp (int value, int min, int max)
		{
			if(value < min) return min;
			if(value > max) return max;

			return value;
		}

		public static float Clamp (float value, float min, float max)
		{
			if(value < min) return min;
			if(value > max) return max;

			return value;
		}
	}
}
