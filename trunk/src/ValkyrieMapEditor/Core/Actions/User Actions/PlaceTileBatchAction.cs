using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine;
using System.Collections;

namespace ValkyrieMapEditor.Core.Actions
{
	public class PlaceTileBatchAction : IUserAction
	{
		public PlaceTileBatchAction(params PlaceTileAction[] actions)
		{
			this.actions = actions;
		}

		public PlaceTileBatchAction(IEnumerable<PlaceTileAction> actions)
		{
			this.actions = actions;
		}

		public void Do (IEngineContext context)
		{
			this.Redo(context);
		}

		public void Redo (IEngineContext context)
		{
			foreach (var action in actions)
				action.Redo(context);
		}

		public void Undo(IEngineContext context)
		{
			foreach (var action in actions)
				action.Undo(context);			
		}

		IEnumerable<PlaceTileAction> actions;
	}
}