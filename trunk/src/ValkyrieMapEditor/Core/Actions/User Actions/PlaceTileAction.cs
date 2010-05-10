using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Core;
using Valkyrie.Engine;
using Valkyrie.Engine.Maps;

namespace ValkyrieMapEditor.Core.Actions
{
	public class PlaceTileAction : IUserAction
	{
		public PlaceTileAction(MapPoint location, MapLayers layer, int tileid)
		{
			this.location = location;
			this.layer = layer;
		}

		public void Do(IEngineContext context)
		{
			this.Redo(context);
		}

		public void Redo (IEngineContext context)
		{
			var world = context.WorldManager.GetWorlds().Values.FirstOrDefault();
			var map = world.Maps.Values.FirstOrDefault();

			map.Map.SetLayerValue(location, layer, tileid);
		}

		public void Undo(IEngineContext context)
		{
			var world = context.WorldManager.GetWorlds().Values.FirstOrDefault();
			var map = world.Maps.Values.FirstOrDefault();

			map.Map.SetLayerValue(location, layer, lastvalue); 
		}

		private MapPoint location = MapPoint.Zero;
		private MapLayers layer;
		private int tileid = 0;
		private int lastvalue = 0;
	}
}
