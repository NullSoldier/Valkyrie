using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Gablarski
{
	public class OrderedDictionary<TKey, TValue>
		: IDictionary<TKey, TValue>
	{
		public OrderedDictionary ()
		{
			this.dict = new Dictionary<TKey, TValue> ();
			this.orderedKeys = new List<TKey> ();
			this.keys = new ReadOnlyCollection<TKey> (this.orderedKeys);
		}

		public OrderedDictionary (int capacity)
		{
			this.dict = new Dictionary<TKey, TValue> (capacity);
			this.orderedKeys = new List<TKey> (capacity);
			this.keys = new ReadOnlyCollection<TKey> (this.orderedKeys);
		}

		/// <summary>
		/// Gets or sets the value for the specified <paramref name="index"/>.
		/// </summary>
		/// <param name="index">The index to get or set the value for.</param>
		/// <returns>The value for the specified index.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is less than 0 or greater than <see cref="Count"/>.</exception>
		public TValue this [int index]
		{
			get { return this.dict [this.orderedKeys [index]]; }
			set { this.dict [this.orderedKeys [index]] = value; }
		}

		/// <summary>
		/// Gets the keys of the dictionary.
		/// </summary>
		public IEnumerable<TKey> Keys
		{
			get { return this.orderedKeys; }
		}

		/// <summary>
		/// Gets the values of the dictionary.
		/// </summary>
		public IEnumerable<TValue> Values
		{
			get { return this.orderedKeys.Select (k => this [k]); }
		}

		#region IDictionary<TKey,TValue> Members
		/// <summary>
		/// Adds the <paramref name="key"/> and <paramref name="value"/> to the dictionary.
		/// </summary>
		/// <param name="key">The key of the element to add.</param>
		/// <param name="value">The value of the element to add.</param>
		public void Add (TKey key, TValue value)
		{
			this.dict.Add (key, value);
			this.orderedKeys.Add (key);
		}

		/// <summary>
		/// Checks whether the specified <paramref name="key"/> exists in the dictionary.
		/// </summary>
		/// <param name="key">The key to check the dictionary for.</param>
		/// <returns><c>true</c> if the key exists in the dictionary, <c>false</c> otherwise.</returns>
		public bool ContainsKey (TKey key)
		{
			return this.dict.ContainsKey (key);
		}

		ICollection<TKey> IDictionary<TKey,TValue>.Keys
		{
			get { return this.keys; }
		}

		/// <summary>
		/// Attempts to remove the specified key and its value from the dictionary.
		/// </summary>
		/// <param name="key">The key to attempt to remove.</param>
		/// <returns><c>true</c> if the element was found and removed, <c>false</c> otherwise.</returns>
		public bool Remove (TKey key)
		{
			this.dict.Remove (key);
			return this.orderedKeys.Remove (key);
		}

		/// <summary>
		/// Attempts to get the <paramref name="value"/> of the <paramref name="key"/>.
		/// </summary>
		/// <param name="key">The key to attempt to retrieve a value from.</param>
		/// <param name="value"></param>
		/// <returns><c>true</c> if the <paramref name="key"/> was found, <c>false</c> otherwise.</returns>
		public bool TryGetValue (TKey key, out TValue value)
		{
			return this.dict.TryGetValue (key, out value);
		}

		ICollection<TValue> IDictionary<TKey,TValue>.Values
		{
			get { return new ReadOnlyCollection<TValue> (this.Values.ToList()); }
		}

		/// <summary>
		/// Gets or sets the value for the <paramref name="key"/>.
		/// </summary>
		/// <param name="key">The key to get or set the value for.</param>
		/// <returns>The value associated with the key.</returns>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException">The <paramref name="key"/> is not in the dictionary.</exception>
		/// <exception cref="System.ArgumentNullException"><paramref name="key"/> is null.</exception>
		public TValue this [TKey key]
		{
			get { return this.dict [key]; }
			set
			{
				if (!this.ContainsKey (key))
					this.orderedKeys.Add (key);

				this.dict [key] = value;
			}
		}

		#endregion

		#region ICollection<KeyValuePair<TKey,TValue>> Members

		public void Add (KeyValuePair<TKey, TValue> item)
		{
			this.Add (item.Key, item.Value);
		}

		/// <summary>
		/// Clears the dictionary of all elements.
		/// </summary>
		public void Clear ()
		{
			this.dict.Clear ();
			this.orderedKeys.Clear ();
		}

		public bool Contains (KeyValuePair<TKey, TValue> item)
		{
			return this.dict.Contains (item);
		}

		public void CopyTo (KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (array == null)
				throw new ArgumentNullException ("array");

			if (arrayIndex < 0)
				throw new ArgumentOutOfRangeException ("arrayIndex");

			if ((array.Length - arrayIndex) > this.Count)
				throw new ArgumentException ("array");

			for (int i = 0; i < this.Count; ++i)
				array [i + arrayIndex] = new KeyValuePair<TKey, TValue> (this.orderedKeys [i], this [i]);
		}

		/// <summary>
		/// Gets the number of elements in the dictionary.
		/// </summary>
		public int Count
		{
			get { return this.dict.Count; }
		}

		/// <summary>
		/// Gets whether the dictionary is readonly or not.
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Attempts to remove the <paramref name="item"/> from the dictionary.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		/// <returns><c>true</c> if the item was found and removed, <c>false</c> if the item was not found.</returns>
		public bool Remove (KeyValuePair<TKey, TValue> item)
		{
			return this.Remove (item.Key);
		}

		#endregion

		#region IEnumerable<KeyValuePair<TKey,TValue>> Members

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator ()
		{
			for (int i = 0; i < this.orderedKeys.Count; ++i)
				yield return new KeyValuePair<TKey, TValue> (this.orderedKeys [i], this [this.orderedKeys [i]]);
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return this.GetEnumerator ();
		}

		#endregion

		private readonly Dictionary<TKey, TValue> dict;
		private readonly List<TKey> orderedKeys;
		private readonly ReadOnlyCollection<TKey> keys;
	}
}