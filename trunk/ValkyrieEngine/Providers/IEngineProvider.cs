using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Valkyrie.Engine.Providers
{
	public interface IEngineProvider
	{
		void LoadEngineContext (IEngineContext context);
		void Unload ();
		bool IsLoaded { get; }
	}
}
