using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Valkyrie.Library.Core;
using Valkyrie.Engine.Animation;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Maps;
using Valkyrie.Engine.Core.Characters;

namespace Valkyrie.Engine.Characters
{
    public abstract class BaseCharacter
		: IMovable, IAnimatable
	{
		#region Properties

		public object ID
		{
			get { return this.id; }
			set { this.id = value; }
		}

		public Texture2D Sprite
		{
			get { return this.sprite; }
			set { this.sprite = value; }
		}

		#endregion

		#region IAnimatable Members

		public bool IsAnimating
		{
			get { return this.isanimating; }
			set { this.isanimating = value; }
		}

		public String CurrentAnimationName
		{
			get { return this.currentanimationname; }
			set { this.currentanimationname = value; }
		}

		public FrameAnimation CurrentAnimation
		{
			get { return this.animations[this.CurrentAnimationName]; }
		}

		public object AnimationTag
		{
			get { return this.animationtag; }
			set { this.animationtag = value; }
		}

		public void AddAnimation (string name, FrameAnimation animation)
		{
			lock(this.animations)
			{
				this.animations.Add (name, animation);
			}
		}

		public bool RemoveAnimation (string name)
		{
			lock(this.animations)
			{
				return this.animations.Remove (name);
			}
		}

		public bool ContainsAnimation (string name)
		{
			lock(this.animations)
			{
				return this.animations.ContainsKey (name);
			}
		}

		public bool ContainsAnimation (FrameAnimation animation)
		{
			lock(this.animations)
			{
				return this.animations.ContainsValue (animation);
			}
		}

		#endregion

		#region IPositionable Members

		// Local mappoint
		public MapPoint LastMapLocation
		{
			get { return this.lastmaplocation; }
			set { this.lastmaplocation = value; }
		}

		// Local mappoint
		public MapPoint LocalTileLocation
		{
			get
			{
				return this.GlobalTileLocation - this.CurrentMap.MapLocation;
			}
		}

		// Global mappoint
		public MapPoint GlobalTileLocation
		{
			get { return this.Location.ToMapPoint(); }
		}

		// Global screenpoint
		public ScreenPoint Location
		{
			get { return this.location; }
			set
			{
				this.location = value;
				this.LastMapLocation = value.ToMapPoint();
			}
		}

		public string WorldName
		{
			get { return this.worldname; }
			set { this.worldname = value; }
		}

		public MapHeader CurrentMap
		{
			get { return this.currentmap; }
			set { this.currentmap = value; }
		}

		#endregion

		#region IMovable Members

		public void OnStartedMoving (object sender, EventArgs ev)
		{
			var handler = this.StartedMoving;

			if(handler != null)
				handler(this, ev);
		}

		public void OnStoppedMoving (object sender, EventArgs ev)
		{
			var handler = this.StoppedMoving;

			if(handler != null)
				handler(this, ev);
		}

		public void OnTileLocationChanged (object sender, EventArgs ev)
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

		public int Density
		{
			get { return this.density; }
			set { this.density = value; }
		}

		public event EventHandler Collided;

		public void OnCollided (object sender, EventArgs e)
		{
			var handler = this.Collided;

			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		#endregion

		public virtual void Update(GameTime gameTime)
		{
            this.CurrentAnimation.Update(gameTime);
		}

        public MapPoint GetLookValue()
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

		private object id = null;
		private Texture2D sprite = null;
		private Directions direction = Directions.South;
		private ScreenPoint location = ScreenPoint.Zero;
		private MapPoint lastmaplocation = MapPoint.Zero;
		private MapHeader currentmap = null;
		private string worldname = string.Empty;
		private Dictionary<string, FrameAnimation> animations = new Dictionary<string, FrameAnimation> ();
		private string currentworldname = string.Empty;
		private string currentanimationname = string.Empty;
		private object animationtag = string.Empty;
		private bool isanimating = false;
		private int density = 1;
	}

	public enum Genders
	{
		Male,
		Female,
		None
	}
}
