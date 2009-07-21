using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ValkyrieLibrary;
using ValkyrieLibrary.Collision;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Maps;

namespace ValkyrieLibrary.Core
{
	class PokePlayer : Player, ICollidable
	{
		public bool IsMoving = false;
        public bool IsJumping = false;

		public float MoveDelay = 0.002f;
		public int Speed = 2;
		public float LastMoveTime = 0;
		public Point MovingDestination;

        private PokeMessage pokeMessage;

        public Point TileLocation
        {
            get { return new Point(this.Location.X / TileEngine.CurrentMapChunk.TileSize.X, this.Location.Y / TileEngine.CurrentMapChunk.TileSize.Y); }
        }

        public Point MapLocation
        {
            get { return TileEngine.GlobalTilePointToLocal(TileLocation); }
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

        public void DisplayMessage(String title, String msg)
        {
            pokeMessage = new PokeMessage(title, msg);
        }

        public void AButton()
        {
            if (pokeMessage != null)
            {
                if (!pokeMessage.Page())
                    pokeMessage = null;

                return;
            }

            MapEvent e = TileEngine.CurrentMapChunk.GetEvent(MapLocation);
            if (e != null)
            {
                if (e.Type == "SignPost" && e.IsSameFacing(Direction) == true)
                {
                    DisplayMessage(e.ParmOne, e.ParmTwo);
                }
            }
        }

        public override void Action(String type)
        {
            switch (type)
            {
                case "AButton":
                    AButton();
                    break;
            }
        }

		public override void Move(Point Destination)
		{
            if (this.pokeMessage != null)
                return;

			if (this.IsMoving)
				return;

			Directions tmpDirection = this.Location.RelativeDirection(Destination);

			Point point = new Point(Destination.X / TileEngine.CurrentMapChunk.TileSize.X, Destination.Y / TileEngine.CurrentMapChunk.TileSize.Y);

            if (tmpDirection == Directions.North)
            {
                this.CurrentAnimationName = "WalkNorth";
                Direction = Directions.North;
            }
            else if (tmpDirection == Directions.East)
            {
                this.CurrentAnimationName = "WalkEast";
                Direction = Directions.East;
            }
            else if (tmpDirection == Directions.South)
            {
                this.CurrentAnimationName = "WalkSouth";
                Direction = Directions.South;
            }
            else if (tmpDirection == Directions.West)
            {
                this.CurrentAnimationName = "WalkWest";
                Direction = Directions.West;
            }

			this.MovingDestination = new Point(point.X * TileEngine.CurrentMapChunk.TileSize.X, point.Y * TileEngine.CurrentMapChunk.TileSize.Y);
			
			// Clear the chunk when moving across boundries
			if( !TileEngine.CurrentMapChunk.TilePointInMapGlobal(new Point(point.X, point.Y)))
				TileEngine.ClearCurrentMapChunk();

			this.IsMoving = true;
		}

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString(PokeGame.font, this.Name, new Vector2(this.DrawScreenLocation.X - (PokeGame.font.MeasureString(this.Name).X / 4), this.DrawScreenLocation.Y - 15), Color.Black);
			base.Draw(spriteBatch);
		}

        public override void DrawOverlay(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (pokeMessage != null)
                pokeMessage.Draw(spriteBatch);
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


                    if (!this.IsJumping && !TileEngine.CollisionManager.CheckCollision(this, this.MovingDestination))
                    {
                        MapEvent e = TileEngine.CurrentMapChunk.GetEvent(MapLocation);
                        if (e != null && e.Type == "Jump" && e.IsSameFacing(Direction) == true)
                        {
                            JumpWall();
                        }
                        else
                        {
                            ReachedMoveDestination();
                        }
                    }
                    else
                    {
                        this.Location = new Point(x, y);
                    }

					this.LastMoveTime = 0;
				}

				if (this.Location == this.MovingDestination)
					ReachedMoveDestination();
			}

			base.Update(gameTime);
		}

        public void JumpWall()
        {
            this.IsJumping = true;

            Point dest = new Point(TileEngine.Player.Location.X, TileEngine.Player.Location.Y);

            switch (Direction)
            {
                case Directions.North:
                    dest = new Point(TileEngine.Player.Location.X, TileEngine.Player.Location.Y - 64);
                    break;
                case Directions.South:
                    dest = new Point(TileEngine.Player.Location.X, TileEngine.Player.Location.Y + 64);
                    break;
                case Directions.West:
                    dest = new Point(TileEngine.Player.Location.X - 64, TileEngine.Player.Location.Y);
                    break;
                case Directions.East:
                    dest = new Point(TileEngine.Player.Location.X + 64, TileEngine.Player.Location.Y);
                    break;
            }

            this.MovingDestination = dest;
        }

		public void ReachedMoveDestination()
		{
			this.IsMoving = false;
			this.MovingDestination = Point.Zero;
			this.LastMoveTime = 0;
            this.IsJumping = false;
		}



		#region ICollidable Members

		public Point GetLocation()
		{
			return this.Location;
		}

		#endregion
	}
}
