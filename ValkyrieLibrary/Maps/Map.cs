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

        public int[] BaseLayer { get; set; }
        public int[] MiddleLayer { get; set; }
        public int[] TopLayer { get; set; }
        public int[] CollisionLayer { get; set; }

        public List<Event> EventList;
        public MapPoint MapSize { get; set; }
        public ScreenPoint TileSize { get; set; }
        public String TextureName { get; set; }
        public String Name { get; set; }

        public int TilesPerRow {get { return ((this.Texture != null) ? (this.Texture.Width / this.TileSize.X) : 0); }}
        public int TilesPerCol {get { return ((this.Texture != null) ? (this.Texture.Height / this.TileSize.Y) : 0); }}

        public Dictionary<int, FrameAnimation> AnimatedTiles;

        public enum EMapLayer
        {
            BaseLayer,
            MiddleLayer,
            TopLayer,
            CollisionLayer
        };

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
            this.BaseLayer = new int[0];
            this.MiddleLayer = new int[0];
            this.TopLayer = new int[0];
            this.CollisionLayer = new int[0];
            this.EventList = new List<Event>();
			this.AnimatedTiles = new Dictionary<int, FrameAnimation>();
		}

		public void Update(GameTime gameTime)
		{
			foreach (FrameAnimation anim in this.AnimatedTiles.Values)
				anim.Update(gameTime);
		}

        public int GetLayerValue(MapPoint point, EMapLayer layer)
		{
			if (point.X < 0 || (point.X > this.MapSize.X) || point.Y < 0 || (point.Y > this.MapSize.Y))
				throw new ArgumentOutOfRangeException();

            switch (layer)
            {
                default:
                case EMapLayer.BaseLayer: 
                    return this.BaseLayer[point.Y * this.MapSize.X + point.X];

                case EMapLayer.MiddleLayer: 
                    return this.MiddleLayer[point.Y * this.MapSize.X + point.X];

                case EMapLayer.TopLayer: 
                    return this.TopLayer[point.Y * this.MapSize.X + point.X];

                case EMapLayer.CollisionLayer: 
                    return this.CollisionLayer[point.Y * this.MapSize.X + point.X];
            }
		}
       
        private void SetLayerValue(MapPoint point, int value, EMapLayer layer)
        {
            if (point.X < 0 || (point.X > this.MapSize.X) || point.Y < 0 || (point.Y > this.MapSize.Y))
                throw new ArgumentOutOfRangeException();

            switch (layer)
            {
                default:
                case EMapLayer.BaseLayer: 
                    this.BaseLayer[point.Y * this.MapSize.X + point.X] = value; break;

                case EMapLayer.MiddleLayer:
                    this.MiddleLayer[point.Y * this.MapSize.X + point.X] = value; break;

                case EMapLayer.TopLayer: 
                    this.TopLayer[point.Y * this.MapSize.X + point.X] = value; break;

                case EMapLayer.CollisionLayer:
                    this.CollisionLayer[point.Y * this.MapSize.X + point.X] = value; break;
            }
        }

		public Rectangle GetLayerSourceRect(MapPoint point, EMapLayer layer)
		{
			if (point.X < 0 || point.X > this.MapSize.X ||
				point.Y < 0 || point.Y >= this.MapSize.Y)
			{
				return Rectangle.Empty;
			}

			int LayerValue = this.GetLayerValue(point, layer);
            if (LayerValue < 0)
				return Rectangle.Empty;

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

						foreach(XmlNode subnode in node.ChildNodes)
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
                    TileEngine.EventManager.LoadEvents(this, innerNodes[i]);
                }
			}

			
			
		}

        public void SetData(EMapLayer layer, MapPoint location, int value)
        {
            if (layer == EMapLayer.CollisionLayer)
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
            Rectangle mapSize = TileEngine.CurWorld[TileEngine.CurrentMapChunk.Name].MapLocation.ToRect(TileEngine.CurrentMapChunk.MapSize.ToPoint());
            return (mapSize.Contains(point.ToPoint()) == true);
		}

        public bool IsVisableToPlayer()
        {
            Rectangle mapSize = TileEngine.CurWorld[TileEngine.CurrentMapChunk.Name].MapLocation.ToRect(TileEngine.CurrentMapChunk.MapSize.ToPoint());
            Rectangle worldSize = new Rectangle(0,0, TileEngine.Viewport.Width, TileEngine.Viewport.Height);
  
            return (mapSize.Intersects(worldSize) == true);
        }
	}


}


