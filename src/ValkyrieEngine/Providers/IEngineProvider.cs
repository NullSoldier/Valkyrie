using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Valkyrie.Engine.Providers
{
	public interface IEngineProvider
	{
		/// <summary>
		/// Loads the provider with an <seealso cref="IEngineProvider"/>. This is only called once by the engine.
		/// </summary>
		/// <param name="context">The <seealso cref="IEngineProvider"/> which provides access to all providers.</param>
		void LoadEngineContext (IEngineContext context);

		/// <summary>
		/// Unloads the provider, called only once when the engine is unloading and preparing to stop.
		/// </summary>
		void Unload ();

		/// <summary>
		/// Determines if the provider can be called by other providers in the engine.
		/// </summary>
		bool IsLoaded { get; }
	}
}
