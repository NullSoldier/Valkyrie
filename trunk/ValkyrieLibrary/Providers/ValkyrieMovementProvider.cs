using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cadenza.Collections;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine;
using Valkyrie.Engine.Core;
using Gablarski;
using Microsoft.Xna.Framework;
using Cadenza.Collections;

namespace Valkyrie.Library.Providers
{
	public class ValkyrieMovementProvider
		: IMovementProvider
	{
		public void BeginMove (IMovable movable, Directions direction)
		{
			 if (movable.IsMoving)
			 {
				 if(movable.Direction == direction)
					 return;

				 this.EndMove(movable, true, false);
			 }

			 if(this.MovableCache.ContainsKey(movable))
				 return;

			 movable.IsMoving = true;
			 movable.Direction = direction;

			 this.InternalMove(movable, this.GetDestinationFromDirection(movable, movable.Direction));

			 this.AddToCache(movable, MovementType.TileBased);

			 movable.OnStartedMoving(this, EventArgs.Empty);
		}

		public void BeginMoveDestination (IMovable movable, ScreenPoint destination)
		{
			this.BeginMoveDestination(movable, destination, true);
		}

		public void BeginMoveDestination (IMovable movable, ScreenPoint destination, bool fireevent)
		{
			 if (movable.IsMoving || this.MovableCache.ContainsKey(movable))
				 this.EndMove(movable, fireevent, false);

			 movable.IgnoreMoveInput = true;
			 movable.IsMoving = true;

			 this.AddToCache(movable, MovementType.Destination);

			 this.InternalMove(movable, destination);

			 movable.OnStartedMoving(this, EventArgs.Empty);
		}

		public void EndMove (IMovable movable)
		{
			this.EndMove(movable, true, false);
		}

		public void EndMove (IMovable movable, bool fireevent, bool forceend)
		{
			if(this.MovableCache.ContainsKey(movable) && this.MovableCache[movable] == MovementType.TileBased
				&& movable.IsMoving && !forceend)
			{
				ScreenPoint destination = this.GetNextScreenPointTile(movable);
				if(this.context.CollisionProvider.CheckCollision(movable, destination))
				{
					movable.IgnoreMoveInput = true;

					movable.MovingDestination = destination;
					this.MovableCache[movable] = MovementType.Destination;

					return;
				}
			}

			if((movable.Location.Y % 32) != 0)
				movable.IsMoving = false;

			movable.IsMoving = false;
			movable.IgnoreMoveInput = false;

			this.RemoveFromCache(movable);

			if(fireevent)
				movable.OnStoppedMoving(this, EventArgs.Empty);
		}

		public void Update (GameTime time)
		{
			List<IMovable> toberemoved = new List<IMovable>();
			List<IMovable> collided = new List<IMovable>();

			lock(this.MovableCache)
			{

				int count = this.MovableCache.Count;

				for(int i = 0; i < count; i++)
				{
					IMovable movable = this.MovableCache.Keys.ElementAt(i);
					MovementType movetype = this.MovableCache[movable];

					movable.LastMoveTime += time.ElapsedGameTime.Milliseconds;

					if(movable.LastMoveTime >= movable.MoveDelay)
					{
						movable.LastMoveTime = 0;

						if(movetype == MovementType.TileBased)
						{
							if(!MoveTileBased(movable, collided))
								toberemoved.Add(movable);
						}
						else
						{
							if(!MoveDestinationBased(movable, collided))
								toberemoved.Add(movable);
						}
					}
				}

				foreach(var movable in toberemoved)
				{
					if(collided.Contains(movable))
					{
						movable.IsMoving = false;
						movable.IgnoreMoveInput = false;

						this.RemoveFromCache(movable);
					}
					this.EndMove(movable, true, false);
				}

				foreach(var movable in collided)
					movable.OnCollided(this, EventArgs.Empty);


			}
		}

		#region IEngineProvider Members

		public void LoadEngineContext (IEngineContext context)
		{
			this.context = context;

			this.isloaded = true;
		}

		public void Unload ()
		{
			this.isloaded = false;
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		#endregion

		private bool isloaded = false;
		private IEngineContext context = null;
		private OrderedDictionary<IMovable, MovementType> MovableCache = new OrderedDictionary<IMovable, MovementType>();

		private void AddToCache (IMovable movable, MovementType type)
		{
			lock(this.MovableCache)
			{
				this.MovableCache.Add(movable, type);
			}
		}

		private void RemoveFromCache (IMovable movable)
		{
			lock(this.MovableCache)
			{
				this.MovableCache.Remove(movable);
			}
		}

		private void InternalMove(IMovable movable, ScreenPoint destination)
		{
			movable.MovingDestination = destination;
		}

		private bool MoveDestinationBased(IMovable movable, List<IMovable> collided)
		{
			bool movedok = true;

			float x = movable.Location.X;
			float y = movable.Location.Y;

			if(x < movable.MovingDestination.X)
			{
				if(x + movable.Speed > movable.MovingDestination.X)
					x = movable.MovingDestination.X;
				else
					x += movable.Speed;
			}
			else if(x > movable.MovingDestination.X)
			{
				if(x + movable.Speed < movable.MovingDestination.X)
					x = movable.MovingDestination.X;
				else
					x -= movable.Speed;
			}

			if(y > movable.MovingDestination.Y)
			{
				if(y - movable.Speed < movable.MovingDestination.Y)
					y = movable.MovingDestination.Y;
				else
					y -= movable.Speed;
			}
			else if(y < movable.MovingDestination.Y)
			{
				if(y + movable.Speed > movable.MovingDestination.Y)
					y = movable.MovingDestination.Y;
				else
					y += movable.Speed;
			}

			ScreenPoint destination = new ScreenPoint((int)x, (int)y);

			ScreenPoint collision = new ScreenPoint(destination.X, destination.Y);

			if(movable.Direction == Directions.South)
				collision = new ScreenPoint(destination.X, destination.Y + 32);
			else if(movable.Direction == Directions.East)
				collision = new ScreenPoint(destination.X + 32, destination.Y);


			MapPoint lastpoint = movable.GlobalTileLocation;

			movable.Location = destination;

			if(destination.ToMapPoint () != lastpoint)
				movable.OnTileLocationChanged (movable, EventArgs.Empty);

			if(destination == movable.MovingDestination)
				movedok = false;

			return movedok;
		}

		private bool MoveTileBased (IMovable movable, List<IMovable> collided)
		{
			bool movedok = true;
			float x = movable.Location.X;
			float y = movable.Location.Y;

			#region Destination calculation
			if(movable.Direction == Directions.North)
			{
				y -= movable.Speed;

				if(y < movable.MovingDestination.Y)
					y = movable.MovingDestination.Y;
			}
			else if(movable.Direction == Directions.South)
			{
				y += movable.Speed;

				if(y > movable.MovingDestination.Y)
					y = movable.MovingDestination.Y;
			}
			else if(movable.Direction == Directions.East)
			{
				x += movable.Speed;

				if(x > movable.MovingDestination.X)
					x = movable.MovingDestination.X;
			}
			else if(movable.Direction == Directions.West)
			{
				x -= movable.Speed;

				if(x < movable.MovingDestination.X)
					x = movable.MovingDestination.X;
			}
			#endregion

			ScreenPoint destination = new ScreenPoint((int)x, (int)y);

			ScreenPoint collision = new ScreenPoint(destination.X, destination.Y);

			if(movable.Direction == Directions.South)
				collision = new ScreenPoint(destination.X, destination.Y + 32 - (int)movable.Speed);
			else if(movable.Direction == Directions.East)
				collision = new ScreenPoint(destination.X + 32 - (int)movable.Speed, destination.Y);

			if(!this.context.CollisionProvider.CheckCollision(movable, collision))
			{
				movable.IsMoving = false;
				movedok = false;

				collided.Add(movable);
			}
			else
			{
				MapPoint lastpoint = movable.GlobalTileLocation;

				movable.Location = destination;

				if(destination.ToMapPoint() != lastpoint)
					movable.OnTileLocationChanged (movable, EventArgs.Empty);

				if(movable.Location == movable.MovingDestination)
				{
					movable.MovingDestination = this.GetDestinationFromDirection(movable, movable.Direction);
				}
			}

			return movedok;
		}

		private ScreenPoint GetDestinationFromDirection (IMovable movable, Directions direction)
		{
			return this.GetDestinationFromDirection(movable.Location, direction);
		}

		private ScreenPoint GetDestinationFromDirection (ScreenPoint source, Directions Direction)
		{
			ScreenPoint newSource = new ScreenPoint(source.X, source.Y);

			if(Direction == Directions.North)
				newSource = new ScreenPoint(newSource.X, newSource.Y - 32);
			else if(Direction == Directions.South)
				newSource = new ScreenPoint(newSource.X, newSource.Y + 32);
			else if(Direction == Directions.East)
				newSource = new ScreenPoint(newSource.X + 32, newSource.Y);
			else if(Direction == Directions.West)
				newSource = new ScreenPoint(newSource.X - 32, newSource.Y);

			return newSource;
		}

		private ScreenPoint GetNextScreenPointTile (IMovable movable)
		{
			float x = movable.Location.X;
			float y = movable.Location.Y;

			if(movable.Direction == Directions.North)
				y = (movable.Location.Y / 32) * 32;
			else if(movable.Direction == Directions.South)
				y = ((movable.Location.Y + 32) / 32) * 32;

			if(movable.Direction == Directions.East)
				x = ((movable.Location.X + 32) / 32) * 32;
			else if(movable.Direction == Directions.West)
				x = (movable.Location.X / 32) * 32;

			return new ScreenPoint((int)x, (int)y);
		}
	}
}