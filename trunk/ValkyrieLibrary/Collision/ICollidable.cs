using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ValkyrieLibrary.Core;

namespace ValkyrieLibrary.Collision
{
	public interface ICollidable
	{
		event EventHandler Collided;
		void OnCollided(object sender, EventArgs e);

		ScreenPoint GetLocation();
		int Density { get; set; }
	}
}
