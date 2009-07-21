using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Characters;

namespace ValkyrieLibrary.Events
{
    public interface IEventHandler
    {
        ActivationTypes ActivationType { get; set; }
        String Type { get; set; }

        bool MeetsCriteria(ActivationTypes activationType);
        void Trigger(Player player, Event e);
    }

    public enum ActivationTypes
    {
        Activate,
        Collision,
        Movement
    }
}
