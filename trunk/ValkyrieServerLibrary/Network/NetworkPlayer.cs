using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Core;
using Gablarski;
using System.Drawing;

namespace ValkyrieLibrary.Network
{
	public class NetworkPlayer
	{
		public IConnection Connection
		{
			get;
			set;
		}

		public uint NetworkID
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public Point Location
		{
			get;
			set;
		}
	}
}
