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
using Valkyrie.Engine.Maps;
using Valkyrie.Engine.Core;

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
			String worldname = this.Parameters["World"];
			String pos = this.Parameters["EntryPointName"];

			IMapEvent tmpevent = null;
			MapHeader tmpheader = null;

			foreach(MapHeader header in context.WorldManager.GetWorld(worldname).Maps.Values)
			{
				tmpevent = context.EventProvider.GetMapsEvents(header.Map).Where( m => m.GetStringType() == "EntryPoint" && m.Parameters["Name"] == pos).FirstOrDefault();
				if(tmpevent != null)
				{
					tmpheader = header;
					break;
				}
			}

			if(tmpevent == null || tmpheader == null)
				return;

			context.MovementProvider.EndMove (character, false, true);

			character.WorldName = worldname;
			character.Location = new MapPoint (tmpevent.Rectangle.X + tmpheader.MapLocation.X,
															tmpevent.Rectangle.Y + tmpheader.MapLocation.Y).ToScreenPoint ();
			character.CurrentMap = null;

			context.SceneProvider.GetCamera ("camera1").CenterOnCharacter (character);
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
