﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using Valkyrie.Library.Core;
using Valkyrie.Engine.Events;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Animation;

namespace Valkyrie.Engine.Maps
{
	public class Map
	{
		#region Public Properties

		public int[] UnderLayer
		{
			get { return this.underlayer; }
			set { this.underlayer = value; }
		}

		public int[] BaseLayer
		{
			get { return this.baselayer; }
			set { this.baselayer = value; }
		}

		public int[] MiddleLayer
		{
			get { return this.middlelayer; }
			set { this.middlelayer = value; }
		}

		public int[] TopLayer
		{
			get { return this.toplayer; }
			set { this.toplayer = value; }
		}

		public int[] CollisionLayer
		{
			get { return this.collisionlayer; }
			set { this.collisionlayer = value; }
		}

		public int[] OpaqueLayer
		{
			get { return this.opaquelayer; }
			set { this.opaquelayer = value; }
		}

		public MapPoint MapSize
		{
			get { return this.mapsize; }
			set { this.mapsize = value; }
		}

		public int TileSize
		{
			get { return this.tilesize; }
			set { this.tilesize = value; }
		}

		public String TextureName
		{
			get { return this.texturename; }
			set { this.texturename = value; }
		}

		public String Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

        public int TilesPerRow {get { return ((this.Texture != null) ? (this.Texture.Width / this.TileSize) : 0); }}
        //public int TilesPerCol {get { return ((this.Texture != null) ? (this.Texture.Height / this.TileSize) : 0); }}

		public Dictionary<int, FrameAnimation> AnimatedTiles
		{
			get { return this.animatedtiles; }
		}

        public Texture2D Texture
        {
            get { return this.texture; }
            set { this.texture = value; }
        }

        #endregion

		public void Update(GameTime gameTime)
		{
			foreach (FrameAnimation anim in this.AnimatedTiles.Values)
				anim.Update(gameTime);
		}

		public int GetLayerValue(MapPoint point, MapLayers layer)
		{
			return this.GetLayerValue (point.IntX, point.IntY, layer);
		}

        public int GetLayerValue(int x, int y, MapLayers layer)
		{
			if (x < 0 || (x >= this.MapSize.X) || y < 0 || (y >= this.MapSize.Y))
                return -2; // throw new ArgumentOutOfRangeException("point");

            switch (layer)
            {
                default:
				case MapLayers.UnderLayer:
					return this.UnderLayer[(int)(y * this.MapSize.X + x)];

                case MapLayers.BaseLayer:
                    return this.BaseLayer[(int)(y * this.MapSize.X + x)];

                case MapLayers.MiddleLayer:
                    return this.MiddleLayer[(int)(y * this.MapSize.X + x)];

                case MapLayers.TopLayer: 
                    return this.TopLayer[(int)(y * this.MapSize.X + x)];

                case MapLayers.CollisionLayer: 
                    return this.CollisionLayer[(int)(y * this.MapSize.X + x)];

				case MapLayers.OpaqueLayer:
					return this.OpaqueLayer[(int)(y * this.MapSize.X + x)];
            }
		}

		public void SetLayerValue(MapPoint point, MapLayers layer, int value)
		{
			this.SetLayerValue(point.IntX, point.IntY, layer, value);
		}

        public void SetLayerValue(int x, int y,  MapLayers layer, int value)
        {
            if (x < 0 || (x > this.MapSize.X) || y < 0 || (y > this.MapSize.Y))
                throw new ArgumentOutOfRangeException("point");

            switch (layer)
            {
                default:
				case MapLayers.UnderLayer:
					this.UnderLayer[(int)(y * this.MapSize.X + x)] = value; break;

                case MapLayers.BaseLayer: 
                    this.BaseLayer[(int)(y * this.MapSize.X + x)] = value; break;

                case MapLayers.MiddleLayer:
                    this.MiddleLayer[(int)(y * this.MapSize.X + x)] = value; break;

                case MapLayers.TopLayer: 
                    this.TopLayer[(int)(y * this.MapSize.X + x)] = value; break;

                case MapLayers.CollisionLayer:
                    this.CollisionLayer[(int)(y * this.MapSize.X + x)] = value; break;

				case MapLayers.OpaqueLayer:
					this.OpaqueLayer[(int)(y * this.MapSize.X + x)] = value; break;
            }
        }

		public int GetTileSetValue (MapPoint point)
		{
			return ((int)(point.Y * this.TilesPerRow + point.X));
		}

		public Rectangle GetLayerSourceRect(MapPoint point, MapLayers layer)
		{
			if (point.X < 0 || point.X > this.MapSize.X || point.Y < 0 || point.Y >= this.MapSize.Y)
				throw new ArgumentOutOfRangeException("point");

			int LayerValue = this.GetLayerValue(point, layer);
            if (LayerValue < 0)
				return Rectangle.Empty;

			if (this.AnimatedTiles.ContainsKey(LayerValue))
				return this.AnimatedTiles[LayerValue].FrameRectangle;
			else
				return new Rectangle(
					(LayerValue % this.TilesPerRow) * TileSize,
					(LayerValue / this.TilesPerRow) * TileSize,
					TileSize - 1, TileSize - 1);
        }

		private int[] underlayer = new int[0];
		private int[] baselayer = new int[0];
		private int[] middlelayer = new int[0];
		private int[] toplayer = new int[0];
		private int[] collisionlayer = new int[0];
		private int[] opaquelayer = new int[0];

		private MapPoint mapsize = MapPoint.Zero;
		private int tilesize = 0;
		private String texturename = string.Empty;
		private String name = string.Empty;

		private Dictionary<int, FrameAnimation> animatedtiles = new Dictionary<int, FrameAnimation>();
		private Texture2D texture = null;
	}


}


