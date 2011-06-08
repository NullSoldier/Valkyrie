using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine;
using System.Collections;
using System.Collections.ObjectModel;

namespace ValkyrieMapEditor.Core.Actions
{
	public class ActionBatchAction<T> : IUserAction
		where T : IUserAction
	{
		public ActionBatchAction()
		{
			this.actions = new List<T>();
		}

		public ActionBatchAction(params T[] actions)
		{
			this.actions = new List<T> (actions.Length);
			this.actions.AddRange(actions);
		}

		public ActionBatchAction(IEnumerable<T> actions)
		{
			this.actions = new List<T>(actions);
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
			// Always undo in the opposite order of which they are added
			actions.Reverse();

			foreach (var action in actions)
				action.Undo(context);

			actions.Reverse();
		}

		public void Add (T value)
		{
			actions.Add(value);
		}

		public void AddRange(IEnumerable<T> values)
		{
			actions.AddRange (values);
		}

		public ReadOnlyCollection<T> Actions
		{
			get { return new ReadOnlyCollection<T>(actions); }
		}

		private List<T> actions;
		
	}
}