using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Characters;

namespace ValkyrieLibrary.Events.EngineEvents
{
	class EntryPointEvent
		: BaseMapEvent
	{
		public override string GetType()
		{
			return "EntryPoint";
		}

		public override void Trigger(BaseCharacter character)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<string> GetParameterNames()
		{
			// Performance intensive??
			// Consider changing to property or cache
			return (new string[] {"Name"}).ToList();
		}

		public override object Clone()
		{
			EntryPointEvent clone = new EntryPointEvent();
			clone.Rectangle = this.Rectangle;
			clone.Activation = this.Activation;
			clone.Direction = this.Direction;
			clone.Parameters = this.Parameters;

			return clone;
		}
	}
}
