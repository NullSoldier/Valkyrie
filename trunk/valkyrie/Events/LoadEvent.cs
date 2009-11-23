using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Library.Core;
using Valkyrie.Library;
using Valkyrie.Library.Events;
using Microsoft.Xna.Framework;
using Valkyrie.Engine.Events;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine;

namespace Valkyrie.Events
{
	public class LoadEvent
		: IMapEvent
	{
		public Rectangle Rectangle { get; set; }
		public ActivationTypes Activation { get; set; }
		public Dictionary<string, string> Parameters { get; set; }
		public Directions Direction { get; set; }

		public string GetStringType ()
		{
			return "Load";
		}

		public void Trigger (BaseCharacter character, IEngineContext context)
		{
			String name = this.Parameters["World"];
			String pos = this.Parameters["EntryPointName"];

			throw new NotImplementedException();
		}

		public IEnumerable<string> GetParameterNames ()
		{
			return new string[] { "Name", "World", "EntryPointName" };
		}

		public object Clone ()
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
