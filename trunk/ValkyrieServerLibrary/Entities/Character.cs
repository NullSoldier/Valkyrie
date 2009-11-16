using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Valkyrie.Library.Characters;
using Valkyrie.Library.Core;

namespace ValkyrieServerLibrary.Entities
{
	public class Character
		: IMapObject
	{
		public virtual int ID
		{
			get; set;
		}

		public virtual string Name
		{
			get; set;
		}

		public virtual string TileSheet
		{
			get; set;
		}

        public virtual int MapX
        {
            get; set;
        }

		public virtual int MapY
		{
			get; set;
		}

		public virtual Directions Direction
		{
			get; set;
		}

		public virtual string WorldName
		{
			get; set;
		}

		#region Non Persistent Members

		public virtual string MapChunkName { get; set; }

		public virtual bool Moving { get; set; }

		public virtual string Animation { get; set; }

		public virtual ScreenPoint Location { get; set; }

		public virtual MapPoint MapLocation
		{
			get { return new MapPoint(this.Location.X / 32, this.Location.Y / 32); }
			set
			{
				this.Location.X = value.X * 32;
				this.Location.X = value.Y * 32;
			}
		}

		#endregion

		#region ICollidable Members

		public virtual event EventHandler Collided;

		public virtual void OnCollided (object sender, EventArgs e)
		{
			var handler = this.Collided;

			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		public virtual ScreenPoint GetLocation ()
		{
			return this.Location;
		}

		public virtual string GetWorld ()
		{
			return this.WorldName;
		}

		public virtual int Density
		{
			get;
			set;
		}

		#endregion

		#region IMovable Members

		public virtual event EventHandler StoppedMoving;

		public virtual event EventHandler StartedMoving;

		public virtual event EventHandler TileLocationChanged;

		public virtual void OnStoppedMoving (object sender, EventArgs ev)
		{
			var handler = this.StoppedMoving;

			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		public virtual void OnStartedMoving (object sender, EventArgs ev)
		{
			var handler = this.StartedMoving;

			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		public virtual void OnTileLocationChanged (object sender, EventArgs ev)
		{
			var handler = this.TileLocationChanged;

			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		public virtual bool IsMoving
		{
			get;
			set;
		}

		public virtual bool IgnoreMoveInput
		{
			get;
			set;
		}

		public virtual float Speed
		{
			get;
			set;
		}

		public virtual float MoveDelay
		{
			get;
			set;
		}

		public virtual float LastMoveTime
		{
			get;
			set;
		}

		public virtual ScreenPoint MovingDestination
		{
			get;
			set;
		}

		public virtual bool EndAfterMovementReached
		{
			get;
			set;
		}

		#endregion

		#region IPositionable Members

		public virtual MapPoint LastMapLocation
		{
			get;
			set;
		}

		public virtual MapPoint TileLocation
		{
			get { return this.MapLocation; }
		}

		#endregion
	}
}
