using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Library.Core;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Core.Characters;

namespace Valkyrie.Engine.Characters
{
	public interface IMovable
		: IPositionable, ICollidable
	{
		event EventHandler StoppedMoving;
		event EventHandler StartedMoving;
		event EventHandler TileLocationChanged;
		event EventHandler Collided;

		void OnStoppedMoving(object sender, EventArgs ev);
		void OnStartedMoving(object sender, EventArgs ev);
		void OnTileLocationChanged(object sender, EventArgs ev);
		void OnCollided (object sender, EventArgs e);

		bool IsMoving { get; set; }
		bool IgnoreMoveInput { get; set; }

		Directions Direction { get; set; }

		float Speed { get; set; }
		float MoveDelay { get; set; }
		float LastMoveTime { get; set; }

		ScreenPoint MovingDestination { get; set; }
		bool EndAfterMovementReached { get; set; }

		int Density { get; set; }
	}
}
