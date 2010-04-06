using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Valkyrie.Engine.Providers;
using Microsoft.Xna.Framework;

namespace Valkyrie.Engine.Core.Scene
{
	public interface IFogRenderer : IEngineProvider
	{
		void RenderFog(SpriteBatch batch, BaseCamera camera, List<Rectangle> infos, MapPoint rayorigin);
	}
}
