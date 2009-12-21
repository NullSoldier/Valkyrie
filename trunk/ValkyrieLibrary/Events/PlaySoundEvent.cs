using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Events;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine;
using Microsoft.Xna.Framework;
using Valkyrie.Engine.Managers;

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
			if(this.context == null)
				this.context = context;

			var soundfilename = this.lastsoundrequested = this.Parameters["FileName"];
			var loop = this.lastloop = this.Parameters["LoopSound"];

			context.SoundManager.GetSoundAsync (soundfilename, this.SoundLoaded);
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

		private void SoundLoaded (SoundLoadedEventArgs ev)
		{
			if(ev.Name == this.lastsoundrequested)
			{
				var sound = this.context.SoundManager.GetSound (ev.Name);
				if(sound != null)
					context.SoundProvider.PlayBGM (sound, ((this.lastloop == "1") ? true : false));
			}
		}

		private string lastsoundrequested;
		private string lastloop;
		private IEngineContext context;
	}
}
