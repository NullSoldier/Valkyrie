using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine.Managers;
using Valkyrie.Engine;
using Cadenza.Collections;

namespace Valkyrie.Library.Managers
{
	public class ValkyriePlayerManager<T> : IPlayerManager<T>
		where T : BaseCharacter
	{
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
			get { return this.GetItem(name); }
		}

		public T GetItem(string name)
		{
			lock (this.items)
			{
				T value;

				if (!items.TryGetValue(name, out value))
					throw new ArgumentException("Camera not found.");

				return value;
			}
		}

		public ReadOnlyDictionary<string, T> GetItems()
		{
			return new ReadOnlyDictionary<string, T>(this.items);
		}

		public bool ContainsKey(string name)
		{
			return items.ContainsKey(name);
		}

		public bool Contains(T item)
		{
			return items.ContainsValue(item);
		}

		public int Count { get { return items.Count; } }

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
