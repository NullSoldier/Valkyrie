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
        string TextureRoot { get; set; }

		void Load(string textureroot);

		void AddTexture(string Name, Texture2D newTexture);
		void AddTexture(Texture2D newTexture);
		void AddTexture(string FileName);

		Texture2D GetTexture(string FileName);
		bool ContainsTexture(string FileName);

		void ClearCache();
		void ClearFromCache(string resourcename);
		

	}
}
