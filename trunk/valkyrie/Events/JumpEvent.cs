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
using Valkyrie.Engine.Core;
using Valkyrie.Engine;

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

		public void Trigger (BaseCharacter character, IEngineContext context)
		{
			if(!(character is PokePlayer))
				return;

			PokePlayer player = (PokePlayer)character;
			player.IgnoreMoveInput = true;
			player.StartedMoving += this.Event_StartedMoving;
			player.StoppedMoving += this.Event_StoppedMoving;

			ScreenPoint dest = new ScreenPoint(player.Location.X, player.Location.Y);
			MapPoint lookvalue = player.GetLookValue();
			ScreenPoint newDest = dest + (new ScreenPoint((lookvalue.X * 32) * 2,  (lookvalue.Y * 32) * 2));

			if(string.IsNullOrEmpty (player.AnimationTag.ToString()))
			{
				player.AnimationTag = "Jumping";
				player.CurrentAnimationName = "Jump";
			}

			context.MovementProvider.BeginMoveDestination(character, newDest);
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
			PokePlayer player = (PokePlayer)sender;
			player.Density = 0;
		}

		public void Event_StoppedMoving (object sender, EventArgs e)
		{
			PokePlayer player = (PokePlayer)sender;
			player.Density = 1;
			player.CurrentAnimationName = player.Direction.ToString();
			player.IgnoreMoveInput = false;

			player.StartedMoving -= this.Event_StartedMoving;
			player.StoppedMoving -= this.Event_StoppedMoving;

			if(player.AnimationTag == "Jumping")
				player.AnimationTag = string.Empty;
		}
	}
}