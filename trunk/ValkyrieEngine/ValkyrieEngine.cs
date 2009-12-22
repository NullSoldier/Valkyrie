using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Valkyrie.Engine.Providers;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Valkyrie.Engine.Managers;
using System.IO;
using Valkyrie.Library.Core;

namespace Valkyrie.Engine
{
	public class ValkyrieEngine
		: IEngineContext
	{
		#region Constructors

		public ValkyrieEngine (EngineConfiguration config)
		{
			this.configuration = config;
		}

		#endregion

		#region Public Properties

		public bool IsLoaded
		{
			get { return this.loaded; }
			set { this.loaded = value; }
		}
		
		public EngineConfiguration Configuration
		{
			get { return this.configuration; }
		}

		public ISoundProvider SoundProvider
		{
			get { return this.soundprovider; }
		}

		public ISceneProvider SceneProvider
		{
			get { return this.drawprovider; }
		}

		public INetworkProvider NetworkProvider
		{
			get { return this.networkprovider; }
		}

		public IEventProvider EventProvider
		{
			get { return this.eventprovider; }
		}

		public IModuleProvider ModuleProvider
		{
			get { return this.moduleprovider; }
		}

		public IMovementProvider MovementProvider
		{
			get { return this.movementprovider; }
		}

		public ICollisionProvider CollisionProvider
		{
			get { return this.collisionprovider; }
		}

		public IVoiceChatProvider VoiceChatProvider
		{
			get { return this.voicechatprovider; }
		}


		public IWorldManager WorldManager
		{
			get { return this.worldmanager; }
		}

		public ITextureManager TextureManager
		{
			get { return this.texturemanager; }
		}

		public ISoundManager SoundManager
		{
			get { return this.soundmanager; }
		}

		#endregion

		public void Load (ISceneProvider draw, IEventProvider events, INetworkProvider network, ISoundProvider sound, IModuleProvider modules, IMovementProvider movement, ICollisionProvider collision, IVoiceChatProvider voicechat, IWorldManager world, ITextureManager texture, ISoundManager soundmanager)
		{
			this.drawprovider = draw;
			this.eventprovider = events;
			this.networkprovider = network;
			this.soundprovider = sound;
			this.moduleprovider = modules;
			this.movementprovider = movement;
			this.collisionprovider = collision;
			this.voicechatprovider = voicechat;
			this.texturemanager = texture;
			this.worldmanager = world;
			this.soundmanager = soundmanager;

			LoadContexts (drawprovider, eventprovider, networkprovider, soundprovider, moduleprovider, movementprovider,
			              collisionprovider, voicechatprovider, texturemanager, worldmanager, soundmanager);
		}

		public void Unload ()
		{
			this.IsLoaded = false;
		}

		public void Update (GameTime gameTime)
		{
			this.ModuleProvider.UpdateCurrent(gameTime);
		}

		public void Draw (SpriteBatch spriteBatch, GameTime gameTime)
		{
			this.ModuleProvider.DrawCurrent(spriteBatch, gameTime);
		}

		private ISoundProvider soundprovider;
		private ISceneProvider drawprovider;
		private INetworkProvider networkprovider;
		private IEventProvider eventprovider;
		private IModuleProvider moduleprovider;
		private IMovementProvider movementprovider;
		private ICollisionProvider collisionprovider;
		private IVoiceChatProvider voicechatprovider;

		private IWorldManager worldmanager;
		private ITextureManager texturemanager;
		private ISoundManager soundmanager;

		private EngineConfiguration configuration;
		private bool loaded = false;

		private void LoadContexts (params IEngineProvider[] providers)
		{
			for (int i = 0; i < providers.Length; ++i)
			{
				//if (providers[i].IsLoaded)
				//    continue;

				providers[i].LoadEngineContext (this);
			}

			IsLoaded = true;
		}
	}
}
