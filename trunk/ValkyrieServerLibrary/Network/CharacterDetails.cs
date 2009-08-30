using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ValkyrieServerLibrary.Network
{
	public class CharacterDetails
	{
		public string Name
		{
			get;
			set;
		}

		public Point MapLocation
		{
			get;
			set;
		}

		public string TileSheet
		{
			get;
			set;
		}
	}
}
