using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Events;
using ValkyrieLibrary.Characters;

namespace ValkyrieLibrary.Events
{
    public abstract class BaseEventHandler : IEventHandler
    {
        public ActivationTypes ActivationType { get; set; }
        public String Type { get; set; }

        public virtual bool MeetsCriteria(ActivationTypes activationType)
        {
            return (this.ActivationType == activationType);
        }

        public abstract void Trigger(Player player, Event e);
    }
}
