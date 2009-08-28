using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Collision;
using ValkyrieLibrary;
using Microsoft.Xna.Framework;
using ValkyrieLibrary.Core;
using ValkyrieLibrary.Maps;
using ValkyrieLibrary.Events;

namespace Valkyrie.Core
{
	class PokeCollisionManager
		: CollisionManager
	{

		public override bool CheckCollision(ICollidable Source, ScreenPoint Destination)
		{
			if (Source.Density == 0)
				return true;

			MapPoint tilePoint = TileEngine.GlobalTilePointToLocal(Destination.ToMapPoint());

			if (!TileEngine.CurrentMapChunk.TilePointInMapLocal(tilePoint))
				return true;

			if (TileEngine.CurrentMapChunk.GetLayerValue(tilePoint, MapLayers.CollisionLayer) != -1)
				return false;

			return true;
		}
	}
}
