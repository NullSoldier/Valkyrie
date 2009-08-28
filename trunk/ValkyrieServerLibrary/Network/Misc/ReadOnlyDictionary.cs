using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gablarski
{
	public class ReadOnlyDictionary<TKey, TValue>
		: IDictionary<TKey, TValue>
	{
		#region Constructors
		public ReadOnlyDictionary (IEnumerable<TValue> elements, Func<TValue, TKey> keySelector)
			: this (elements.ToDictionary(keySelector))
		{
		}

		public ReadOnlyDictionary (IDictionary<TKey, TValue> dict)
		{
			this.dict = dict;
		}
		#endregion

		#region IDictionary<TKey,TValue> Members

		/// <summary>
		/// Not supported.
		/// </summary>
		/// <exception cref="System.NotSupportedException">Always.</exception>
		public void Add (TKey key, TValue value)
		{
			throw new NotSupportedException ();
		}

		/// <summary>
		/// Gets whether <paramref name="key"/> exists in the dictionary.
		/// </summary>
		/// <param name="key">The key to check for.</param>
		/// <returns><c>true</c> if the key was found, <c>false</c> otherwise.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="key"/> is null.</exception>
		public bool ContainsKey (TKey key)
		{
			return this.dict.ContainsKey (key);
		}

		/// <summary>
		/// Gets a collection of this dictionary's keys.
		/// </summary>
		public ICollection<TKey> Keys
		{
			get { return this.dict.Keys; }
		}

		/// <summary>
		/// Not supported.
		/// </summary>
		/// <exception cref="System.NotSupportedException">Always.</exception>
		public bool Remove (TKey key)
		{
			throw new NotSupportedException ();
		}

		/// <summary>
		/// Attempts to get the <paramref name="value"/> for <paramref name="key"/>.
		/// </summary>
		/// <param name="key">The key to attempt to get <paramref name="value"/> for.</param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool TryGetValue (TKey key, out TValue value)
		{
			return this.dict.TryGetValue (key, out value);
		}

		/// <summary>
		/// Gets a collection of the values in this dictionary.
		/// </summary>
		public ICollection<TValue> Values
		{
			get { return this.dict.Values; }
		}

		/// <summary>
		/// Gets the value associated with <paramref name="key"/>.
		/// </summary>
		/// <param name="key">The key to get the value for.</param>
		/// <returns>The value of <paramref name="key"/>.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException"><paramref name="key"/> is not found.</exception>
		public TValue this[TKey key]
		{
			get
			{
				return this.dict[key];
			}

			set
			{
				throw new NotSupportedException ();
			}
		}

		#endregion

		#region ICollection<KeyValuePair<TKey,TValue>> Members

		/// <summary>
		/// Not supported.
		/// </summary>
		/// <exception cref="System.NotSupportedException">Always.</exception>
		public void Add (KeyValuePair<TKey, TValue> item)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Not supported.
		/// </summary>
		/// <exception cref="System.NotSupportedException">Always.</exception>
		public void Clear ()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Gets whether <paramref name="item"/> is in the collection.
		/// </summary>
		/// <param name="item">The <c>KeyValuePair</c> to check for.</param>
		/// <returns><c>true</c> if <paramref name="item"/> exists.</returns>
		public bool Contains (KeyValuePair<TKey, TValue> item)
		{
			return this.dict.Contains (item);
		}

		/// <summary>
		/// Copies the collection to <paramref name="array"/> starting at <paramref name="arrayIndex"/>.
		/// </summary>
		/// <param name="array">The array to copy the collection to.</param>
		/// <param name="arrayIndex">The offset in <paramref name="array"/> to start copying to.</param>
		public void CopyTo (KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			this.dict.CopyTo (array, arrayIndex);
		}

		/// <summary>
		/// Gets the size of the collection.
		/// </summary>
		public int Count
		{
			get { return this.dict.Count; }
		}

		/// <summary>
		/// Gets whether this collection is readonly (always <c>true</c>.)
		/// </summary>
		public bool IsReadOnly
		{
			get { return true; }
		}

		/// <summary>
		/// Not supported.
		/// </summary>
		/// <exception cref="System.NotSupportedException">Always.</exception>
		public bool Remove (KeyValuePair<TKey, TValue> item)
		{
			throw new NotSupportedException ();
		}

		#endregion

		#region IEnumerable<KeyValuePair<TKey,TValue>> Members

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator ()
		{
			return this.dict.GetEnumerator ();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return ((System.Collections.IEnumerable)this.dict).GetEnumerator ();
		}

		#endregion

		private readonly IDictionary<TKey, TValue> dict;
	}
}
