using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Library.Core;
using Valkyrie.Engine.Core;

namespace Valkyrie.Engine.Characters
{
	public interface IPositionable
	{
		string WorldName { get; set; }

		MapPoint LocalTileLocation { get; }
		MapPoint GlobalTileLocation { get; }

		ScreenPoint Location { get; set; }
	}
}
