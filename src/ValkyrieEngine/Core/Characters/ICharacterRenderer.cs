using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Characters;
using Microsoft.Xna.Framework.Graphics;
using Valkyrie.Engine.Maps;

namespace Valkyrie.Engine.Core.Characters
{
	public interface ICharacterRenderer
	{
		void Draw (BaseCharacter character, SpriteBatch spriteBatch, BaseCamera camera);
		void DrawLayer (BaseCharacter character, SpriteBatch spriteBatch, BaseCamera camera, MapLayers layer);
	}
}
