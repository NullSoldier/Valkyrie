using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine;
using Valkyrie.Engine.Core.Scene;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Valkyrie.Engine.Core;

namespace Valkyrie.Library.Providers
{
	public class ValkyrieFogRenderer : IFogRenderer
	{
		public ValkyrieFogRenderer (Texture2D fogtexture, float opacity)
		{
			this.fogtexture = fogtexture;
			this.opacity = opacity;
		}

		public void RenderFog(SpriteBatch batch, BaseCamera camera, List<Rectangle> infos, MapPoint rayorigin)
		{
			Rectangle view = new Rectangle(camera.Origin.IntX, camera.Origin.IntY, (int)camera.CurrentSize.X, (int)camera.CurrentSize.Y);

			int tilesize = 32;
			float NumberOfRays = 270f;
			float AngleStep = (360f / NumberOfRays);


			for (float i = 0; i < 360; i += AngleStep)
			{
				Vector2 pos = new Vector2(rayorigin.X * tilesize, rayorigin.Y * tilesize);
				Vector2 vector = new Vector2((float)Math.Cos(i) * 2, (float)Math.Sin(i) * 2);
				bool addfogpoints = false;

				while (view.Contains((int)pos.X, (int)pos.Y))
				{
					var worldpoint = this.ScreenSpaceToWorldPoint(pos.X, pos.Y, tilesize);

					if (!addfogpoints)
					{
						Rectangle tile = infos.Where(r => r.Contains((int)pos.X, (int)pos.Y)).FirstOrDefault();

						if (tile != default(Rectangle))
						{
							pointsdone.Add(worldpoint);
							addfogpoints = true;
						}
					}
					else
					{
						if (!pointsdone.Contains(worldpoint) && addfogpoints)
						{
							batch.Draw(fogtexture, new Rectangle((int)worldpoint.X, (int)worldpoint.Y, tilesize, tilesize), new Color(0f, 0f, 0f, this.opacity));
							pointsdone.Add(worldpoint);
						}
					}

					pos += vector;
				}
			}

			pointsdone.Clear();
		}

		#region IEngineProvider Members

		public void LoadEngineContext(IEngineContext context)
		{
			this.context = context;

			this.isloaded = true;
		}

		public void Unload()
		{
			this.isloaded = false;
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		#endregion

		private IEngineContext context = null;
		private Texture2D fogtexture = null;
		private List<Vector2> pointsdone = new List<Vector2>();
		private bool isloaded = false;
		private float opacity = 0f;

		private Vector2 ScreenSpaceToWorldPoint(float x, float y, int tilesize)
		{
			return new Vector2((int)(x/tilesize) * tilesize, (int)(y/tilesize) * tilesize);
		}
	}
}
