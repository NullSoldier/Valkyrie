﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine.Core;
using Microsoft.Xna.Framework;

namespace Valkyrie.Engine.Providers
{
	public interface IMovementProvider
		: IEngineProvider
	{
		void BeginMove(IMovable movable, Directions directioon);
		void BeginMoveDestination (IMovable movable, ScreenPoint destination);
		void EndMove (IMovable movable, bool fireevent);

		void Update(GameTime time);
	}

	public enum MovementType
	{
		Destination,
		TileBased,
	}
}