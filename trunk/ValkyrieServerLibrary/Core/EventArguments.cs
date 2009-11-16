using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Library.Network;

namespace ValkyrieServerLibrary.Core
{
	public class UserEventArgs
		: EventArgs
	{
		public UserEventArgs (NetworkPlayer player)
		{
			this.Player = player;
		}

		public NetworkPlayer Player { get; set; }
	}
}
