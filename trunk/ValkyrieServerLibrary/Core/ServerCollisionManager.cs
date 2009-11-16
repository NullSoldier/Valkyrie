using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Library.Collision;
using Valkyrie.Library.Core;
using Valkyrie.Library.Maps;

namespace ValkyrieServerLibrary.Core
{
	public class ServerCollisionManager
		: CollisionManager
	{
		public ServerCollisionManager (WorldManager worldmanager)
		{
			this.WorldManager = worldmanager;
		}

		public WorldManager WorldManager { get; set; }

		public override bool CheckCollision (ICollidable Source, ScreenPoint Destination)
		{
			if(Source.Density == 0)
				return true;

			// Get value from global x, y
			MapPoint tilePoint = Destination.ToMapPoint();

			if(this.WorldManager.GetGlobalLayerValue(Source.GetWorld(), tilePoint, MapLayers.CollisionLayer) != -1)
				return false;

			return true;
		}
	}
}
