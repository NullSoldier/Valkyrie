using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Collision;
using ValkyrieLibrary.Core;
using ValkyrieLibrary.Events;
using ValkyrieLibrary.Maps;

namespace ValkyrieLibrary
{
	public static class TileEngine
	{
        public static Viewport Viewport;
        public static BaseCamera Camera;
        public static Dictionary<string, string> Configuration;
        public static BaseCharacter Player;

        public static TextureManager TextureManager;
        public static ModuleManager ModuleManager;
        public static CollisionManager CollisionManager;
        public static WorldManager WorldManager;
        public static MapEventManager EventManager;

		public static int TileSize = 32;
        public static bool IsMapLoaded{ get { return (TileEngine.CurrentMapChunk != null); } }
		
        private static Map currentmapchunk;
        public static Map CurrentMapChunk
        {
            get
            {
                if (TileEngine.Player == null || TileEngine.WorldManager.CurrentWorld == null)
                    return null;

                if (TileEngine.currentmapchunk == null)
                {
                    foreach (var map in TileEngine.WorldManager.CurrentWorld.MapList.Values)
                    {
                        MapPoint playerLoc = TileEngine.Player.Location.ToMapPoint();
                        Rectangle mapSize = map.MapLocation.ToRect(map.Map.MapSize.ToPoint());

                        if (mapSize.Contains (playerLoc.ToPoint()))
                        {
                            TileEngine.currentmapchunk = map.Map;
                            break;
                        }

                    }
                }

                return TileEngine.currentmapchunk;
            }
        }

		public static void Initialize(ContentManager content, GraphicsDevice device)
		{
			Check.NullArgument (content, "content");
			Check.NullArgument (device, "device");

			TileEngine.TextureManager = new TextureManager(content, device, "Graphics");
			TileEngine.Player = null; // Cannot assign to abstract class, removed player
            TileEngine.ModuleManager = new ModuleManager();
            TileEngine.Configuration = new Dictionary<string, string>();
            TileEngine.WorldManager = new WorldManager();
            TileEngine.EventManager = new MapEventManager();
		}

        public static void Load (FileInfo Configuration)
        {
        	Check.NullArgument (Configuration, "Configuration");

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

		public static void ClearCurrentMapChunk()
		{
			TileEngine.currentmapchunk = null;
		}

		public static Point GlobalPixelPointToLocal(MapPoint localpoint)
		{
			Check.NullArgument (localpoint, "localpoint");

            int x = localpoint.X - (TileEngine.WorldManager.CurrentWorld.MapList[TileEngine.CurrentMapChunk.Name].MapLocation.X - TileEngine.CurrentMapChunk.TileSize.X);
            int y = localpoint.Y - (TileEngine.WorldManager.CurrentWorld.MapList[TileEngine.CurrentMapChunk.Name].MapLocation.Y - TileEngine.CurrentMapChunk.TileSize.Y);

			return new Point(x, y);
		}

		public static MapPoint GlobalTilePointToLocal(MapPoint localpoint)
		{
			Check.NullArgument (localpoint, "localpoint");

            return localpoint - TileEngine.WorldManager.CurrentWorld.MapList[TileEngine.CurrentMapChunk.Name].MapLocation;
		}

		public static void Update(GameTime time)
		{
			Check.NullArgument (time, "time");

			// Run tile engine logic here
            ModuleManager.CurrentModule.Tick(time);

            if (TileEngine.WorldManager.CurrentWorld == null)
                return;

            foreach (var header in TileEngine.WorldManager.CurrentWorld.MapList.Values)
            {
                if (header.Map.IsVisableToPlayer())
                    header.Map.Update(time);
            }
		}

		#region Draw Methods
        public static void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
        	Check.NullArgument (spriteBatch, "spriteBatch");
        	Check.NullArgument (gameTime, "gameTime");

            TileEngine.ModuleManager.CurrentModule.Draw(spriteBatch, gameTime);
		}

        public static void DrawLayerMap(SpriteBatch spriteBatch, MapLayers layer)
        {
        	Check.NullArgument (spriteBatch, "spriteBatch");

            DrawLayerMap(spriteBatch, layer, Color.White);
        }

        public static void DrawLayerMap(SpriteBatch spriteBatch, MapLayers layer, Color tint)
        {
        	Check.NullArgument (spriteBatch, "spriteBatch");

            foreach (var header in TileEngine.WorldManager.CurrentWorld.MapList.Values)
            {
                if (!header.Map.IsVisableToPlayer())
                    continue;

                TileEngine.DrawLayerMap(spriteBatch, header, layer);
            }
        }

