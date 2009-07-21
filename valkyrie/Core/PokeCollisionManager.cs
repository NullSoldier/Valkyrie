using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Collision;
using ValkyrieLibrary;
using Microsoft.Xna.Framework;

namespace ValkyrieLibrary.Core
{
	class PokeCollisionManager
		: CollisionManager
	{

		public override bool CheckCollision(ICollidable Source, ScreenPoint Destination)
		{
			MapPoint tilePoint = TileEngine.GlobalTilePointToLocal(Destination.ToMapPoint());

			if (!TileEngine.CurrentMapChunk.TilePointInMapLocal(tilePoint))
				return true;

			if (TileEngine.CurrentMapChunk.GetLayerValue(tilePoint, Maps.Map.EMapLayer.CollisionLayer) != -1)
				return false;

			return true;
		}
	}
}
