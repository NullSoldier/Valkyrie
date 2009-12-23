using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Valkyrie.Library;
using Valkyrie.Library.Core;
using Valkyrie.Core.Characters;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Animation;
using Valkyrie.Engine.Characters;

namespace Valkyrie.Characters
{
	public class PokePlayer
		: PokeCharacter
	{
		public PokePlayer ()
			: base()
		{
			this.IsLoaded = false;

			if(this.Gender == Genders.Male)
			{
				this.AddAnimation ("North", new FrameAnimation(new Rectangle(56, 0, 28, 42), 1));
				this.AddAnimation ("South", new FrameAnimation(new Rectangle(56, 84, 28, 42), 1));
				this.AddAnimation ("East", new FrameAnimation(new Rectangle(56, 42, 28, 42), 1));
				this.AddAnimation ("West", new FrameAnimation(new Rectangle(56, 126, 28, 42), 1));

				this.AddAnimation ("WalkNorth", new FrameAnimation(new Rectangle(0, 0, 28, 42), 3));
				this.AddAnimation ("WalkEast", new FrameAnimation(new Rectangle(0, 42, 28, 42), 3));
				this.AddAnimation ("WalkSouth", new FrameAnimation(new Rectangle(0, 84, 28, 42), 3));
				this.AddAnimation ("WalkWest", new FrameAnimation(new Rectangle(0, 126, 28, 42), 3));

				this.AddAnimation ("Jump", new FrameAnimation(new Rectangle(0, 168, 27, 56), 1));
				this.AddAnimation ("Spin", new FrameAnimation(new Rectangle(0, 224, 28, 41), 4, 0.1f));
			}

			this.Name = "NULL";
			this.CurrentAnimationName = "South";
		}

		public override void Update (GameTime gameTime)
		{
			this.CurrentAnimation.Update(gameTime);

			if(this.IsLoaded)
			{
				base.Update(gameTime);
			}
		}

	}

	public static class HelperMethods
	{
		// Base point is not abstract so it doesn't matter if you add this extension method.
		// You can't just cast MapPoint to BasePoint
		public static MapPoint ToMapPoint (this BasePoint value)
		{
			return new MapPoint(value.X * 32, value.Y * 32);
		}
	}
}
