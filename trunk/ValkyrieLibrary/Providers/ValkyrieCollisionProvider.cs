using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine;
using Valkyrie.Engine.Maps;
using Microsoft.Xna.Framework;

namespace Valkyrie.Library.Providers
{
	public class ValkyrieCollisionProvider
		: ICollisionProvider
	{
		#region ICollisionProvider Members

		public bool CheckCollision (IMovable Source, ScreenPoint Destination)
		{
			return this.CheckCollision(Source, Destination.ToMapPoint());
		}

		public bool CheckCollision (IMovable source, MapPoint Destination)
		{
			if(source.Density < 1)
				return true;

			World currentworld = this.context.WorldManager.GetWorld(source.WorldName);

			foreach(MapHeader header in currentworld.Maps.Values)
			{
				Rectangle rect = (header.MapLocation).ToRect(header.Map.MapSize.ToPoint());

				if(rect.Contains(Destination.ToPoint()))
				{
					return (header.Map.GetLayerValue(Destination - header.MapLocation, MapLayers.CollisionLayer) == -1);
				}
			}

			return true;
		}

		#endregion

		#region IEngineProvider Members

		public void LoadEngineContext (IEngineContext context)
		{
			this.context = context;

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

		private bool isloaded = false;
		private IEngineContext context = null;
	}
}
