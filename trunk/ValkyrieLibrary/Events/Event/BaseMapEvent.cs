using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Core;
using ValkyrieLibrary.Characters;
using Microsoft.Xna.Framework;

namespace ValkyrieLibrary.Events
{
	/*
	public abstract class BaseMapEvent
		: ICloneable
	{
		
		public BaseMapEvent() { }

		public BaseMapEvent(MapPoint location, MapPoint size)
			: this( new Rectangle(location.X, location.Y, size.X, size.Y) )	{ }

		public BaseMapEvent(Rectangle rect)
		{
			this.Rectangle = rect;
		}

		#region IMapEvent Members

		public Rectangle Rectangle { get; set; }

		public ActivationTypes Activation { get; set; }
		public Directions Direction { get; set; }
		public Dictionary<String, String> Parameters { get; set; }

		public abstract string GetStringType();
		public abstract void Trigger(BaseCharacter character);
		public abstract IEnumerable<string> GetParameterNames();

		// Backwards compatability with older code, but it should use Rectangle
		public virtual BasePoint Location
		{
			get { return new BasePoint(this.Rectangle.X, this.Rectangle.Y); }
		}

		public virtual BasePoint Size
		{
			get { return new BasePoint(this.Rectangle.Width, this.Rectangle.Height); }
		}

		#endregion

		#region ICloneable Members

		public abstract object Clone();

		#endregion
	}
		 */
}
