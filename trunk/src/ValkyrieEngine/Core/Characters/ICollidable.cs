using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Valkyrie.Engine.Core.Characters
{
	public interface ICollidable
	{
		event EventHandler Collided;
		int Density { get; set; }
        Point Size { get; }
        Rectangle BoundingBox { get; }

		void OnCollided (object sender, EventArgs e);
	}
}
