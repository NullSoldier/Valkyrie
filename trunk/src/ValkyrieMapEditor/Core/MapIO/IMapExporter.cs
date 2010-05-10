using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Maps;
using System.IO;

namespace ValkyrieMapEditor.Core
{
	public interface IMapExporter
	{
		void Export(Map map, FileInfo file);
	}
}
