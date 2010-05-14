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
using Valkyrie.Engine.Events;
using ValkyrieMapEditor.Core.Actions;

namespace ValkyrieMapEditor
{
	public static class MapEditorManager
	{
		#region Public Properties

		public static EditorXNA GameInstance
		{
			get;
			set;
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
			get;
			private set;
		}

		public static bool IsMapLoaded
		{
			get { return (CurrentMap != null); }
		}

		public static ActionManager ActionManager
		{
			get;
			set;
		}

		public static bool IgnoreInput
		{
			get;
			set;
		}

		public static bool NoEvents
		{
			get;
			set;
		}

		public static Rectangle SelectedTilesRectangle
		{
			get { return selectedtilesrectangle; }
			set { selectedtilesrectangle = value; }
		}

		public static event EventHandler MapChanged
		{
			add { mapchanged += value; }
			remove { mapchanged -= value; }
		}


		#endregion

		#region Public Methods

		public static void OnMapChanged()
		{
			var handler = mapchanged;
			if (handler != null)
				handler(null, EventArgs.Empty);
		}

		public static Map LoadMap(string path, IMapProvider provider)
		{
			var map = provider.GetMap(path, MapEditorManager.GameInstance.Engine.EventProvider);
			map.Texture = GameInstance.Engine.TextureManager.GetTexture(map.TextureName);

			return map;
		}

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
			tilepixelsize.InnerText = map.TileSize.ToString();

			// Under Layer
			var underlayer = doc.CreateElement("UnderLayer");

			var underlayerbuilder = new StringBuilder();
			for (int i = 0; i < map.UnderLayer.Length; i++)
			{
				underlayerbuilder.Append(map.UnderLayer[i]);
				underlayerbuilder.Append(" ");
			}

			underlayer.InnerText = underlayerbuilder.ToString();

			// Base Layer
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

			// Opaque Layer
			var opaqueLayer = doc.CreateElement("OpaqueLayer");

			var opaquelayerbuilder = new StringBuilder();
			for (int i = 0; i < map.OpaqueLayer.Length; i++)
			{
				opaquelayerbuilder.Append(map.OpaqueLayer[i]);
				opaquelayerbuilder.Append(" ");
			}

			opaqueLayer.InnerText = opaquelayerbuilder.ToString();

			// Events
			var eventLayer = doc.CreateElement("Events");
			foreach (IMapEvent mapevent in GameInstance.Engine.EventProvider.GetMapsEvents(map.Name))
			{
				eventLayer.AppendChild(EventToXmlNode(mapevent, doc));
			}

			// Animations
			var animations = doc.CreateElement("AnimatedTiles");

			foreach (var FrameAnimation in map.AnimatedTiles.Values)
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
			mapElement.AppendChild(opaqueLayer);
			mapElement.AppendChild(eventLayer);
			mapElement.AppendChild(animations);

			doc.AppendChild(mapElement);
			doc.Save(location.FullName);
		}

		public static void SetCurrentMap(Map map)
		{
			if (map == null)
				throw new ArgumentNullException("map");

			GameInstance.Engine.WorldManager.ClearWorlds();

			MapEditorManager.CurrentMap = map;

			MapHeader header = new MapHeader(map.Name, new MapPoint(0, 0), string.Empty);
			header.Map = map;

			World world = new World("Default");
			world.AddMap(header);

			GameInstance.Engine.WorldManager.AddWorld(world);

			var camera = GameInstance.Engine.SceneProvider.Cameras.GetItems().FirstOrDefault().Value;
			if (camera != null)
			{
				camera.CenterOriginOnPoint(0, 0);
				camera.CurrentMap = header;
			}

			ActionManager.Reset();			
		}

		public static Map ApplyMapProperties(Map oldMap)
		{
			return MapEditorManager.ResizeMap(oldMap, oldMap.MapSize);
		}

