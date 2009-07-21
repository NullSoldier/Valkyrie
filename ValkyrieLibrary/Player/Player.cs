using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ValkyrieLibrary.Animation;

namespace ValkyrieLibrary.Characters
{
	public class Player : BaseCharacter
	{
		public Player()
		{
		
		}

		public Genders Gender;
		public int Level;
		public int Gold;

        public virtual void DrawOverlay(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {

        }

		public override void Move(Point Destination)
		{
			throw new NotImplementedException();
		}
	}
}
