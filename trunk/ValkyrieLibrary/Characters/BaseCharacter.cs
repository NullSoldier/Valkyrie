using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Valkyrie.Library.Animation;
using Valkyrie.Library.Events;
using Valkyrie.Library.Collision;
using Valkyrie.Library.Characters;
using Valkyrie.Library.Maps;
using Valkyrie.Library.Core;

namespace Valkyrie.Library.Characters
{
    public abstract class BaseCharacter
		: IMapObject
	{
		public Texture2D Sprite { get; set; }

		public bool Animating { get; set; }
		public Dictionary<string, FrameAnimation> Animations { get; set; }
		public String CurrentAnimationName { get; set; }
		public FrameAnimation CurrentAnimation{get { return this.Animations[this.CurrentAnimationName]; }}

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
            this.Animations = new Dictionary<string, FrameAnimation>();
            this.Animating = false;
            this.Direction = Directions.South;
            this.Location = new ScreenPoint(0, 0);
            this.Sprite = null;
        }

		public abstract void DrawOverlay(SpriteBatch spriteBatch);

		public abstract void Draw(SpriteBatch spriteBatch);

		public virtual void Update(GameTime gameTime)
		{
            this.CurrentAnimation.Update(gameTime);
		}

		public abstract void Action(String type);

        public MapPoint GetLookPoint()
        {
            MapPoint point = new MapPoint(0,0);

            if (this.Direction == Directions.North)
                point = new MapPoint(0, -1);

            else if (this.Direction == Directions.South)
                point = new MapPoint(0, 1);

            else if (this.Direction == Directions.West)
                point = new MapPoint(-1, 0);

            else if (this.Direction == Directions.East)
                point = new MapPoint(1, 0);

            return point;
        }

        #region IPositionable Members
		
		public MapPoint LastMapLocation { get; set; }
		public MapPoint MapLocation { get { return TileEngine.GlobalTilePointToLocal(TileLocation); } }

		public ScreenPoint Location
		{
			get { return this.location; }
			set
			{
				this.LastMapLocation = value.ToMapPoint();
				this.location = value;
			}
		}

		public MapPoint TileLocation { get { return this.Location.ToMapPoint(); } }

		private ScreenPoint location;
        #endregion

		#region IMovable Members

		public void OnStartedMoving(object sender, EventArgs ev)
		{
			var handler = this.StartedMoving;

			if (handler != null)
				handler(this, ev);
		}

		public void OnStoppedMoving(object sender, EventArgs ev)
		{
			var handler = this.StoppedMoving;

			if (handler != null)
				handler(this, ev);
		}

		public void OnTileLocationChanged(object sender, EventArgs ev)
		{
			var handler = this.TileLocationChanged;

			if(handler != null)
				handler(sender, ev);
		}

		public event EventHandler StartedMoving;
		public event EventHandler StoppedMoving;
		public event EventHandler TileLocationChanged;

		public bool IsMoving { get; set; }
		public bool IgnoreMoveInput { get; set; }

		public Directions Direction { get; set; }

		public MapPoint LastMapPoint { get; set; }
		public float LastMoveTime { get; set; }
		public float MoveDelay { get; set; }
		public float Speed { get; set; }

		public bool EndAfterMovementReached { get; set; }
		public ScreenPoint MovingDestination { get; set; }

		public bool IsMovable { get; set; }

		#endregion

		#region ICollidable Members

		public int Density { get; set; }
		public event EventHandler Collided;

		public void OnCollided(object sender, EventArgs e)
		{
			var handler = this.Collided;

			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		public ScreenPoint GetLocation()
		{
			return this.Location;
		}

		public string GetWorld ()
		{
			return TileEngine.WorldManager.CurrentWorld.Name;
		}

		#endregion
	}

	public enum Genders
	{
		Male,
		Female,
		None
	}
}
