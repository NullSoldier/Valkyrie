﻿using System;
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

		#region BaseCharacter Methods
		public override void DrawOverlay(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
		{
			throw new NotImplementedException();
		}

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
		{
			throw new NotImplementedException();
		}

		public override void Action(string type)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
