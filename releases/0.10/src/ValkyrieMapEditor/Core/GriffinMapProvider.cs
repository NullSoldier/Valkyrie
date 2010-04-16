using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using System.IO;
using System.Xml;
using Valkyrie.Engine.Maps;
using Valkyrie.Engine.Core;
using Microsoft.Xna.Framework;
using Valkyrie.Engine.Animation;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine.Events;
using System.Reflection;

namespace ValkyrieMapEditor.Core
{
	public class GriffinMapProvider
		: IMapProvider
	{
		#region Constructors 
		
		public GriffinMapProvider (IEnumerable<Assembly> eventassemblies)
		{
			this.eventassemblies = eventassemblies;
		}

		#endregion

		public Map GetMap (string filepath, IEventProvider eventprovider)
		{
            Uri location = new Uri(filepath);

			FileInfo MapPath = new FileInfo(location.LocalPath);
			if(!MapPath.Exists)
				throw new ArgumentException("Invalid file path. Map does not exist.", "location");

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
					Map.TileSize = Convert.ToInt32(innerNodes[i].InnerText);
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
				else if(innerNodes[i].Name == "Events" )
				{
					var root = innerNodes[i];

					XmlNodeList events = root.ChildNodes;

					foreach(XmlNode node in events)
					{
						#region Event Loading

						string type = string.Empty;
						Directions dir = Directions.Any;
						Dictionary<String, String> parameters = new Dictionary<string, string>();
						MapPoint eventlocation = MapPoint.Zero;
						MapPoint size = MapPoint.Zero;
						ActivationTypes activation = ActivationTypes.Static;

						foreach(XmlNode cnode in node.ChildNodes)
						{
							switch(cnode.Name)
							{
								case "Type": type = cnode.InnerText; break;
								case "Dir": dir = (Directions)Enum.Parse(typeof(Directions), cnode.InnerText); break;
								case "Parameters": parameters = this.LoadParametersFromXml(cnode); break;
								case "Location": eventlocation = new MapPoint(cnode); break;
								case "Size": size = new MapPoint(cnode); break;
								case "Activation": activation = (ActivationTypes)Enum.Parse(typeof(ActivationTypes), cnode.InnerText); break;
							}
						}

						IMapEvent newEvent = this.CreateEventFromString(type);
						if(newEvent == null)
						{
							MapEditorManager.NoEvents = true;
							break;
						}
						
						newEvent.Direction = dir;
						newEvent.Parameters = parameters;
						newEvent.Rectangle = new Rectangle(eventlocation.IntX, eventlocation.IntY, size.IntX, size.IntY);
						newEvent.Activation = activation;

						eventprovider.AddEvent(Map, newEvent);
						
						#endregion
					}
				}
			}

			return Map;
		}

		private Dictionary<string, string> LoadParametersFromXml (XmlNode node)
		{
			var parameters = new Dictionary<string, string>();

			foreach(XmlNode parameter in node.ChildNodes)
			{
				String name = "";
				String value = "";

				foreach(XmlNode child in parameter.ChildNodes)
				{
					switch(child.Name)
					{
						case "Name": name = child.InnerText; break;
						case "Value": value = child.InnerText; break;
					}
				}

				if(!String.IsNullOrEmpty(name))
					parameters.Add(name, value);
			}

			return parameters;
		}

		private IMapEvent CreateEventFromString (string typename)
		{
			Type type = null;

			foreach(Assembly assembly in this.eventassemblies)
			{
				type = assembly.GetType(typename);

				if(type != null)
					break;
			}

			if(type == null)
				return null; // throw new Exception("Type has not been found in any of the referenced assembly. The map cannot load the event.");

			return (IMapEvent)Activator.CreateInstance(type, true);
		}

		private int ConvertStringToInt (string value)
		{
			return Convert.ToInt32(value);
		}

		private IEnumerable<Assembly> eventassemblies;
	}
}
