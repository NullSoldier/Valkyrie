using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Library.Core;
using Microsoft.Xna.Framework;

namespace Valkyrie.Library.Characters
{
	public interface IMovementManager
	{
		void Move(IMapObject movable, ScreenPoint destination);
		void Move(IMapObject movable, ScreenPoint destination, Boolean fireevent);

		void BeginMove(IMapObject movable, Directions direction);
		void EndMove(IMapObject movable, Boolean fireevent);
		void EndMoveFunctional (IMapObject movable);

		void Update(GameTime time);
	}

	public enum MovementType
	{
		Destination,
		TileBased,
	}
}
