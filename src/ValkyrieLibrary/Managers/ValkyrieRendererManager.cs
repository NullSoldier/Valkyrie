using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Managers;
using System.Collections.ObjectModel;
using Valkyrie.Engine.Core.Scene;
using Valkyrie.Engine.Maps;
using Valkyrie.Engine;

namespace Valkyrie.Library.Managers
{
	public class ValkyrieRendererManager : IRendererManager
	{
		public void AddItem (IRenderer renderer, MapLayers layer)
		{
			lock (this.renderers)
			{
				this.renderers[layer].Add(renderer);
			}
		}

		public bool RemoveItem (IRenderer renderer)
		{
			lock (this.renderers)
			{
				foreach (var value in Enum.GetValues(typeof(MapLayers)))
				{
					if (this.renderers[(MapLayers)value].Remove(renderer))
						return true;
				}

				return false;
			}
		}

		public ReadOnlyCollection<IRenderer> GetItems()
		{
			lock (this.renderers)
			{
				return new ReadOnlyCollection<IRenderer>(this.renderers.SelectMany(p => p.Value).ToList());
			}
		}

		public ReadOnlyCollection<IRenderer> GetItems(MapLayers layer)
		{
			lock (this.renderers)
			{
				return new ReadOnlyCollection<IRenderer>(this.renderers.SelectMany(p => p.Value).ToList());
			}
		}

		public ReadOnlyCollection<IRenderer> this[MapLayers layer]
		{
			get { return this.GetItems(layer); }
		}

		#region IEngineProvider Members

		public void LoadEngineContext(IEngineContext context)
		{
			this.isloaded = true;
		}

		public void Unload()
		{
			this.renderers.Clear();

			this.isloaded = false;
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		#endregion

		private bool isloaded = false;

		private readonly Dictionary<MapLayers, List<IRenderer>> renderers = new Dictionary<MapLayers, List<IRenderer>>()
        {
            {MapLayers.BaseLayer, new List<IRenderer>() },
            {MapLayers.CollisionLayer, new List<IRenderer>() },
            {MapLayers.MiddleLayer, new List<IRenderer>() },
            {MapLayers.TopLayer, new List<IRenderer>() },
            {MapLayers.UnderLayer, new List<IRenderer>() }
        };
	}
}
