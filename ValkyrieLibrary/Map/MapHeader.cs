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
		public event EventHandler MapLoaded;
		public event EventHandler MapUnloaded;

		#region Constructors
		public MapHeader(string mapname, string filepath)
			: this(mapname, filepath, new Point(0,0))	{ }

		public MapHeader(string mapname, string filepath, Point point)
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

		public string MapName
		{
			get { return this.mapname; }
			set { this.mapname = value; }
		}

		public string MapFileLocation
		{
			get { return this.mapfilelocation; }
			set { this.mapfilelocation = value; }
		}

		public Point MapLocation
		{
			get { return this.maplocation; }
			set { this.maplocation = value; }
		}

		private Map map;
		private string mapname;
		private Point maplocation;
		private string mapfilelocation;

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
