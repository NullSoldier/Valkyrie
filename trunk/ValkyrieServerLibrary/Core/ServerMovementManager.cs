using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Cadenza;
using Microsoft.Xna.Framework;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine.Core;
using Valkyrie.Engine;
using Valkyrie.Library;
using ValkyrieServerLibrary.Entities;

namespace ValkyrieServerLibrary.Core
{
	public class ServerMovementProvider
	{
		#region Constructors

		public ServerMovementProvider (ICollisionProvider collisionprovider)
		{
			this.collisionprovider = collisionprovider;
		}

		#endregion

		public void BeginMove (IMovable movable, Directions direction, string animationname)
		{
			this.AddToCache(movable, new MovementItem(ScreenPoint.Zero, MovementType.TileBased, direction, animationname));

			movable.OnStartedMoving(this, EventArgs.Empty);
		}

		public void EndMoveLocation (IMovable movable, MapPoint destination, string animationname)
		{
			lock(this.movablecache)
			{
				if(!this.movablecache.ContainsKey(movable))
					return;

				this.movablecache[movable].Enqueue(new MovementItem(destination.ToScreenPoint(), MovementType.Destination, Directions.Any, animationname));
			}
		}

		public void Update (GameTime time)
		{
			List<IMovable> toberemoved = new List<IMovable>();
			List<IMovable> collided = new List<IMovable>();

			lock(this.movablecache)
			{
				int count = this.movablecache.Count;

				for(int i = 0; i < count; i++)
				{
					var movable = (Character)this.movablecache.Keys.ElementAt(i);
					MovementItem moveitem = this.movablecache[movable].FirstOrDefault();
					MovementType movetype = moveitem.Type;

					movable.MovingDestination = moveitem.Destination;

					if(movetype == MovementType.TileBased && this.movablecache[movable].Count > 1)
					{
						this.movablecache[movable].Dequeue();
						continue;
					}

					movable.LastMoveTime += time.ElapsedGameTime.Milliseconds;

					if(movable.LastMoveTime >= movable.MoveDelay)
					{
						movable.LastMoveTime = 0;

						if(movetype == MovementType.TileBased)
						{
							movable.Direction = moveitem.Direction;
							movable.Animation = moveitem.AnimationName;

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
						movable.OnCollided(this, EventArgs.Empty);
					}

					this.movablecache[movable].Dequeue();
					movable.IsMoving = false;
					movable.IgnoreMoveInput = false;

					if(this.movablecache[movable].Count == 0)
						this.movablecache.Remove(movable);

					movable.OnStoppedMoving(this, EventArgs.Empty);
				}


			}
		}

		#region IEngineProvider Members

		public void LoadEngineContext (IEngineContext context)
		{
			// This is optional, use the constructor to load

			if(context != null)
				this.collisionprovider = context.CollisionProvider;

			this.isloaded = true;
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		#endregion

		private bool isloaded = false;
		private ICollisionProvider collisionprovider = null;
		// Why is this ordered, why do they need to be in order? It just slows everything down.
		private OrderedDictionary<IMovable, Queue<MovementItem>> movablecache = new OrderedDictionary<IMovable, Queue<MovementItem>>();

		private void AddToCache (IMovable movable, MovementItem moveitem)
		{
			lock(this.movablecache)
			{
				if(!this.movablecache.ContainsKey(movable))
					this.movablecache.Add(movable, new Queue<MovementItem>());

				this.movablecache[movable].Enqueue(moveitem);
			}
		}

		private void AddToCache (IMovable movable, ScreenPoint destination, MovementType type, Directions direction, string animationname)
		{
			lock(this.movablecache)
			{
				if(!this.movablecache.ContainsKey(movable))
					this.movablecache.Add(movable, new Queue<MovementItem>());

				this.movablecache[movable].Enqueue(new MovementItem(destination, type, direction, animationname));
			}
		}

		public void RemoveFromCache (IMovable movable)
		{
			lock(this.movablecache)
			{
				this.movablecache.Remove(movable);
			}
		}

		private void InternalMove (IMovable movable, ScreenPoint destination)
		{
			movable.MovingDestination = destination;
		}

		private bool MoveDestinationBased (IMovable movable, List<IMovable> collided)
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

			movable.Location = destination;

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
			}
			else if(movable.Direction == Directions.South)
			{
				y += movable.Speed;
			}
			else if(movable.Direction == Directions.East)
			{
				x += movable.Speed;
			}
			else if(movable.Direction == Directions.West)
			{
				x -= movable.Speed;
			}
			#endregion

			movable.Location = new ScreenPoint((int)x, (int)y);

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
