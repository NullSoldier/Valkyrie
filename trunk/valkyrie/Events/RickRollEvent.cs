using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using ValkyrieLibrary.Events;
using ValkyrieLibrary.Characters;
using Microsoft.Xna.Framework;

namespace Valkyrie.Events
{
	class RickRollEvent
		: IMapEvent
	{
		public Rectangle Rectangle { get; set; }
		public ActivationTypes Activation { get; set; }
		public Dictionary<string, string> Parameters { get; set; }
		public Directions Direction { get; set; }

		public string GetStringType()
		{
			return "RickRoll";
		}

		public void Trigger(BaseCharacter character)
		{
			Process.Start("http://www.youtube.com/watch?v=Yu_moia-oVI");
		}

		public IEnumerable<string> GetParameterNames()
		{
			return new List<string>();
		}

		public object Clone()
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
