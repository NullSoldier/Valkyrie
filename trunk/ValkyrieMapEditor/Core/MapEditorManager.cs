using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Valkyrie.Library.Core;
using System.Xml;
using Microsoft.Xna.Framework;
using Valkyrie.Library.Events;
using Valkyrie.Library;
using Valkyrie.Engine.Maps;
using Valkyrie.Engine;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine.Core;
using Valkyrie.Library.Providers;
using System.Reflection;

namespace ValkyrieMapEditor
{
    public static class MapEditorManager
	{
		#region Public Properties
		
		public static EditorXNA GameInstance
		{
			get { return gameinstance; }
			set { gameinstance = value; }
		}

		public static MapLayers CurrentLayer
		{
			get { return currentlayer; }
			set { currentlayer = value; }
		}

		public static Tools CurrentTool
		{
			get { return tool; }
			set { tool = value; }
		}

		public static ViewMode ViewMode
		{
			get { return viewmode; }
			set { viewmode = value; }
		}

		public static Map CurrentMap
		{
			get { return currentmap; }
		}

		public static bool IgnoreInput
		{
			get { return ignoreinput; }
			set { ignoreinput = value; }
		}

		public static bool NoEvents
		{
			get { return noevents; }
			set { noevents = value; }
		}

		public static Rectangle SelectedTilesRectangle
		{
			get { return selectedtilesrectangle; }
			set { selectedtilesrectangle = value; }
		}
		
		#endregion

		#region Public Methods

		public static Map LoadMap(string path, IMapProvider provider)
		{
			var map = provider.GetMap(new Uri(path), MapEditorManager.GameInstance.Engine.EventProvider);
			map.Texture = GameInstance.Engine.TextureManager.GetTexture(map.TextureName);
			
			return map;
		}

		public static void SaveMap (Map map, FileInfo location)
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
			tilepixelsize.InnerText = map.TileSize.ToString();

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
			//foreach(IMapEvent e in map.EventList)
			//{
			//eventLayer.AppendChild(TileEngine.EventManager.EventToXmlNode(e, doc));
			//}

			// Animations
			var animations = doc.CreateElement("AnimatedTiles");

			foreach(var FrameAnimation in map.AnimatedTiles.Values)
			{
				var tileNode = doc.CreateElement("AnimatedTile");

				var tileid = doc.CreateElement("TileID");
				tileid.InnerText = ((FrameAnimation.InitialFrameRect.Y / CurrentMap.TileSize) * CurrentMap.TilesPerRow + FrameAnimation.InitialFrameRect.X).ToString();

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
		}

		public static void SetCurrentMap (Map map)
		{
			GameInstance.Engine.WorldManager.ClearWorlds();

			MapEditorManager.currentmap = map;

			MapHeader header = new MapHeader(map.Name, new MapPoint(0, 0), string.Empty);
			header.Map = map;

			World world = new World("Default");
			world.AddMap(header);

			GameInstance.Engine.WorldManager.AddWorld(world);
			GameInstance.Engine.SceneProvider.GetCameras().FirstOrDefault().Value.CenterOriginOnPoint(0, 0);
		}

		public static Map ApplyMapProperties (Map oldMap)
		{
			Map newMap = new Map();

			newMap.Name = oldMap.Name;
			newMap.MapSize = oldMap.MapSize;
			newMap.TileSize = oldMap.TileSize;
			newMap.TextureName = oldMap.TextureName;
			// newMap.Texture = TileEngine.TextureManager.GetTexture(newMap.TextureName);

			newMap.UnderLayer = new int[newMap.MapSize.X * newMap.MapSize.Y];
			newMap.BaseLayer = new int[newMap.MapSize.X * newMap.MapSize.Y];
			newMap.MiddleLayer = new int[newMap.MapSize.X * newMap.MapSize.Y];
			newMap.TopLayer = new int[newMap.MapSize.X * newMap.MapSize.Y];
			newMap.CollisionLayer = new int[newMap.MapSize.X * newMap.MapSize.Y];

			// Initialize to -1 for optimization. The engine doesn't render -1
			for(int i = 0; i < (newMap.MapSize.X * newMap.MapSize.Y); i++)
			{
				newMap.UnderLayer[i] = -1;
				newMap.BaseLayer[i] = -1;
				newMap.MiddleLayer[i] = -1;
				newMap.TopLayer[i] = -1;
				newMap.CollisionLayer[i] = -1;
			}

			return newMap;
		}

		#endregion

		private static Map currentmap = null;
		private static EditorXNA gameinstance = null;
		private static MapLayers currentlayer = MapLayers.BaseLayer;
		private static ViewMode viewmode = ViewMode.All;
		private static Tools tool = Tools.Pencil;
		private static Rectangle selectedtilesrectangle = Rectangle.Empty;
		private static bool ignoreinput = false;
		private static bool noevents = false;

		#region OldGarbage
		//public static Point MouseLocation
		//{
		//    get
		//    {
		//        lock(MapEditorManager.MouseLock)
		//            return MapEditorManager.mouselocation;
		//    }
		//    set
		//    {
		//        lock(MapEditorManager.MouseLock)
		//            MapEditorManager.mouselocation = value;
		//    }
		//}
		#endregion
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