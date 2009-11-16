using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Valkyrie.Library.Core;
using Valkyrie.Engine;

namespace Valkyrie.Library.Managers
{
	public class ValkyrieTextureManager
		: ITextureManager
	{
		#region Constructors

		public ValkyrieTextureManager (ContentManager content, GraphicsDevice device)
		{
			this.Content = content;
			this.Device = device;
		}

		#endregion

		#region Properties and Methods

		public string TextureRoot
		{
			get { return this.textureroot; }
			set { this.textureroot = value; }
		}

		public void Load (string textureroot)
		{
			this.TextureRoot = textureroot;
		}

		public void AddTexture (string Name, Texture2D newTexture)
		{
			this.Resources.Add(Name, newTexture);
		}

		public void AddTexture (Texture2D newTexture)
		{
			this.Resources.Add(newTexture.Name, newTexture);
		}

		public void AddTexture (string FileName)
		{
			Texture2D newTexture = Texture2D.FromFile(this.Device, Path.Combine(TextureRoot, FileName));

			this.AddTexture(FileName, newTexture);
		}

		public Texture2D GetTexture (string FileName)
		{
			if(!this.Resources.ContainsKey(FileName))
				this.AddTexture(FileName);

			return this.Resources[FileName];
		}

		public bool ContainsTexture (string FileName)
		{
			return (this.Resources.ContainsKey(FileName));
		}

		public void ClearCache ()
		{
			foreach(var resource in this.Resources)
				resource.Value.Dispose();

			this.Resources.Clear();
		}

		public void ClearFromCache (string resourcename)
		{
			if(this.Resources.Keys.Contains(resourcename))
			{
				this.Resources[resourcename].Dispose();
				this.Resources.Remove(resourcename);
			}
			else
				throw new ArgumentException("Resource does not exist in the cache"); // uneccessary?
		}

		#endregion

		private Dictionary<string, Texture2D> Resources = new Dictionary<string, Texture2D>();
		private ContentManager Content = null;
		private GraphicsDevice Device = null;
		private string textureroot = string.Empty;
		private IEngineContext context = null;
		private bool isloaded = false;

		#region IEngineProvider Members

		public void LoadEngineContext (IEngineContext context)
		{
			this.context = context;

			this.isloaded = true;
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		#endregion
	}
}
