using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine.Events;
using Valkyrie.Engine;

namespace Valkyrie.Library.Events
{
	public class EntryPointEvent
		: IMapEvent
	{
		public Dictionary<string, string> Parameters { get; set; }
		public Directions Direction { get; set; }
		public ActivationTypes Activation { get; set; }

		public Rectangle Rectangle { get; set; }

		public string GetStringType()
		{
			return "EntryPoint";
		}

		public void Trigger(BaseCharacter character, IEngineContext context)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<string> GetParameterNames()
		{
			// Performance intensive??
			// Consider changing to property or cache
			return (new string[] {"Name"}).ToList();
		}

		public object Clone()
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
