using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Core.Characters;
using Valkyrie.Engine.Characters;
using Microsoft.Xna.Framework.Graphics;
using Valkyrie.Engine;
using Microsoft.Xna.Framework;
using Valkyrie.Core.Characters;

namespace Valkyrie.Library.Core.Characters
{
	public class PokeCharacterRenderer
		: ICharacterRenderer
	{
		public void Draw (BaseCharacter character, SpriteBatch spriteBatch, BaseCamera camera)
		{
			Vector2 location = new Vector2();
			location.X = (int)camera.MapOffset.X + character.Location.X + 32 / 2 - character.CurrentAnimation.FrameRectangle.Width / 2;
			location.Y = (int)camera.MapOffset.Y + character.Location.Y + 32 - character.CurrentAnimation.FrameRectangle.Height;

			spriteBatch.Draw(character.Sprite, location, character.CurrentAnimation.FrameRectangle, Color.White);

			if(character is PokeCharacter)
			{
				string name = ((PokeCharacter)character).Name;
				spriteBatch.DrawString(PokeGame.font, name, new Vector2(location.X - (PokeGame.font.MeasureString(name).X / 2) + 16, location.Y - 15), Color.Black);
			}
		}
	}
}
