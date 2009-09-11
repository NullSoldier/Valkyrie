using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieServerLibrary.Entities;
using FluentNHibernate.Mapping;

namespace ValkyrieServerLibrary.Mappings
{
	public class AccountMap
		: ClassMap<Account>
	{
		public AccountMap()
		{
			Table("`accounts`");
			Id(a => a.ID, "`accountID`");
			
			Map(a => a.Username, "`accountUsername`");
			Map(a => a.Password, "`accountPassword`");
		}
	}
}
