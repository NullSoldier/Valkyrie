using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Valkyrie.Library.Characters;
using Valkyrie.Library.Core;
using Valkyrie.Library.Maps;
using System.Reflection;

namespace Valkyrie.Library.Events
{
    public class MapEventManager
    {
		public MapEventManager(IEnumerable<Assembly> assemblies)
		{
			this.Assemblies = assemblies.ToList();
		}

		public readonly Dictionary<Map, List<IMapEvent>> events = new Dictionary<Map, List<IMapEvent>>();
		public readonly IEnumerable<Assembly> Assemblies;

		public void AddEvent(IMapEvent mapevent)
		{
			this.events[TileEngine.CurrentMapChunk].Add(mapevent);
		}

		public void RemoveEvent(IMapEvent mapevent)
		{
			this.events[TileEngine.CurrentMapChunk].Remove(mapevent);
		}

		public void ClearEventCache()
		{
			this.events.Clear();
		}

		public bool HandleEvent(BaseCharacter player, ActivationTypes activation)
		{
			BasePoint pos = new BasePoint(player.MapLocation.X, player.MapLocation.Y);

			if (activation == ActivationTypes.Activate || activation == ActivationTypes.Collision)
			{
				BasePoint newpt = player.GetLookPoint();
				pos = new BasePoint(pos.X + newpt.X, pos.Y + newpt.Y);

			}

			IMapEvent mapevent = GetEvent(new MapPoint(pos.X, pos.Y));

			if (mapevent != null &&
				(mapevent.Direction == Directions.Any || mapevent.Direction == player.Direction)
				&& mapevent.Activation == activation)
			{
				mapevent.Trigger(player);
				return true;
			}

			return false;
		}

		#region MapEvent Management Methods
		public IMapEvent GetEventInRect(BasePoint loc, BasePoint size)
		{
			return this.GetEventInRect(new Rectangle(loc.X, loc.Y, size.X, size.Y));
		}

		public IMapEvent GetEventInRect(Rectangle rect)
		{
			for(int x = rect.X; x < (rect.X + rect.Width); x++)
			{
				for (int y = rect.Y; y < (rect.Y + rect.Height); y++)
				{
					var mapevent = this.GetEvent(new MapPoint(x, y));
					
					if (mapevent != null)
						return mapevent;
				}
			}

			return null;

			#region MoreOldCode, needs testing
			/* // Test performance vs new method
			foreach (IMapEvent ev in this.events[TileEngine.CurrentMapChunk])
			{
				if (pos.ToRect(size.ToPoint()).Intersects(ev.ToRect()) == true)
					return ev;
			}

			return null;*/
			#endregion
		}

		public IMapEvent GetEvent(MapPoint location)
		{
			return this.events[TileEngine.CurrentMapChunk].Where(ev => ev.Rectangle.Contains(location.ToPoint())).FirstOrDefault();
		}

		public void SetOrAddEvent(IMapEvent e)
		{
			if (this.events[TileEngine.CurrentMapChunk].Contains(e))
			{
				int index = this.events[TileEngine.CurrentMapChunk].IndexOf(e);

				this.events[TileEngine.CurrentMapChunk][index] = (IMapEvent)e.Clone();
			}
			else
				this.events[TileEngine.CurrentMapChunk].Add(e);
		}

		public void DelEvent(IMapEvent e)
		{
			this.events[TileEngine.CurrentMapChunk].Remove(e);
		}

		public void LoadEvents(Map map)
		{
			// Set the list reference in the dictionary to the maps event list
			this.events[map] = map.EventList;
		}

		// bleh

