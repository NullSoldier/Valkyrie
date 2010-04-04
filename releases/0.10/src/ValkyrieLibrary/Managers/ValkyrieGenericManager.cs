using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cadenza.Collections;
using Valkyrie.Engine.Managers;
using Valkyrie.Engine;

namespace Valkyrie.Library.Managers
{
	public class ValkyrieGenericManager<T> : IGenericManager <T>
	{
		public int Count { get { return items.Count; } }

		public void AddItem(string name, T item)
		{
			lock (this.items)
			{
				if (this.items.ContainsKey(name))
					throw new ArgumentException("Item already exists with that ID");

				this.items.Add(name, item);
			}
		}

		public bool RemoveItem(string name)
		{
			lock (this.items)
			{
				return this.items.Remove(name);
			}
		}

		public T this[string name]
		{
			get { return this.items[name]; }
		}

		public T GetItem(string name)
		{
			lock (this.items)
			{
				if (!this.items.ContainsKey(name))
					throw new ArgumentException("Camera not found.");

				return this.items[name];
			}
		}

		public ReadOnlyDictionary<string, T> GetItems()
		{
			return new ReadOnlyDictionary<string, T> (this.items);
		}

		public bool ContainsKey (string name)
		{
			return items.ContainsKey(name);
		}

		public bool Contains(T item)
		{
			return items.ContainsValue(item);
		}

		#region IEngineProvider Members

		public void LoadEngineContext(IEngineContext context)
		{
			this.isloaded = true;
		}

		public void Unload()
		{
			this.items.Clear();

			this.isloaded = false;
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		#endregion

		private Dictionary<string, T> items = new Dictionary<string, T>();
		private bool isloaded = false;
	}
}
