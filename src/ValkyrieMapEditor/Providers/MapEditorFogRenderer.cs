using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Core.Scene;
using Microsoft.Xna.Framework.Graphics;
using Valkyrie.Engine;
using Microsoft.Xna.Framework;
using Valkyrie.Engine.Core;

namespace ValkyrieMapEditor.Providers
{
	public class MapEditorFogRenderer : IFogRenderer
	{
		#region IFogRenderer Members

		public float Opacity { get; set; }

		public Texture2D Texture { get; set; }

		public void RenderFog(SpriteBatch batch, BaseCamera camera, List<Rectangle> infos, MapPoint rayorigin)
		{
			
		}

		#endregion

		#region IEngineProvider Members

		public void LoadEngineContext (IEngineContext context)
		{
			throw new NotImplementedException();
		}

		public void Unload()
		{
			
		}

		public bool IsLoaded
		{
			get { return false; }
		}

		#endregion
	}
}
