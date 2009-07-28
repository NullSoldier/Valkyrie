using System;
using System.Diagnostics;

namespace ValkyrieLibrary
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