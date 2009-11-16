using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieServerLibrary.Entities;
using FluentNHibernate.Mapping;
using Valkyrie.Library.Characters;

namespace ValkyrieServerLibrary.Mappings
{
	public class CharacterMap
		: ClassMap<Character>
	{
		public CharacterMap()
		{
			Table("`characters`");

			Id(c => c.ID, "`characterID`");
			Map(c => c.Name, "`characterName`");

			Map(c => c.Direction, "`characterDirection`").CustomType<Directions> ();
			Map(c => c.MapX, "`characterMapX`");
			Map(c => c.MapY, "`characterMapY`");

			Map(c => c.TileSheet, "`characterTileSheet`");
			Map(c => c.WorldName, "`characterWorld`");
		}
	}
}
