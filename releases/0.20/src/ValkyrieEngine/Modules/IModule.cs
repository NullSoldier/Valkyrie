using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Valkyrie.Engine
{
	public interface IModule
	{
		void Update (GameTime gameTime);
		void Draw (SpriteBatch spriteBatch, GameTime gameTime);
		void Load (IEngineContext enginecontext);
		void Unload ();
		void Activate ();
		void Deactivate ();

		string Name { get; }
	}
}