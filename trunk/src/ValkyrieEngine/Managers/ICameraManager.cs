using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Valkyrie.Engine.Managers
{
	public interface ICameraManager<T> : IGenericManager<T>
		where T : BaseCamera
	{
	}
}
