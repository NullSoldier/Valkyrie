using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine.Providers;
using Valkyrie.Library.Core;
using Microsoft.Xna.Framework;
using Valkyrie.Engine;
using Valkyrie.Characters;
using Valkyrie.Core.Characters;
using Valkyrie.Library;
using Cadenza.Collections;

namespace Valkyrie.Providers
{
	public class NetworkMovementProvider
	{
		#region Constructors

		public NetworkMovementProvider (ICollisionProvider collisionprovider)
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
			lock (this.movablecache)
			{
				if(!this.movablecache.ContainsKey (movable))
					this.movablecache.Add (movable, new Queue<MovementItem> ());
					//return

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
					BaseCharacter player = (BaseCharacter)this.movablecache.Keys.ElementAt(i);
					MovementItem moveitem = this.movablecache[player].FirstOrDefault();

					// Used to speed up if this movable is behind
					int speedmodifier = 1;

					// If we're behind on movement, increase the speed modifier
					if(this.movablecache[player].Count > 6)
						speedmodifier = 5;

					/* If it's TileBased and there is more in the queue, skip it
					 * Or if it's DestinationBased and there are more in the queue and we are at our destination */
					while(moveitem.Type == MovementType.TileBased && this.movablecache[player].Count > 1)
					{
						this.movablecache[player].Dequeue();
						moveitem = this.movablecache[player].FirstOrDefault();
					}

					player.CurrentAnimationName = moveitem.AnimationName;
					player.MovingDestination = moveitem.Destination;
					player.LastMoveTime += time.ElapsedGameTime.Milliseconds;

					if(player.LastMoveTime >= player.MoveDelay)
					{
						player.LastMoveTime = 0;

						if(moveitem.Type == MovementType.TileBased)
						{
							player.Direction = moveitem.Direction;

							if(!MoveTileBased(player, collided))
								toberemoved.Add(player);
						}
						else
						{
							player.Direction = this.GetDirectionFromAnimation (player.CurrentAnimationName);

							if(!MoveDestinationBased(player, collided, speedmodifier))
								toberemoved.Add(player);
						}
					}
				}

				foreach(var movable in toberemoved)
				{
					if(collided.Contains(movable))
					{
						movable.OnCollided(this, EventArgs.Empty);
					}

					// Remove players that either collided or reached their destination
					this.movablecache[movable].Dequeue();
					movable.IsMoving = false;
					movable.IgnoreMoveInput = false;

					if(this.movablecache[movable].Count == 0)
						this.movablecache.Remove(movable);

					movable.OnStoppedMoving(this, EventArgs.Empty);
				}
			}
		}

		public int GetMoveCount (IMovable movable)
		{
			if(!this.movablecache.ContainsKey (movable))
				return 0;
			else return this.movablecache[movable].Count;
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

		private bool MoveDestinationBased (IMovable movable, List<IMovable> collided, float speedmodifier)
		{
			bool movedok = true;

			float x = movable.Location.X;
			float y = movable.Location.Y;
			float speed = movable.Speed * speedmodifier;

			if(x < movable.MovingDestination.X)
			{
				if(x + speed > movable.MovingDestination.X)
					x = movable.MovingDestination.X;
				else
					x += speed;
			}
			else if(x > movable.MovingDestination.X)
			{
				if(x + speed < movable.MovingDestination.X)
					x = movable.MovingDestination.X;
				else
					x -= speed;
			}

			if(y > movable.MovingDestination.Y)
			{
				if(y - speed < movable.MovingDestination.Y)
					y = movable.MovingDestination.Y;
				else
					y -= speed;
			}
			else if(y < movable.MovingDestination.Y)
			{
				if(y + speed > movable.MovingDestination.Y)
					y = movable.MovingDestination.Y;
				else
					y += speed;
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

		private Directions GetDirectionFromAnimation (string animation)
		{
			if(animation.Contains("North"))
				return Directions.North;
			else if(animation.Contains("South"))
				return Directions.South;
			else if(animation.Contains("East"))
				return Directions.East;
			else if(animation.Contains("West"))
				return Directions.West;
			else
				return Directions.South;
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
