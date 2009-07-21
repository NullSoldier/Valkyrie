﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Core;
using ValkyrieLibrary;

namespace ValkyrieLibrary.Events
{
    public class LoadEvent : BaseEventHandler
    {
        public LoadEvent()
        {
            this.Parameters.Add("Name");
            this.Parameters.Add("World");
            this.Parameters.Add("Entry");

            this.Type = "Load";
            this.ActivationType = ActivationTypes.Collision;
        }

        public override void Trigger(Player player, Event e)
        {
            String name = e.Parms["World"];
            String pos = e.Parms["Entry"];

            TileEngine.WorldManager.SetWorld(name, pos);
        }
    }
}
