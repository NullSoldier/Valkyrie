using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ValkyrieLibrary.Animation;
using ValkyrieLibrary.Core;

namespace ValkyrieLibrary.Characters
{
    public abstract class BaseCharacter
    {
        public bool IsMoving = false;
        public bool IsJumping = false;
        public string Name;

        public Texture2D Sprite;
        public ScreenPoint Location;
        public Directions Direction;

        public Dictionary<string, FrameAnimation> Animations;
        public string CurrentAnimationName;
        public bool Animating;

		public BaseCharacter()
		{
			this.Animations = new Dictionary<string, FrameAnimation>();
			this.Animating = false;

			this.Direction = Directions.South;

            this.Location = new ScreenPoint(0, 0);
            this.Sprite = null;
		}


		public FrameAnimation CurrentAnimation
		{
			get { return this.Animations[this.CurrentAnimationName]; }
		}

        public virtual void ReachedMoveDestination()
        {
            this.IsMoving = false;
            this.IsJumping = false;
        }

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

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(this.Sprite, this.DrawScreenLocation, Animations[this.CurrentAnimationName].FrameRectangle, Color.White);
		}

		public virtual void Update(GameTime gameTime)
		{
			this.CurrentAnimation.Update(gameTime);
		}

        public abstract void Move(ScreenPoint Destination);

        public virtual void Action(String type) { }

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
