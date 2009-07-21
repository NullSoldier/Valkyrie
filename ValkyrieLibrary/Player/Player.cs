using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ValkyrieLibrary.Animation;
using Microsoft.Xna.Framework.Graphics;
using ValkyrieLibrary.Core;

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

        public virtual void DrawOverlay(SpriteBatch spriteBatch)
        {

        }

		public override void Move(ScreenPoint Destination)
		{
			throw new NotImplementedException();
		}
    }
}
