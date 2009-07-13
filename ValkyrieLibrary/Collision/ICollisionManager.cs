using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ValkyrieLibrary.Collision
{
	public abstract class CollisionManager
	{
		public List<ICollidable> Collidables;

		public abstract bool CheckCollision(ICollidable Source, Point Destination);






	}
}
