using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Characters;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ValkyrieLibrary.Core;
using ValkyrieLibrary;

namespace Valkyrie.Core.Characters
{
	/// <summary>
	/// A class used for coupling the basic logic for any character in Pokemon Online
	/// </summary>
	class PokeCharacter
		: BaseCharacter
	{
		// Character properties
		public String Name { get; set; }
		public Genders Gender { get; set; }

		public PokeCharacter()
		{
			this.Animating = false;
			this.Speed = 2;
			this.MoveDelay = 0.002f;
			this.LastMoveTime = 0;
			this.IsMoving = false;
			this.Density = 1;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(this.Sprite, new Vector2(this.Location.X, this.Location.Y), Animations[this.CurrentAnimationName].FrameRectangle, Color.White);
		}

		public override void DrawOverlay(SpriteBatch spriteBatch)
		{
		}

		public override void Action(string type)
		{
		}
	}
}
