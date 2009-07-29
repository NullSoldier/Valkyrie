using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Core;

namespace ValkyrieWorldEditor.Core
{
    class WorldEditorPlayer
		: BaseCharacter
    {
        public WorldEditorPlayer()
        {
            this.Location = new ScreenPoint(0, 0);
        }
    }
}
