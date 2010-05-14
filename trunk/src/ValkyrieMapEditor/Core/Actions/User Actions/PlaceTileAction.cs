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
		public PlaceTileAction(int x, int y, MapLayers layer, int tileid)
		{
			this.x = x;
			this.y = y;
			this.tileid = tileid;
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
			
			lastvalue = map.Map.GetLayerValue(x, y, layer);

			map.Map.SetLayerValue(x, y, layer, tileid);
		}

		public void Undo(IEngineContext context)
		{
			var world = context.WorldManager.GetWorlds().Values.FirstOrDefault();
			var map = world.Maps.Values.FirstOrDefault();

			map.Map.SetLayerValue(x, y, layer, lastvalue); 
		}

		private int x;
		private int y;
		private MapLayers layer;
		private int tileid = 0;
		private int lastvalue = 0;
	}
}
