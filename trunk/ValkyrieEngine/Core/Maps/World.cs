using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Valkyrie.Engine.Core;
using Mono.Rocks;
using Microsoft.Xna.Framework;

namespace Valkyrie.Engine.Maps
{
    public class World
    {
		#region Constructors

		public World (string name)
			: this(name, null)
		{
		}

		public World (string name, Dictionary<string, MapHeader> mapcollection)
		{
			if(mapcollection != null)
				this.maps = mapcollection;

			this.name = name;
		}

		#endregion

		#region Public Properties and Methods

		public string Name { get { return this.name; } }

		public ReadOnlyDictionary<string, MapHeader> Maps
		{
			get
			{
				return new ReadOnlyDictionary<string, MapHeader> (this.maps);
			}
		}

		public void AddMap (MapHeader header)
		{
			lock(syncroot)
			{
				this.maps.Add(header.MapName, header); 
			}
		}
		
		public MapHeader GetLocalMapFromPoint(MapPoint globalpoint)
		{
			foreach(MapHeader header in this.Maps.Values)
			{
				Rectangle rect = (header.MapLocation).ToRect(header.Map.MapSize.ToPoint());

				if(rect.Contains(globalpoint.ToPoint()))
				{
					return header;
				}
			}

			return null;
		}

		#endregion

		private string name = string.Empty;
		private Dictionary<string, MapHeader> maps = new Dictionary<string, MapHeader>();
		private object syncroot = new object();

    }
}
