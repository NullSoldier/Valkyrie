using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ValkyrieLibrary.Core;
using System.Xml;
using Microsoft.Xna.Framework;
using ValkyrieLibrary.Maps;

namespace ValkyrieMapEditor
{
    public static class MapEditorManager
    {
		public static Rectangle SelectedTilesRect = new Rectangle(0, 0, 1, 1);
        public static MapLayer CurrentLayer = MapLayer.BaseLayer;
		public static FileInfo CurrentMapLocation;
		public static bool IgnoreInput = false;
		public static Tools CurrentTool = Tools.Pencil;

		public static Point MouseLocation
		{
			get
			{
				lock(MapEditorManager.MouseLock)
					return MapEditorManager.mouselocation;
			}
			set
			{
				lock(MapEditorManager.MouseLock)
					MapEditorManager.mouselocation = value;
			}
		}

		private static object MouseLock = new Object();
		private static Point mouselocation = new Point(0, 0);

        public static void SaveMap(Map map, FileInfo location)
        {
			XmlDocument doc = new XmlDocument();

			var mapElement = doc.CreateElement("Map");

			var name = doc.CreateElement("Name");
			name.InnerText = map.Name;

			var tileset = doc.CreateElement("TileSet");
			tileset.InnerText = map.TextureName;

			var mapsize = doc.CreateElement("MapSize");

			var mapsizex = doc.CreateElement("X");
			mapsizex.InnerText = map.MapSize.X.ToString();
			var mapsizey = doc.CreateElement("Y");
			mapsizey.InnerText = map.MapSize.Y.ToString();

			var tilepixelsize = doc.CreateElement("TilePixelSize");
			tilepixelsize.InnerText = map.TileSize.X.ToString();

			// Base layer
			var baselayer = doc.CreateElement("BaseLayer");
			
			var baselayerbuilder = new StringBuilder();
			for (int i = 0; i < map.BaseLayer.Length; i++)
			{
				baselayerbuilder.Append(map.BaseLayer[i]);
				baselayerbuilder.Append(" ");
			}

			baselayer.InnerText = baselayerbuilder.ToString();

			// Middle Layer
			var middlelayer = doc.CreateElement("MiddleLayer");

			var middlelayerbuilder = new StringBuilder();
			for (int i = 0; i < map.MiddleLayer.Length; i++)
			{
				middlelayerbuilder.Append(map.MiddleLayer[i]);
				middlelayerbuilder.Append(" ");
			}

			middlelayer.InnerText = middlelayerbuilder.ToString();

			// Top Layer
			var toplayer = doc.CreateElement("TopLayer");

			var toplayerbuilder = new StringBuilder();
			for (int i = 0; i < map.TopLayer.Length; i++)
			{
				toplayerbuilder.Append(map.TopLayer[i]);
				toplayerbuilder.Append(" ");
			}

			toplayer.InnerText = toplayerbuilder.ToString();

			// Collision Layer
			var collisionLayer = doc.CreateElement("CollisionLayer");

			var collisionlayerbuilder = new StringBuilder();
			for (int i = 0; i < map.CollisionLayer.Length; i++)
			{
				collisionlayerbuilder.Append(map.CollisionLayer[i]);
				collisionlayerbuilder.Append(" ");
			}

			collisionLayer.InnerText = collisionlayerbuilder.ToString();

			// Append children and save
			mapsize.AppendChild(mapsizex);
			mapsize.AppendChild(mapsizey);

			mapElement.AppendChild(name);
			mapElement.AppendChild(tileset);
			mapElement.AppendChild(mapsize);
			mapElement.AppendChild(tilepixelsize);
			mapElement.AppendChild(baselayer);
			mapElement.AppendChild(middlelayer);
			mapElement.AppendChild(collisionLayer);
			mapElement.AppendChild(toplayer);

			doc.AppendChild(mapElement);
			doc.Save(location.FullName);

			MapEditorManager.CurrentMapLocation = location;

        }

        public static Map LoadMap(FileInfo location)
        {
            Map newMap = new Map();
            newMap.LoadMap(location);

			MapEditorManager.CurrentMapLocation = location;
            return newMap;
        }

        public static Map ApplySettings(Map oldMap)
        {
            Map newMap = new Map();

            newMap.Name = oldMap.Name;
            newMap.MapSize = oldMap.MapSize;
            newMap.TileSize = oldMap.TileSize;
            newMap.TextureName = oldMap.TextureName;
            newMap.Texture = TileEngine.TextureManager.GetTexture(newMap.TextureName);

            newMap.BaseLayer = new int[oldMap.MapSize.X * oldMap.MapSize.Y];
            newMap.MiddleLayer = new int[oldMap.MapSize.X * oldMap.MapSize.Y];
            newMap.TopLayer = new int[oldMap.MapSize.X * oldMap.MapSize.Y];
			newMap.CollisionLayer = new int[oldMap.MapSize.X * oldMap.MapSize.Y];

            for (int i = 0; i < (oldMap.MapSize.X * oldMap.MapSize.Y); i++)
            {
                newMap.BaseLayer[i] = ((i < oldMap.BaseLayer.Length) ? oldMap.BaseLayer[i] : 0);
                newMap.MiddleLayer[i] = ((i < oldMap.MiddleLayer.Length) ? oldMap.MiddleLayer[i] : -1);
                newMap.TopLayer[i] = ((i < oldMap.TopLayer.Length) ? oldMap.TopLayer[i] : -1);
				newMap.CollisionLayer[i] = ((i < oldMap.CollisionLayer.Length) ? oldMap.CollisionLayer[i] : -1);
            }


            return newMap;

        }

		public static void SetWorldMap(Map map)
		{
			foreach (MapHeader tmpHeader in TileEngine.World.Values)
				tmpHeader.Unload();

			TileEngine.World.Clear();

			MapHeader header = new MapHeader(map.Name, string.Empty);
			header.Map = map;
			header.MapLocation = new Point(0, 0);

			TileEngine.World.Add(map.Name, header);
				
		}
    }

	public enum Tools
	{
		Pencil,
		Rectangle,
		Bucket,
	}
}
