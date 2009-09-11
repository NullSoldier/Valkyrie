using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ValkyrieLibrary.Characters;

namespace ValkyrieServerLibrary.Entities
{
	public class Character
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

		#region Non Persistent Members
		public virtual Point Location
		{
			get; set;
		}

		public virtual Point MapLocation
		{
			get
			{
				return new Point(this.MapX, this.MapY);
			}
			set
			{
				this.MapX = value.X;
				this.MapY = value.Y;
			}
		}

		public virtual bool Moving
		{
			get; set;
		}

		public virtual string Animation
		{
			get; set;
		}
		#endregion
	}
}
