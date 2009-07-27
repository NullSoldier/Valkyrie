using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using ValkyrieLibrary.Animation;
using ValkyrieLibrary.Core;
using ValkyrieLibrary.Maps;
using ValkyrieLibrary.Events;

namespace ValkyrieLibrary.Maps
{
	public class Map
	{
        #region Public Properties

		public int[] UnderLayer { get; set; }
        public int[] BaseLayer { get; set; }
        public int[] MiddleLayer { get; set; }
        public int[] TopLayer { get; set; }
        public int[] CollisionLayer { get; set; }

        public List<BaseMapEvent> EventList;
        public MapPoint MapSize { get; set; }
        public ScreenPoint TileSize { get; set; }
        public String TextureName { get; set; }
        public String Name { get; set; }

        public int TilesPerRow {get { return ((this.Texture != null) ? (this.Texture.Width / this.TileSize.X) : 0); }}
        public int TilesPerCol {get { return ((this.Texture != null) ? (this.Texture.Height / this.TileSize.Y) : 0); }}

        public Dictionary<int, FrameAnimation> AnimatedTiles;

        public Texture2D Texture
        {
            get
            {
                // Lazy texture loading
                this.texture = TileEngine.TextureManager.GetTexture(this.TextureName);

                return this.texture;
            }
            set { this.texture = value; }
        }

        private Texture2D texture;

        #endregion



		public Map()
		{
			this.UnderLayer = new int[0];
            this.BaseLayer = new int[0];
            this.MiddleLayer = new int[0];
            this.TopLayer = new int[0];
            this.CollisionLayer = new int[0];
            this.EventList = new List<BaseMapEvent>();
			this.AnimatedTiles = new Dictionary<int, FrameAnimation>();
		}

		public void Update(GameTime gameTime)
		{
			foreach (FrameAnimation anim in this.AnimatedTiles.Values)
				anim.Update(gameTime);
		}

        public int GetLayerValue(MapPoint point, MapLayers layer)
		{
			if (point.X < 0 || (point.X > this.MapSize.X) || point.Y < 0 || (point.Y > this.MapSize.Y))
				throw new ArgumentOutOfRangeException();

            switch (layer)
            {
                default:
				case MapLayers.UnderLayer:
					return this.UnderLayer[point.Y * this.MapSize.X + point.X];

                case MapLayers.BaseLayer: 
                    return this.BaseLayer[point.Y * this.MapSize.X + point.X];

                case MapLayers.MiddleLayer: 
                    return this.MiddleLayer[point.Y * this.MapSize.X + point.X];

                case MapLayers.TopLayer: 
                    return this.TopLayer[point.Y * this.MapSize.X + point.X];

                case MapLayers.CollisionLayer: 
                    return this.CollisionLayer[point.Y * this.MapSize.X + point.X];
            }
		}
       
        private void SetLayerValue(MapPoint point, int value, MapLayers layer)
        {
            if (point.X < 0 || (point.X > this.MapSize.X) || point.Y < 0 || (point.Y > this.MapSize.Y))
                throw new ArgumentOutOfRangeException();

            switch (layer)
            {
                default:
				case MapLayers.UnderLayer:
					this.UnderLayer[point.Y * this.MapSize.X + point.X] = value; break;

                case MapLayers.BaseLayer: 
                    this.BaseLayer[point.Y * this.MapSize.X + point.X] = value; break;

                case MapLayers.MiddleLayer:
                    this.MiddleLayer[point.Y * this.MapSize.X + point.X] = value; break;

                case MapLayers.TopLayer: 
                    this.TopLayer[point.Y * this.MapSize.X + point.X] = value; break;

                case MapLayers.CollisionLayer:
                    this.CollisionLayer[point.Y * this.MapSize.X + point.X] = value; break;
            }
        }

		public Rectangle GetLayerSourceRect(MapPoint point, MapLayers layer)
		{
			if (point.X < 0 || point.X > this.MapSize.X ||
				point.Y < 0 || point.Y >= this.MapSize.Y)
			{
				return Rectangle.Empty;
			}

			int LayerValue = this.GetLayerValue(point, layer);
            if (LayerValue < 0)
				return Rectangle.Empty;

			if (this.AnimatedTiles.ContainsKey(LayerValue))
				return this.AnimatedTiles[LayerValue].FrameRectangle;
			else
				return new Rectangle(
					(LayerValue % this.TilesPerRow) * TileSize.X,
					(LayerValue / this.TilesPerRow) * TileSize.Y,
					TileSize.X - 1, TileSize.Y - 1);
        }

