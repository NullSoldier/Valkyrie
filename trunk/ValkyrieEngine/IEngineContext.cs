using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine.Managers;
using Valkyrie.Library.Core;

namespace Valkyrie.Engine
{
	public interface IEngineContext
	{
		ISoundProvider SoundProvider { get; }
		ISceneProvider SceneProvider { get; }
		INetworkProvider NetworkProvider { get; }
		IEventProvider EventProvider { get; }
		IModuleProvider ModuleProvider { get; }
		IMovementProvider MovementProvider { get; }
		ICollisionProvider CollisionProvider { get; }

		IWorldManager WorldManager { get; }
		ITextureManager TextureManager { get; }

		EngineConfiguration Configuration { get; }
	}
}
