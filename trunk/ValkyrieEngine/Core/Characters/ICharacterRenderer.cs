using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Characters;
using Microsoft.Xna.Framework.Graphics;

namespace Valkyrie.Engine.Core.Characters
{
	public interface ICharacterRenderer
	{
		void Draw (BaseCharacter character, SpriteBatch spriteBatch, BaseCamera camera);
		void DrawUnder (BaseCharacter character, SpriteBatch spriteBatch, BaseCamera camera);
		void DrawBase (BaseCharacter character, SpriteBatch spriteBatch, BaseCamera camera);
		void DrawTop (BaseCharacter character, SpriteBatch spriteBatch, BaseCamera camera);
		void DrawOverlays (BaseCharacter character, SpriteBatch spriteBatch, BaseCamera camera);
	}
}
