using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ValkyrieLibrary;
using System.Diagnostics;
using System.IO;
using System.Xml;
using ValkyrieLibrary.Collision;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Maps;
using ValkyrieLibrary.Events;

namespace ValkyrieLibrary.Core
{
	public static class TileEngine
	{
		public static Map CurrentMapChunk
		{
			get
			{
                if (TileEngine.Player == null)
                    return null;

				if( TileEngine.currentmapchunk == null)
				{
					foreach (var map in TileEngine.World.Values)
					{
                        MapPoint playerLoc = TileEngine.Player.Location.ToMapPoint();
                        Rectangle mapSize = map.MapLocation.ToRect(map.Map.MapSize.ToPoint());

						if (mapSize.Contains(playerLoc.ToPoint()) == true)
                        {
							TileEngine.currentmapchunk = map.Map;
							break;
						}
						
					}
				}

				return TileEngine.currentmapchunk;
			}			
		}

		private static Map currentmapchunk;
		private static Viewport viewport;

        public static TextureManager TextureManager;
        public static ModuleManager ModuleManager;
        public static BaseCamera Camera;
        public static Dictionary<string, string> Configuration;
		public static CollisionManager CollisionManager;
		public static Dictionary<string, MapHeader> World;
        public static Player Player = null;
        public static EventManager EventSystem;
		public static int TileSize = 32;
        
		/*public static Map Map
		{
			get { return TileEngine.CurrentMapChunk; }
			set { TileEngine.SetMap(value); }
		}*/

		public static bool IsMapLoaded
		{
			get { return (TileEngine.CurrentMapChunk != null); }
		}

		public static Viewport Viewport
		{
			get { return TileEngine.viewport; }
			set { TileEngine.viewport = value; }
		}

		static TileEngine()
		{
		}

		public static void Initialize(ContentManager content, GraphicsDevice device)
		{
			TileEngine.TextureManager = new TextureManager(content, device, "Graphics");
            TileEngine.Player = new Player();
            TileEngine.ModuleManager = new ModuleManager();
            TileEngine.Configuration = new Dictionary<string, string>();
			TileEngine.World = new Dictionary<string, MapHeader>();
            TileEngine.EventSystem = new EventManager();
		}

        public static void Load(FileInfo Configuration)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Configuration.FullName);
            
            XmlNodeList nodes = doc.GetElementsByTagName("Config");
            foreach (XmlNode node in nodes[0].ChildNodes)
            {
                if (TileEngine.Configuration.ContainsKey(node.Name))
                    TileEngine.Configuration[node.Name] = node.InnerText;
                else
                    TileEngine.Configuration.Add(node.Name, node.InnerText);
            }

            TileEngine.TextureManager.TextureRoot = TileEngine.Configuration["GraphicsRoot"];

