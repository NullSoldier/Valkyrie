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
			Id(a => a.ID, "`ID`");
			
			Map(a => a.Username, "`Username`");
			Map(a => a.Password, "`Password`");
		}
	}
}
