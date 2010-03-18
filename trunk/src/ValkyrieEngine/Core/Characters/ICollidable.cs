using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Valkyrie.Engine.Core.Characters
{
	public interface ICollidable
	{
		event EventHandler Collided;
		int Density { get; set; }

		void OnCollided (object sender, EventArgs e);
	}
}
