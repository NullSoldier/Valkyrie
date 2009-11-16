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
using Valkyrie.Library.Animation;
using Valkyrie.Library.Core;
using Valkyrie.Library.Maps;
using Valkyrie.Library.Events;

namespace Valkyrie.Library.Maps
{
	public class Map
	{
        #region Public Properties

		public int[] UnderLayer { get; set; }
        public int[] BaseLayer { get; set; }
        public int[] MiddleLayer { get; set; }
        public int[] TopLayer { get; set; }
        public int[] CollisionLayer { get; set; }

        public List<IMapEvent> EventList;
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
            this.EventList = new List<IMapEvent>();
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
	}


}


