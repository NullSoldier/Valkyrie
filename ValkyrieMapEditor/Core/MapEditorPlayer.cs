using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Core.Points;

namespace ValkyrieMapEditor.Core
{
	class MapEditorPlayer : BaseCharacter
	{
		public MapEditorPlayer()
		{
			this.Location = new ScreenPoint(0, 0);
		}

		public override void Move(ScreenPoint Destination)
		{
			
		}

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
		{
			
		}

		public override void DrawOverlay(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
		{
			
		}

		public override void Action(string type)
		{
			throw new NotSupportedException();
		}

		public override BasePoint GetLookPoint()
		{
			throw new NotSupportedException();
		}

		public override void StopMoving()
		{
			
		}
	}
}
