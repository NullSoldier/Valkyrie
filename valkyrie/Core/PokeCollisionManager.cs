using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Collision;
using ValkyrieLibrary;
using Microsoft.Xna.Framework;

namespace valkyrie.Core
{
	class PokeCollisionManager
		: CollisionManager
	{

		public override bool CheckCollision(ICollidable Source, Point Destination)
		{
			Point tilePoint = new Point(Destination.X / TileEngine.Map.TileSize.X, Destination.Y / TileEngine.Map.TileSize.Y);

			if (TileEngine.Map.GetCollisionLayerValue(tilePoint) != -1)
				return false;



			return true;
		}
	}
}
