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
		/// <summary>
		/// Adds a module to the <seealso cref="IModuleProvider"/>
		/// </summary>
		/// <param name="module">The module to add.</param>
		/// <exception cref="ArgumentException">The module already exists.</exception>
		void AddModule (IModule module);

		/// <summary>
		/// Removes a module from the <seealso cref="IModuleProvider"/>
		/// </summary>
		/// <param name="name">The name of the module to remove</param>
		/// <exception cref="ArgumentException">The module already exists.</exception>
		bool RemoveModule (string name);

		/// <summary>
		/// Removes a module from the the <seealso cref="IModuleProvider"/>
		/// </summary>
		/// <param name="module">The module to remove.</param>
		/// <returns>True if the <paramref name="module"/> was removed.</returns>
		bool RemoveModule (IModule module);

		/// <summary>
		/// Pushes the module to the screen by making it the current module.
		/// </summary>
		/// <param name="name">The name of the module to push.</param>
		void PushModule (string name);

		/// <summary>
		/// Runs update logic on the current module.
		/// </summary>
		/// <param name="gameTime">The current gameTime.</param>
        /// <exception cref="InvalidOperatingException">Thrown when there is no module currently active.</exception>
		void UpdateCurrent (GameTime gameTime);

		/// <summary>
		/// Draw the current module.
		/// </summary>
		/// <param name="spriteBatch">The spriteBatch to render to.</param>
		/// <param name="gameTime">The current gameTime.</param>
        /// <exception cref="InvalidOperatingException">Thrown when there is no module currently active.</exception>
		void DrawCurrent (SpriteBatch spriteBatch, GameTime gameTime);

		/// <summary>
		/// Gets the current <seealso cref="IModule"/>.
		/// </summary>
		IModule CurrentModule { get; }

		/// <summary>
		/// Gets a module with a specified name.
		/// </summary>
		/// <param name="name">The name of the module to get.</param>
		/// <returns>The module with specified <paramref name="name"/>.</returns>
		IModule GetModule (string name);

		/// <summary>
		/// Gets the count of how many modules are stored.
		/// </summary>
		int Count { get; }
	}
}