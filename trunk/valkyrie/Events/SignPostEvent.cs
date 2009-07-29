using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Events;
using Valkyrie.Characters;

namespace Valkyrie.Events
{
	public class SignPostEvent
		: BaseMapEvent
	{

		public override string GetType()
		{
			return "SignPost";
		}

		public override void Trigger(BaseCharacter character)
		{
			if (character is PokePlayer)
				((PokePlayer)character).DisplayMessage(this.Parameters["Title"], this.Parameters["Text"]);
		}

		public override IEnumerable<string> GetParameterNames()
		{
			return (new string[] {"Title", "Text"}).ToList();
		}

		public override object Clone()
		{
			SignPostEvent clone = new SignPostEvent();
			clone.Rectangle = this.Rectangle;
			clone.Activation = this.Activation;
			clone.Direction = this.Direction;
			clone.Parameters = this.Parameters;

			return clone;
		}
	}
}
