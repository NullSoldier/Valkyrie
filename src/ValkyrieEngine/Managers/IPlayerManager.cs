using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Characters;

namespace Valkyrie.Engine.Managers
{
	public interface IPlayerManager<T> : IGenericManager<T>
		where T : BaseCharacter
	{
	}
}
