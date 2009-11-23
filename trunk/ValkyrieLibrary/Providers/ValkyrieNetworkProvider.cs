using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine;

namespace Valkyrie.Library.Providers
{
	public class ValkyrieNetworkProvider
		: INetworkProvider
	{
		#region IEngineProvider Members

		public void LoadEngineContext (IEngineContext context)
		{

			this.isloaded = true;
		}

		public bool IsLoaded
		{
			get { return this.isloaded = true; }
		}

		private bool isloaded = false;

		#endregion
	}
}
