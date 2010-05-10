using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine;

namespace ValkyrieMapEditor.Core.Actions
{
	public class ActionManager
	{
		public ActionManager(IEngineContext context)
		{
			this.context = context;
		}

		public void PerformAction(IUserAction action)
		{
			action.Do (context);

			Redo.Clear();
			Undo.Push(action);
		}

		public void UndoAction()
		{
			if (Undo.Count < 0) 
				return;

			var action = Undo.Pop();

			action.Undo(context);
			Redo.Push(action);
		}

		public void RedoAction()
		{
			var action = Redo.Pop();

			action.Redo(context);
			Undo.Push(action);
		}

		private Stack<IUserAction> Redo;
		private Stack<IUserAction> Undo;
		private IEngineContext context;
	}
}
