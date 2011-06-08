using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Providers;

namespace ValkyrieLibrary
{
    public class ModuleManager
		: IModuleProvider
    {

		public void AddModule (IModule module)
		{
			lock(this.moduleSync)
			{
				if(this.modules.ContainsKey(module.Name))


				this.modules.Add(module.Name, module);
			}
		}

		public void RemoveModule (string name)
		{
			lock(this.moduleSync)
			{
				if(!this.modules.ContainsKey(name))
					throw new KeyNotFoundException(string.Format("The module `{0}` does not exist in the module manager.", name));

				this.modules.Remove(name);
			}
		}

		public void RemoveModule (IModule module)
		{
			lock(this.moduleSync)
			{
				if(!this.modules.ContainsKey(module.Name))
					throw new KeyNotFoundException(string.Format("The module `{0}` does not exist in the module manager.", module.Name));

				this.modules.Remove(module.Name);
			}
		}

		public IModule GetModule (string name)
		{
			lock(this.moduleSync)
			{
				if(!this.modules.ContainsKey(name))
					throw new KeyNotFoundException(string.Format("The module `{0}` does not exist in the module manager.", name));

				return this.modules[name];
			}
		}

		public void PushModule (string name)
		{
			lock(this.moduleSync)
			{
				if(!this.modules.ContainsKey(name))
					throw new KeyNotFoundException(string.Format("The module `{0}` does not exist in the module manager.", name));

				this.currentmoduleName = name;
			}
		}

		public IModule CurrentModule
		{
			get
			{
				lock(this.moduleSync)
				{
					if(String.IsNullOrEmpty(this.currentmoduleName))
						return null;

					return this.modules[this.currentmoduleName];
				}
			}
		}

		public int Count
		{
			get
			{
				lock(this.moduleSync)
					return this.modules.Count;
			}
		}

		public void LoadEngineContext (IEngineContext context)
		{
			throw new NotImplementedException();
		}

		private readonly Dictionary<string, IModule> modules = new Dictionary<string, IModule>();
		private object moduleSync;
		private string currentmoduleName;

	}
}
