using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace ValkyrieLibrary.Core
{
	public class TextureManager
	{
		private Dictionary<string, Texture2D> Resources;
		private ContentManager Content;
        private GraphicsDevice Device;
        public string TextureRoot = string.Empty;

		public TextureManager(ContentManager content, GraphicsDevice device, string root)
		{
			this.Resources = new Dictionary<string, Texture2D>();
			this.Content = content;
            this.Device = device;
            this.TextureRoot = root;
		}

		public void AddTexture(string Name, Texture2D newTexture)
		{
			this.Resources.Add(Name, newTexture);
		}

		public void AddTexture(Texture2D newTexture)
		{
			this.Resources.Add(newTexture.Name, newTexture);
		}

		public void AddTexture(string FileName)
		{
			Texture2D newTexture = Texture2D.FromFile(this.Device, TextureRoot + "\\" + FileName);

            this.AddTexture(FileName, newTexture);
		}

		public Texture2D GetTexture(string FileName)
		{
            if (!this.Resources.ContainsKey(FileName))
                this.AddTexture(FileName);

			return this.Resources[FileName];
		}

		public bool ContainsTexture(string FileName)
		{
			return (this.Resources.ContainsKey(FileName));
		}

		public void ClearCache()
		{
			foreach (var resource in this.Resources)
				this.ClearFromCache(resource.Key);
		}

		public void ClearFromCache(string resourcename)
		{
			if (this.Resources.Keys.Contains(resourcename))
			{
				this.Resources[resourcename].Dispose();
				this.Resources.Remove(resourcename);
			}
			else
				throw new ArgumentException("Resource does not exist in the cache"); // uneccessary?
		}
		

	}
}
