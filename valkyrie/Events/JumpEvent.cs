using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Events;
using Valkyrie.Characters;

namespace Valkyrie.Events
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
			if( player is PokePlayer )
				((PokePlayer)player).JumpWall();
        }
    }
}