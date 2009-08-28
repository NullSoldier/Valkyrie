using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Collision;
using ValkyrieLibrary.Characters;

namespace ValkyrieLibrary.Core
{
	public interface IMapObject
		: ICollidable, IMovable, IPositionable
	{
	}
}
