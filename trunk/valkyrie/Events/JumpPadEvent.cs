using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Library.Core;
using Valkyrie.Library.Events;
using System.Diagnostics;
using Valkyrie.Characters;
using Valkyrie.Library;
using Microsoft.Xna.Framework;
using Valkyrie.Engine.Events;
using Valkyrie.Engine.Characters;

namespace Valkyrie.Events
{

	public class JumpPadEvent
		: IMapEvent
	{
		public Rectangle Rectangle { get; set; }
		public Dictionary<string, string> Parameters { get; set; }
		public ActivationTypes Activation { get; set; }
		public Directions Direction { get; set; }

		public string GetStringType ()
		{
			return "JumpPad";
		}

		public void Trigger (BaseCharacter character)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<string> GetParameterNames ()
		{
			return new string[] { "DestinationName", "Speed" };
		}

		public object Clone ()
		{
			JumpPadEvent clone = new JumpPadEvent();
			clone.Rectangle = this.Rectangle;
			clone.Activation = this.Activation;
			clone.Direction = this.Direction;
			clone.Parameters = this.Parameters;

			return clone;
		}

		public void OnStartedMoving (object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		public void OnStoppedMoving (object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}
