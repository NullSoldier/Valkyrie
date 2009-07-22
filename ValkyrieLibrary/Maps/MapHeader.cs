using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Core;
using System.IO;
using Microsoft.Xna.Framework;

namespace ValkyrieLibrary.Maps
{
	public class MapHeader
	{
		//public event EventHandler MapLoaded;
		//public event EventHandler MapUnloaded;

		#region Constructors
		public MapHeader(string mapname, string filepath)
            : this(mapname, filepath, new MapPoint(0, 0)) { }

		public MapHeader(string mapname, string filepath, MapPoint point)
		{
			this.MapName = mapname;
			this.MapLocation = point;
			this.MapFileLocation = filepath;
		}
		#endregion

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

        public string MapName { get; set; }
        public string MapFileLocation { get; set; }
        public MapPoint MapLocation { get; set; }

		private Map map;


		public void Load()
		{
			this.map = new Map();
			this.map.LoadMap(new FileInfo(Path.Combine(TileEngine.Configuration["MapRoot"], this.MapFileLocation)));
			this.map.Name = this.MapName;
		}

		public void Unload()
		{
			
		}

	}
}
