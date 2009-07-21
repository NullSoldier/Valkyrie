using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ValkyrieLibrary;
using ValkyrieLibrary.Collision;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Maps;
using ValkyrieLibrary.Core;

namespace ValkyrieLibrary.Characters
{
	class PokePlayer : Player
	{
		public PokePlayer()
		{
			if (this.Gender == Genders.Male)
			{
                this.Animations.Add("North", new FrameAnimation(new Rectangle(56, 0, 28, 42), 1));
                this.Animations.Add("South", new FrameAnimation(new Rectangle(56, 84, 28, 42), 1));
                this.Animations.Add("East", new FrameAnimation(new Rectangle(56, 42, 28, 42), 1));
                this.Animations.Add("West", new FrameAnimation(new Rectangle(56, 126, 28, 42), 1));

                this.Animations.Add("WalkNorth", new FrameAnimation(new Rectangle(0, 0, 28, 42), 3));
                this.Animations.Add("WalkEast", new FrameAnimation(new Rectangle(0, 42, 28, 42), 3));
                this.Animations.Add("WalkSouth", new FrameAnimation(new Rectangle(0, 84, 28, 42), 3));
                this.Animations.Add("WalkWest", new FrameAnimation(new Rectangle(0, 126, 28, 42), 3));
			}
		}

        public override void DisplayMessage(String title, String msg)
        {
        }

        public void AButton()
        {
            if (TileEngine.EventManager.Action(this))
                return;
        }

        public override void Action(String type)
        {
            switch (type)
            {
                case "AButton":
                    AButton();
                    break;
            }
        }

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString(PokeGame.font, this.Name, new Vector2(this.DrawScreenLocation.X - (PokeGame.font.MeasureString(this.Name).X / 4), this.DrawScreenLocation.Y - 15), Color.Black);
			base.Draw(spriteBatch);
		}
	}
}
