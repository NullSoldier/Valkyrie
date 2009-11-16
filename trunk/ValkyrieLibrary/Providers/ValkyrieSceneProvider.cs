using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Valkyrie.Engine.Maps;
using Valkyrie.Engine;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine.Core;
using Mono.Rocks;

namespace Valkyrie.Library.Providers
{
	public class ValkyrieSceneProvider
		: ISceneProvider
	{
		#region ISceneProvider Members

		public void Update (GameTime gameTime)
		{
			foreach(BaseCamera camera in this.Cameras.Values)
				camera.Update(gameTime);

			foreach(BaseCharacter player in this.Players.Values)
				player.Update(gameTime);

			foreach(MapHeader header in this.context.WorldManager.Worlds.SelectMany(w => w.Value.Maps.Values).Where( h => h.IsLoaded))
				header.Map.Update(gameTime);
		}

		#region Public Draw Methods

		public void DrawCamera (SpriteBatch spriteBatch, string cameraname)
		{
			this.DrawCamera(spriteBatch, this.Cameras[cameraname]);
		}

		public void DrawCameraLayer (SpriteBatch spriteBatch, string cameraname, MapLayers layer, MapHeader header)
		{
			this.DrawCameraLayer (spriteBatch, this.Cameras[cameraname], layer, header);
		}

		public void DrawAllCameras (SpriteBatch spriteBatch)
		{
			foreach(BaseCamera camera in this.Cameras.Values)
			{
				this.DrawCamera(spriteBatch, camera);
			}
		}

		public void DrawPlayer (SpriteBatch spriteBatch, string playername)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Private Draw Methods

		public void DrawCamera (SpriteBatch spriteBatch, BaseCamera camera)
		{
			foreach(var header in this.context.WorldManager.Worlds[camera.WorldName].Maps.Values)
			{
				if(!header.IsVisible(camera))
					continue;

				this.DrawCameraLayer(spriteBatch, camera, MapLayers.UnderLayer, header);
				this.DrawCameraLayer(spriteBatch, camera, MapLayers.BaseLayer, header);
				this.DrawCameraLayer(spriteBatch, camera, MapLayers.MiddleLayer, header);

				// Draw players here

				this.DrawCameraLayer(spriteBatch, camera, MapLayers.TopLayer, header);
			}

		}

		private void DrawCameraLayer (SpriteBatch spriteBatch, BaseCamera camera, MapLayers layer, MapHeader header)
		{
			this.DrawLayerMap(spriteBatch, camera, layer, header, Color.White);
		}

		private void DrawLayerMap (SpriteBatch spriteBatch, BaseCamera camera, MapLayers layer, MapHeader header, Color tint)
		{
			Check.NullArgument(spriteBatch, "spriteBatch");
			Check.NullArgument(header, "header");

			Map currentMap = header.Map;
			ScreenPoint camOffset = camera.Offset;
			int tileSize = currentMap.TileSize;

			for(int y = 0; y < currentMap.MapSize.Y; y++)
			{
				for(int x = 0; x < currentMap.MapSize.X; x++)
				{
					ScreenPoint pos = new MapPoint(x, y).ToScreenPoint() + camOffset + header.MapLocation.ToScreenPoint();
					Rectangle des = new Rectangle(pos.X, pos.Y, tileSize, tileSize);

					if(!camera.CheckIsVisible(des))
						continue;

					Rectangle sourceRectangle = currentMap.GetLayerSourceRect(new MapPoint(x, y), layer);

					if(sourceRectangle.IsEmpty)
						continue;

					spriteBatch.Draw(currentMap.Texture, des, sourceRectangle, Color.White);
				}
			}
		}

		#endregion

		public ReadOnlyDictionary<string, BaseCamera> Cameras
		{
			get
			{
				lock(this.cameras)
				{
					return new ReadOnlyDictionary<string, BaseCamera>(this.cameras);
				}
			}
		}

		public void AddCamera (string name, BaseCamera camera)
		{
			lock(this.cameras)
			{
				this.cameras.Add(name, camera);
			}
		}

		public bool RemoveCamera (string name)
		{
			lock(this.cameras)
			{
				return this.cameras.Remove(name);
			}
		}

		public ReadOnlyDictionary<string, BaseCharacter> Players
		{
			get
			{
				lock(this.players)
				{
					return new ReadOnlyDictionary<string, BaseCharacter>(this.players);
				}
			}
		}

		public void AddPlayer (string name, BaseCharacter character)
		{
			lock(this.players)
			{
				this.players.Add(name, character);
			}
		}

		public bool RemoveCharacter (string name)
		{
			lock(this.players)
			{
				return this.players.Remove(name);
			}
		}

		#endregion

		#region IEngineProvider Members

		public void LoadEngineContext (IEngineContext context)
		{
			this.context = context;

			this.isloaded = true;
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		#endregion

		private readonly Dictionary<string, BaseCamera> cameras = new Dictionary<string, BaseCamera>();
		private readonly Dictionary<string, BaseCharacter> players = new Dictionary<string, BaseCharacter>();
		private IEngineContext context = null;
		private bool isloaded = false;
	}
}
