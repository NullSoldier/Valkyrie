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

		public override bool CheckCollision(ICollidable Source, Point Destination)
		{
			Point tilePoint = TileEngine.GlobalTilePointToLocal(new Point(Destination.X / TileEngine.CurrentMapChunk.TileSize.X, Destination.Y / TileEngine.CurrentMapChunk.TileSize.Y));

			if (!TileEngine.CurrentMapChunk.TilePointInMapLocal(tilePoint))
				return true;

			if (TileEngine.CurrentMapChunk.GetCollisionLayerValue(tilePoint) != -1)
				return false;

			return true;
		}
	}
}
