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

namespace valkyrie.Core
{
	public class Map
	{
		public Map()
		{
            this.BaseLayer = new int[0];
            this.MiddleLayer = new int[0];
            this.TopLayer = new int[0];
            this.CollisionLayer = new int[0];

			this.AnimatedTiles = new Dictionary<int, FrameAnimation>();
		}

		public void Update(GameTime gameTime)
		{
			foreach (FrameAnimation anim in this.AnimatedTiles.Values)
				anim.Update(gameTime);
		}

		#region Public Properties

		public string Name { get; set; }

		public Texture2D Texture { get; set; }
		public string TextureName { get; set; }

		public Point MapSize { get; set; }
        //public Point TileSetSize { get; set; }
		public Point TileSize { get; set; }

		public int[] BaseLayer { get; set; }
		public int[] MiddleLayer { get; set; }
        public int[] TopLayer { get; set; }
		public int[] CollisionLayer { get; set; }

		public int TilesPerRow { get; set; }

		public Dictionary<int, FrameAnimation> AnimatedTiles;

		#endregion

        #region Top Layer
        public int GetTopLayerValue(Point MapPoint)
		{
			if (MapPoint.X < 0 || (MapPoint.X > this.MapSize.X) || MapPoint.Y < 0 || (MapPoint.Y > this.MapSize.Y))
				throw new ArgumentOutOfRangeException();

			return this.TopLayer[MapPoint.Y * this.MapSize.X + MapPoint.X];
		}
       
        private void SetTopLayerValue(Point MapPoint, int value)
        {
            if (MapPoint.X < 0 || (MapPoint.X > this.MapSize.X) || MapPoint.Y < 0 || (MapPoint.Y > this.MapSize.Y))
                throw new ArgumentOutOfRangeException();

            this.TopLayer[MapPoint.Y * this.MapSize.X + MapPoint.X] = value;
        }

		public Rectangle GetTopLayerSourceRect(Point MapPoint)
		{
			if (MapPoint.X < 0 || MapPoint.X > this.MapSize.X ||
				MapPoint.Y < 0 || MapPoint.Y >= this.MapSize.Y)
			{
				return Rectangle.Empty;
			}

			int TopLayerValue = this.GetTopLayerValue(MapPoint);

			if (TopLayerValue < 0)
				return Rectangle.Empty;

			return new Rectangle(
				(TopLayerValue % this.TilesPerRow) * TileSize.X,
				(TopLayerValue / this.TilesPerRow) * TileSize.Y,
				TileSize.X - 1, TileSize.Y - 1);
        }
        #endregion

        #region Middle Layer
        public int GetMiddleLayerValue(Point MapPoint)
		{
			if (MapPoint.X < 0 || (MapPoint.X > this.MapSize.X) || MapPoint.Y < 0 || (MapPoint.Y > this.MapSize.Y))
				throw new ArgumentOutOfRangeException();

			return this.MiddleLayer[MapPoint.Y * this.MapSize.X + MapPoint.X];
		}

        private void SetMiddleLayerValue(Point MapPoint, int value)
        {
            if (MapPoint.X < 0 || (MapPoint.X > this.MapSize.X) || MapPoint.Y < 0 || (MapPoint.Y > this.MapSize.Y))
                throw new ArgumentOutOfRangeException();

            this.MiddleLayer[MapPoint.Y * this.MapSize.X + MapPoint.X] = value;
        }

		public Rectangle GetMiddleLayerSourceRect(Point MapPoint)
		{
			if (MapPoint.X < 0 || MapPoint.X > this.MapSize.X ||
				MapPoint.Y < 0 || MapPoint.Y >= this.MapSize.Y)
			{
				return Rectangle.Empty;
			}

			int MiddleLayerValue = this.GetMiddleLayerValue(MapPoint);

			if (MiddleLayerValue < 0)
				return Rectangle.Empty;

			if (this.AnimatedTiles.ContainsKey(MiddleLayerValue))
				return this.AnimatedTiles[MiddleLayerValue].FrameRectangle;
			else
				return new Rectangle(
					(MiddleLayerValue % this.TilesPerRow) * TileSize.X,
					(MiddleLayerValue / this.TilesPerRow) * TileSize.Y,
					TileSize.X - 1, TileSize.Y - 1);
        }
        #endregion

        #region BaseLayer
        public int GetBaseLayerValue(Point MapPoint)
		{
			if (MapPoint.X < 0 || (MapPoint.X > this.MapSize.X ) || MapPoint.Y < 0 || (MapPoint.Y > this.MapSize.Y) )
				throw new ArgumentOutOfRangeException();

			return this.BaseLayer[MapPoint.Y * this.MapSize.X + MapPoint.X];
		}

        private void SetBaseLayerValue(Point MapPoint, int value)
		{
			if (MapPoint.X < 0 || (MapPoint.X > this.MapSize.X ) || MapPoint.Y < 0 || (MapPoint.Y > this.MapSize.Y) )
				throw new ArgumentOutOfRangeException();

			this.BaseLayer[MapPoint.Y * this.MapSize.X + MapPoint.X] = value;
		}

		public Rectangle GetBaseLayerSourceRect(Point MapPoint)
		{
			if (MapPoint.X < 0 || MapPoint.X > this.MapSize.X ||
				MapPoint.Y < 0 || MapPoint.Y >= this.MapSize.Y)
			{
				return Rectangle.Empty;
			}

			int baseLayerValue = this.GetBaseLayerValue(MapPoint);

			if (baseLayerValue < 0)
				return Rectangle.Empty;

			if (this.AnimatedTiles.ContainsKey(baseLayerValue))
				return this.AnimatedTiles[baseLayerValue].FrameRectangle;
			else
				return new Rectangle(
					(baseLayerValue % this.TilesPerRow) * TileSize.X,
					(baseLayerValue / this.TilesPerRow) * TileSize.Y,
					TileSize.X - 1, TileSize.Y - 1); // -1 so it doesn't over extend into the next tile
		}
        #endregion

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
					int[] size = Array.ConvertAll<string, int>(innerNodes[i].InnerText.Split(' '), new Converter<string, int>(this.ConvertStringToInt));

					this.MapSize = new Point(size[0], size[1]);
				}
				else if (innerNodes[i].Name == "TilePixelSize")
				{
					int size = Convert.ToInt32(innerNodes[i].InnerText);
					this.TileSize = new Point(size, size);
				}
                else if (innerNodes[i].Name == "TileSetSize")
                {
                    var size = Array.ConvertAll<string, int>(innerNodes[i].InnerText.Split(' '), new Converter<string, int>(this.ConvertStringToInt));
                    this.TilesPerRow = size[0];

                    //this.TileSetSize = new Point(size[0], size[1]);
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

			}

			
			
		}
		
		public void SaveMap(FileInfo MapFile)
		{
		}

        public void SetData(MapLayer layer, Point location, int value)
        {
            switch (layer)
            {
                case MapLayer.BaseLayer:
                    SetBaseLayerValue(location, value);                    
                    break;
                case MapLayer.MiddleLayer:
                    SetMiddleLayerValue(location, value);
                    break;
                case MapLayer.TopLayer:
                    SetTopLayerValue(location, value);
                    break;
                default:
                    break;
            }
        }
        #endregion

		private int ConvertStringToInt(string value)
		{
			return Convert.ToInt32(value);
		}
	}

	public enum MapLayer
	{
		BaseLayer,
		MiddleLayer,
		TopLayer,
		CollisionLayer
	}
}


