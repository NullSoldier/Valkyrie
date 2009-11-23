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

		public IWorldManager WorldManager
		{
			get { return this.worldmanager; }
		}

		public ITextureManager TextureManager
		{
			get { return this.texturemanager; }
		}

		#endregion

		public void Load (ISceneProvider draw, IEventProvider events, INetworkProvider network, ISoundProvider sound, IModuleProvider modules, IMovementProvider movement, ICollisionProvider collision, IWorldManager world, ITextureManager texture)
		{
			this.drawprovider = draw;
			this.drawprovider.LoadEngineContext(this);

			this.eventprovider = events;
			this.eventprovider.LoadEngineContext(this);

			this.networkprovider = network;
			this.networkprovider.LoadEngineContext(this);

			this.soundprovider = sound;
			this.soundprovider.LoadEngineContext(this);

			this.moduleprovider = modules;
			this.moduleprovider.LoadEngineContext(this);

			this.movementprovider = movement;
			this.movementprovider.LoadEngineContext(this);

			this.collisionprovider = collision;
			this.collisionprovider.LoadEngineContext(this);

			this.texturemanager = texture;
			this.texturemanager.LoadEngineContext(this);			

			this.worldmanager = world;
			this.worldmanager.LoadEngineContext(this);

			this.IsLoaded = true;
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

		private IWorldManager worldmanager;
		private ITextureManager texturemanager;

		private EngineConfiguration configuration;
		private bool loaded = false;
	}
}
