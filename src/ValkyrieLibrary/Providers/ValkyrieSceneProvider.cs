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
using Valkyrie.Engine.Events;
using Cadenza.Collections;
using Valkyrie.Engine.Core.Scene;
using System.Collections.ObjectModel;
using Valkyrie.Engine.Managers;
using Valkyrie.Library.Managers;

namespace Valkyrie.Library.Providers
{
	public class ValkyrieSceneProvider : ISceneProvider
	{
		#region Constructors

		public ValkyrieSceneProvider(GraphicsDevice graphicsdevice, SpriteBatch spritebatch, IFogRenderer fogrenderer)
		{
			this.device = graphicsdevice;
			this.spritebatch = spritebatch;

			players = new ValkyriePlayerManager<BaseCharacter>();
			cameras = new ValkyrieCameraManager<BaseCamera>();
			renderers = new ValkyrieRendererManager();
			this.fogrenderer = fogrenderer;
		}

		#endregion

		#region Public Properties / Methods

		public IPlayerManager<BaseCharacter> Players { get { return this.players; } }
		public ICameraManager<BaseCamera> Cameras { get { return this.cameras; } }
		public IRendererManager Renderers { get { return this.renderers; } }
		public IFogRenderer FogRenderer { get { return this.fogrenderer; } }

		public void BeginScene(string cameraid)
		{
			var camera = this.cameras[cameraid];
			
			this.BeginScene(camera);
		}

		public void BeginScene(BaseCamera camera)
		{
			Check.NullArgument<BaseCamera>(camera, "camera");

			currentcamera = camera;
			device.Viewport = currentcamera.Viewport;
			spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise,  null, currentcamera.TransformMatrix);

			this.isstarted = true;
		}

		public void EndScene()
		{
			currentcamera = null;
			spritebatch.End();

			this.isstarted = false;
		}

		public void Update (GameTime gameTime)
		{
			// Update all players as well as their current map
			this.players.GetItems().Values.ForEach(p => { p.Update(gameTime); UpdateCurrentMap(p); });

            // Update all cameras
			this.Cameras.GetItems().Values.ForEach(c => { c.Update(gameTime); this.ResolvePositionableCurrentMap(c); });

            // Update every map in every world
			this.context.WorldManager.GetWorlds().SelectMany(w => w.Value.Maps.Values).Where( h => h.IsLoaded).ForEach( h => h.Map.Update(gameTime));

            // Update all renderers
            this.renderers.GetItems().ForEach(r => r.Update(gameTime));
		}

		public void Draw (RenderFlags flags)
		{
			this.DrawCamera(spritebatch, currentcamera, flags);
		}

		public void DrawLayer (MapLayers layer)
		{
			this.DrawLayerMap (spritebatch, currentcamera, layer);
		}

		public void DrawPlayer (string playername)
		{
			var player = this.Players[playername];

			Check.NullArgument<BaseCharacter>(player, "player");

			this.DrawPlayer(spritebatch, player, currentcamera);
		}

		/// <summary>
		/// Garentees the return of the positionables local map if they are on one
		/// </summary>
		/// <param name="positionable"></param>
		/// <returns></returns>
		public MapHeader GetPositionableLocalMap(BaseCharacter positionable)
		{
			Check.NullArgument<BaseCharacter>(positionable, "positionable");

			if (positionable.CurrentMap != null)
				return positionable.CurrentMap;
			else
			{
				this.ResolvePositionableCurrentMap(positionable);
				this.context.EventProvider.HandleEvent(positionable, ActivationTypes.OnMapEnter);
			}

			return positionable.CurrentMap;
		}

		#endregion

		#region IEngineProvider Members

		public void LoadEngineContext (IEngineContext context)
		{
			this.context = context;
			
			this.players.LoadEngineContext(context);
			this.cameras.LoadEngineContext(context);
			this.renderers.LoadEngineContext(context);

			this.isloaded = true;
		}