		public IMapEvent LoadEventFromXml(XmlNode node)
		{
			string type = string.Empty;
			Directions dir = Directions.Any;
			Dictionary<String, String> parameters = new Dictionary<string, string>();
			MapPoint location = new MapPoint(0, 0);
			MapPoint size = new MapPoint(0, 0);
			ActivationTypes activation = ActivationTypes.Activate;

			foreach (XmlNode cnode in node.ChildNodes)
			{
				switch (cnode.Name)
				{
					case "Type": type = cnode.InnerText; break;
					case "Dir": dir = (Directions)Enum.Parse(typeof(Directions), cnode.InnerText); break;
					case "Parameters": parameters = LoadParametersFromXml(cnode); break;
					case "Location": location = new MapPoint(cnode); break;
					case "Size": size = new MapPoint(cnode); break;
					case "Activation": activation = (ActivationTypes)Enum.Parse(typeof(ActivationTypes), cnode.InnerText); break;
				}
			}

			IMapEvent newEvent = this.CreateEventFromString(type);
			
			var assemblytype = Assembly.GetEntryAssembly().GetType(type);
			
			newEvent.Direction = dir;
			newEvent.Parameters = parameters;
			newEvent.Rectangle = new Rectangle(location.X, location.Y, size.X, size.Y);
			newEvent.Activation = activation;

			return newEvent;
		}

		/// <summary>
		/// Gets an instance of the type from the qualified assembly name provided
		/// </summary>
		/// <param name="qualifiedassemblyname">An Assembly.FullyQualifiedName that references a type</param>
		/// <returns></returns>
		private IMapEvent CreateEventFromString(string typename)
		{
			Type type = null;

			foreach (Assembly assembly in this.Assemblies)
			{
				type = assembly.GetType(typename);
				
				if (type != null)
					break;
			}

			if(type == null)
				throw new Exception("Type has not been found in any of the referenced assembly. The map cannot load the event.");

			return (IMapEvent)Activator.CreateInstance(type, true);
		}

		public XmlNode EventToXmlNode(IMapEvent mapevent, XmlDocument doc)
		{
			XmlElement xmlevent = doc.CreateElement("Event");

			// Properties of event	
            XmlElement loc = doc.CreateElement("Location");
			new MapPoint(mapevent.Rectangle.X, mapevent.Rectangle.Y).ToXml(doc, loc);

            XmlElement size = doc.CreateElement("Size");
			new MapPoint(mapevent.Rectangle.Width, mapevent.Rectangle.Height).ToXml(doc, size);

            XmlElement type = doc.CreateElement("Type");
			type.InnerText = mapevent.GetType().FullName;

            XmlElement dir = doc.CreateElement("Dir");
            dir.InnerText = mapevent.Direction.ToString();

			XmlElement activation = doc.CreateElement("Activation");
			activation.InnerText = ((int)mapevent.Activation).ToString();

            XmlElement parmRoot = doc.CreateElement("Parameters");

            foreach (var parm in mapevent.Parameters)
            {
                XmlElement pname = doc.CreateElement("Name");
                pname.InnerText = parm.Key;

                XmlElement pvalue = doc.CreateElement("Value");
                pvalue.InnerText = parm.Value;

                XmlElement parmNode = doc.CreateElement("Parameter");
                parmNode.AppendChild(pname);
                parmNode.AppendChild(pvalue);

                parmRoot.AppendChild(parmNode);
            }

			xmlevent.AppendChild(type);
			xmlevent.AppendChild(activation);
			xmlevent.AppendChild(dir);
			xmlevent.AppendChild(loc);
			xmlevent.AppendChild(size);
			xmlevent.AppendChild(parmRoot);

			return xmlevent;
		}

		public Dictionary<string, string> LoadParametersFromXml(XmlNode root)
		{
			var parameters = new Dictionary<string, string>();

			foreach (XmlNode parameter in root.ChildNodes)
			{
				String name = "";
				String value = "";

				foreach (XmlNode child in parameter.ChildNodes)
				{
					switch (child.Name)
					{
						case "Name": name = child.InnerText; break;
						case "Value": value = child.InnerText; break;
					}
				}

				if (!String.IsNullOrEmpty(name))
					parameters.Add(name, value);
			}

			return parameters;
		}
		#endregion
    }
}
