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
            ActivationType = ActivationTypes.Activate;
        }

        public override void Trigger(Player player, Event e)
        {
            if (typeof(PokePlayer) == player.GetType())
                ((PokePlayer)player).DisplayMessage(e.ParmOne, e.ParmTwo);
        }
    }
}
