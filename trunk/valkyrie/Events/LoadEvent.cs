using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Core;
using ValkyrieLibrary;
using ValkyrieLibrary.Events;

namespace Valkyrie.Events
{
	public class LoadEvent
		: BaseMapEvent
	{
		public override string GetType()
		{
			return "Load";
		}

		public override void Trigger(BaseCharacter character)
		{
			String name = this.Parameters["World"];
			String pos = this.Parameters["Entry"];

			TileEngine.WorldManager.SetWorld(name, pos);
		}

		public override IEnumerable<string> GetParameterNames()
		{
			return (new string[] { "Name", "World", "EntryPointName" }).ToList();
		}

		public override object Clone()
		{
			LoadEvent clone = new LoadEvent();
			clone.Rectangle = this.Rectangle;
			clone.Activation = this.Activation;
			clone.Direction = this.Direction;
			clone.Parameters = this.Parameters;

			return clone;
		}
	}
}
