using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Core.Scene;
using Valkyrie.Engine.Maps;
using System.Collections.ObjectModel;
using Valkyrie.Engine.Providers;

namespace Valkyrie.Engine.Managers
{
	public interface IRendererManager : IEngineProvider
	{
		void AddItem (IRenderer renderer, MapLayers layer);
		bool RemoveItem (IRenderer renderer);

		ReadOnlyCollection<IRenderer> GetItems();
		ReadOnlyCollection<IRenderer> GetItems(MapLayers layer);

		ReadOnlyCollection<IRenderer> this[MapLayers layer] { get; }
	}
}
