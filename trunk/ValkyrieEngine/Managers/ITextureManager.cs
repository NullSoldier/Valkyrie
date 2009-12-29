using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Valkyrie.Engine.Providers;

namespace Valkyrie.Library.Core
{
	public interface ITextureManager
		: IEngineProvider
	{
		/// <summary>
		/// The folder relative to the game where textures are stored
		/// </summary>
        string TextureRoot { get; set; }

		/// <summary>
		/// Add a texture to the ITextureManager.
		/// </summary>
		/// <param name="filename">The name of the texture to add.</param>
		/// <param name="newTexture">The texture to add.</param>
		void AddTexture(string filename, Texture2D newTexture);

		/// <summary>
		/// Add a texture to the ITextureManager
		/// </summary>
		/// <param name="newTexture">The texture to add.</param>
		void AddTexture(Texture2D newTexture);

		/// <summary>
		/// Retrieves and adds a texture from disk from the texture root
		/// </summary>
		/// <param name="FileName">The file name of the texture to add.</param>
		void AddTexture(string FileName);
		
		/// <summary>
		/// Gets a specified sound first from the cache and then from disk if it is not cached.
		/// </summary>
		/// <param name="FileName">The file name of the texture to get</param>
		/// <returns>Returns the requested texture if it is found, otherwise it returns null.</returns>
		Texture2D GetTexture(string FileName);

		/// <summary>
		/// Checks to see if a texture is stored in the cache
		/// </summary>
		/// <param name="FileName">The file name of the texture</param>
		/// <returns>True if the texture is stored in the cache.</returns>
		bool ContainsTexture(string FileName);

		/// <summary>
		/// Clears all textures from the ITextureManager's cache
		/// </summary>
		void ClearCache();

		/// <summary>
		/// Clears a specific texture from the ITextureProvider's cache
		/// </summary>
		/// <param name="resourcename">The file name of the texture to clear</param>
		/// <exception cref="ArgumentException">Thrown when the texture to clear is not found.</exception>
		void ClearFromCache(string resourcename);
	}
}
