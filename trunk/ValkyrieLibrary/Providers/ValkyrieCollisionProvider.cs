using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine;

namespace Valkyrie.Library.Providers
{
	public class ValkyrieCollisionProvider
		: ICollisionProvider
	{
		#region ICollisionProvider Members

		public bool CheckCollision (IMovable Source, ScreenPoint Destination)
		{
			//if (Source.Density == 0)
			//    return true;

			//MapPoint tilePoint = TileEngine.GlobalTilePointToLocal(Destination.ToMapPoint());

			//if (!TileEngine.WorldManager.CurrentWorld.MapList[TileEngine.CurrentMapChunk.Name].TilePointInMapLocal(tilePoint))
			//    return true;

			//if (TileEngine.CurrentMapChunk.GetLayerValue(tilePoint, MapLayers.CollisionLayer) != -1)
			//    return false;

			//return true;
			throw new NotImplementedException();
		}

		public bool CheckCollision (IMovable source, MapPoint Destination)
		{
			throw new NotImplementedException();
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

		private bool isloaded = false;
		private IEngineContext context = null;
	}
}
