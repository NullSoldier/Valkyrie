using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Cadenza.Collections;
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

		public event EventHandler<MovementChangedEventArgs> PlayerStartedMoving;
		public event EventHandler<MovementChangedEventArgs> PlayerStoppedMoving;
		public event EventHandler<MovementChangedEventArgs> FailedMovementVerification;

		public void BeginMove (Character movable, Directions direction, string animationname)
		{
			//this.AddToCache (movable, new MovementItem (ScreenPoint.Zero, MovementType.TileBased, direction, animationname));

			//movable.OnStartedMoving (this, EventArgs.Empty);
		}

		public void EndMoveLocation (Character movable, MapPoint destination, string animationname)
		{
			//lock(this.movablecache)
			//{
			//    if(!this.movablecache.ContainsKey (movable))
			//        return;

			//    this.movablecache[movable].Enqueue (new MovementItem (destination.ToScreenPoint (), MovementType.Destination, Directions.Any, animationname));
			//}
		}

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
					if(player.MoveHiatus)
					{
						// Add to remove and skip
						toberemoved.Add (player);
						continue;
					}

					MovementInfo moveitem = player.MovementQueue.FirstOrDefault ();

					int speedmodifier = 1;
					bool failedverification = false;

					//Skip to the next moveitem when theres a destination in the queue
					while(moveitem.Type == MovementType.TileBased && player.MovementQueue.Count > 1)
					{
						player.MovementQueue.Dequeue ();
						moveitem = player.MovementQueue.FirstOrDefault ();
					}

					player.Direction = moveitem.Direction;
					player.Animation = moveitem.Animation;

					// Notify player started moving and do verification
					if(player.NextMoveActive && moveitem.Stage == MovementStage.StartMovement)
					{
						// Do start move verification
						if(player.Location.ToMapPoint () != moveitem.Location.ToMapPoint ())
						{
							// Notify the server code that the user has failed a verifiation
							var handlerfailed = this.FailedMovementVerification;
							if(handlerfailed != null)
								handlerfailed (this, new MovementChangedEventArgs (player, MovementStage.StartMovement, moveitem.Location));

							continue;
						}

						var handler = this.PlayerStartedMoving;
						if(handler != null)
							handler (this, new MovementChangedEventArgs (player, MovementStage.StartMovement, moveitem.Location));

						player.NextMoveActive = false;
					}
					else if(moveitem.Stage == MovementStage.EndMovement && moveitem.Location == ScreenPoint.Zero)
					{
						moveitem.Location = this.GetNextScreenPointTile (player);
					}

					// Note: shouldn't matter because movement type determines which method is called
					player.MovingDestination = moveitem.Location;
					player.LastMoveTime += time.ElapsedGameTime.Milliseconds;

					if(player.LastMoveTime >= player.MoveDelay)
					{
						player.LastMoveTime = 0;

						if(moveitem.Type == MovementType.TileBased)
						{
							if(!MoveTileBased (player, collided))
								toberemoved.Add (player);
						}
						else
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

					// Remove players that either collided or reached their destination
					if(movable.ClientMovementQueue.Count > 0)
					{
						bool failvalidation = false;
						var item = movable.ClientMovementQueue.Dequeue();

						// Check if in one tile in direction moving
						MapPoint difference = item.Location.ToMapPoint () - movable.Location.ToMapPoint ();
						if(difference.Y != 0 && (item.Direction == Directions.East || item.Direction == Directions.West))
						{
							failvalidation = true;
						}
						else if(difference.X != 0 && (item.Direction == Directions.North || item.Direction == Directions.South))
						{
							failvalidation = true;
						}

						// Collision on the destination tile
						if (!this.collisionprovider.CheckCollision (movable, item.Location))
						{
							failvalidation = true;
						}


						if(failvalidation)
						{
							// Kick that hacker out!
							this.RemoveFromProvider (movable);

							var handler = this.FailedMovementVerification;
							if(handler != null)
								handler (this, new MovementChangedEventArgs (movable, MovementStage.EndMovement, movable.Location));

							continue;
						}

						movable.Location = item.Location;
						movable.MoveHiatus = false;
					}
					else
					{
						movable.MoveHiatus = true;
					}
					
					movable.IsMoving = false;
					movable.IgnoreMoveInput = false;
					movable.NextMoveActive = true;

					// Deque the current end movement
					movable.MovementQueue.Dequeue ();

					if(movable.MovementQueue.Count == 0)
						this.RemoveFromProvider (movable);

					var stophandler = this.PlayerStoppedMoving;
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

		private bool MoveTileBased (Character movable, List<Character> collided)
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

			movable.Location = new ScreenPoint ((int) x, (int) y);

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

	public class MovementChangedEventArgs
		: EventArgs
	{
		public MovementChangedEventArgs (Character character, MovementStage stage, ScreenPoint location)
		{
			this.Character = character;
			this.Stage = stage;
			this.location = location;
		}

		public Character Character { get; set; }
		public MovementStage Stage { get; set; }
		public ScreenPoint location { get; set; }
	}
}
