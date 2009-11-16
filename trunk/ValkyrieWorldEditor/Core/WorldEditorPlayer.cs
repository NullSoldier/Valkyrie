using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Library.Characters;
using Valkyrie.Library.Core;

namespace ValkyrieWorldEditor.Core
{
    class WorldEditorPlayer
		: BaseCharacter
    {
        public WorldEditorPlayer()
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
