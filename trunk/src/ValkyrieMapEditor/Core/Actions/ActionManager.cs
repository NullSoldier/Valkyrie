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
			undo = new Stack<IUserAction>();
			redo = new Stack<IUserAction>();

			this.context = context;
		}

		public void PerformAction(IUserAction action)
		{
			action.Do (context);

			AddNoPerform(action);
		}

		public void AddNoPerform(IUserAction action)
		{
			redo.Clear();
			undo.Push(action);
		}

		public void UndoAction()
		{
			if (undo.Count <= 0) 
				return;

			var action = undo.Pop();

			action.Undo(context);
			redo.Push(action);
		}

		public void RedoAction()
		{
			var action = redo.Pop();

			action.Redo(context);
			undo.Push(action);
		}

		public void Reset()
		{
			this.redo.Clear();
			this.undo.Clear();
		}

		private Stack<IUserAction> redo;
		private Stack<IUserAction> undo;
		private IEngineContext context;
	}
}
