using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Player;
using Microsoft.Xna.Framework;
using ValkyrieLibrary.Animation;

namespace ValkyrieLibrary.Player
{
	public class Player : BaseCharacter
	{
		public Player()
		{
		
		}

		public Genders Gender;
		public int Level;
		public int Gold;


		public override void Move(Point Destination)
		{
			throw new NotImplementedException();
		}
	}
}
