using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Characters;


namespace ValkyrieLibrary.Events
{
    public class JumpEvent : BaseEventHandler
    {
        public JumpEvent()
        {
            Type = "Jump";
            ActivationType = ActivationTypes.Collision;
        }

        public override void Trigger(Player player, Event e)
        {
            if (typeof(PokePlayer) == player.GetType())
                ((PokePlayer)player).JumpWall();
        }
    }
}
