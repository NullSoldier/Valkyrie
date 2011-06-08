using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Library.Collision;
using Valkyrie.Library.Characters;

namespace Valkyrie.Library.Core
{
	public interface IMapObject
		: ICollidable, IMovable, IPositionable
	{
	}
}
