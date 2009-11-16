using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Valkyrie.Library.Characters;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Valkyrie.Library.Core;
using Valkyrie.Library;
using Valkyrie.Engine.Characters;

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
	}
}
