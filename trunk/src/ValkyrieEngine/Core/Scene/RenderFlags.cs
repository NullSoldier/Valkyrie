using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Valkyrie.Engine.Core.Scene
{
	[Flags]
	public enum RenderFlags
	{
		None = 0,
		NoPlayers = 1,
		NoFog = 2
	}
}
