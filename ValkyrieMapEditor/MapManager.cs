using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using valkyrie.Core;
using System.Xml;

namespace ValkyrieMapEditor
{
    public static class MapManager
    {
        public static int CurrentTile = 0;
        public static MapLayer CurrentLayer = MapLayer.BaseLayer;
		public static FileInfo CurrentMapLocation;

        public static void SaveMap(Map map, FileInfo location)
        {
			XmlDocument doc = new XmlDocument();

			var mapElement = doc.CreateElement("Map");

			var name = doc.CreateElement("Name");
			name.InnerText = map.Name;

			var tileset = doc.CreateElement("TileSet");
			tileset.InnerText = map.TextureName;

			var tilesetsize = doc.CreateElement("TileSetSize");
			tilesetsize.InnerText = String.Format("{0} {1}", map.TileSize.X, map.TileSize.Y);

			var mapsize = doc.CreateElement("MapSize");
			mapsize.InnerText = String.Format("{0} {1}", map.MapSize.X, map.MapSize.Y);

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

			// Append children and save
			mapElement.AppendChild(name);
			mapElement.AppendChild(tileset);
			mapElement.AppendChild(tilesetsize);
			mapElement.AppendChild(mapsize);
			mapElement.AppendChild(tilepixelsize);
			mapElement.AppendChild(baselayer);
			mapElement.AppendChild(middlelayer);
			mapElement.AppendChild(toplayer);

			doc.AppendChild(mapElement);
			doc.Save(location.FullName);

			MapManager.CurrentMapLocation = location;

        }

        public static Map LoadMap(FileInfo location)
        {
            Map newMap = new Map();
            newMap.LoadMap(location);

			MapManager.CurrentMapLocation = location;
            return newMap;
        }

        public static Map ApplySettings(Map oldMap)
        {
            Map newMap = new Map();

            newMap.Name = oldMap.Name;
            newMap.MapSize = oldMap.MapSize;
            newMap.TileSize = oldMap.TileSize;
            newMap.TilesPerRow = oldMap.TilesPerRow;
            newMap.TextureName = oldMap.TextureName;
            newMap.Texture = TileEngine.TextureManager.GetTexture(newMap.TextureName);

            newMap.BaseLayer = new int[oldMap.MapSize.X * oldMap.MapSize.Y];
            newMap.MiddleLayer = new int[oldMap.MapSize.X * oldMap.MapSize.Y];
            newMap.TopLayer = new int[oldMap.MapSize.X * oldMap.MapSize.Y];

            for (int i = 0; i < (oldMap.MapSize.X * oldMap.MapSize.Y); i++)
            {
                newMap.BaseLayer[i] = ((i < oldMap.BaseLayer.Length) ? oldMap.BaseLayer[i] : 0);
                newMap.MiddleLayer[i] = ((i < oldMap.MiddleLayer.Length) ? oldMap.MiddleLayer[i] : -1);
                newMap.TopLayer[i] = ((i < oldMap.TopLayer.Length) ? oldMap.TopLayer[i] : -1);
            }


            return newMap;

        }
    }
}
