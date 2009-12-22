using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Valkyrie
{
	public static class ProviderUtils
	{
		private static char[] characters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
											 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
											 '.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '@' };
		
		public static string GetRandomString (int size)
		{
			char[] randString = new char[size];

			Random r = new Random();
			for (int i = 0; i < randString.Length; ++i)
				randString[i] = characters[r.Next(characters.Length - 1)];

			return new string(randString);
		}
	}
}