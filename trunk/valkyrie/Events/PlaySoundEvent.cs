using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Events;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine;
using Valkyrie.Characters;
using Microsoft.Xna.Framework;

namespace Valkyrie.Library.Events
{
	public class PlaySoundEvent
		: IMapEvent
	{
		public Rectangle Rectangle { get; set; }
		public ActivationTypes Activation { get; set; }
		public Directions Direction { get; set; }
		public Dictionary<string, string> Parameters { get; set; }

		public string GetStringType ()
		{
			return "PlaySound";
		}

		public void Trigger (BaseCharacter character, IEngineContext context)
		{
			var soundfilename = this.Parameters["FileName"];
			var loop = this.Parameters["LoopSound"];

			var sound = context.SoundManager.GetSound (soundfilename);

			if(sound != null)
				context.SoundProvider.PlaySound (sound, ((loop == "1") ? true : false));
		}

		public IEnumerable<string> GetParameterNames ()
		{
			return new string[] {"FileName", "LoopSound"};
		}

		public object Clone ()
		{
			PlaySoundEvent clone = new PlaySoundEvent ();
			clone.Rectangle = this.Rectangle;
			clone.Activation = this.Activation;
			clone.Direction = this.Direction;
			clone.Parameters = this.Parameters;

			return clone;
		}
	}
}
