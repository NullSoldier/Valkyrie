using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ValkyrieLibrary.Animation;
using valkyrie.Core;

namespace ValkyrieLibrary.Player
{
    public abstract class BaseCharacter
    {
		public BaseCharacter()
		{
			this.Animations = new Dictionary<string, FrameAnimation>();
			this.Animating = false;

			this.Direction = Directions.South;
		}

        public string Name;

        public Texture2D Sprite;
		public Point Location;
		public Directions Direction;

		public FrameAnimation CurrentAnimation
		{
			get { return this.Animations[this.CurrentAnimationName]; }
		}

		public Dictionary<string, FrameAnimation> Animations;
		public string CurrentAnimationName;
		public bool Animating;

		public Vector2 DrawScreenLocation
		{
			get
			{
				Vector2 location = new Vector2();
				location.X = (int)TileEngine.Camera.MapOffset.X + this.Location.X + TileEngine.Map.TileSize.X/2 - this.CurrentAnimation.FrameRectangle.Width/2 ;
				location.Y = (int)TileEngine.Camera.MapOffset.Y + this.Location.Y + TileEngine.Map.TileSize.Y - this.CurrentAnimation.FrameRectangle.Height;

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

        public abstract void Move(Point Destination);


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
