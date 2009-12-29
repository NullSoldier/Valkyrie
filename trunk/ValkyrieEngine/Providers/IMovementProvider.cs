using System;
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
		/// <summary>
		/// Begins directional movement for the <paramref name="movable"/>.
		/// </summary>
		/// <param name="movable">The <seealso cref="IMovable"/> to begin moving.</param>
		/// <param name="direction">The direction the <paramref name="movable"/> should move.</param>
		void BeginMove(IMovable movable, Directions direction);

		/// <summary>
		/// Begins destination based point movement for the <paramref name="movable"/>
		/// </summary>
		/// <param name="movable">The <seealso cref="IMovable"/> to begin moving.</param>
		/// <param name="destination">The destination point to move the <paramref name="movable"/> to.</param>
		void BeginMoveDestination (IMovable movable, ScreenPoint destination);

		/// <summary>
		/// Begins ending of movement the <paramref name="movable"/>.
		/// </summary>
		/// <param name="movable">The <seealso cref="IMovable"/> to end movement for.</param>
		/// <param name="fireevent">Whether or not IMovable.StoppedMoving should be fired.</param>
		/// <param name="forceend">Whether or not movement should absolutely come to a stop and not continue under any circumstances.</param>
		void EndMove (IMovable movable, bool fireevent, bool forceend);

		/// <summary>
		/// Updates and moves all <seealso cref="IMovable"/> objects in the IMovementProvider.
		/// </summary>
		/// <param name="time">The current GameTime.</param>
		void Update(GameTime gameTime);
	}

	public enum MovementType
	{
		Destination,
		TileBased,
	}
}
