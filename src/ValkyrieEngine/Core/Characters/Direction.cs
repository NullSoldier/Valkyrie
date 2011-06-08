using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Valkyrie.Library.Core;

namespace Valkyrie.Engine.Characters
{
	[Flags]
	public enum Directions
	{
		Any = 0,
		North = 2,
		East = 4,
		South = 8,
		West = 16
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
