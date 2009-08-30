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
using Valkyrie.Core.Characters;

namespace Valkyrie.Characters
{
	class PokePlayer
		: PokeCharacter
	{
		public bool Loaded
		{
			get;
			set;
		}

		public PokePlayer()
			: base()
		{
			this.Loaded = false;

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
				
				this.Animations.Add("Jump", new FrameAnimation(new Rectangle(0, 168, 27, 56), 1));
				this.Animations.Add("Spin", new FrameAnimation(new Rectangle(0, 224, 28, 41), 4, 0.1f));			
			}

			this.Name = "NULL";
			this.CurrentAnimationName = "South";
		}

		public override void Draw(SpriteBatch spriteBatch)
        {
			if (this.Loaded)
			{
				spriteBatch.Draw(this.Sprite, this.DrawScreenLocation, Animations[this.CurrentAnimationName].FrameRectangle, Color.White);
				spriteBatch.DrawString(PokeGame.font, this.Name, new Vector2(this.DrawScreenLocation.X - (PokeGame.font.MeasureString(this.Name).X / 2) + 16, this.DrawScreenLocation.Y - 15), Color.Black);
			}
		}

		public Vector2 DrawScreenLocation
		{
			get
			{
				Vector2 location = new Vector2();
				location.X = (int)TileEngine.Camera.MapOffset.X + this.Location.X + TileEngine.CurrentMapChunk.TileSize.X / 2 - this.CurrentAnimation.FrameRectangle.Width / 2;
				location.Y = (int)TileEngine.Camera.MapOffset.Y + this.Location.Y + TileEngine.CurrentMapChunk.TileSize.Y - this.CurrentAnimation.FrameRectangle.Height;
				return location;
			}
		}

		public override void Update(GameTime gameTime)
		{
			if (this.Loaded)
			{
				base.Update(gameTime);
			}
		}

		#region Game Methods
		public void AButton()
        {
            if (TileEngine.EventManager.HandleEvent((BaseCharacter)this, ActivationTypes.Activate))
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

		#endregion

	}

	public static class HelperMethods
	{
		// Base point is not abstract so it doesn't matter if you add this extension method.
		// You can't just cast MapPoint to BasePoint
		public static MapPoint ToMapPoint(this BasePoint value)
		{
			return new MapPoint(value.X * TileEngine.TileSize, value.Y * TileEngine.TileSize);
		}
	}
}
