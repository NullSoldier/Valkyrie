using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Player;
using ValkyrieLibrary.Animation;
using Microsoft.Xna.Framework;

namespace valkyrie.Core
{
	class PokePlayer : Player
	{
		public bool IsMoving = false;
		public float MoveDelay = 0.002f;
		public int Speed = 3;
		public float LastMoveTime = 0;
		public Point MovingDestination;

		public PokePlayer()
		{
			this.CurrentAnimationName = "South";

			if (this.Gender == Genders.Male)
			{
				this.Animations.Add("North", new FrameAnimation(new Rectangle(64, 0, 32, 44), 1));
                this.Animations.Add("South", new FrameAnimation(new Rectangle(64, 88, 32, 44), 1));
                this.Animations.Add("East", new FrameAnimation(new Rectangle(64, 44, 32, 44), 1));
                this.Animations.Add("West", new FrameAnimation(new Rectangle(64, 132, 32, 44), 1));

                this.Animations.Add("WalkNorth", new FrameAnimation(new Rectangle(0, 0, 32, 44), 3));
                this.Animations.Add("WalkEast", new FrameAnimation(new Rectangle(0, 44, 32, 44), 3));
                this.Animations.Add("WalkSouth", new FrameAnimation(new Rectangle(0, 88, 32, 44), 3));
                this.Animations.Add("WalkWest", new FrameAnimation(new Rectangle(0, 132, 32, 44), 3));
			}

		}

		public override void Move(Point Destination)
		{
			if (this.IsMoving)
				return;

			Directions tmpDirection = this.Location.RelativeDirection(Destination);

			Point point = new Point(Destination.X / TileEngine.Map.TileSize.X, Destination.Y / TileEngine.Map.TileSize.Y);

			if (tmpDirection == Directions.North)
				this.CurrentAnimationName = "WalkNorth";
			else if (tmpDirection == Directions.East)
				this.CurrentAnimationName = "WalkEast";
			else if (tmpDirection == Directions.South)
				this.CurrentAnimationName = "WalkSouth";
			else if (tmpDirection == Directions.West)
				this.CurrentAnimationName = "WalkWest";

			this.MovingDestination = new Point(point.X * TileEngine.Map.TileSize.X, point.Y * TileEngine.Map.TileSize.Y);
			this.IsMoving = true;
		}

		public override void Update(GameTime gameTime)
		{
			if (this.IsMoving)
			{
				this.LastMoveTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (this.LastMoveTime >= this.MoveDelay)
				{
					int x = this.Location.X;
					int y = this.Location.Y;

					if (this.Location.X < this.MovingDestination.X)
					{
						if (x + this.Speed > this.MovingDestination.X)
							x = this.MovingDestination.X;
						else
							x += this.Speed;
					}
					else if (this.Location.X > this.MovingDestination.X)
					{
						if (x - this.Speed < this.MovingDestination.X)
							x = this.MovingDestination.X;
						else
							x -= this.Speed;
					}

					if (this.Location.Y < this.MovingDestination.Y)
					{
						if (y + this.Speed > this.MovingDestination.Y)
							y = this.MovingDestination.Y;
						else
							y += this.Speed;
					}
					else if (this.Location.Y > this.MovingDestination.Y)
					{
						if (y - this.Speed < this.MovingDestination.Y)
							y = this.MovingDestination.Y;
						else
							y -= this.Speed;
					}

					
					this.Location = new Point(x, y);

					this.LastMoveTime = 0;
				}

				if (this.Location == this.MovingDestination)
				{
					this.IsMoving = false;
					this.CurrentAnimationName = this.CurrentAnimationName.Substring(4, this.CurrentAnimationName.Length - 4);
					this.MovingDestination = Point.Zero;
					this.LastMoveTime = 0;
				}
			}

			base.Update(gameTime);
		}

		
	}

	public static class Helper
	{
		public static Directions RelativeDirection(this Point Source, Point Destination)
		{
			Directions direction = new Directions();

			if (Destination.X < Source.X)
				direction |= Directions.West;
			else if (Destination.X > Source.X)
				direction |= Directions.East;

			if (Destination.Y < Source.Y)
				direction |= Directions.North;
			else if (Destination.Y > Source.Y)
				direction |= Directions.South;

			return direction;
		}
	}
}
