using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ValkyrieLibrary.Core;

namespace ValkyrieLibrary.Characters
{
    public enum MoveDirection
    {
        // 8 Directional movement
        North = 0,
        South,
        East,
        West,
        NorthEast,
        NorthWest,
        SouthEast,
        SouthWest
    }

	public static class DirectionsHelper
	{
		public static Directions RelativeDirection(this Point Source, Point Destination)
		{
			Directions direction = new Directions();

			if (Destination.X < Source.X)
				direction |= Directions.West;
			else if (Destination.X > Source.X)
				direction |= Directions.East;

			if (Destination.Y < Source.Y)
				direction |= Directions.North;
			else if (Destination.Y > Source.Y)
				direction |= Directions.South;

			return direction;
		}
	}
}
