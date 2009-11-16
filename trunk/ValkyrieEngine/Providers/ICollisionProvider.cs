using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine.Core;

namespace Valkyrie.Engine.Providers
{
	public interface ICollisionProvider
		: IEngineProvider
	{
		bool CheckCollision (IMovable Source, ScreenPoint Destination);
		bool CheckCollision (IMovable source, MapPoint Destination);
	}
}
