using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using ValkyrieLibrary.Characters;
using ValkyrieLibrary.Core;
using ValkyrieLibrary.Maps;
using System.Reflection;

namespace ValkyrieLibrary.Events
{
    public class MapEventManager
    {
		public readonly Dictionary<Map, List<BaseMapEvent>> events = new Dictionary<Map, List<BaseMapEvent>>();
		public Dictionary<string, Type> EventTypes = new Dictionary<string, Type>(); // string type to type

		public void AddEvent(BaseMapEvent mapevent)
		{
			this.events[TileEngine.CurrentMapChunk].Add(mapevent);
		}

		public void RemoveEvent(BaseMapEvent mapevent)
		{
			this.events[TileEngine.CurrentMapChunk].Remove(mapevent);
		}

		public void ClearEventCache()
		{
			this.events.Clear();
		}


		public bool HandleEvent(BaseCharacter player, ActivationTypes activation)
		{
			BasePoint pos = player.MapLocation;

			if (activation == ActivationTypes.Activate || activation == ActivationTypes.Collision)
				pos += player.GetLookPoint();

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
		public void LoadEventTypesFromAssemblies(Assembly[] assemblies)
		{
			Dictionary<String, Type> tmpTypes = new Dictionary<string, Type>();

			for (int i = 0; i < assemblies.Length; i++)
			{
				var types = assemblies[i].GetTypes().Where(p => p.IsSubclassOf(typeof(BaseMapEvent)));

				foreach (var mapevent in types)
				{
					var newObj = (BaseMapEvent)Activator.CreateInstance(mapevent);
					tmpTypes.Add(newObj.GetType(), mapevent);
				}
			}

			this.EventTypes = tmpTypes;		
		}

		public BaseMapEvent GetEventInRect(BasePoint loc, BasePoint size)
		{
			return this.GetEventInRect(new Rectangle(loc.X, loc.Y, size.X, size.Y));
		}

		public BaseMapEvent GetEventInRect(Rectangle rect)
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

		public BaseMapEvent GetEvent(MapPoint location)
		{
			return this.events[TileEngine.CurrentMapChunk].Where(ev => ev.Rectangle.Contains(location.ToPoint())).FirstOrDefault();
		}

		public void SetOrAddEvent(BaseMapEvent e)
		{
			if (this.events[TileEngine.CurrentMapChunk].Contains(e))
			{
				int index = this.events[TileEngine.CurrentMapChunk].IndexOf(e);

				this.events[TileEngine.CurrentMapChunk][index] = (BaseMapEvent)e.Clone();
			}
			else
				this.events[TileEngine.CurrentMapChunk].Add(e);
		}

		public void DelEvent(BaseMapEvent e)
		{
			this.events[TileEngine.CurrentMapChunk].Remove(e);
		}

		public void LoadEvents(Map map)
		{
			// Set the list reference in the dictionary to the maps event list
			this.events[map] = map.EventList;
		}

		// bleh

		public BaseMapEvent LoadEventFromXml(XmlNode node)
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

			if ( !this.EventTypes.ContainsKey(type) )
				throw new ArgumentException("Event type does not exist in the currently loaded assemblies. Did you load the engines event types from the assemblies using event manager method \"public void LoadEventTypesFromAssemblies(Assembly[] assemblies)\"?");

			BaseMapEvent newEvent = (BaseMapEvent)Activator.CreateInstance(this.EventTypes[type]);
			newEvent.Direction = dir;
			newEvent.Parameters = parameters;
			newEvent.Rectangle = new Rectangle(location.X, location.Y, size.X, size.Y);
			newEvent.Activation = activation;

			return newEvent;
		}

		public XmlNode EventToXmlNode(BaseMapEvent mapevent, XmlDocument doc)
		{
			XmlElement xmlevent = doc.CreateElement("Event");

			// Properties of event	
            XmlElement loc = doc.CreateElement("Location");
			new MapPoint(mapevent.Rectangle.X, mapevent.Rectangle.Y).ToXml(doc, loc);

            XmlElement size = doc.CreateElement("Size");
			new MapPoint(mapevent.Rectangle.Width, mapevent.Rectangle.Height).ToXml(doc, size);

            XmlElement type = doc.CreateElement("Type");
            type.InnerText = mapevent.GetType();

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
