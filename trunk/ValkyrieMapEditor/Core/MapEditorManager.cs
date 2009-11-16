using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Valkyrie.Library.Core;
using System.Xml;
using Microsoft.Xna.Framework;
using Valkyrie.Library.Maps;
using Valkyrie.Library.Events;
using Valkyrie.Library;
using Valkyrie.Library.Maps.MapProvider;

namespace ValkyrieMapEditor
{
    public static class MapEditorManager
    {
		public static Rectangle SelectedTilesRect = new Rectangle(0, 0, 0, 0);
        public static MapLayers CurrentLayer = MapLayers.BaseLayer;
		public static FileInfo CurrentMapLocation;

        public static ViewMode ViewMode = ViewMode.All;

		public static bool IgnoreInput = false;
		public static Tools CurrentTool = Tools.Pencil;
        public static EditorXNA GameInstance = null;
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

		public static FileInfo CurrentTileSetLocation
		{
			get
			{
				if (TileEngine.IsMapLoaded)
					return new FileInfo(Path.Combine(Environment.CurrentDirectory, Path.Combine(TileEngine.Configuration[TileEngineConfigurationName.GraphicsRoot], TileEngine.CurrentMapChunk.TextureName)));
				else
					return new FileInfo(String.Empty);
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

			// Under Layer
			var underlayer = doc.CreateElement("UnderLayer");

			var underlayerbuilder = new StringBuilder();
			for(int i = 0; i < map.UnderLayer.Length; i++)
			{
				underlayerbuilder.Append(map.UnderLayer[i]);
				underlayerbuilder.Append(" ");
			}

			underlayer.InnerText = underlayerbuilder.ToString();

			// Base Layer
			var baselayer = doc.CreateElement("BaseLayer");

			var baselayerbuilder = new StringBuilder();
			for(int i = 0; i < map.BaseLayer.Length; i++)
			{
				baselayerbuilder.Append(map.BaseLayer[i]);
				baselayerbuilder.Append(" ");
			}

			baselayer.InnerText = baselayerbuilder.ToString();

			// Middle Layer
			var middlelayer = doc.CreateElement("MiddleLayer");

			var middlelayerbuilder = new StringBuilder();
			for(int i = 0; i < map.MiddleLayer.Length; i++)
			{
				middlelayerbuilder.Append(map.MiddleLayer[i]);
				middlelayerbuilder.Append(" ");
			}

			middlelayer.InnerText = middlelayerbuilder.ToString();

			// Top Layer
			var toplayer = doc.CreateElement("TopLayer");

			var toplayerbuilder = new StringBuilder();
			for(int i = 0; i < map.TopLayer.Length; i++)
			{
				toplayerbuilder.Append(map.TopLayer[i]);
				toplayerbuilder.Append(" ");
			}

			toplayer.InnerText = toplayerbuilder.ToString();

			// Collision Layer
			var collisionLayer = doc.CreateElement("CollisionLayer");

			var collisionlayerbuilder = new StringBuilder();
			for(int i = 0; i < map.CollisionLayer.Length; i++)
			{
				collisionlayerbuilder.Append(map.CollisionLayer[i]);
				collisionlayerbuilder.Append(" ");
			}

			collisionLayer.InnerText = collisionlayerbuilder.ToString();

			// Events
			var eventLayer = doc.CreateElement("Events");
			foreach(IMapEvent e in map.EventList)
			{
				eventLayer.AppendChild(TileEngine.EventManager.EventToXmlNode(e, doc));
			}

			// Animations
			var animations = doc.CreateElement("AnimatedTiles");

			foreach(var FrameAnimation in map.AnimatedTiles.Values)
			{
				var tileNode = doc.CreateElement("AnimatedTile");

				var tileid = doc.CreateElement("TileID");
				tileid.InnerText = ((FrameAnimation.InitialFrameRect.Y / TileEngine.TileSize) * TileEngine.CurrentMapChunk.TilesPerRow + FrameAnimation.InitialFrameRect.X).ToString();

				var tilerect = doc.CreateElement("TileRect");
				tilerect.InnerText = string.Format("{0} {1} {2} {3}", FrameAnimation.InitialFrameRect.X, FrameAnimation.InitialFrameRect.Y, FrameAnimation.InitialFrameRect.Width, FrameAnimation.InitialFrameRect.Height);

				var framecount = doc.CreateElement("FrameCount");
				framecount.InnerText = FrameAnimation.FrameCount.ToString();

				tileNode.AppendChild(tileid);
				tileNode.AppendChild(tilerect);
				tileNode.AppendChild(framecount);

				animations.AppendChild(tileNode);
			}

			// Append children and save
			mapsize.AppendChild(mapsizex);
			mapsize.AppendChild(mapsizey);

			mapElement.AppendChild(name);
			mapElement.AppendChild(tileset);
			mapElement.AppendChild(mapsize);
			mapElement.AppendChild(tilepixelsize);
			mapElement.AppendChild(underlayer);
			mapElement.AppendChild(baselayer);
			mapElement.AppendChild(middlelayer);
			mapElement.AppendChild(toplayer);
			mapElement.AppendChild(collisionLayer);
			mapElement.AppendChild(eventLayer);
			mapElement.AppendChild(animations);

			doc.AppendChild(mapElement);
			doc.Save(location.FullName);

			MapEditorManager.CurrentMapLocation = location;
		}

        public static Map LoadMap(FileInfo location, IMapProvider provider)
        {
			MapEditorManager.CurrentMapLocation = location;

            return provider.GetMap(location, TileEngine.EventManager);

			//location.FullName.Substring(0, location.FullName.Length - location.Name.Length)
        }

        public static Map ApplySettings(Map oldMap)
        {
            Map newMap = new Map();

            newMap.Name = oldMap.Name;
            newMap.MapSize = oldMap.MapSize;
            newMap.TileSize = oldMap.TileSize;
            newMap.TextureName = oldMap.TextureName;
            newMap.Texture = TileEngine.TextureManager.GetTexture(newMap.TextureName);

			newMap.UnderLayer = new int[newMap.MapSize.X * newMap.MapSize.Y];
			newMap.BaseLayer = new int[newMap.MapSize.X * newMap.MapSize.Y];
			newMap.MiddleLayer = new int[newMap.MapSize.X * newMap.MapSize.Y];
			newMap.TopLayer = new int[newMap.MapSize.X * newMap.MapSize.Y];
			newMap.CollisionLayer = new int[newMap.MapSize.X * newMap.MapSize.Y];

			// Initialize to -1 for optimization. The engine doesn't render -1
			for (int i = 0; i < (newMap.MapSize.X * newMap.MapSize.Y); i++)
			{
				newMap.UnderLayer[i] = -1;
				newMap.BaseLayer[i] = -1;
				newMap.MiddleLayer[i] = -1;
				newMap.TopLayer[i] = -1;
				newMap.CollisionLayer[i] = -1;
			}

			// Copy over the -1 initialized engine
			for (int x = 0; x < newMap.MapSize.X; x++)
			{
				for (int y = 0; y < newMap.MapSize.Y; y++)
				{
				/*	newMap.BaseLayer[i] = ((i < oldMap.BaseLayer.Length) ? oldMap.BaseLayer[i] : -1);
					newMap.MiddleLayer[i] = ((i < oldMap.MiddleLayer.Length) ? oldMap.MiddleLayer[i] : -1);
					newMap.TopLayer[i] = ((i < oldMap.TopLayer.Length) ? oldMap.TopLayer[i] : -1);
					newMap.CollisionLayer[i] = ((i < oldMap.CollisionLayer.Length) ? oldMap.CollisionLayer[i] : -1);*/
				}
			}

            return newMap;
        }

		public static void SetWorldMap(Map map)
		{
            if (TileEngine.WorldManager.CurrentWorld != null)
            {
                foreach (MapHeader tmpHeader in TileEngine.WorldManager.CurrentWorld.MapList.Values)
                    tmpHeader.Unload();

                TileEngine.WorldManager.CurrentWorld.MapList.Clear();
            }

            TileEngine.WorldManager.WorldsList.Clear();

			MapHeader header = new MapHeader(map.Name, string.Empty);
			header.Map = map;
			header.MapLocation = new MapPoint(0, 0);

            World w = new World();
            w.MapList.Add(map.Name, header);

            TileEngine.WorldManager.WorldsList.Add(map.Name, w);
            TileEngine.WorldManager.SetWorld(map.Name, null, false);
            TileEngine.Camera.CenterOriginOnPoint(0, 0);
		}

		public int GetTileSetValue (MapPoint point)
		{
			// Returns the TileID of a tile in the tile set using it's X, and Y coordinents
			return (point.Y * this.TilesPerRow + point.X);
		}
    }

	public enum Tools
	{
		Pencil,
		Rectangle,
		Bucket,
	}

    public enum ViewMode
    {
        All,
        Below,
        Dim
    }
}