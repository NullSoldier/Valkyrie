using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Valkyrie.Engine.Modules
{
    public class ModuleNotLoadedException
        : Exception
    {
        public ModuleNotLoadedException(IModule module)
            : base ("The module " + module.Name + " was not loaded.")
        {
            this.Module = module;
        }

        public IModule Module
        {
            get;
            private set;
        }
    }
}
