using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine;
using Valkyrie.Engine.Core;

namespace Valkyrie.Library.Providers
{
	public class ValkyrieMovementProvider
		: IMovementProvider
	{
		public void BeginMove (IMovable movable, Directions directioon)
		{
			throw new NotImplementedException();
		}

		public void BeginMoveDestination (IMovable movable, ScreenPoint destination)
		{
			throw new NotImplementedException();
		}

		public void EndMove (IMovable movable, bool fireevent)
		{
			throw new NotImplementedException();
		}

		public void Update (Microsoft.Xna.Framework.GameTime time)
		{
			throw new NotImplementedException();
		}

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
		private List<IMovable> movable = new List<IMovable>();
	}
}
