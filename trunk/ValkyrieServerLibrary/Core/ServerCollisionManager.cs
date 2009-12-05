using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Library.Core;
using Valkyrie.Engine;
using Microsoft.Xna.Framework;
using Valkyrie.Engine.Maps;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine.Managers;

namespace ValkyrieServerLibrary.Core
{
	public class ServerCollisionProvider
		: ICollisionProvider
	{
		#region Constructor

		public ServerCollisionProvider (IWorldManager worldmanager)
		{
			this.worldmanager = worldmanager;
		}

		#endregion

		public bool CheckCollision (IMovable Source, ScreenPoint Destination)
		{
			return this.CheckCollision(Source, Destination.ToMapPoint());
		}

		public bool CheckCollision (IMovable source, MapPoint Destination)
		{
			if(source.Density < 1)
				return true;

			World currentworld = this.worldmanager.GetWorld(source.WorldName);

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

		#region IEngineProvider Members

		public void LoadEngineContext (IEngineContext context)
		{
			if(context != null)
				this.worldmanager = context.WorldManager;

			this.isloaded = true;
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
		}

		#endregion

		private IWorldManager worldmanager = null;
		private bool isloaded = false;
	}
}
