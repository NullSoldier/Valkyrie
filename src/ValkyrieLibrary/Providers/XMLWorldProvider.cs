using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Valkyrie.Engine.Maps;
using Valkyrie.Engine.Core;
using Valkyrie.Engine.Providers;

namespace Valkyrie.Library.Providers
{
	public class XMLWorldProvider
		: IWorldProvider
	{
		#region IWorldProvider Members

		public World GetWorld(Uri location, string name)
		{
			if( !location.IsFile)
				throw new ArgumentException("World location must be a file on the local system.");

			return this.GetWorlds(location).Where(p => p.Name == name).FirstOrDefault();
		}

		public IEnumerable<World> GetWorlds(Uri location)
		{
			if(!location.IsFile)
				throw new ArgumentException("World location must be a file on the local system.");
			else if(!new FileInfo(location.LocalPath).Exists)
				throw new FileNotFoundException("The world file was not found.");

			List<World> worlds = new List<World>();

			XmlDocument doc = new XmlDocument();
			doc.Load(location.LocalPath);

			XmlNodeList worldNodesParent = doc.GetElementsByTagName("Worlds");

			foreach(XmlNode worldNodes in worldNodesParent)
			{
				foreach(XmlNode worldNode in worldNodes)
				{
					World world = new World(worldNode.Attributes.GetNamedItem("Name").InnerText);

					foreach(XmlNode node in worldNode.ChildNodes)
					{
						if(node.Name == "MapLoc")
						{
							/* Load the map header from an XML node */

							string mapname = string.Empty;
							string mappath = string.Empty;
							int x = 0, y = 0;

							foreach(XmlNode cnode in node.ChildNodes)
							{
								if(cnode.Name == "Name")
									mapname = cnode.InnerText;
								else if(cnode.Name == "Uri")
									mappath =  cnode.InnerText;
								else if(cnode.Name == "X")
									x = Convert.ToInt32(cnode.InnerText);
								else if(cnode.Name == "Y")
									y = Convert.ToInt32(cnode.InnerText);
							}

							MapHeader header = new MapHeader(mapname, new MapPoint(x, y), mappath);

							world.AddMap(header);
						}
					}

					worlds.Add(world);
				}
			}

			return worlds;
		}

		#endregion
	}
}
