using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Core;
using ValkyrieLibrary;
using ValkyrieLibrary.Events;
using Microsoft.Xna.Framework;

namespace Valkyrie.Events
{
	public class LoadEvent
		: IMapEvent
	{
		public Rectangle Rectangle { get; set; }
		public ActivationTypes Activation { get; set; }
		public Dictionary<string, string> Parameters { get; set; }
		public Directions Direction { get; set; }

		public string GetStringType()
		{
			return "Load";
		}

		public void Trigger(BaseCharacter character)
		{
			String name = this.Parameters["World"];
			String pos = this.Parameters["EntryPointName"];

			TileEngine.WorldManager.SetWorld(name, pos, true);
		}

		public IEnumerable<string> GetParameterNames()
		{
			return new string[] { "Name", "World", "EntryPointName" };
		}

		public object Clone()
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
