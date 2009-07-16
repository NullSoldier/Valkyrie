using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using PrimitivesSample;
using ValkyrieLibrary.Player;
using ValkyrieLibrary;
using System.Diagnostics;
using System.IO;
using System.Xml;
using ValkyrieLibrary.Collision;

namespace valkyrie.Core
{
	public static class TileEngine
	{
		private static Map map;

		private static Viewport viewport;
		//private static PrimitiveBatch PrimitiveBatch;
        public static TextureManager TextureManager;
        public static ModuleManager ModuleManager;
        public static Camera Camera;
        public static Dictionary<string, string> Configuration;
		public static CollisionManager CollisionManager;

        public static Player Player;
        
		public static Map Map
		{
			get { return TileEngine.map; }
			set { TileEngine.map = value; }
		}

		public static bool IsMapLoaded
		{
			get { return (TileEngine.Map != null); }
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
			//TileEngine.PrimitiveBatch = new PrimitiveBatch(device);
            TileEngine.Player = new Player();
            TileEngine.ModuleManager = new ModuleManager();
            TileEngine.Configuration = new Dictionary<string, string>();
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

		public static void SetMap(Map newMap)
		{
			if (newMap == null)
				throw new ArgumentNullException();

			TileEngine.map = newMap;

			if (TileEngine.map.Texture == null)
			{
				if (!TileEngine.TextureManager.ContainsTexture(TileEngine.map.TextureName))
					TileEngine.TextureManager.AddTexture(TileEngine.map.TextureName);

				TileEngine.map.Texture = TileEngine.TextureManager.GetTexture(TileEngine.map.TextureName);
			}
		}

		public static void Update(GameTime time)
		{
			// Run tile engine logic here
            ModuleManager.CurrentModule.Tick(time);
		}

        public static void DrawScreen(SpriteBatch spriteBatch, GameTime gameTime)
		{
            TileEngine.ModuleManager.CurrentModule.Draw(spriteBatch, gameTime);
		}

		public static void DrawBaseLayer(SpriteBatch spriteBatch)
		{
			if (spriteBatch == null)
				throw new ArgumentNullException();

			for (int y = 0; y < TileEngine.map.MapSize.Y; y++)
			{
				for (int x = 0; x < TileEngine.map.MapSize.X; x++)
				{
					Rectangle destRectangle = new Rectangle(0, 0, map.TileSize.X, map.TileSize.Y);
					destRectangle.X = (int)TileEngine.Camera.MapOffset.X + (int)TileEngine.Camera.CameraOffset.X + (x * TileEngine.map.TileSize.X);
					destRectangle.Y = (int)TileEngine.Camera.MapOffset.Y + (int)TileEngine.Camera.CameraOffset.Y + (y * TileEngine.map.TileSize.Y);

					if( TileEngine.Camera.CheckVisible(destRectangle) )
					{

						Point MapLoc = new Point(x, y);
						Rectangle sourceRectangle = Map.GetBaseLayerSourceRect(MapLoc);
						if( !sourceRectangle.IsEmpty )
						{
							spriteBatch.Begin();
							spriteBatch.Draw(map.Texture, destRectangle, sourceRectangle, Color.White);
							spriteBatch.End();
						}
					}
				}
			}
		}

		public static void DrawMiddleLayer(SpriteBatch spriteBatch)
		{
			if (spriteBatch == null)
				throw new ArgumentNullException();


			for (int y = 0; y < TileEngine.map.MapSize.Y; y++)
			{
				for (int x = 0; x < TileEngine.map.MapSize.X; x++)
				{
					Rectangle destRectangle = new Rectangle(0, 0, map.TileSize.X, map.TileSize.Y);
					destRectangle.X = (int)TileEngine.Camera.MapOffset.X + x * TileEngine.map.TileSize.X;
					destRectangle.Y = (int)TileEngine.Camera.MapOffset.Y + y * TileEngine.map.TileSize.Y;

					if (TileEngine.Camera.CheckVisible(destRectangle))
					{

						Point MapLoc = new Point(x, y);
						Rectangle sourceRectangle = Map.GetMiddleLayerSourceRect(MapLoc);
						if (!sourceRectangle.IsEmpty)
						{
							spriteBatch.Begin();
							spriteBatch.Draw(map.Texture, destRectangle, sourceRectangle, Color.White);
							spriteBatch.End();
						}
					}
				}
			}
		}

		public static void DrawTopLayer(SpriteBatch spriteBatch)
		{
			if (spriteBatch == null)
				throw new ArgumentNullException();


			for (int y = 0; y < TileEngine.map.MapSize.Y; y++)
			{
				for (int x = 0; x < TileEngine.map.MapSize.X; x++)
				{
					Rectangle destRectangle = new Rectangle(0, 0, map.TileSize.X, map.TileSize.Y);
					destRectangle.X = (int)TileEngine.Camera.MapOffset.X + x * TileEngine.map.TileSize.X;
					destRectangle.Y = (int)TileEngine.Camera.MapOffset.Y + y * TileEngine.map.TileSize.Y;

					if (TileEngine.Camera.CheckVisible(destRectangle))
					{

						Point MapLoc = new Point(x, y);
						Rectangle sourceRectangle = Map.GetTopLayerSourceRect(MapLoc);
						if (!sourceRectangle.IsEmpty)
						{
							spriteBatch.Begin();
							spriteBatch.Draw(map.Texture, destRectangle, sourceRectangle, Color.White);
							spriteBatch.End();
						}
					}
				}
			}
		}

        public static void DrawCharacters(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

			TileEngine.Player.Draw(spriteBatch);
			TileEngine.Camera.CenterOnPoint( new Vector2(Player.Location.X + (Player.CurrentAnimation.FrameRectangle.Width / 2), Player.Location.Y + (Player.CurrentAnimation.FrameRectangle.Height / 2)) );

            spriteBatch.End();
        }
	}

}

