using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary;
using Microsoft.Xna.Framework;

namespace valkyrie.Core
{
	public class PokeCamera
		: BaseCamera
	{
		public PokeCamera()
			: base() { }

		public PokeCamera(int X, int Y, int Width, int Height)
			: base(X, Y, Width, Height) { }

		public Point CameraTileLocation
		{
			get
			{
				int x = (int)(this.CameraOffset.X / TileEngine.Map.TileSize.X);
				int y = (int)(this.CameraOffset.Y / TileEngine.Map.TileSize.Y);

				return new Point(x, y);
			}
			set
			{
				this.CenterOnPoint(new Point(value.X * TileEngine.Map.TileSize.X, value.Y * TileEngine.Map.TileSize.Y));
			}
		}
	}
}
