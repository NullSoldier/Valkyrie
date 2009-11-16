using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine;

namespace Valkyrie.Providers
{
	public class PokeNetworkProvider
		: INetworkProvider
	{

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		public void LoadEngineContext (IEngineContext context)
		{
			this.context = context;

			this.isloaded = true;
		}

		private IEngineContext context = null;
		private bool isloaded = false;
	}
}