        #region Map Management
		public void LoadMap(FileInfo MapFile)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(MapFile.FullName);
			
			XmlNodeList nodes = doc.GetElementsByTagName("Map");
			XmlNodeList innerNodes = nodes[0].ChildNodes;

			for (int i = 0; i < innerNodes.Count; i++)
			{
				if (innerNodes[i].Name == "Name")
					this.Name = innerNodes[i].InnerText;

				else if (innerNodes[i].Name == "TileSet")
					this.TextureName = innerNodes[i].InnerText;

				else if (innerNodes[i].Name == "MapSize")
				{
					int x=0, y=0;

					foreach (XmlNode node in innerNodes[i].ChildNodes)
					{
						if (node.Name == "X")
							x = Convert.ToInt32(node.InnerText);
						else if (node.Name == "Y")
							y = Convert.ToInt32(node.InnerText);
					}
					this.MapSize = new MapPoint(x, y);
				}
				else if (innerNodes[i].Name == "TilePixelSize")
				{
					int size = Convert.ToInt32(innerNodes[i].InnerText);
					this.TileSize = new ScreenPoint(size, size); // Tiles are always square
				}

				else if (innerNodes[i].Name == "UnderLayer")
				{
					string[] baseText = innerNodes[i].InnerText.Replace("\r\n", string.Empty).Replace("   ", string.Empty).Trim().Split(' ');

					this.UnderLayer = Array.ConvertAll<string, int>(baseText, new Converter<string, int>(this.ConvertStringToInt));
				}

				else if (innerNodes[i].Name == "BaseLayer")
				{
					string[] baseText = innerNodes[i].InnerText.Replace("\r\n", string.Empty).Replace("   ", string.Empty).Trim().Split(' ');

					this.BaseLayer = Array.ConvertAll<string, int>(baseText, new Converter<string, int>(this.ConvertStringToInt));
				}

				else if (innerNodes[i].Name == "MiddleLayer")
				{
					string[] baseText = innerNodes[i].InnerText.Replace("\r\n", string.Empty).Replace("   ", string.Empty).Trim().Split(' ');

					this.MiddleLayer = Array.ConvertAll<string, int>(baseText, new Converter<string, int>(this.ConvertStringToInt));
				}

				else if (innerNodes[i].Name == "TopLayer")
				{
					string[] baseText = innerNodes[i].InnerText.Replace("\r\n", string.Empty).Replace("   ", string.Empty).Trim().Split(' ');

					this.TopLayer = Array.ConvertAll<string, int>(baseText, new Converter<string, int>(this.ConvertStringToInt));
				}
				else if (innerNodes[i].Name == "CollisionLayer")
				{
					string[] baseText = innerNodes[i].InnerText.Replace("\r\n", string.Empty).Replace("   ", string.Empty).Trim().Split(' ');

					this.CollisionLayer = Array.ConvertAll<string, int>(baseText, new Converter<string, int>(this.ConvertStringToInt));
				}
				else if (innerNodes[i].Name == "AnimatedTiles")
				{
					XmlNodeList tiles = innerNodes[i].ChildNodes;

					foreach (XmlNode node in tiles)
					{
						int tileID = 0;
						int frameCount = 0;
						Rectangle tileRect = Rectangle.Empty;

						foreach (XmlNode subnode in node.ChildNodes)
						{
							if (subnode.Name == "TileID")
								tileID = Convert.ToInt32(subnode.InnerText);
							else if (subnode.Name == "FrameCount")
								frameCount = Convert.ToInt32(subnode.InnerText);
							else if (subnode.Name == "TileRect")
							{
								var data = Array.ConvertAll<string, int>(subnode.InnerText.Split(' '), new Converter<string, int>(this.ConvertStringToInt));
								tileRect = new Rectangle(data[0], data[1], data[2], data[3]);
							}

						}

						this.AnimatedTiles.Add(tileID, new FrameAnimation(tileRect, frameCount));
					}
				}
				else if (innerNodes[i].Name == "Events")
				{
					if (this.EventList.Count > 0) // Already loaded? Don't load again
						continue;

					var root = innerNodes[i];

					XmlNodeList events = root.ChildNodes;

					foreach (XmlNode node in events)
					{
						this.EventList.Add(TileEngine.EventManager.LoadEventFromXml(node));
					}

					TileEngine.EventManager.LoadEvents(this);
				}
			}

			
			
		}

        public void SetData(MapLayers layer, MapPoint location, int value)
        {
            if (layer == MapLayers.CollisionLayer)
            {
                if (GetLayerValue(location, layer) != -1)
                    SetLayerValue(location, -1, layer);
                else
                    SetLayerValue(location, 1, layer);
            }
            else
            {
                SetLayerValue(location, value, layer); 
            }
        }
        #endregion

