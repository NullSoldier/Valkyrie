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

		public event EventHandler UndoUsed;
		public event EventHandler RedoUsed;
		public event EventHandler ActionPerformed;

		public bool ContainsUndoActions { get { return undo.Count > 0; } }
		public bool ContainsRedoActions { get { return redo.Count > 0; } }

		public void PerformAction(IUserAction action)
		{
			action.Do (context);

			AddNoPerform(action);

			var handler = ActionPerformed;
			if (handler != null) handler(this, EventArgs.Empty);
		}

		public void AddNoPerform(IUserAction action)
		{
			redo.Clear();
			undo.Push(action);

			var handler = ActionPerformed;
			if (handler != null) handler(this, EventArgs.Empty);
		}

		public void UndoAction()
		{
			if (undo.Count <= 0) 
				return;

			var action = undo.Pop();

			action.Undo(context);
			redo.Push(action);

			var handler = UndoUsed;
			if (handler != null) handler(this, EventArgs.Empty);
		}

		public void RedoAction()
		{
			if (redo.Count <= 0)
				return;

			var action = redo.Pop();

			action.Redo(context);
			undo.Push(action);

			var handler = RedoUsed;
			if (handler != null) handler(this, EventArgs.Empty);
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
