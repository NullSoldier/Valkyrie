using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ValkyrieLibrary.Core;
using System.Xml;
using Microsoft.Xna.Framework;
using ValkyrieLibrary.Maps;
using ValkyrieLibrary.Events;
using ValkyrieLibrary;

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
            map.Save(location.FullName);
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