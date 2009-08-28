using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Core;
using Microsoft.Xna.Framework;

namespace ValkyrieLibrary.Characters
{
	public interface IMovementManager
	{
		void Move(IMapObject movable, ScreenPoint destination);

		void BeginMove(IMapObject movable, Directions direction);
		void EndMove(IMapObject movable, Boolean fireevent);

		void Update(GameTime time);
	}

	public enum MovementType
	{
		Destination,
		TileBased
	}
}