		public void Unload ()
		{
			this.isloaded = false;
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		#endregion

		private IEngineContext context = null;
		private GraphicsDevice device = null;
		private SpriteBatch spritebatch = null;
		private IPlayerManager<BaseCharacter> players = null;
		private ICameraManager<BaseCamera> cameras = null;
		private IRendererManager renderers = null;
		private IFogRenderer fogrenderer = null;
		private List<Rectangle> lastfoginfos = new List<Rectangle>();
		private BaseCamera currentcamera = null;
		private bool isloaded = false;
		private bool isstarted = false;

		private bool ResolvePositionableCurrentMap (IPositionable player)
		{
			if (!this.NeedsResolveMap(player))
				return true;

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

		private void UpdateCurrentMap(BaseCharacter player)
		{
			Check.NullArgument<IPositionable>(player, "player");

			// Update players current map
			if (player.CurrentMap == null
				|| player.LocalTileLocation.X < 0
				|| player.LocalTileLocation.Y < 0
				|| player.LocalTileLocation.X > player.CurrentMap.Map.MapSize.X
				|| player.LocalTileLocation.Y > player.CurrentMap.Map.MapSize.Y)
			{
				this.ResolvePositionableCurrentMap(player);
				this.context.EventProvider.HandleEvent(player, ActivationTypes.OnMapEnter);
			}
		}

		private bool NeedsResolveMap(IPositionable player)
		{
			Check.NullArgument<IPositionable>(player, "player");

			var map = player.CurrentMap;
			var loc = player.GlobalTileLocation;

			if (map == null) return true;

			var maprect = new Rectangle(map.MapLocation.IntX,
										map.MapLocation.IntY,
										map.Map.MapSize.IntX,
										map.Map.MapSize.IntY);

			return (!maprect.Contains(loc.IntX, loc.IntY));
		}

		#region Private Draw Methods

		private void DrawCamera(SpriteBatch spriteBatch, BaseCamera camera, RenderFlags flags)
		{
			DrawPass(spriteBatch, camera, flags); // Draw only non opaque
		}

		private void DrawPass(SpriteBatch spriteBatch, BaseCamera camera, RenderFlags flags)
		{
			foreach (var header in this.context.WorldManager.GetWorld(camera.WorldName).Maps.Values)
			{
				if (!header.IsVisible(camera))
					continue;

				// Draw under
				this.DrawLayerMap(spriteBatch, camera, MapLayers.UnderLayer);
				this.renderers[MapLayers.UnderLayer].ForEach(r => r.Draw(spriteBatch));

				// Draw base
				this.DrawLayerMap(spriteBatch, camera, MapLayers.BaseLayer);
				this.renderers[MapLayers.BaseLayer].ForEach(r => r.Draw(spriteBatch));

				// Draw middle
				this.DrawLayerMap(spriteBatch, camera, MapLayers.MiddleLayer);

				if ((flags & RenderFlags.NoPlayers) != RenderFlags.NoPlayers)
				{
					foreach (BaseCharacter player in this.Players.GetItems().Values)
						this.DrawPlayer(spriteBatch, player, camera);
				}

				this.renderers[MapLayers.MiddleLayer].ForEach(r => r.Draw(spriteBatch));

				//Draw top
				this.DrawLayerMap(spriteBatch, camera, MapLayers.TopLayer);
				this.renderers[MapLayers.TopLayer].ForEach(r => r.Draw(spriteBatch));
			}
		}

		private void DrawLayerMap(SpriteBatch spriteBatch, BaseCamera camera, MapLayers layer)
		{
			if(camera.CurrentMap == null)
				ResolvePositionableCurrentMap(camera);

			this.DrawLayerMap(spriteBatch, camera, layer, camera.CurrentMap, Color.White);
		}

		private void DrawLayerMap(SpriteBatch spriteBatch, BaseCamera camera, MapLayers layer, MapHeader header, Color tint)
		{
			Check.NullArgument(spriteBatch, "spriteBatch");
			Check.NullArgument(header, "header");

			Map currentMap = header.Map;
			ScreenPoint camOffset = ScreenPoint.Zero;// camera.Offset;
			int tileSize = currentMap.TileSize;

			for (int y = 0; y < currentMap.MapSize.Y; y++)
			{
				for (int x = 0; x < currentMap.MapSize.X; x++)
				{
					ScreenPoint pos = new MapPoint(x, y).ToScreenPoint() + camOffset + header.MapLocation.ToScreenPoint();
					Rectangle des = new Rectangle(pos.IntX, pos.IntY, tileSize, tileSize);
					MapPoint point = new MapPoint(x, y);

					if (!camera.CheckIsVisible(des))
						continue;

					// Opaque only and not opaque
					var opacity = currentMap.GetLayerValue(point, MapLayers.OpaqueLayer);

					if (opacity >= 1)
					{
						var info = new Rectangle()
						{
							X = pos.IntX,
							Y = pos.IntY,
							Width = tileSize,
							Height = tileSize
						};

						if(!this.lastfoginfos.Contains(info))
							this.lastfoginfos.Add(info);
					}

					Rectangle sourceRectangle = currentMap.GetLayerSourceRect(point, layer);

					if (sourceRectangle.IsEmpty)
						continue;

					spriteBatch.Draw(currentMap.Texture, des, sourceRectangle, tint);
				}
			}
		}

		private void DrawPlayer(SpriteBatch spriteBatch, BaseCharacter player, BaseCamera camera)
		{
			if (!player.IsVisible)
				return;

			Vector2 location = new Vector2();
			location.X = player.Location.X;// +(32 / 2) - (player.CurrentAnimation.FrameRectangle.Width / 2);
			location.Y = player.Location.Y + 32 - player.CurrentAnimation.FrameRectangle.Height;

			spriteBatch.Draw(player.Sprite, location, player.CurrentAnimation.FrameRectangle, Color.White);

			//if(this.FogRenderer.Texture != null)
			//	spriteBatch.Draw(this.FogRenderer.Texture, new Rectangle((int)location.X, (int)location.Y, 32, 32), Color.White);
		}

		#endregion
	}
}
