using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine;

namespace ValkyrieMapEditor.Core
{
	public interface IUserAction
	{
		void Do(IEngineContext context);
		void Redo(IEngineContext context);
		void Undo(IEngineContext context);
	}
}
