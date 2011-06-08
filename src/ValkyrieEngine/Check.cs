using System;
using System.Diagnostics;

namespace Valkyrie.Engine
{
	public static class Check
	{
		[Conditional ("DEBUG")]
		public static void NullArgument<T> (T argument, string argumentName)
			where T : class
		{
			if (argument == null)
				throw new ArgumentNullException (argumentName);
		}
	}
}