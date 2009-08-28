using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Events;
using Valkyrie.Characters;
using Microsoft.Xna.Framework;
using ValkyrieLibrary;
using ValkyrieLibrary.Core;

namespace Valkyrie.Events
{
	
	public class JumpEvent
		: IMapEvent
	{
		public Rectangle Rectangle { get; set; }
		public ActivationTypes Activation { get; set; }
		public Directions Direction { get; set; }

		public Dictionary<string, string> Parameters { get; set; }

		public string GetStringType()
		{
			return "Jump";
		}

		public void Trigger(BaseCharacter character)
		{
			if (!(character is PokePlayer))
				return;

			PokePlayer player = (PokePlayer)character;
			player.IgnoreMoveInput = true;
			player.StartedMoving += this.Event_StartedMoving;
			player.StoppedMoving += this.Event_StoppedMoving;
			player.EndAfterMovementReached = true;
			
			ScreenPoint dest = new ScreenPoint(TileEngine.Player.Location.X, TileEngine.Player.Location.Y);
            ScreenPoint newDest = dest + (new ScreenPoint(player.GetLookPoint().ToMapPoint().X * 2, player.GetLookPoint().ToMapPoint().Y) * 2);

			TileEngine.MovementManager.Move(character, newDest);
		}

		public IEnumerable<string> GetParameterNames()
		{
			return new string[0];
		}

		public object Clone()
		{
			JumpEvent clone = new JumpEvent();
			clone.Rectangle = this.Rectangle;
			clone.Activation = this.Activation;
			clone.Direction = this.Direction;
			clone.Parameters = this.Parameters;

			return clone;
		}

		public void Event_StartedMoving(object sender, EventArgs e)
		{
			PokePlayer player = (PokePlayer)sender;
			player.Density = 0;
			player.CurrentAnimationName = "Jump";
		}

		public void Event_StoppedMoving(object sender, EventArgs e)
		{
			PokePlayer player = (PokePlayer)sender;
			player.Density = 1;
			player.CurrentAnimationName = player.Direction.ToString();
			player.IgnoreMoveInput = false;

			player.StartedMoving -= this.Event_StartedMoving;
			player.StoppedMoving -= this.Event_StoppedMoving;
		}
	}
}