using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ValkyrieLibrary.Events
{
	class RickRollEvent
		: BaseMapEvent
	{

		public override string GetType()
		{
			return "RickRoll";
		}

		public override void Trigger(ValkyrieLibrary.Characters.BaseCharacter character)
		{
			Process.Start("http://www.youtube.com/watch?v=Yu_moia-oVI");
		}

		public override IEnumerable<string> GetParameterNames()
		{
			return new List<string>();
		}

		public override object Clone()
		{
			RickRollEvent clone = new RickRollEvent();
			clone.Rectangle = this.Rectangle;
			clone.Activation = this.Activation;
			clone.Direction = this.Direction;
			clone.Parameters = this.Parameters;

			return clone;
		}
	}
}
