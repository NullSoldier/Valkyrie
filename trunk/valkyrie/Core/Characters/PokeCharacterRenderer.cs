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
using Valkyrie.Engine.Maps;

namespace Valkyrie.Library.Core.Characters
{
	public class PokeCharacterRenderer
		: ICharacterRenderer
	{
		public void Draw (BaseCharacter character, SpriteBatch spriteBatch, BaseCamera camera)
		{
			this.DrawUnder (character, spriteBatch, camera);
			this.DrawBase (character, spriteBatch, camera);
			this.DrawMiddle (character, spriteBatch, camera);
			this.DrawTop (character, spriteBatch, camera);
		}

		public void DrawLayer (BaseCharacter character, SpriteBatch spriteBatch, BaseCamera camera, MapLayers layer)
		{
			switch (layer)
			{
				case MapLayers.UnderLayer:
					this.DrawUnder (character, spriteBatch, camera);
					break;
				case MapLayers.BaseLayer:
					this.DrawBase (character, spriteBatch, camera);
					break;
				case MapLayers.MiddleLayer:
					this.DrawMiddle (character, spriteBatch, camera);
					break;
				case MapLayers.TopLayer:
					this.DrawTop (character, spriteBatch, camera);
					break;
			}
		}

		private void DrawUnder (BaseCharacter character, SpriteBatch spriteBatch, BaseCamera camera)
		{
			
		}

		private void DrawBase (BaseCharacter character, SpriteBatch spriteBatch, BaseCamera camera)
		{
			Vector2 location = new Vector2();
			location.X = (int)camera.MapOffset.X + character.Location.X + 32 / 2 - character.CurrentAnimation.FrameRectangle.Width / 2;
			location.Y = (int)camera.MapOffset.Y + character.Location.Y + 32 - character.CurrentAnimation.FrameRectangle.Height;

			spriteBatch.Draw(character.Sprite, location, character.CurrentAnimation.FrameRectangle, Color.White);
		}

		private void DrawMiddle (BaseCharacter character, SpriteBatch spriteBatch, BaseCamera camera)
		{
			
		}

		private void DrawTop (BaseCharacter character, SpriteBatch spriteBatch, BaseCamera camera)
		{
			Vector2 location = new Vector2 ();
			location.X = (int) camera.MapOffset.X + character.Location.X + 32 / 2 - character.CurrentAnimation.FrameRectangle.Width / 2;
			location.Y = (int) camera.MapOffset.Y + character.Location.Y + 32 - character.CurrentAnimation.FrameRectangle.Height;

			Color color = ((character.IsTalking) ? Color.Red : Color.Black);

			if(character is PokeCharacter)
			{
				string name = ((PokeCharacter) character).Name;
				spriteBatch.DrawString (PokeGame.font, name, new Vector2 (location.X - (PokeGame.font.MeasureString (name).X / 2) + 16, location.Y - 15), color);
			}
		}
	}
}
