using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Characters;
using Microsoft.Xna.Framework;
using ValkyrieLibrary.Core.Collections;
using ValkyrieLibrary.Core.Messages;
using Valkyrie.Characters;

namespace ValkyrieLibrary.Core
{
	public class PokeMovementManager
		: IMovementManager
	{
		public OrderedDictionary <IMapObject, MovementType> MovableCache = new OrderedDictionary <IMapObject, MovementType> ();

		private void RemoveMapObject(IMapObject value)
		{
			this.MovableCache.Remove(value);
		}

		private void AddMapObject(IMapObject value, MovementType movement)
		{
			this.MovableCache.Add(value, movement);
			//if (this.MovableCache.ContainsKey(value))
			//	this.MovableCache[value] = movement;
		}
		#region IMovementManager Members

		public void Move(IMapObject movable, ScreenPoint destination)
		{
			if (movable.IsMoving)
			{
				this.EndMove(movable, false);
				this.RemoveMapObject(movable);
			}

			this.RemoveMapObject(movable);
			this.AddMapObject(movable, MovementType.Destination);

			movable.IsMoving = true;
			movable.OnStartedMoving(this, EventArgs.Empty);
			movable.EndAfterMovementReached = true;
			movable.MovingDestination = destination;
		}

		public void BeginMove(IMapObject movable, Directions direction)
		{
			if (movable.EndAfterMovementReached)
				return;

			if ((movable.Direction == direction && movable.IsMoving))
				return; // Already going in that direction

			if (movable.IsMoving)
			{
				this.EndMove(movable, false);
				this.RemoveMapObject(movable);
			}
			
			movable.IsMoving = true;
			movable.Direction = direction;
			movable.EndAfterMovementReached = true;

			this.AddMapObject(movable, MovementType.TileBased);
			movable.OnStartedMoving(this, EventArgs.Empty);
		}

		public void EndMove(IMapObject movable, bool fireEvent)
		{
			if (this.MovableCache.ContainsKey(movable))
			{

				if (this.MovableCache[movable] == MovementType.TileBased)
				{
					this.MovableCache[movable] = MovementType.Destination;

					movable.EndAfterMovementReached = true;
					movable.MovingDestination = this.GetNextScreenPointTile(movable);

					return;
				}
			}

			movable.IsMoving = false;
			movable.EndAfterMovementReached = false;
			if (fireEvent)
				movable.OnStoppedMoving(this, EventArgs.Empty);
		}

		public void Update(GameTime gameTime)
		{
			List<IMapObject> tmp = new List<IMapObject>();

			int count = this.MovableCache.Count();

			for (int i = 0; i < count; i++)
			{
				IMapObject movable = this.MovableCache.Keys.ElementAt(i);

				if (!movable.IsMoving)
				{
					tmp.Add(movable);
					continue;
				}

				movable.LastMoveTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

				bool result = true;
				if (movable.LastMoveTime >= movable.MoveDelay)
				{
					movable.LastMoveTime = 0;
			
					// Check movement and get the result, false = stopped moving for some reason, true = moved successfully
					if (this.MovableCache[movable] == MovementType.Destination)
						result = this.MoveDestinationBased(movable, gameTime);
					else
						result = this.MoveTileBased(movable, gameTime);

					if(!result) // Remove from the cache because it stopped moving
						tmp.Add(movable);
				}
			}

			// Remove them if they stopped moving this time
			foreach(var tmpObject in tmp)
				this.RemoveMapObject(tmpObject);
		}

		#endregion

		public bool MoveTileBased(IMapObject movable, GameTime gameTime)
		{
			float newx = movable.Location.X;
			float newy = movable.Location.Y;

			float checkx = 0;
			float checky = 0;

			if (movable.Direction == Directions.North)
			{
				newy -= movable.Speed;
			}
			if (movable.Direction == Directions.South)
			{
				newy += movable.Speed;
				checky += TileEngine.TileSize;
			}
			if (movable.Direction == Directions.West)
			{
				newx -= movable.Speed;
			}
			if (movable.Direction == Directions.East)
			{
				newx += movable.Speed;
				checkx += 32; 
			}

			ScreenPoint testPoint = new ScreenPoint((int)(newx + checkx), (int)(newy + checky));

			if (!TileEngine.CollisionManager.CheckCollision(movable, testPoint))
			{
				this.EndMove(movable, true);
				movable.OnCollided(this, EventArgs.Empty);
				return false;
			}

			movable.Location = new ScreenPoint((int)newx, (int)newy);

			if (movable.MapLocation != movable.LastMapLocation)
				movable.OnTileLocationChanged(this, EventArgs.Empty);

			return true;
		}

		public bool MoveDestinationBased(IMapObject movable, GameTime gameTime)
		{
			float x = movable.Location.X;
			float y = movable.Location.Y;

			if (movable.Location.X < movable.MovingDestination.X)
			{
				if (x + movable.Speed > movable.MovingDestination.X)
					x = movable.MovingDestination.X;
				else
					x += movable.Speed;
			}
			else if (movable.Location.X > movable.MovingDestination.X)
			{
				if (x - movable.Speed < movable.MovingDestination.X)
					x = movable.MovingDestination.X;
				else
					x -= movable.Speed;
			}

			if (movable.Location.Y < movable.MovingDestination.Y)
			{
				if (y + movable.Speed > movable.MovingDestination.Y)
					y = movable.MovingDestination.Y;
				else
					y += movable.Speed;
			}
			else if (movable.Location.Y > movable.MovingDestination.Y)
			{
				if (y - movable.Speed < movable.MovingDestination.Y)
					y = movable.MovingDestination.Y;
				else
					y -= movable.Speed;
			}

			if (!TileEngine.CollisionManager.CheckCollision(movable, movable.MovingDestination))
			{
				movable.OnCollided(this, EventArgs.Empty);

				this.EndMove(movable, true);
				return false;
			}
			else
			{
				movable.Location = new ScreenPoint((int)x, (int)y);

				if (movable.MapLocation != movable.LastMapLocation)
					movable.OnTileLocationChanged(this, EventArgs.Empty);
			}

			// Reached destination
			if (movable.Location == movable.MovingDestination)
			{
				this.EndMove(movable, true);
				return false;
			}

			return true;
		}

		private ScreenPoint GetNextScreenPointTile(IMapObject movable)
		{
			float x = movable.Location.X;
			float y = movable.Location.Y;

			if (movable.Direction == Directions.North)
				y = (movable.Location.Y / 32) * 32;
			else if(movable.Direction == Directions.South)
				y = ((movable.Location.Y + 32) / 32) * 32;

			if (movable.Direction == Directions.East)
				x = ((movable.Location.X + 32) / 32) * 32;
			else if(movable.Direction == Directions.West)
				x = (movable.Location.X / 32) * 32;

			return new ScreenPoint((int)x, (int)y);
		}
	}
}
