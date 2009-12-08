using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Library.Core;
using Gablarski;
using System.Drawing;
using ValkyrieServerLibrary.Entities;
using ValkyrieServerLibrary.Core;

namespace Valkyrie.Library.Network
{
	public class NetworkPlayer
	{
		public IConnection Connection
		{
			get;
			set;
		}

		public int AccountID
		{
			get;
			set;
		}

		public uint NetworkID
		{
			get;
			set;
		}

		public Character Character
		{
			get;
			set;
		}

		public PlayerState State
		{
			get;
			set;
		}

	}
}
