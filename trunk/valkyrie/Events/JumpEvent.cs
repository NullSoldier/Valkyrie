using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Library.Events;
using Valkyrie.Characters;
using Microsoft.Xna.Framework;
using Valkyrie.Library;
using Valkyrie.Library.Core;
using Valkyrie.Engine.Events;
using Valkyrie.Engine.Characters;

namespace Valkyrie.Events
{
	public class JumpEvent
		: IMapEvent
	{
		public Rectangle Rectangle { get; set; }
		public ActivationTypes Activation { get; set; }
		public Directions Direction { get; set; }

		public Dictionary<string, string> Parameters { get; set; }

		public string GetStringType ()
		{
			return "Jump";
		}

		public void Trigger (BaseCharacter character)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<string> GetParameterNames ()
		{
			return new string[0];
		}

		public object Clone ()
		{
			JumpEvent clone = new JumpEvent();
			clone.Rectangle = this.Rectangle;
			clone.Activation = this.Activation;
			clone.Direction = this.Direction;
			clone.Parameters = this.Parameters;

			return clone;
		}

		public void Event_StartedMoving (object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		public void Event_StoppedMoving (object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}