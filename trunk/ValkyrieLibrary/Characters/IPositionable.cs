using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Core;

namespace ValkyrieLibrary.Characters
{
	public interface IPositionable
	{
		MapPoint LastMapLocation { get; set; }
		MapPoint MapLocation { get; }

		MapPoint TileLocation { get; }

		ScreenPoint Location { get; set; }
	}
}
