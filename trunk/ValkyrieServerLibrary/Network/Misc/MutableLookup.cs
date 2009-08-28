/*
Copyright (c) 2009, Eric Maupin (http://gablarski.org)
All rights reserved.

Redistribution and use in source and binary forms, with
or without modification, are permitted provided that
the following conditions are met:

 - Redistributions of source code must retain the above 
   copyright notice, this list of conditions and the
   following disclaimer.

 - Redistributions in binary form must reproduce the above
   copyright notice, this list of conditions and the
   following disclaimer in the documentation and/or other
   materials provided with the distribution.

 - Neither the name of Gablarski nor the names of its
   contributors may be used to endorse or promote products
   derived from this software without specific prior
   written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS
AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH
DAMAGE.
 */

using System.Collections.Generic;
using System.Linq;

namespace Gablarski
{
	/// <summary>
	/// A mutable lookup implementing <see cref="ILookup{TKey,TElement}"/>
	/// </summary>
	/// <typeparam name="TKey">The lookup key.</typeparam>
	/// <typeparam name="TElement">The elements under each <typeparamref name="TKey"/>.</typeparam>
	public class MutableLookup<TKey, TElement>
		: ILookup<TKey, TElement>
	{
		/// <summary>
		/// Adds <paramref name="element"/> under the specified <paramref name="key"/>. <paramref name="key"/> does not need to exist.
		/// </summary>
		/// <param name="key">The key to add <paramref name="element"/> under.</param>
		/// <param name="element">The element to add.</param>
		public void Add (TKey key, TElement element)
		{
			if (!this.groupings.ContainsKey (key))
				this.groupings.Add (key, new MutableLookupGrouping (key));

			this.groupings[key].Add (element);
		}

		/// <summary>
		/// Removes <paramref name="element"/> from the <paramref name="key"/>.
		/// </summary>
		/// <param name="key">The key that <paramref name="element"/> is located under.</param>
		/// <param name="element">The element to remove from <paramref name="key"/>. </param>
		/// <returns><c>true</c> if <paramref name="key"/> and <paramref name="element"/> existed, <c>false</c> if not.</returns>
		public bool Remove (TKey key, TElement element)
		{
			if (!this.groupings.ContainsKey (key))
				return false;

			return this.groupings[key].Remove (element);
		}

		/// <summary>
		/// Removes <paramref name="key"/> from the lookup.
		/// </summary>
		/// <param name="key">They to remove.</param>
		/// <returns><c>true</c> if <paramref name="key"/> existed.</returns>
		public bool Remove (TKey key)
		{
			return this.groupings.Remove (key);
		}

		#region ILookup Members
		/// <summary>
		/// Gets the number of groupings.
		/// </summary>
		public int Count
		{
			get { return this.groupings.Count; }
		}

		/// <summary>
		/// Gets the elements for <paramref name="key"/>.
		/// </summary>
		/// <param name="key">The key to get the elements for.</param>
		/// <returns>The elements under <paramref name="key"/>.</returns>
		public IEnumerable<TElement> this[TKey key]
		{
			get { return this.groupings[key]; }
		}

		/// <summary>
		/// Gets whether or not there's a grouping for <paramref name="key"/>.
		/// </summary>
		/// <param name="key">The key to check for.</param>
		/// <returns><c>true</c> if <paramref name="key"/> is present.</returns>
		public bool Contains (TKey key)
		{
			return this.groupings.ContainsKey (key);
		}

		public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator ()
		{
			return this.groupings.Values.Cast<IGrouping<TKey, TElement>> ().GetEnumerator ();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return this.GetEnumerator ();
		}
		#endregion

		private readonly Dictionary<TKey, MutableLookupGrouping> groupings = new Dictionary<TKey, MutableLookupGrouping> ();

		private class MutableLookupGrouping
			: List<TElement>, IGrouping<TKey, TElement>
		{
			public MutableLookupGrouping (TKey key)
			{
				this.Key = key;
			}

			#region IGrouping<TKey,TElement> Members

			public TKey Key
			{
				get;
				private set;
			}

			#endregion
		}
	}
}