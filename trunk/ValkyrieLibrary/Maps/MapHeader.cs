using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Library.Core;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Valkyrie.Library.Maps.MapProvider;
using Valkyrie.Library.Events;

namespace Valkyrie.Library.Maps
{
	public class MapHeader
	{
		#region Constructors

		public MapHeader () { }

		public MapHeader (FileInfo location)
			: this(location.Name, location.FullName.Substring(0, location.FullName.Length - location.Name.Length)) { }

		public MapHeader(string mapname, string filepath)
            : this(mapname, filepath, new MapPoint(0, 0)) { }

		public MapHeader(string mapname, string filepath, MapPoint point)
		{
			this.MapName = mapname;
			this.MapFileLocation = filepath;
			this.MapLocation = point;
		}

		#endregion

		public string MapName { get; set; }
		public string MapFileLocation { get; set; }
		public MapPoint MapLocation { get; set; }
		public IMapProvider MapProvider { get; set; }
		public MapEventManager MapManager { get; set; }
		public bool IsLoaded { get { return (this.map == null); } }

		public Map Map
		{
			get
			{
				if (this.map == null)
					this.Load();

				return this.map;
			}
			set { this.map = value; }
		}

		public void Load()
		{
			this.map = this.MapProvider.GetMap(new FileInfo(this.MapFileLocation), this.MapManager);
		}

		public void Unload()
		{
			this.map = null;
		}

		#region Header Methods

		public bool IsVisableToPlayer (BaseCamera camera)
		{
			int TileSize = 32;

			Rectangle mapSize = this.MapLocation.ToRect(this.Map.MapSize.ToPoint());

			Rectangle cameraRect = new Rectangle((int)(camera.MapOffset.X * -1) / TileSize,
				(int)(camera.MapOffset.Y * -1) / TileSize,
				(camera.Screen.Width / TileSize) + 32,
				(camera.Screen.Height / TileSize) + 32);

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

		private Map map;
	}
}
