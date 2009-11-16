using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Valkyrie.Library.Core;
using Microsoft.Xna.Framework;
using Valkyrie.Library.Animation;
using Valkyrie.Library.Events;
using Valkyrie.Library.Characters;
using System.Reflection;

namespace Valkyrie.Library.Maps.MapProvider
{
	public class XMLMapProvider
		: IMapProvider
	{
		// TileEngine.EventManager.LoadEvents(this);

		#region IMapProvider Members

		public Map GetMap (object providerdata, MapEventManager mapmanager)
		{
			FileInfo MapPath = (FileInfo)providerdata;
			if(!MapPath.Exists)
				throw new ArgumentException("Invalid file path. Map does not exist.");

			Map Map = new Map();
			
			XmlDocument doc = new XmlDocument();
			doc.Load(MapPath.FullName);

			XmlNodeList nodes = doc.GetElementsByTagName("Map");
			XmlNodeList innerNodes = nodes[0].ChildNodes;

			for(int i = 0; i < innerNodes.Count; i++)
			{
				if(innerNodes[i].Name == "Name")
					Map.Name = innerNodes[i].InnerText;

				else if(innerNodes[i].Name == "TileSet")
					Map.TextureName = innerNodes[i].InnerText;

				else if(innerNodes[i].Name == "MapSize")
				{
					int x = 0, y = 0;

					foreach(XmlNode node in innerNodes[i].ChildNodes)
					{
						if(node.Name == "X")
							x = Convert.ToInt32(node.InnerText);
						else if(node.Name == "Y")
							y = Convert.ToInt32(node.InnerText);
					}
					Map.MapSize = new MapPoint(x, y);
				}
				else if(innerNodes[i].Name == "TilePixelSize")
				{
					int size = Convert.ToInt32(innerNodes[i].InnerText);
					Map.TileSize = new ScreenPoint(size, size); // Tiles are always square
				}

				else if(innerNodes[i].Name == "UnderLayer")
				{
					string[] baseText = innerNodes[i].InnerText.Replace("\r\n", string.Empty).Replace("   ", string.Empty).Trim().Split(' ');

					Map.UnderLayer = Array.ConvertAll<string, int>(baseText, new Converter<string, int>(this.ConvertStringToInt));
				}

				else if(innerNodes[i].Name == "BaseLayer")
				{
					string[] baseText = innerNodes[i].InnerText.Replace("\r\n", string.Empty).Replace("   ", string.Empty).Trim().Split(' ');

					Map.BaseLayer = Array.ConvertAll<string, int>(baseText, new Converter<string, int>(this.ConvertStringToInt));
				}

				else if(innerNodes[i].Name == "MiddleLayer")
				{
					string[] baseText = innerNodes[i].InnerText.Replace("\r\n", string.Empty).Replace("   ", string.Empty).Trim().Split(' ');

					Map.MiddleLayer = Array.ConvertAll<string, int>(baseText, new Converter<string, int>(this.ConvertStringToInt));
				}

				else if(innerNodes[i].Name == "TopLayer")
				{
					string[] baseText = innerNodes[i].InnerText.Replace("\r\n", string.Empty).Replace("   ", string.Empty).Trim().Split(' ');

					Map.TopLayer = Array.ConvertAll<string, int>(baseText, new Converter<string, int>(this.ConvertStringToInt));
				}
				else if(innerNodes[i].Name == "CollisionLayer")
				{
					string[] baseText = innerNodes[i].InnerText.Replace("\r\n", string.Empty).Replace("   ", string.Empty).Trim().Split(' ');

					Map.CollisionLayer = Array.ConvertAll<string, int>(baseText, new Converter<string, int>(this.ConvertStringToInt));
				}
				else if(innerNodes[i].Name == "AnimatedTiles")
				{
					XmlNodeList tiles = innerNodes[i].ChildNodes;

					foreach(XmlNode node in tiles)
					{
						int tileID = 0;
						int frameCount = 0;
						Rectangle tileRect = Rectangle.Empty;

						foreach(XmlNode subnode in node.ChildNodes)
						{
							if(subnode.Name == "TileID")
								tileID = Convert.ToInt32(subnode.InnerText);
							else if(subnode.Name == "FrameCount")
								frameCount = Convert.ToInt32(subnode.InnerText);
							else if(subnode.Name == "TileRect")
							{
								var data = Array.ConvertAll<string, int>(subnode.InnerText.Split(' '), new Converter<string, int>(this.ConvertStringToInt));
								tileRect = new Rectangle(data[0], data[1], data[2], data[3]);
							}

						}

						Map.AnimatedTiles.Add(tileID, new FrameAnimation(tileRect, frameCount));
					}
				}
				else if(innerNodes[i].Name == "Events" && mapmanager != null)
				{
					if(Map.EventList.Count > 0) // Already loaded? Don't load again
						continue;

					var root = innerNodes[i];

					XmlNodeList events = root.ChildNodes;

					foreach(XmlNode node in events)
					{
						Map.EventList.Add(mapmanager.LoadEventFromXml(node));
					}
				}
			}

			return Map;
		}

		#endregion

		private int ConvertStringToInt (string value)
		{
			return Convert.ToInt32(value);
		}
	}
}
