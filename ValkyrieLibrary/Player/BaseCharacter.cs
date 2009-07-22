using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ValkyrieLibrary.Animation;
using ValkyrieLibrary.Events;
using ValkyrieLibrary.Collision;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Maps;
using ValkyrieLibrary.Core;

namespace ValkyrieLibrary.Characters
{
    public abstract class BaseCharacter : ICollidable
    {
        public float MoveDelay = 0.002f;
        public float LastMoveTime = 0;
        public int Speed = 2;

        public bool Animating = false;
        public bool IsMoving = false;
        public bool IsJumping = false;

        public String Name;
        public Texture2D Sprite;
        public ScreenPoint Location;
        public ScreenPoint MovingDestination;
        public Directions Direction;
        public Dictionary<string, FrameAnimation> Animations;
        public String CurrentAnimationName;
		public FrameAnimation CurrentAnimation{get { return this.Animations[this.CurrentAnimationName]; }}

        public MapPoint TileLocation { get { return this.Location.ToMapPoint(); } }
        public MapPoint MapLocation { get { return TileEngine.GlobalTilePointToLocal(TileLocation); } }

		public Vector2 DrawScreenLocation
		{
			get
			{
				Vector2 location = new Vector2();
				location.X = (int)TileEngine.Camera.MapOffset.X + this.Location.X + TileEngine.CurrentMapChunk.TileSize.X/2 - this.CurrentAnimation.FrameRectangle.Width/2 ;
				location.Y = (int)TileEngine.Camera.MapOffset.Y + this.Location.Y + TileEngine.CurrentMapChunk.TileSize.Y - this.CurrentAnimation.FrameRectangle.Height;
				return location;
			}
		}

        public BaseCharacter()
        {
            this.CurrentAnimationName = "South";
            this.Name = "NullSoldier";

            this.Animations = new Dictionary<string, FrameAnimation>();
            this.Animating = false;
            this.Direction = Directions.South;
            this.Location = new ScreenPoint(0, 0);
            this.Sprite = null;
        }

        public virtual void ReachedMoveDestination()
        {
            this.IsMoving = false;
            this.IsJumping = false;
            this.MovingDestination = new ScreenPoint(0, 0); // eh?
            this.LastMoveTime = 0;
        }

        public virtual void Move(ScreenPoint Destination)
        {
            if (this.IsMoving)
                return;

            TileEngine.EventManager.Movement(this);

            Directions tmpDirection = this.Location.ToPoint().RelativeDirection(Destination.ToPoint());

            MapPoint point = Destination.ToMapPoint();

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

            this.MovingDestination = new ScreenPoint(point.X * TileEngine.CurrentMapChunk.TileSize.X, point.Y * TileEngine.CurrentMapChunk.TileSize.Y);

            // Clear the chunk when moving across boundries
            if (!TileEngine.CurrentMapChunk.TilePointInMapGlobal(new MapPoint(point.X, point.Y)))
                TileEngine.ClearCurrentMapChunk();

            this.IsMoving = true;
        }

        public virtual void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
			spriteBatch.Draw(this.Sprite, this.DrawScreenLocation, Animations[this.CurrentAnimationName].FrameRectangle, Color.White);
		}

		public virtual void Update(GameTime gameTime)
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
                        if (!TileEngine.EventManager.Collision(this))
                            ReachedMoveDestination();
                    }
                    else
                    {
                        this.Location = new ScreenPoint(x, y);
                    }

					this.LastMoveTime = 0;
				}

				if (this.Location == this.MovingDestination)
					ReachedMoveDestination();
			}

            this.CurrentAnimation.Update(gameTime);
		}

        public virtual void Action(String type) 
        { 
        }

        public virtual void DisplayMessage(String title, String msg)
        {
        }

        public MapPoint GetLookPoint()
        {
            MapPoint point = new MapPoint(0,0);

            if (this.Direction == Directions.North)
                point = new MapPoint(0, -1);

            if (this.Direction == Directions.South)
                point = new MapPoint(0, 1);

            if (this.Direction == Directions.West)
                point = new MapPoint(-1, 0);

            if (this.Direction == Directions.East)
                point = new MapPoint(1, 0);

            return point;
        }

        public virtual void JumpWall()
        {
            this.IsJumping = true;

            ScreenPoint dest = new ScreenPoint(TileEngine.Player.Location.X, TileEngine.Player.Location.Y);
            ScreenPoint newDest = dest + (GetLookPoint().ToScreenPoint() * 2);
            this.MovingDestination = newDest;
        }

        #region ICollidable Members

        public ScreenPoint GetLocation()
        {
            return this.Location;
        }

        #endregion
    }

	public enum Genders
	{
		Male,
		Female,
		None
	}

	public enum Directions
	{
		North,
		East,
		South,
		West
	}
}
