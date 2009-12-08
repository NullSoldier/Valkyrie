using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieServerLibrary.Core;

namespace ValkyrieServerLibrary.Entities
{
	public class Account
	{
		public virtual int ID
		{
			get; set;
		}

		public virtual string Username
		{
			get; set;
		}

		public virtual string Password
		{
			get; set;
		}
	}
}
