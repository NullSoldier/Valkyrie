// Copyright (c) 2009, Eric Maupin
// All rights reserved.
//
// Redistribution and use in source and binary forms, with
// or without modification, are permitted provided that
// the following conditions are met:
//
// - Redistributions of source code must retain the above 
//   copyright notice, this list of conditions and the
//   following disclaimer.
//
// - Redistributions in binary form must reproduce the above
//   copyright notice, this list of conditions and the
//   following disclaimer in the documentation and/or other
//   materials provided with the distribution.
//
// - Neither the name of Gablarski nor the names of its
//   contributors may be used to endorse or promote products
//   or services derived from this software without specific
//   prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS
// AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
// WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
// INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH
// DAMAGE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Gablarski.OpenAL;

namespace Gablarski.OpenAL
{
	public class SourcePool<T>
		where T : class
	{
		public event EventHandler<SourceFinishedEventArgs<T>> SourceFinished;

		public Source RequestSource (T owner)
		{
			Source free;

			lock (owners)
			{
				if (owner == lastOwner && lastSource != null)
				{
					return lastSource;
				}

				free = owners.Where (kvp => kvp.Value == owner).Select (kvp => kvp.Key).FirstOrDefault();
	
				if (free == null)
				{
					free = owners.Where (kvp => kvp.Value == null).Select (kvp => kvp.Key).FirstOrDefault ();
	
					if (free == null)
					{
						free = Source.Generate ();
					}
	
					owners[free] = owner;
				}

				lastOwner = owner;
				lastSource = free;
			}
			
			return free;
		}

		public void FreeSource (T sourceOwner)
		{
			lock (owners)
			{
				var source = owners.FirstOrDefault (kvp => kvp.Value == sourceOwner).Key;
				if (source != null)
				{
					owners[source] = default (T);

					if (source == lastSource)
						lastSource = null;
				}
			}
		}

		public void FreeSource (Source source)
		{
			lock (owners)
			{
				owners[source] = default(T);

				if (source == lastSource)
					lastSource = null;
			}
		}

		public void FreeSources (IEnumerable<Source> sources)
		{
			lock (owners)
			{
				foreach (Source csource in sources)
				{
					owners[csource] = default (T);

					if (csource == lastSource)
						lastSource = null;
				}
			}
		}

		public void Tick()
		{		
			Source[] tofree = new Source[owners.Count];
			int i = 0;

			lock (owners)
			{
				foreach (Source s in owners.Keys)
				{
					if(!s.IsStopped)
						continue;
					
					tofree[i++] = s;
	
					OnSourceFinished (new SourceFinishedEventArgs<T> (owners[s], s));
				}
	
				for (i = 0; i < tofree.Length && tofree[i] != null; ++i)
				    FreeSource (tofree[i]);
			}
		}

		private readonly Dictionary<Source, T> owners = new Dictionary<Source, T> ();
		private T lastOwner;
		private Source lastSource;

		private void OnSourceFinished (SourceFinishedEventArgs<T> e)
		{
			var finished = this.SourceFinished;
			if (finished != null)
				finished (this, e);
		}
	}

	public class SourceFinishedEventArgs<T>
		: EventArgs
		where T : class 
	{
		public SourceFinishedEventArgs (T owner, Source source)
		{
			this.Owner = owner;
			this.Source = source;
		}

		public T Owner
		{
			get; private set;
		}

		public Source Source
		{
			get; private set;
		}
	}
}