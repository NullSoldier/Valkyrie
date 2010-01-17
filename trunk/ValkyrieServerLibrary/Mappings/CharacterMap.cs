using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieServerLibrary.Entities;
using FluentNHibernate.Mapping;
using Valkyrie.Engine.Characters;

namespace ValkyrieServerLibrary.Mappings
{
	public class CharacterMap
		: ClassMap<Character>
	{
		public CharacterMap()
		{
			Table("`characters`");

			Id(c => c.ID, "`ID`");
			Map (c => c.AccountID, "`AccountID`");
			Map(c => c.Name, "`Name`");

			Map(c => c.Direction, "`Direction`").CustomType<Directions> ();
			Map(c => c.MapX, "`MapX`");
			Map(c => c.MapY, "`MapY`");
			Map (c => c.Speed, "`Speed`");

			Map(c => c.TileSheet, "`TileSheet`");
			Map(c => c.WorldName, "`World`");
		}
	}
}
