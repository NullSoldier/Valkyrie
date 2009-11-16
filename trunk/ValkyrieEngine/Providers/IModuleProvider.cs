using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Valkyrie.Engine.Providers
{
	public interface IModuleProvider
		: IEngineProvider
	{
		void AddModule (IModule module);
		void RemoveModule (string name);
		void RemoveModule (IModule module);
		void PushModule (string name);

		void UpdateCurrent (GameTime gameTime);
		void DrawCurrent (SpriteBatch spriteBatch, GameTime gameTime);

		IModule CurrentModule { get; }
		IModule GetModule (string name);

		int Count { get; }
	}
}