using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine;
using Valkyrie.Engine.Maps;
using Valkyrie.Engine.Core;
using System.Drawing;

namespace ValkyrieMapEditor.Core.Components
{
	public static class ComponentHelpers
	{
		public static bool PointInMap(Map map, MapPoint mpoint)
		{
			Check.NullArgument<Map>(map, "map");
			Check.NullArgument<MapPoint>(mpoint, "mpoint");

			return (mpoint.X >= 0
				&& mpoint.Y >= 0
				&& mpoint.X < map.MapSize.X
				&& mpoint.Y < map.MapSize.Y);
		}

		public static bool PointInBounds(BaseCamera camera, int x, int y)
		{
			Check.NullArgument<BaseCamera>(camera, "camera");

			return (x >= 0
				&& y >= 0
				&& x <= camera.Screen.Width
				&& y <= camera.Screen.Height);
		}

		public static Microsoft.Xna.Framework.Rectangle GetSelectionRectangle(Microsoft.Xna.Framework.Point spoint, Microsoft.Xna.Framework.Point epoint)
		{
			int x = -1;
			int y = -1;
			int width = -1;
			int height = -1;

			if (spoint.X <= epoint.X)
			{
				x = spoint.X;
				width = (epoint.X - spoint.X) + 32;
			}
			else
			{
				x = epoint.X;
				width = ((spoint.X + 32) - epoint.X);
			}

			if (spoint.Y <= epoint.Y)
			{
				y = spoint.Y;
				height = (epoint.Y - spoint.Y) + 32;
			}
			else
			{
				y = epoint.Y;
				height = ((spoint.Y + 32) - epoint.Y);
			}

			return new Microsoft.Xna.Framework.Rectangle(x, y, width, height);
		}

		public static Microsoft.Xna.Framework.Rectangle GetSelectionRectangleTiles(Microsoft.Xna.Framework.Point spoint, Microsoft.Xna.Framework.Point epoint)
		{
			int x = -1;
			int y = -1;
			int width = -1;
			int height = -1;

			if (spoint.X <= epoint.X)
			{
				x = spoint.X;
				width = (epoint.X - spoint.X) + 1;
			}
			else
			{
				x = epoint.X;
				width = ((spoint.X + 1) - epoint.X);
			}

			if (spoint.Y <= epoint.Y)
			{
				y = spoint.Y;
				height = (epoint.Y - spoint.Y) + 1;
			}
			else
			{
				y = epoint.Y;
				height = ((spoint.Y + 1) - epoint.Y);
			}

			return new Microsoft.Xna.Framework.Rectangle(x, y, width, height);
		}
	}
}