        public static void DrawEverything(SpriteBatch spriteBatch)
		{
        	Check.NullArgument (spriteBatch, "spriteBatch");

            if (TileEngine.WorldManager.CurrentWorld == null)
                return;

            foreach (var header in TileEngine.WorldManager.CurrentWorld.MapList.Values)
			{
				TileEngine.DrawLayerMap(spriteBatch, header, MapLayers.UnderLayer);
                TileEngine.DrawLayerMap(spriteBatch, header, MapLayers.BaseLayer);
                TileEngine.DrawLayerMap(spriteBatch, header, MapLayers.MiddleLayer);
                TileEngine.DrawLayerMap(spriteBatch, header, MapLayers.TopLayer);
            }
		}
		public static void DrawAllLayers(SpriteBatch spriteBatch, bool drawCharacters)
		{
			Check.NullArgument (spriteBatch, "spriteBatch");

            if (TileEngine.WorldManager.CurrentWorld == null)
                return;

            foreach (var header in TileEngine.WorldManager.CurrentWorld.MapList.Values)
			{
                if (!header.Map.IsVisableToPlayer())
                    continue;

				TileEngine.DrawLayerMap(spriteBatch, header, MapLayers.UnderLayer);
                TileEngine.DrawLayerMap(spriteBatch, header, MapLayers.BaseLayer);
                TileEngine.DrawLayerMap(spriteBatch, header, MapLayers.MiddleLayer);

                if (!drawCharacters)
                    TileEngine.DrawLayerMap(spriteBatch, header, MapLayers.TopLayer);
            }

			if (drawCharacters)
            {
				TileEngine.DrawCharacters(spriteBatch);

                foreach (var header in TileEngine.WorldManager.CurrentWorld.MapList.Values)
			    {
                    if (!header.Map.IsVisableToPlayer())
                        continue;

                    TileEngine.DrawLayerMap(spriteBatch, header, MapLayers.TopLayer);
                }
            }
		}

        public static void DrawLayerMap(SpriteBatch spriteBatch, MapHeader header, MapLayers layer)
        {
        	Check.NullArgument (spriteBatch, "spriteBatch");

            DrawLayerMap(spriteBatch, header, layer, Color.White);
        }

        public static void DrawLayerMap(SpriteBatch spriteBatch, MapHeader header, MapLayers layer, Color tint)
		{
        	Check.NullArgument (spriteBatch, "spriteBatch");
        	Check.NullArgument (header, "header");

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

                    Rectangle sourceRectangle = currentMap.GetLayerSourceRect(new MapPoint(x, y), layer);

                    if (sourceRectangle.IsEmpty)
                        continue;

                    spriteBatch.Draw(currentMap.Texture, des, sourceRectangle, Color.White);
				}
			}
		}


        //this is used to draw a map at 0,0 incase we use a custom rendertarget
        public static void DrawMapLocal(SpriteBatch spriteBatch, MapHeader header)
        {
			Check.NullArgument (spriteBatch, "spriteBatch");
			Check.NullArgument (header, "header");

            TileEngine.DrawLayerMapLocal(spriteBatch, header, MapLayers.UnderLayer);
            TileEngine.DrawLayerMapLocal(spriteBatch, header, MapLayers.BaseLayer);
            TileEngine.DrawLayerMapLocal(spriteBatch, header, MapLayers.MiddleLayer);
            TileEngine.DrawLayerMapLocal(spriteBatch, header, MapLayers.TopLayer);  
        }

        public static void DrawLayerMapLocal(SpriteBatch spriteBatch, MapHeader header, MapLayers layer)
        {
            Check.NullArgument (spriteBatch, "spriteBatch");
			Check.NullArgument (header, "header");

            Map currentMap = header.Map;
            ScreenPoint camOffset = TileEngine.Camera.Offset();
            ScreenPoint tileSize = currentMap.TileSize;

            for (int y = 0; y < currentMap.MapSize.Y; y++)
            {
                for (int x = 0; x < currentMap.MapSize.X; x++)
                {
                    ScreenPoint pos = new MapPoint(x, y).ToScreenPoint();
                    Rectangle des = pos.ToRect(tileSize.ToPoint());

                    Rectangle sourceRectangle = currentMap.GetLayerSourceRect(new MapPoint(x, y), layer);

                    if (sourceRectangle.IsEmpty)
                        continue;

                    spriteBatch.Draw(currentMap.Texture, des, sourceRectangle, Color.White);
                }
            }         
        }

        public static void DrawCharacters(SpriteBatch spriteBatch)
        {
			Check.NullArgument (spriteBatch, "spriteBatch");

			TileEngine.Player.Draw(spriteBatch);
        }

        public static void DrawOverlay(SpriteBatch spriteBatch)
        {
			Check.NullArgument (spriteBatch, "spriteBatch");

            TileEngine.Player.DrawOverlay(spriteBatch);
        }

		#endregion
	}
}