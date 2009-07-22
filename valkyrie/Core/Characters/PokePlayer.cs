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
using ValkyrieLibrary.Core;
using Valkyrie.Core.Characters;

namespace Valkyrie.Characters
{
	class PokePlayer : PokeCharacter
	{
		public bool IsJumping = false;

		public PokePlayer()
		{
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

		public override void Draw(SpriteBatch spriteBatch)
        {
			spriteBatch.Draw(this.Sprite, this.DrawScreenLocation, Animations[this.CurrentAnimationName].FrameRectangle, Color.White);
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
						if (!TileEngine.EventManager.Collision(this))
							this.StopMoving();
					}
					else
					{
						this.Location = new ScreenPoint(x, y);
					}

					this.LastMoveTime = 0;
				}

				if (this.Location == this.MovingDestination)
					this.StopMoving();
			}

			base.Update(gameTime);
		}

		#region Game Methods
		public void AButton()
        {
            if (TileEngine.EventManager.Action(this))
                return;
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

		public override void DisplayMessage(String title, String msg)
		{
			throw new NotSupportedException();
		}

		public void JumpWall()
        {
            this.IsJumping = true;

            ScreenPoint dest = new ScreenPoint(TileEngine.Player.Location.X, TileEngine.Player.Location.Y);
            ScreenPoint newDest = dest + (new ScreenPoint(this.GetLookPoint().X, this.GetLookPoint().Y) * 2);
            this.MovingDestination = newDest;
        }
		#endregion

		#region Moving Methods
		public override void Move(ScreenPoint Destination)
		{
			if (this.IsMoving)
				return;

			Directions tmpDirection = this.Location.ToPoint().RelativeDirection(Destination.ToPoint());

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

			base.Move(Destination);
		}

		public override void StopMoving()
		{
			this.IsJumping = false;

			base.StopMoving();
		}
		#endregion
	}
}
