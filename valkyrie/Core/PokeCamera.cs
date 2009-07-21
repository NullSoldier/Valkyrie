using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary;
using Microsoft.Xna.Framework;

namespace ValkyrieLibrary.Core
{
	public class PokeCamera
		: BaseCamera
	{
		public PokeCamera()
			: base() { }

		public PokeCamera(int X, int Y, int Width, int Height)
			: base(X, Y, Width, Height) { }

		public MapPoint CameraTileLocation
		{
			get
			{
				int x = (int)(this.CameraOffset.X / TileEngine.CurrentMapChunk.TileSize.X);
				int y = (int)(this.CameraOffset.Y / TileEngine.CurrentMapChunk.TileSize.Y);

				return new MapPoint(x, y);
			}
			set
			{
				this.CenterOnPoint(value.ToScreenPoint());
			}
		}
	}
}
