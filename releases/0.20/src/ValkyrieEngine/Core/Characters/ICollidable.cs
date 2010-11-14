using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Valkyrie.Engine.Core.Characters
{
	public interface ICollidable
	{
		void OnCollided(object sender, EventArgs e);
		event EventHandler Collided;

        Point Size { get; }
        Rectangle BoundingBox { get; }

		int Density { get; set; }
	}
}
