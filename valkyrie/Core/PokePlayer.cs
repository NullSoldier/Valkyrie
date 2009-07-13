using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Player;
using ValkyrieLibrary.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ValkyrieLibrary;

namespace valkyrie.Core
{
	class PokePlayer : Player, ICollidable
	{
		public bool IsMoving = false;
		public float MoveDelay = 0.002f;
		public int Speed = 2;
		public float LastMoveTime = 0;
		public Point MovingDestination;


        public Point TileLocation
        {
            get { return new Point(this.Location.X / TileEngine.Map.TileSize.X, this.Location.Y / TileEngine.Map.TileSize.Y); }
        }

		public PokePlayer()
		{
			this.CurrentAnimationName = "South";
			this.Name = "NullSoldier";

			if (this.Gender == Genders.Male)
			{
                this.Animations.Add("North", new FrameAnimation(new Rectangle(56, 0, 28, 42), 1));
                this.Animations.Add("South", new FrameAnimation(new Rectangle(56, 84, 28, 42), 1));
                this.Animations.Add("East", new FrameAnimation(new Rectangle(56, 42, 28, 42), 1));
                this.Animations.Add("West", new FrameAnimation(new Rectangle(56, 126, 28, 42), 1));

                this.Animations.Add("WalkNorth", new FrameAnimation(new Rectangle(0, 0, 28, 42), 3));
                this.Animations.Add("WalkEast", new FrameAnimation(new Rectangle(0, 42, 28, 42), 3));
                this.Animations.Add("WalkSouth", new FrameAnimation(new Rectangle(0, 84, 28, 42), 3));
                this.Animations.Add("WalkWest", new FrameAnimation(new Rectangle(0, 126, 28, 42), 3));
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

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString(RPGGame.font, this.Name, new Vector2(this.DrawScreenLocation.X - (RPGGame.font.MeasureString(this.Name).X / 4), this.DrawScreenLocation.Y - 15), Color.Black);
			base.Draw(spriteBatch);
		}

		public override void Update(GameTime gameTime)
		{
			if (!this.IsMoving && this.CurrentAnimationName.Contains("Walk"))
				this.CurrentAnimationName = this.CurrentAnimationName.Substring(4, this.CurrentAnimationName.Length - 4);

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


					if (!TileEngine.CollisionManager.CheckCollision(this, this.MovingDestination))
						ReachedMoveDestination();
					else
						this.Location = new Point(x, y);

					this.LastMoveTime = 0;
				}

				if (this.Location == this.MovingDestination)
					ReachedMoveDestination();
			}

			base.Update(gameTime);
		}

		public void ReachedMoveDestination()
		{
			this.IsMoving = false;
			this.MovingDestination = Point.Zero;
			this.LastMoveTime = 0;
		}



		#region ICollidable Members

		public Point GetLocation()
		{
			return this.Location;
		}

		#endregion
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
