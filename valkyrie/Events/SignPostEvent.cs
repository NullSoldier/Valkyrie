using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Events;
using Valkyrie.Characters;

namespace Valkyrie.Events
{
    public class SignPostEvent : BaseEventHandler
    {
        public SignPostEvent()
        {
            Type = "SignPost";

            this.Parameters.Add("Title");
            this.Parameters.Add("Text");
        }

        public override bool MeetsCriteria(ActivationTypes activationType)
        {
            return (activationType == ActivationTypes.Activate || activationType == ActivationTypes.LookActivate);
        }

        public override void Trigger(BaseCharacter player, Event e)
        {
			if( player is PokePlayer)
				((PokePlayer)player).DisplayMessage(e.Parms["Title"], e.Parms["Text"]);
        }
    }
}
