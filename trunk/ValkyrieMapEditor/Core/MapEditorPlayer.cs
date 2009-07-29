using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Core;

namespace ValkyrieMapEditor.Core
{
	class MapEditorPlayer
		: BaseCharacter
	{
		public MapEditorPlayer()
		{
			this.Location = new ScreenPoint(0, 0);
		}
	}
}
