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
        public bool Animating = false;

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

        public virtual void StopMoving()
        {

        }

        public virtual void DrawOverlay(SpriteBatch spriteBatch)
        {
        }

        public virtual void Move(ScreenPoint Destination)
        {
        }

        public virtual void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
		}

		public virtual void Update(GameTime gameTime)
		{
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
		Any=0,
		North=2,
		East=4,
		South=8,
		West=16
	}
}
