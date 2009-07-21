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
        public static Viewport Viewport;
        public static BaseCamera Camera = null;
        public static Dictionary<string, string> Configuration = null;
        public static Player Player = null;

        public static TextureManager TextureManager = null;
        public static ModuleManager ModuleManager = null;
        public static CollisionManager CollisionManager = null;
        public static WorldManager WorldManager = null;
        public static EventManager EventManager = null;

		public static int TileSize = 32;
        public static bool IsMapLoaded { get { return (TileEngine.CurrentMapChunk != null); } }
        public static Dictionary<string,MapHeader> CurWorld {get {return TileEngine.WorldManager.CurrentWorld.WorldList;}}
		
        private static Map currentmapchunk;
        public static Map CurrentMapChunk
        {
            get
            {
                if (TileEngine.Player == null || TileEngine.WorldManager.CurrentWorld == null)
                    return null;

                if (TileEngine.currentmapchunk == null)
                {
                    foreach (var map in TileEngine.WorldManager.CurrentWorld.WorldList.Values)
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

        static TileEngine()
        {
        }

		public static void Initialize(ContentManager content, GraphicsDevice device)
		{
			TileEngine.TextureManager = new TextureManager(content, device, "Graphics");
            TileEngine.Player = new Player();
            TileEngine.ModuleManager = new ModuleManager();
            TileEngine.Configuration = new Dictionary<string, string>();
            TileEngine.WorldManager = new WorldManager();
            TileEngine.EventManager = new EventManager();
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



		public static void ClearCurrentMapChunk()
		{
			TileEngine.currentmapchunk = null;
		}

		public static Point GlobalPixelPointToLocal(MapPoint localpoint)
		{
			int x = localpoint.X - (TileEngine.CurWorld[TileEngine.CurrentMapChunk.Name].MapLocation.X - TileEngine.CurrentMapChunk.TileSize.X);
			int y = localpoint.Y - (TileEngine.CurWorld[TileEngine.CurrentMapChunk.Name].MapLocation.Y - TileEngine.CurrentMapChunk.TileSize.Y);

			return new Point(x, y);
		}

		public static MapPoint GlobalTilePointToLocal(MapPoint localpoint)
		{
            MapPoint temp = localpoint - TileEngine.CurWorld[TileEngine.CurrentMapChunk.Name].MapLocation;
            return temp;

		}

		public static void Update(GameTime time)
		{
			// Run tile engine logic here
            ModuleManager.CurrentModule.Tick(time);

            foreach (var header in TileEngine.CurWorld.Values)
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
            spriteBatch.Begin();

			foreach (var header in TileEngine.CurWorld.Values)
			{
                if (!header.Map.IsVisableToPlayer())
                    continue;

                TileEngine.DrawLayerMap(spriteBatch, header, Map.EMapLayer.BaseLayer);
                TileEngine.DrawLayerMap(spriteBatch, header, Map.EMapLayer.MiddleLayer);

                if (!drawcharacters)
                    TileEngine.DrawLayerMap(spriteBatch, header, Map.EMapLayer.TopLayer);
            }

			if (drawcharacters)
            {
				TileEngine.DrawCharacters(spriteBatch);

			    foreach (var header in TileEngine.CurWorld.Values)
			    {
                    if (!header.Map.IsVisableToPlayer())
                        continue;

                    TileEngine.DrawLayerMap(spriteBatch, header, Map.EMapLayer.TopLayer);
                }
            }

            spriteBatch.End();
		}

        public static void DrawLayerMap(SpriteBatch spriteBatch, MapHeader header, Map.EMapLayer layer)
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

                    Rectangle sourceRectangle = currentMap.GetLayerSourceRect(new MapPoint(x, y), layer);

					if( !sourceRectangle.IsEmpty )
                        spriteBatch.Draw(currentMap.Texture, des, sourceRectangle, Color.White);
				}
			}
		}

        public static void DrawCharacters(SpriteBatch spriteBatch)
        {
			TileEngine.Player.Draw(spriteBatch);
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

