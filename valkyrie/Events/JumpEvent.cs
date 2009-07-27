using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Events;
using Valkyrie.Characters;

namespace Valkyrie.Events
{
	public class JumpEvent
		: BaseMapEvent
	{
		public override string GetType()
		{
			return "Jump";
		}

		public override void Trigger(BaseCharacter character)
		{
			if (character is PokePlayer)
				((PokePlayer)character).JumpWall();
		}

		public override IEnumerable<string> GetParameterNames()
		{
			return new List<string>();
		}

		public override object Clone()
		{
			JumpEvent clone = new JumpEvent();
			clone.Rectangle = this.Rectangle;
			clone.Activation = this.Activation;
			clone.Direction = this.Direction;
			clone.Parameters = this.Parameters;

			return clone;
		}
	}
}