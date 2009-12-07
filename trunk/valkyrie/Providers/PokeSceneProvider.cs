﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine;
using Gablarski.Network;
using System.Net;
using System.Net.Sockets;
using Valkyrie.Characters;
using Gablarski.Messages;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine.Maps;
using Mono.Rocks;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Core.Characters;
using Valkyrie.Library.Core.Characters;

namespace Valkyrie.Providers
{
	public class PokeSceneProvider
		: ISceneProvider
	{
		#region Constructors

		public PokeSceneProvider (GraphicsDevice graphicsdevice)
		{
			this.device = graphicsdevice;
		}

		#endregion

		#region ISceneProvider Members

		public void Update (GameTime gameTime)
		{
			foreach(BaseCamera camera in this.cameras.Values)
				camera.Update(gameTime);

			foreach(BaseCharacter player in this.players.Values)
			{
				player.Update(gameTime);

				// Update players current map
				if(player.CurrentMap == null || player.LocalTileLocation.X < 0 || player.LocalTileLocation.Y < 0 ||
					player.LocalTileLocation.X > player.CurrentMap.Map.MapSize.X || player.LocalTileLocation.Y > player.CurrentMap.Map.MapSize.Y)
				{
					this.ResolvePositionableCurrentMap(player);
				}
			}

			foreach(MapHeader header in this.context.WorldManager.GetWorlds().SelectMany(w => w.Value.Maps.Values).Where(h => h.IsLoaded))
				header.Map.Update(gameTime);
		}

		/// <summary>
		/// Garentees the return of the positionables local map if they are on one
		/// </summary>
		/// <param name="positionable"></param>
		/// <returns></returns>
		public MapHeader GetPositionableLocalMap (IPositionable positionable)
		{
			if(positionable.CurrentMap != null)
				return positionable.CurrentMap;

			this.ResolvePositionableCurrentMap(positionable);
			return positionable.CurrentMap;
		}

		#region Public Draw Methods

		public void Draw (SpriteBatch spriteBatch)
		{
			// Empty
		}

		public void DrawCamera (SpriteBatch spriteBatch, string cameraname)
		{
			this.DrawCamera(spriteBatch, this.cameras[cameraname]);
		}

		public void DrawCameraLayer (SpriteBatch spriteBatch, string cameraname, MapLayers layer)
		{
			this.DrawCameraLayer(spriteBatch, this.cameras[cameraname], layer);
		}

		private void DrawCameraLayer (SpriteBatch spriteBatch, BaseCamera camera, MapLayers layer)
		{
			foreach(var header in this.context.WorldManager.GetWorld(camera.WorldName).Maps.Values)
			{
				if(!header.IsVisible(camera))
					continue;

				spriteBatch.Begin();
				this.device.Viewport = camera.Viewport;

				this.DrawCameraLayer(spriteBatch, camera, layer, header);

				spriteBatch.End();
			}
		}

		public void DrawAllCameras (SpriteBatch spriteBatch)
		{
			foreach(BaseCamera camera in this.cameras.Values)
			{
				this.DrawCamera(spriteBatch, camera);
			}
		}

		public void DrawPlayer (SpriteBatch spriteBatch, string playername, BaseCamera camera)
		{
			this.DrawPlayer(spriteBatch, this.players[playername], camera);
		}

		public void DrawPlayer (SpriteBatch spriteBatch, BaseCharacter player, BaseCamera camera)
		{
			this.playerrenderer.Draw(player, spriteBatch, camera);
		}

		#endregion

		#region Private Draw Methods

		private void DrawCamera (SpriteBatch spriteBatch, BaseCamera camera)
		{
			foreach(var header in this.context.WorldManager.GetWorld(camera.WorldName).Maps.Values)
			{
				if(!header.IsVisible(camera))
					continue;

				spriteBatch.Begin();
				this.device.Viewport = camera.Viewport;

				this.DrawCameraLayer(spriteBatch, camera, MapLayers.UnderLayer, header);
				this.DrawCameraLayer(spriteBatch, camera, MapLayers.BaseLayer, header);
				this.DrawCameraLayer(spriteBatch, camera, MapLayers.MiddleLayer, header);

				foreach(BaseCharacter player in this.players.Values)
					this.DrawPlayer(spriteBatch, player, camera);

				// Draw network players
				var network = (PokeNetworkProvider)this.context.NetworkProvider;
				foreach(PokePlayer player in network.GetPlayers())
				{
					if(player.Loaded)
						this.DrawPlayer(spriteBatch, player, camera);
				}

				this.DrawCameraLayer(spriteBatch, camera, MapLayers.TopLayer, header);

				spriteBatch.End();
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

					spriteBatch.Draw(currentMap.Texture, des, sourceRectangle, tint);
				}
			}
		}

		#endregion

		public BaseCamera GetCamera (string name)
		{
			lock(this.cameras)
			{
				if(!this.cameras.ContainsKey(name))
					throw new Exception("Camera not found.");

				return this.cameras[name];
			}
		}

		public ReadOnlyDictionary<string, BaseCamera> GetCameras ()
		{
			return new ReadOnlyDictionary<string, BaseCamera>(this.cameras);
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

		public BaseCharacter GetPlayer (string name)
		{
			lock(this.players)
			{
				if(!this.players.ContainsKey(name))
					throw new Exception("Player not found");

				return this.players[name];
			}
		}

		public ReadOnlyDictionary<string, BaseCharacter> GetPlayers ()
		{
			return new ReadOnlyDictionary<string, BaseCharacter>(this.players);
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
		private GraphicsDevice device = null;
		private ICharacterRenderer playerrenderer = new PokeCharacterRenderer();
		private bool isloaded = false;

		private bool ResolvePositionableCurrentMap (IPositionable player)
		{
			player.CurrentMap = null;

			bool found = false;

			World currentworld = this.context.WorldManager.GetWorld(player.WorldName);

			foreach(MapHeader header in currentworld.Maps.Values)
			{
				Rectangle rect = (header.MapLocation).ToRect(header.Map.MapSize.ToPoint());

				if(rect.Contains(player.GlobalTileLocation.ToPoint()))
				{
					player.CurrentMap = header;
					found = true;
				}
			}

			return found;
		}
	}
}