		public int GetTileSetValue(MapPoint point)
		{
			// Returns the TileID of a tile in the tile set using it's X, and Y coordinents
			return (point.Y * this.TilesPerRow + point.X);
		}

		private int ConvertStringToInt(string value)
		{
			return Convert.ToInt32(value);
		}

		public bool TilePointInMapLocal(MapPoint point)
		{
            Rectangle mapSize = (new MapPoint(0,0)).ToRect(TileEngine.CurrentMapChunk.MapSize.ToPoint());
            return (mapSize.Contains(point.ToPoint()) == true);
		}

		public bool TilePointInMapGlobal(MapPoint point)
		{
            Rectangle mapSize = TileEngine.WorldManager.CurrentWorld.MapList[this.Name].MapLocation.ToRect(this.MapSize.ToPoint());
            return (mapSize.Contains(point.ToPoint()) == true);
		}

        public bool IsVisableToPlayer()
        {
            Rectangle mapSize = TileEngine.WorldManager.CurrentWorld.MapList[this.Name].MapLocation.ToRect(this.MapSize.ToPoint());
            Rectangle worldSize = new Rectangle(0,0, TileEngine.Viewport.Width, TileEngine.Viewport.Height);
  
            return (mapSize.Intersects(worldSize) == true);
        }

        public void Save(String location)
        {
            XmlDocument doc = new XmlDocument();

            var mapElement = doc.CreateElement("Map");

            var name = doc.CreateElement("Name");
            name.InnerText = this.Name;

            var tileset = doc.CreateElement("TileSet");
            tileset.InnerText = this.TextureName;

            var mapsize = doc.CreateElement("MapSize");

            var mapsizex = doc.CreateElement("X");
            mapsizex.InnerText = this.MapSize.X.ToString();
            var mapsizey = doc.CreateElement("Y");
            mapsizey.InnerText = this.MapSize.Y.ToString();

            var tilepixelsize = doc.CreateElement("TilePixelSize");
            tilepixelsize.InnerText = this.TileSize.X.ToString();

            // Under Layer
            var underlayer = doc.CreateElement("UnderLayer");

            var underlayerbuilder = new StringBuilder();
            for (int i = 0; i < this.UnderLayer.Length; i++)
            {
                underlayerbuilder.Append(this.UnderLayer[i]);
                underlayerbuilder.Append(" ");
            }

            underlayer.InnerText = underlayerbuilder.ToString();

            // Base Layer
            var baselayer = doc.CreateElement("BaseLayer");

            var baselayerbuilder = new StringBuilder();
            for (int i = 0; i < this.BaseLayer.Length; i++)
            {
                baselayerbuilder.Append(this.BaseLayer[i]);
                baselayerbuilder.Append(" ");
            }

            baselayer.InnerText = baselayerbuilder.ToString();

            // Middle Layer
            var middlelayer = doc.CreateElement("MiddleLayer");

            var middlelayerbuilder = new StringBuilder();
            for (int i = 0; i < this.MiddleLayer.Length; i++)
            {
                middlelayerbuilder.Append(this.MiddleLayer[i]);
                middlelayerbuilder.Append(" ");
            }

            middlelayer.InnerText = middlelayerbuilder.ToString();

            // Top Layer
            var toplayer = doc.CreateElement("TopLayer");

            var toplayerbuilder = new StringBuilder();
            for (int i = 0; i < this.TopLayer.Length; i++)
            {
                toplayerbuilder.Append(this.TopLayer[i]);
                toplayerbuilder.Append(" ");
            }

            toplayer.InnerText = toplayerbuilder.ToString();

            // Collision Layer
            var collisionLayer = doc.CreateElement("CollisionLayer");

            var collisionlayerbuilder = new StringBuilder();
            for (int i = 0; i < this.CollisionLayer.Length; i++)
            {
                collisionlayerbuilder.Append(this.CollisionLayer[i]);
                collisionlayerbuilder.Append(" ");
            }

            collisionLayer.InnerText = collisionlayerbuilder.ToString();

            // Events
            var eventLayer = doc.CreateElement("Events");
            foreach (BaseMapEvent e in this.EventList)
            {
				eventLayer.AppendChild(TileEngine.EventManager.EventToXmlNode(e, doc));
            }

            // Animations
            var animations = doc.CreateElement("AnimatedTiles");

            foreach (var FrameAnimation in this.AnimatedTiles.Values)
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
			doc.Save(location);
        }
	}


}


