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
using ValkyrieLibrary.Input;
using Gablarski.Network;

namespace ValkyrieLibrary
{
	public static class TileEngine
	{
        public static Viewport Viewport;
        public static BaseCamera Camera;
		public static BaseCharacter Player
		{
			get { return TileEngine.player; }
			set
			{
				if(TileEngine.player != null)
					TileEngine.player.TileLocationChanged -= TileEngine.PlayerTileLocationChanged;

				TileEngine.player = value;

				if(TileEngine.player != null)
					TileEngine.player.TileLocationChanged += TileEngine.PlayerTileLocationChanged;
			}
		}

		private static BaseCharacter player;

        public static TextureManager TextureManager;
        public static ModuleManager ModuleManager;
        public static CollisionManager CollisionManager;
        public static WorldManager WorldManager;
        public static MapEventManager EventManager;
		public static IMovementManager MovementManager;
		public static GraphicsDevice GraphicsDevice;

		public static NetworkClientConnection NetworkManager;
		public static Dictionary<uint, BaseCharacter> NetworkPlayerCache;
		public static uint NetworkID = 0;
		
		//public static KeybindController KeyManager;

		public static int TileSize = 32;
        public static bool IsMapLoaded{ get { return (TileEngine.CurrentMapChunk != null); } }

		public static TileEngineConfiguration Configuration
		{
			get; set;
		}

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
            TileEngine.Configuration = null;
            TileEngine.WorldManager = new WorldManager();
			TileEngine.NetworkManager = new NetworkClientConnection();
			TileEngine.NetworkPlayerCache = new Dictionary<uint, BaseCharacter>();
			TileEngine.GraphicsDevice = device;
		}

        public static void Load (FileInfo configuration)
        {
        	Check.NullArgument (configuration, "configuration");

            XmlDocument doc = new XmlDocument();
            doc.Load(configuration.FullName);
            
			XmlNodeList nodes = doc.GetElementsByTagName("Config");
        	TileEngine.Configuration =
        		new TileEngineConfiguration(nodes[0].ChildNodes.OfType<XmlNode>().ToDictionary(x => (TileEngineConfigurationName)Enum.Parse (typeof(TileEngineConfigurationName), x.Name), x => x.InnerText));
			
			//foreach (XmlNode node in nodes[0].ChildNodes)
			//{
			//    if (TileEngine.Configuration.ContainsKey(node.Name))
			//        TileEngine.Configuration[node.Name] = node.InnerText;
			//    else
			//        TileEngine.Configuration.Add (node.Name, node.InnerText);
			//}

            TileEngine.TextureManager.TextureRoot = TileEngine.Configuration[TileEngineConfigurationName.GraphicsRoot];

            if( TileEngine.Configuration.ContainsKey(TileEngineConfigurationName.DefaultModule) )
                TileEngine.ModuleManager.PushModuleToScreen(TileEngine.Configuration[TileEngineConfigurationName.DefaultModule]);
        }

		public static void Unload()
		{
			foreach (IModule module in TileEngine.ModuleManager.Modules.Values)
			{
				module.Unload();
			}

			TileEngine.NetworkManager.Disconnect();
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

		private static void PlayerTileLocationChanged(object sender, EventArgs e)
		{
			TileEngine.ClearCurrentMapChunk();
		}

		public static void Update(GameTime time)
		{
			Check.NullArgument (time, "time");

			// Run tile engine logic here
            ModuleManager.CurrentModule.Update(time);
			TileEngine.Camera.Update(time);

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

			foreach (BaseCharacter player in TileEngine.NetworkPlayerCache.Values)
				player.Draw(spriteBatch);
        }

        public static void DrawOverlay(SpriteBatch spriteBatch)
        {
			Check.NullArgument (spriteBatch, "spriteBatch");

            TileEngine.Player.DrawOverlay(spriteBatch);
			
        }

		#endregion
	}
}