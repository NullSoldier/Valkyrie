using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Characters;

namespace ValkyrieLibrary.Events
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
            player.DisplayMessage(e.Parms["Title"], e.Parms["Text"]);
        }
    }
}
