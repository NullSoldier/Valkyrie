using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Library.Core;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Valkyrie.Library;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Providers;

namespace Valkyrie.Engine.Maps
{
	public class MapHeader
	{
		#region Constructors

        public MapHeader (Map map)
            : this (map, MapPoint.Zero) { }

        public MapHeader(Map map, MapPoint location)
        {
            this.map = map;
            this.mapname = map.Name;
            this.maplocation = location;
        }

		public MapHeader (string mapname, MapPoint point, string location)
		{
			this.MapName = mapname;
			this.MapLocation = point;
			this.MapPath = location;
		}

		#endregion

		#region Public Properties & Methods

		public MapPoint MapLocation
		{
			get { return this.maplocation; }
			set { this.maplocation = value; }
		}

		public string MapPath
		{
			get { return this.mappath; }
			set { this.mappath = value; }
		}

		public bool IsLoaded
		{
			get { return (this.map != null); }
		}

		public Map Map
		{
			get
			{
				return this.map;
			}
			set { this.map = value; }
		}

		public string MapName
		{
			get { return ((this.IsLoaded) ? this.Map.Name : this.mapname); }
			set { this.mapname = value; }
		}

		#region Header Methods

		public bool IsVisible (BaseCamera camera)
		{
			if(!this.IsLoaded)
				throw new Exception("Map is not loaded.");

			int tilesize = this.Map.TileSize;
			Rectangle mapSize = this.MapLocation.ToRect(this.Map.MapSize.ToPoint());

			Rectangle cameraRect = new Rectangle(
                camera.Origin.X / tilesize,
				camera.Origin.Y / tilesize,
				(camera.Screen.Width / this.Map.TileSize) + tilesize,
				(camera.Screen.Height / this.Map.TileSize) + tilesize);

			return mapSize.Intersects(cameraRect);
		}

		public bool TilePointInMapLocal (MapPoint point)
		{
			Rectangle mapSize = (new MapPoint(0, 0)).ToRect(this.Map.MapSize.ToPoint());
			return (mapSize.Contains(point.ToPoint()) == true);
		}

		public bool TilePointInMapGlobal (MapPoint point)
		{
			Rectangle mapSize = this.MapLocation.ToRect(this.Map.MapSize.ToPoint());

			return (mapSize.Contains(point.ToPoint()) == true);
		}

		#endregion

		#endregion

		private Map map = null;
		private MapPoint maplocation = MapPoint.Zero;
		private string mappath = null;
		private string mapname = string.Empty;
	}
}
