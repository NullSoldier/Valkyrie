using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Core;
using ValkyrieLibrary.Collision;

namespace ValkyrieLibrary.Characters
{
	public interface IMovable
	{
		event EventHandler StoppedMoving;
		event EventHandler StartedMoving;
		event EventHandler TileLocationChanged;

		void OnStoppedMoving(object sender, EventArgs ev);
		void OnStartedMoving(object sender, EventArgs ev);
		void OnTileLocationChanged(object sender, EventArgs ev);

		bool IsMoving { get; set; }
		bool IgnoreMoveInput { get; set; }

		Directions Direction { get; set; }

		float Speed { get; set; }
		float MoveDelay { get; set; }
		float LastMoveTime { get; set; }

		ScreenPoint MovingDestination { get; set; }
		bool EndAfterMovementReached { get; set; }
	}
}
