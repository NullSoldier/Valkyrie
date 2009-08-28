using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Core;
using ValkyrieLibrary.Events;
using System.Diagnostics;
using Valkyrie.Characters;
using ValkyrieLibrary;
using ValkyrieLibrary.Maps;
using Microsoft.Xna.Framework;

namespace Valkyrie.Events
{
	
	public class JumpPadEvent
		: IMapEvent
	{
		public Rectangle Rectangle { get; set; }
		public Dictionary<string, string> Parameters { get; set; }
		public ActivationTypes Activation { get; set; }
		public Directions Direction { get; set; }

		public string GetStringType()
		{
			return "JumpPad";
		}

		public void Trigger(BaseCharacter character)
		{
			/*if (!(character is PokePlayer)) return;

			BasePoint tmpDest = new BasePoint(0, 0);
			Map tmpMap = TileEngine.CurrentMapChunk;

			bool found = false;

			// Search for the destination, need methods and optimizations for this
			foreach (var map in TileEngine.EventManager.events.Keys)
			{
				if (found) break;

				foreach (var mapevent in TileEngine.EventManager.events[map])
				{
					if (mapevent.GetStringType() == "EntryPoint" &&
						mapevent.Parameters["Name"] == this.Parameters["DestinationName"])
					{
						tmpDest = new BasePoint(mapevent.Rectangle.Y, mapevent.Rectangle.Y);
						tmpMap = map;

						found = true;
						break;
					}
				}
			}

			PokePlayer player = (PokePlayer)character;
			player.StopMoving();

			player.ReachedDestination += this.OnCharacterLanded;
			player.CurrentAnimationName = "Spin";
			player.Speed = Convert.ToInt32(this.Parameters["Speed"]);
			player.Density = 0;
			player.IsJumping = true;
			
			// Need methods to convert from local coordinents into global
			ScreenPoint destination = new MapPoint(tmpDest.X + TileEngine.WorldManager.CurrentWorld.MapList[tmpMap.Name].MapLocation.X,
				tmpDest.Y + TileEngine.WorldManager.CurrentWorld.MapList[tmpMap.Name].MapLocation.Y).ToScreenPoint();

			player.Move(destination);*/

		}

		public IEnumerable<string> GetParameterNames()
		{
			return new string[] { "DestinationName", "Speed" };
		}

		public object Clone()
		{
			JumpPadEvent clone = new JumpPadEvent();
			clone.Rectangle = this.Rectangle;
			clone.Activation = this.Activation;
			clone.Direction = this.Direction;
			clone.Parameters = this.Parameters;

			return clone;
		}

		public void OnCharacterLanded(object sender, EventArgs e)
		{
			/*
			if (!(sender is PokePlayer)) return;

			PokePlayer player = (PokePlayer)sender;
			player.IsJumping = false;
			player.Density = 1;
			player.Speed = 2;
			player.CurrentAnimationName = player.Direction.ToString();
			player.ReachedDestination -= this.OnCharacterLanded;
		  */
		}
	}
}
