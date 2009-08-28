using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ValkyrieLibrary.Core
{
	public static class EventTypeLoader
	{
		public static IEnumerable<Type> LoadImplementers<T>(Assembly[] assemblies)
			where T : class
		{
			return assemblies.SelectMany(a => a.GetTypes()).Where(t => typeof(T).IsAssignableFrom(t) && !t.IsInterface);
		}
	}
}
