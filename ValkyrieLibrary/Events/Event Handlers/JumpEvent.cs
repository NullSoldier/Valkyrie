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

        public override void Trigger(BaseCharacter player, Event e)
        {
            player.JumpWall();
        }
    }
}
