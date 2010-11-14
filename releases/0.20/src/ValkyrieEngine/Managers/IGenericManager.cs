using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Cadenza.Collections;
using Microsoft.Xna.Framework;

namespace Valkyrie.Engine.Managers
{
	public interface IGenericManager<T> : IEngineProvider
	{
		void AddItem(string id, T item);
		bool RemoveItem(string id);

		bool ContainsKey(string id);
		bool Contains(T item);

		T GetItem (string id);
		ReadOnlyDictionary<string, T> GetItems ();

		T this[string id] { get; }
		int Count { get; }
	}
}