		public static Map ResizeMap(Map map, MapPoint newsize)
		{
			Map newMap = new Map();
			newMap.Name = map.Name;
			newMap.TextureName = map.TextureName;
			newMap.Texture = map.Texture;
			newMap.TileSize = map.TileSize;
			foreach (var animation in map.AnimatedTiles)
				newMap.AnimatedTiles.Add(animation.Key, animation.Value);

			newMap.MapSize = newsize;
			newMap.UnderLayer = new int[newsize.IntX * newsize.IntY];
			newMap.BaseLayer = new int[newsize.IntX * newsize.IntY];
			newMap.MiddleLayer = new int[newsize.IntX * newsize.IntY];
			newMap.TopLayer = new int[newsize.IntX * newsize.IntY];
			newMap.CollisionLayer = new int[newsize.IntX * newsize.IntY];
			newMap.OpaqueLayer = new int[newsize.IntX * newsize.IntY];

			for (int i = 0; i < (newMap.MapSize.X * newMap.MapSize.Y); i++)
			{
				newMap.UnderLayer[i] = -1;
				newMap.BaseLayer[i] = -1;
				newMap.MiddleLayer[i] = -1;
				newMap.TopLayer[i] = -1;
				newMap.CollisionLayer[i] = -1;
				newMap.OpaqueLayer[i] = -1;
			}

			for (int y = 0; y < newsize.Y; y++)
			{
				for (int x = 0; x < newsize.X; x++)
				{
					// If it's outside the Y range always fill with -1
					if (y >= map.MapSize.Y)
					{
						newMap.SetLayerValue(new MapPoint(x, y), MapLayers.UnderLayer, -1);
						continue;
					}

					// If it's outside the X range but not the Y range, fill with 0
					if (x >= map.MapSize.X)
					{
						newMap.SetLayerValue(new MapPoint(x, y), MapLayers.UnderLayer, -1);
						continue;
					}

					//Fill the new location with the value of the old location
					newMap.SetLayerValue(new MapPoint(x, y), MapLayers.UnderLayer, map.GetLayerValue(new MapPoint(x, y), MapLayers.UnderLayer));
					newMap.SetLayerValue(new MapPoint(x, y), MapLayers.BaseLayer, map.GetLayerValue(new MapPoint(x, y), MapLayers.BaseLayer));
					newMap.SetLayerValue(new MapPoint(x, y), MapLayers.MiddleLayer, map.GetLayerValue(new MapPoint(x, y), MapLayers.MiddleLayer));
					newMap.SetLayerValue(new MapPoint(x, y), MapLayers.TopLayer, map.GetLayerValue(new MapPoint(x, y), MapLayers.TopLayer));
					newMap.SetLayerValue(new MapPoint(x, y), MapLayers.CollisionLayer, map.GetLayerValue(new MapPoint(x, y), MapLayers.CollisionLayer));
					newMap.SetLayerValue(new MapPoint(x, y), MapLayers.OpaqueLayer, map.GetLayerValue(new MapPoint(x, y), MapLayers.OpaqueLayer));
				}
			}

			return newMap;
		}

		#endregion

		private static MapLayers currentlayer = MapLayers.BaseLayer;
		private static ViewMode viewmode = ViewMode.All;
		private static Tools tool = Tools.Select;
		private static Rectangle selectedtilesrectangle = new Rectangle(0, 0, 1, 1);
		private static event EventHandler mapchanged;

		private static XmlNode EventToXmlNode(IMapEvent mapevent, XmlDocument doc)
		{
			XmlElement xmlevent = doc.CreateElement("Event");

			// Properties of event	
			XmlElement loc = doc.CreateElement("Location");
			new MapPoint(mapevent.Rectangle.X, mapevent.Rectangle.Y).ToXml(doc, loc);

			XmlElement size = doc.CreateElement("Size");
			new MapPoint(mapevent.Rectangle.Width, mapevent.Rectangle.Height).ToXml(doc, size);

			XmlElement type = doc.CreateElement("Type");
			type.InnerText = mapevent.GetType().FullName;

			XmlElement dir = doc.CreateElement("Dir");
			dir.InnerText = mapevent.Direction.ToString();

			XmlElement activation = doc.CreateElement("Activation");
			activation.InnerText = ((int)mapevent.Activation).ToString();

			XmlElement parmRoot = doc.CreateElement("Parameters");

			foreach (var parm in mapevent.Parameters)
			{
				XmlElement pname = doc.CreateElement("Name");
				pname.InnerText = parm.Key;

				XmlElement pvalue = doc.CreateElement("Value");
				pvalue.InnerText = parm.Value;

				XmlElement parmNode = doc.CreateElement("Parameter");
				parmNode.AppendChild(pname);
				parmNode.AppendChild(pvalue);

				parmRoot.AppendChild(parmNode);
			}

			xmlevent.AppendChild(type);
			xmlevent.AppendChild(activation);
			xmlevent.AppendChild(dir);
			xmlevent.AppendChild(loc);
			xmlevent.AppendChild(size);
			xmlevent.AppendChild(parmRoot);

			return xmlevent;
		}
	}

	public enum Tools
	{
		Pencil,
		Rectangle,
		Bucket,
		Select
	}

	public enum ViewMode
	{
		All,
		Below,
		Dim
	}

	public class TextureNotFoundException : Exception
	{

	}
}