            if( TileEngine.Configuration.ContainsKey("DefaultModule") )
                TileEngine.ModuleManager.PushModuleToScreen(TileEngine.Configuration["DefaultModule"]);
        }

		public static void LoadWorld(FileInfo WorldConfiguration)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(WorldConfiguration.FullName);

			XmlNodeList nodes = doc.GetElementsByTagName("World");
			foreach (XmlNode node in nodes[0].ChildNodes)
			{
				string name = string.Empty;
				string path = string.Empty;
				int x = 0, y = 0;

				foreach (XmlNode subnode in node.ChildNodes)
				{
					if (subnode.Name == "Name")
						name = subnode.InnerText;
					else if (subnode.Name == "FilePath")
						path = subnode.InnerText;
					else if (subnode.Name == "X")
						x = Convert.ToInt32(subnode.InnerText);
					else if (subnode.Name == "Y")
						y = Convert.ToInt32(subnode.InnerText);
				}

				MapHeader header = new MapHeader(name, path, new MapPoint(x, y));
				TileEngine.World.Add(header.MapName, header);
			}
		}

		public static void ClearCurrentMapChunk()
		{
			TileEngine.currentmapchunk = null;
		}

		public static Point GlobalPixelPointToLocal(MapPoint localpoint)
		{
			int x = localpoint.X - (TileEngine.World[TileEngine.CurrentMapChunk.Name].MapLocation.X - TileEngine.CurrentMapChunk.TileSize.X);
			int y = localpoint.Y - (TileEngine.World[TileEngine.CurrentMapChunk.Name].MapLocation.Y - TileEngine.CurrentMapChunk.TileSize.Y);

			return new Point(x, y);
		}

		public static MapPoint GlobalTilePointToLocal(MapPoint localpoint)
		{
            MapPoint temp = localpoint - TileEngine.World[TileEngine.CurrentMapChunk.Name].MapLocation;
            return temp;

		}

		public static void Update(GameTime time)
		{
			// Run tile engine logic here
            ModuleManager.CurrentModule.Tick(time);

            foreach (var header in TileEngine.World.Values)
            {
                if (header.Map.IsVisableToPlayer())
                    header.Map.Update(time);
            }
		}

		#region Draw Methods
        public static void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
            TileEngine.ModuleManager.CurrentModule.Draw(spriteBatch, gameTime);
		}

		public static void DrawAllLayers(SpriteBatch spriteBatch, bool drawcharacters)
		{
			bool charsDrawn = false;

			foreach (var header in TileEngine.World.Values)
			{
                if (!header.Map.IsVisableToPlayer())
                    continue;

				TileEngine.DrawBaseLayerMap(spriteBatch, header);
				TileEngine.DrawMiddleLayerMap(spriteBatch, header);

				if (drawcharacters && !charsDrawn)
				{
					TileEngine.DrawCharacters(spriteBatch);
					charsDrawn = true;
				}

				TileEngine.DrawTopLayerMap(spriteBatch, header);
			}
		}

		public static void DrawBaseLayer(SpriteBatch spriteBatch)
		{
            spriteBatch.Begin();

            foreach (var header in TileEngine.World.Values)
            {
                if (header.Map.IsVisableToPlayer())
                    TileEngine.DrawBaseLayerMap(spriteBatch, header);
            }

            spriteBatch.End();
		}

		public static void DrawBaseLayerMap(SpriteBatch spriteBatch, MapHeader header)
		{
			if (spriteBatch == null)
				throw new ArgumentNullException();

			Map currentMap = header.Map;
            ScreenPoint camOffset = TileEngine.Camera.Offset();
            ScreenPoint tileSize = currentMap.TileSize;

			for (int y = 0; y < currentMap.MapSize.Y; y++)
			{
				for (int x = 0; x < currentMap.MapSize.X; x++)
				{
                    ScreenPoint pos = new MapPoint(x, y).ToScreenPoint() + camOffset + header.MapLocation.ToScreenPoint();
                    Rectangle des = pos.ToRect(tileSize.ToPoint());

                    if (!TileEngine.Camera.CheckIsVisible(des))
                        continue;

					Rectangle sourceRectangle = currentMap.GetBaseLayerSourceRect(new MapPoint(x, y));
					if( !sourceRectangle.IsEmpty )
                        spriteBatch.Draw(currentMap.Texture, des, sourceRectangle, Color.White);
				}
			}
		}
		
		public static void DrawMiddleLayer(SpriteBatch spriteBatch)
		{
            spriteBatch.Begin();

            foreach (var header in TileEngine.World.Values)
            {
                if (header.Map.IsVisableToPlayer())
                    TileEngine.DrawMiddleLayerMap(spriteBatch, header);
            }

            spriteBatch.End();
		}

        public static void DrawMiddleLayerMap(SpriteBatch spriteBatch, MapHeader header)
        {
            if (spriteBatch == null)
                throw new ArgumentNullException();

            Map currentMap = header.Map;
            ScreenPoint camOffset = TileEngine.Camera.Offset();
            ScreenPoint tileSize = currentMap.TileSize;

            for (int y = 0; y < currentMap.MapSize.Y; y++)
            {
                for (int x = 0; x < currentMap.MapSize.X; x++)
                {
                    ScreenPoint pos = new MapPoint(x, y).ToScreenPoint() + camOffset + header.MapLocation.ToScreenPoint();
                    Rectangle des = pos.ToRect(tileSize.ToPoint());

                    if (!TileEngine.Camera.CheckIsVisible(des))
                        continue;

                    Rectangle sourceRectangle = currentMap.GetMiddleLayerSourceRect(new MapPoint(x, y));
                    if (!sourceRectangle.IsEmpty)
                        spriteBatch.Draw(currentMap.Texture, des, sourceRectangle, Color.White);
                }
            }
        }

		public static void DrawTopLayer(SpriteBatch spriteBatch)
		{
            spriteBatch.Begin();
            foreach (var header in TileEngine.World.Values)
            {
                if (header.Map.IsVisableToPlayer())
                    DrawTopLayerMap(spriteBatch, header);
            }
            spriteBatch.End();
		}

		public static void DrawTopLayerMap(SpriteBatch spriteBatch, MapHeader header)
		{
			if (spriteBatch == null)
				throw new ArgumentNullException();

			Map currentMap = header.Map;
            ScreenPoint camOffset = TileEngine.Camera.Offset();
            ScreenPoint tileSize = currentMap.TileSize;

			for (int y = 0; y < currentMap.MapSize.Y; y++)
			{
                for (int x = 0; x < currentMap.MapSize.X; x++)
                {
                    ScreenPoint pos = new MapPoint(x, y).ToScreenPoint() + camOffset + header.MapLocation.ToScreenPoint();
                    Rectangle des = pos.ToRect(tileSize.ToPoint());

                    if (!TileEngine.Camera.CheckIsVisible(des))
                        continue;

                    Rectangle sourceRectangle = currentMap.GetTopLayerSourceRect(new MapPoint(x, y));
                    if (!sourceRectangle.IsEmpty)
                        spriteBatch.Draw(currentMap.Texture, des, sourceRectangle, Color.White);
                }
			}
		}

        public static void DrawCharacters(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

			TileEngine.Player.Draw(spriteBatch);

            spriteBatch.End();
        }

        public static void DrawOverlay(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            TileEngine.Player.DrawOverlay(spriteBatch);
            spriteBatch.End();
        }

		#endregion
	}

}

