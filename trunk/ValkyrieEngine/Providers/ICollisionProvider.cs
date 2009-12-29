using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine.Core;

namespace Valkyrie.Engine.Providers
{
	public interface ICollisionProvider
		: IEngineProvider
	{
		/// <summary>
		/// Checks to see if the <paramref name="source"/> has collided in the <paramref name="destination"/> space.
		/// </summary>
		/// <param name="source">An <seealso cref="IMovable"/> to test for collisions.</param>
		/// <param name="destination">The <seealso cref="ScreenPoint"/> coordinent to use to test for a collision.</param>
		/// <returns>True if there was no collision.</returns>
		bool CheckCollision (IMovable source, ScreenPoint destination);

		/// <summary>
		/// Checks to see if the <paramref name="source"/> has collided in the <paramref name="destination"/> space.
		/// </summary>
		/// <param name="source">An <seealso cref="IMovable"/> to test for collisions.</param>
		/// <param name="destination">The <seealso cref="MapPoint"/> coordinent to use to test for a collision.</param>
		/// <returns>True if there was no collision.</returns>
		bool CheckCollision (IMovable source, MapPoint destination);
	}
}
