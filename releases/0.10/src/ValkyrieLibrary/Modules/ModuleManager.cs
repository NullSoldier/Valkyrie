using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Valkyrie.Library
{
	public class ModuleManager
	{
		public IModuleLibrary CurrentModule
		{
			get;
			private set;
		}

		public readonly Dictionary<string, IModuleLibrary> Modules;

		public ModuleManager ()
		{
			this.Modules = new Dictionary<string, IModuleLibrary>();
		}

		public void AddModule (IModuleLibrary Module, string Name)
		{
			this.Modules.Add(Name, Module);
		}

		public void PushModuleToScreen (string name)
		{
			if(!this.Modules.ContainsKey(name))
				throw new ArgumentException("That Module does not exist or is not loaded into the module cache.");

			if(this.CurrentModule != null)
				this.CurrentModule.Deactivate();

			this.CurrentModule = this.Modules[name];
			this.CurrentModule.Activate();
		}
	}
}