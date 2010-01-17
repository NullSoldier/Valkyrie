using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Characters;
using ValkyrieServerLibrary.Entities;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine.Core;
using Microsoft.Xna.Framework;
using Valkyrie.Engine;

namespace ValkyrieServerLibrary.Core
{
	public class ServerNewMovementProvider
	{
		#region Constructors

		public ServerNewMovementProvider (ICollisionProvider collisionprovider)
		{
			this.collisionprovider = collisionprovider;
		}

		#endregion

		public event EventHandler<MovementChangedEventArgs> PlayerMoved;
		public event EventHandler<MovementChangedEventArgs> FailedMovementVerification;

		public void AddToProvider (Character character)
		{
			lock(this.movablecache)
			{
				this.movablecache.Add (character);
			}
		}

		public bool RemoveFromProvider (Character character)
		{
			lock(this.movablecache)
			{
				return this.movablecache.Remove (character);
			}
		}

		public bool ContainsCharacter (Character character)
		{
			lock(this.movablecache)
			{
				return this.movablecache.Contains (character);
			}
		}

		public void Update (GameTime time)
		{
			List<Character> toberemoved = new List<Character> ();
			List<Character> collided = new List<Character> ();

			lock(this.movablecache)
			{
				int count = this.movablecache.Count;

				foreach(var player in this.movablecache)
				{
					MovementInfo moveitem = player.MovementQueue.FirstOrDefault ();

					int speedmodifier = 1;
					bool failedverification = false;

					/* Fire player started moving event and do movement verification */
					if(player.NextMoveActive)
					{
						if(!this.collisionprovider.CheckCollision (player, moveitem.Location))
						{
							// Walking through dense tiles
							if(player.Density == 1)
								failedverification = true;
						}


						if(failedverification)
						{
							// Kick the hacker out
							toberemoved.Add (player);

							var verifyhandler = this.FailedMovementVerification;
							if(verifyhandler != null)
								verifyhandler (this, new MovementChangedEventArgs (player, moveitem.Stage, moveitem.Location));

							continue;
						}
						var handler = this.PlayerMoved;
						if(handler != null)
							handler (this, new MovementChangedEventArgs (player, MovementStage.StartMovement, moveitem.Location));

						player.NextMoveActive = false;
					}

					player.Direction = moveitem.Direction;
					player.Animation = moveitem.Animation;
					player.MovingDestination = moveitem.Location;
					player.LastMoveTime += time.ElapsedGameTime.Milliseconds;

					if(player.LastMoveTime >= player.MoveDelay)
					{
						player.LastMoveTime = 0;

						if(moveitem.Type == MovementType.Destination)
						{
							// Need to get the direction because destination based direction doesn't neccessarily have a specified direction
							player.Direction = this.GetDirectionFromAnimation (player.Animation);

							if(!MoveDestinationBased (player, collided, speedmodifier))
								toberemoved.Add (player);
						}
					}
				}

				foreach(var movable in toberemoved)
				{
					if(collided.Contains (movable))
					{
						movable.OnCollided (this, EventArgs.Empty);
					}

					movable.IsMoving = false;
					movable.IgnoreMoveInput = false;
					movable.NextMoveActive = true;

					// Deque the current end movement
					movable.MovementQueue.Dequeue ();

					if(movable.MovementQueue.Count == 0)
						this.RemoveFromProvider (movable);

					var stophandler = this.PlayerMoved;
					if(stophandler != null)
						stophandler (this, new MovementChangedEventArgs (movable, MovementStage.EndMovement, movable.Location));
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
		private List<Character> movablecache = new List<Character> ();

		private bool MoveDestinationBased (Character movable, List<Character> collided, float speedmodifier)
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

			ScreenPoint destination = new ScreenPoint ((int) x, (int) y);

			movable.Location = destination;

			if(destination == movable.MovingDestination)
				movedok = false;

			return movedok;
		}

		private Directions GetDirectionFromAnimation (string animation)
		{
			if(animation.Contains ("North"))
				return Directions.North;
			else if(animation.Contains ("South"))
				return Directions.South;
			else if(animation.Contains ("East"))
				return Directions.East;
			else if(animation.Contains ("West"))
				return Directions.West;
			else
				return Directions.South;
		}

		private ScreenPoint GetDestinationFromDirection (Character movable, Directions direction)
		{
			return this.GetDestinationFromDirection (movable.Location, direction);
		}

		private ScreenPoint GetDestinationFromDirection (ScreenPoint source, Directions Direction)
		{
			ScreenPoint newSource = new ScreenPoint (source.X, source.Y);

			if(Direction == Directions.North)
				newSource = new ScreenPoint (newSource.X, newSource.Y - 32);
			else if(Direction == Directions.South)
				newSource = new ScreenPoint (newSource.X, newSource.Y + 32);
			else if(Direction == Directions.East)
				newSource = new ScreenPoint (newSource.X + 32, newSource.Y);
			else if(Direction == Directions.West)
				newSource = new ScreenPoint (newSource.X - 32, newSource.Y);

			return newSource;
		}

		public ScreenPoint GetNextScreenPointTile (Character movable)
		{
			return this.GetNextScreenPointTile (movable.Location, movable.Direction);
		}

		public ScreenPoint GetNextScreenPointTile (ScreenPoint point, Directions direction)
		{
			float x = point.X;
			float y = point.Y;

			if(direction == Directions.North)
				y = (point.Y / 32) * 32;
			else if(direction == Directions.South)
				y = ((point.Y + 32) / 32) * 32;

			if(direction == Directions.East)
				x = ((point.X + 32) / 32) * 32;
			else if(direction == Directions.West)
				x = (point.X / 32) * 32;

			return new ScreenPoint ((int) x, (int) y);
		}
	}
}
