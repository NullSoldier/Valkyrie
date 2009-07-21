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
        }

        public override bool MeetsCriteria(ActivationTypes activationType)
        {
            return (activationType == ActivationTypes.Activate || activationType == ActivationTypes.LookActivate);
        }

        public override void Trigger(Player player, Event e)
        {
            if (typeof(PokePlayer) == player.GetType())
                ((PokePlayer)player).DisplayMessage(e.Parms["Title"], e.Parms["Text"]);
        }
    }
}
