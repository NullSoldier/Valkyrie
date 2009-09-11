using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Core.Collections;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace ValkyrieLibrary.Core
{
	public class PokeMovementManagerNew
		: IMovementManager
	{
		private OrderedDictionary<IMapObject, MovementType> MovableCache = new OrderedDictionary<IMapObject, MovementType>();
		private object cacheLock = new object();

		public void AddToCache(IMapObject movable, MovementType type)
		{
			lock(this.cacheLock)
			{
				this.MovableCache.Add(movable, type);
			}
		}

		public void RemoveFromCache(IMapObject movable)
		{
			lock (this.cacheLock)
			{
				Trace.WriteLine(String.Format("Removed at: {0}:{1}", DateTime.Now.Minute, DateTime.Now.Millisecond));

				this.MovableCache.Remove(movable);
			}
		}

		#region IMovementManager Members

		public void Move(IMapObject movable, ScreenPoint destination)
		{
			this.Move(movable, destination, true);
		}

		public void Move(IMapObject movable, ScreenPoint destination, bool fireevent)
		{
			if (movable.IsMoving || this.MovableCache.ContainsKey(movable))
				this.EndMove(movable, fireevent);

			movable.IgnoreMoveInput = true;
			movable.IsMoving = true;

			this.AddToCache(movable, MovementType.Destination);

			this.InternalMove(movable, destination);

			movable.OnStartedMoving(this, EventArgs.Empty);
		}

		private void InternalMove(IMapObject movable, ScreenPoint destination)
		{
			movable.MovingDestination = destination;
		}

		public void BeginMove(IMapObject movable, Directions direction)
		{
			if (movable.IsMoving)
			{
				if (movable.Direction == direction)
					return;

				this.EndMove(movable, true);
				//movable.IsMoving = false;
				//movable.IgnoreMoveInput = false;

				//this.RemoveFromCache(movable);

				//movable.OnStoppedMoving(this, EventArgs.Empty);
			}

			if (this.MovableCache.ContainsKey(movable))
				return;

			movable.IsMoving = true;
			movable.Direction = direction;
			
			this.InternalMove(movable, this.GetDestinationFromDirection(movable, movable.Direction));
			
			this.AddToCache(movable, MovementType.TileBased);

			movable.OnStartedMoving(this, EventArgs.Empty);
		}

		public void EndMove(IMapObject movable, bool fireevent)
		{
			if (this.MovableCache.ContainsKey(movable) &&
				this.MovableCache[movable] == MovementType.TileBased &&
				movable.IsMoving)
			{
				ScreenPoint destination = this.GetNextScreenPointTile(movable);
				if (TileEngine.CollisionManager.CheckCollision(movable, destination))
				{
					movable.IgnoreMoveInput = true;

					movable.MovingDestination = destination;
					this.MovableCache[movable] = MovementType.Destination;

					return;
				}
			}

			if ((movable.Location.Y % 32) != 0)
				movable.IsMoving = false;

			movable.IsMoving = false;
			movable.IgnoreMoveInput = false;
			
			this.RemoveFromCache(movable);

			if (fireevent)
				movable.OnStoppedMoving(this, EventArgs.Empty);
		}

		public void Update(GameTime time)
		{
			List<IMapObject> toberemoved = new List<IMapObject>();
			List<IMapObject> collided = new List<IMapObject>();

			lock (this.cacheLock)
			{
				
				int count = this.MovableCache.Count;

				for(int i=0; i < count; i++)
				{
					IMapObject movable = this.MovableCache.Keys.ElementAt(i);
					MovementType movetype = this.MovableCache[movable];

					movable.LastMoveTime += time.ElapsedGameTime.Milliseconds;

					if (movable.LastMoveTime >= movable.MoveDelay)
					{
						movable.LastMoveTime = 0;

						if (movetype == MovementType.TileBased)
						{
							if (!MoveTileBased(movable, collided))
								toberemoved.Add(movable);
						}
						else
						{
							if (!MoveDestinationBased(movable, collided))
								toberemoved.Add(movable);
						}
					}
				}

				foreach (var movable in toberemoved)
				{
					if (collided.Contains(movable))
					{
						movable.IsMoving = false;
						movable.IgnoreMoveInput = false;

						this.RemoveFromCache(movable);

						movable.OnStoppedMoving(this, EventArgs.Empty);
					}
					this.EndMove(movable, true);
				}

				foreach (var movable in collided)
					movable.OnCollided(this, EventArgs.Empty);

				
			}
		}

		private bool MoveDestinationBased(IMapObject movable, List<IMapObject> collided)
		{
			bool movedok = true;

			float x = movable.Location.X;
			float y = movable.Location.Y;

			if (x < movable.MovingDestination.X)
			{
				if (x + movable.Speed > movable.MovingDestination.X)
					x = movable.MovingDestination.X;
				else
					x += movable.Speed;
			}
			else if (x > movable.MovingDestination.X)
			{
				if (x + movable.Speed < movable.MovingDestination.X)
					x = movable.MovingDestination.X;
				else
					x -= movable.Speed;
			}

			if (y > movable.MovingDestination.Y)
			{
				if (y - movable.Speed < movable.MovingDestination.Y)
					y = movable.MovingDestination.Y;
				else
					y -= movable.Speed;
			}
			else if (y < movable.MovingDestination.Y)
			{
				if (y + movable.Speed > movable.MovingDestination.Y)
					y = movable.MovingDestination.Y;
				else
					y += movable.Speed;
			}

			ScreenPoint destination = new ScreenPoint((int)x, (int)y);

			ScreenPoint collision = new ScreenPoint(destination.X, destination.Y);

			if (movable.Direction == Directions.South)
				collision = new ScreenPoint(destination.X, destination.Y + 32);
			else if (movable.Direction == Directions.East)
				collision = new ScreenPoint(destination.X + 32, destination.Y);

			movable.Location = destination;

			if (destination == movable.MovingDestination)
				movedok = false;						

			return movedok;
		}

		private bool MoveTileBased(IMapObject movable, List<IMapObject> collided)
		{
			bool movedok = true;
			float x = movable.Location.X;
			float y = movable.Location.Y;

			#region Destination calculation
			if (movable.Direction == Directions.North)
			{
				y -= movable.Speed;

				if (y < movable.MovingDestination.Y)
					y = movable.MovingDestination.Y;
			}
			else if (movable.Direction == Directions.South)
			{
				y += movable.Speed;

				if (y > movable.MovingDestination.Y)
					y = movable.MovingDestination.Y;
			}
			else if (movable.Direction == Directions.East)
			{
				x += movable.Speed;

				if (x > movable.MovingDestination.X)
					x = movable.MovingDestination.X;
			}
			else if (movable.Direction == Directions.West)
			{
				x -= movable.Speed;

				if (x < movable.MovingDestination.X)
					x = movable.MovingDestination.X;
			}
			#endregion

			ScreenPoint destination = new ScreenPoint((int)x, (int)y);

			ScreenPoint collision = new ScreenPoint(destination.X, destination.Y);

			if (movable.Direction == Directions.South)
				collision = new ScreenPoint(destination.X, destination.Y + 32 - (int)movable.Speed);
			else if (movable.Direction == Directions.East)
				collision = new ScreenPoint(destination.X + 32 - (int)movable.Speed, destination.Y);

			if (!TileEngine.CollisionManager.CheckCollision(movable, collision))
			{
				movable.IsMoving = false;
				movedok = false;

				collided.Add(movable);
			}
			else
			{
				movable.Location = destination;

				if (movable.MapLocation != movable.LastMapLocation)
					movable.OnTileLocationChanged(this, EventArgs.Empty);

				if (movable.Location == movable.MovingDestination)
				{
					movable.MovingDestination = this.GetDestinationFromDirection(movable, movable.Direction);
				}
			}

			return movedok;
		}

		#endregion

		private ScreenPoint GetDestinationFromDirection(IMapObject movable, Directions direction)
		{
			return this.GetDestinationFromDirection(movable.Location, direction);
		}

		private ScreenPoint GetDestinationFromDirection(ScreenPoint source, Directions Direction)
		{
			ScreenPoint newSource = new ScreenPoint(source.X, source.Y);

			if (Direction == Directions.North)
				newSource = new ScreenPoint(newSource.X, newSource.Y - 32);
			else if (Direction == Directions.South)
				newSource = new ScreenPoint(newSource.X, newSource.Y + 32);
			else if (Direction == Directions.East)
				newSource = new ScreenPoint(newSource.X + 32, newSource.Y);
			else if (Direction == Directions.West)
				newSource = new ScreenPoint(newSource.X - 32, newSource.Y);

			return newSource;
		}

		private ScreenPoint GetNextScreenPointTile(IMapObject movable)
		{
			float x = movable.Location.X;
			float y = movable.Location.Y;

			if (movable.Direction == Directions.North)
				y = (movable.Location.Y / 32) * 32;
			else if (movable.Direction == Directions.South)
				y = ((movable.Location.Y + 32) / 32) * 32;

			if (movable.Direction == Directions.East)
				x = ((movable.Location.X + 32) / 32) * 32;
			else if (movable.Direction == Directions.West)
				x = (movable.Location.X / 32) * 32;

			return new ScreenPoint((int)x, (int)y);
		}
	}
}
