using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine.Characters;

namespace Valkyrie.Library
{
	public class MovementItem
	{
		public MovementItem (ScreenPoint destination, MovementType type, Directions direction, string animationname)
		{
			this.destination = destination;
			this.type = type;
			this.direction = direction;
			this.animationname = animationname;
		}

		public ScreenPoint Destination
		{
			get { return this.destination; }
			set { this.destination = value; }
		}

		public MovementType Type
		{
			get { return this.type; }
			set { this.type = value; }
		}

		public Directions Direction
		{
			get { return this.direction; }
			set { this.direction = value; }
		}

		public string AnimationName
		{
			get { return this.animationname; }
			set { this.animationname = value; }
		}

		private ScreenPoint destination;
		private MovementType type;
		private Directions direction;
		private string animationname;
	}
}
