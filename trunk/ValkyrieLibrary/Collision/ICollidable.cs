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
		ScreenPoint GetLocation();
	}
}
