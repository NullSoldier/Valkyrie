using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ValkyrieLibrary
{
	public static class StringExtensions
	{
		public static string PathCombine (this string self, string path)
		{
			return Path.Combine(self, path);
		}
	}
}