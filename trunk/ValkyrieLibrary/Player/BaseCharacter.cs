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
		#region Public Properties
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

		public bool Animating = false;
		#endregion

		public BaseCharacter()
        {
            this.CurrentAnimationName = "South";
			this.Name = string.Empty;

            this.Animations = new Dictionary<string, FrameAnimation>();
            this.Animating = false;
            this.Direction = Directions.South;
            this.Location = new ScreenPoint(0, 0);
            this.Sprite = null;
		}

		#region Methods
		public abstract void Move(ScreenPoint Destination);

		public abstract void Draw(SpriteBatch spriteBatch);

		public abstract void DrawOverlay(SpriteBatch spriteBatch);

		public abstract void Action(String type);

		public virtual BasePoint GetLookPoint()
		{
			BasePoint point = new BasePoint(0, 0);

            if (this.Direction == Directions.North)
				point = new BasePoint(0, -1);

            if (this.Direction == Directions.South)
				point = new BasePoint(0, 1);

            if (this.Direction == Directions.West)
				point = new BasePoint(-1, 0);

            if (this.Direction == Directions.East)
				point = new BasePoint(1, 0);

			return point;
		}

		public abstract void StopMoving();

		public virtual void Update(GameTime gameTime)
		{
			this.CurrentAnimation.Update(gameTime);
		}
		#endregion

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
		Any,
		North,
		East,
		South,
		West
	}
}
