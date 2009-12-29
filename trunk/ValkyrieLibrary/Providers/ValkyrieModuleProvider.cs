using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Valkyrie.Library.Providers
{
    public class ValkyrieModuleProvider
		: IModuleProvider
    {
		public int Count
		{
			get { return this.modules.Count; }
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
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

		public void UpdateCurrent (GameTime gameTime)
		{
			if(!this.IsLoaded)
				throw new ProviderNotLoadedException();

			this.CurrentModule.Update(gameTime);
		}

		public void DrawCurrent (SpriteBatch spriteBatch, GameTime gameTime)
		{
			if(!this.IsLoaded)
				throw new ProviderNotLoadedException();

			this.CurrentModule.Draw(spriteBatch, gameTime);
		}

		public void AddModule (IModule module)
		{
			if(!this.IsLoaded)
				throw new ProviderNotLoadedException();

			lock(this.moduleSync)
			{
				if(this.modules.ContainsKey(module.Name))
					throw new Exception(string.Format("Module {0} already exists in module provider.", module.Name));

				this.modules.Add(module.Name, module);
			}
		}

		public bool RemoveModule (string name)
		{
			lock(this.moduleSync)
			{
				if(!this.modules.ContainsKey(name))
					throw new KeyNotFoundException(string.Format("The module {0} does not exist in the module manager.", name));

				return this.modules.Remove (name);
			}
		}

		public bool RemoveModule (IModule module)
		{
			lock(this.moduleSync)
			{
				if(!this.modules.ContainsKey(module.Name))
					throw new KeyNotFoundException(string.Format("The module {0} does not exist in the module manager.", module.Name));

				return this.modules.Remove(module.Name);
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
				if(this.CurrentModule != null)
					this.CurrentModule.Deactivate();

				if(!this.modules.ContainsKey(name))
					throw new KeyNotFoundException(string.Format("The module `{0}` does not exist in the module manager.", name));

				this.currentmoduleName = name;

				this.CurrentModule.Activate();
			}
		}

		public void LoadEngineContext (IEngineContext context)
		{
			this.context = context;

			this.isloaded = true;
		}

		public void Unload ()
		{
			lock(this.moduleSync)
			{
				foreach(IModule module in this.modules.Values)
				{
					module.Deactivate ();
					module.Unload ();
				}

				this.modules.Clear ();
			}
				
			this.isloaded = false;
		}

		private readonly Dictionary<string, IModule> modules = new Dictionary<string, IModule>();
		private object moduleSync = new object();
		private string currentmoduleName = string.Empty;
		private bool isloaded = false;
		private IEngineContext context = null;
	}
}
