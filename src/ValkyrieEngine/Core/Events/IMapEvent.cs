using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;
using Valkyrie.Engine.Characters;

namespace Valkyrie.Engine.Events
{
	public interface IMapEvent
		: ICloneable
	{
		// Properties
		Rectangle Rectangle { get; set; }
		ActivationTypes Activation { get; set; }
		Directions Direction { get; set; }
		Dictionary<String, String> Parameters { get; set; }
	
		// Methods
		string GetStringType();
		void Trigger(BaseCharacter character, IEngineContext context);
		
		IEnumerable<string> GetParameterNames();
	}
}

#region Old Code
/*
		#region Constructors
		public MapEvent(MapPoint loc)
			: this(loc, new MapPoint(1, 1), string.Empty, Directions.Any) { }

		public MapEvent(MapPoint loc, MapPoint size)
			: this(loc, size, string.Empty, Directions.Any) { }

		public MapEvent(MapPoint loc, MapPoint size, string type)
			: this(loc, size, type, Directions.Any) { }

		public MapEvent(MapPoint location, MapPoint size, string type, Directions direction)
		{
			this.Parameters = new Dictionary<String, String>();

			this.Location = location;
			this.Size = size;
			this.Type = type;
			this.Direction = direction;
		}

		public MapEvent(XmlNode node)
		{
			Parameters = new Dictionary<String, String>();

			foreach (XmlNode cnode in node.ChildNodes)
			{
				switch (cnode.Name)
				{
					case "Type": Type = cnode.InnerText; break;
					case "Dir": Direction = (Directions)Enum.Parse(typeof(Directions), cnode.InnerText); break;
					case "Parameters": LoadParms(cnode); break;
					case "Location": Location = new MapPoint(cnode); break;
					case "Size": Size = new MapPoint(cnode); break;
				}
			}
		}
		#endregion

        public void LoadParms(XmlNode root)
        {
            foreach (XmlNode parameter in root.ChildNodes)
            {
                String name = "";
                String type = "";

                foreach (XmlNode child in parameter.ChildNodes)
                {
                    switch (child.Name)
                    {
                        case "Name": name = child.InnerText; break;
                        case "Type": type = child.InnerText; break;
                    }
                }

                if (name != "" && type != "")
                    Parameters.Add(name, type);
            }
        }

        public void Copy(MapEvent e)
        {
            this.Type = e.Type;
            this.Location = e.Location;
            this.Size = e.Size;

            foreach (var parm in e.Parameters.Keys)
            {
                this.Parameters.Add(parm, e.Parameters[parm]);
            }
        }

        public Rectangle ToRect()
        {
            return new Rectangle(Location.X, Location.Y, Size.X, Size.Y);
        }

        public bool IsSameFacing(Directions facing)
        {
			return (this.Direction == Directions.Any || facing == this.Direction);
        }

        public void toXml(XmlDocument doc, XmlElement parent)
        {
            XmlElement loc = doc.CreateElement("Location");
            Location.ToXml(doc, loc);

            XmlElement size = doc.CreateElement("Size");
            Size.ToXml(doc, size);

            XmlElement type = doc.CreateElement("Type");
            type.InnerText = Type;

            XmlElement dir = doc.CreateElement("Dir");
            dir.InnerText = Direction.ToString();

            XmlElement parmRoot = doc.CreateElement("Parameters");

            foreach (var parm in this.Parameters.Keys)
            {
                XmlElement pname = doc.CreateElement("Name");
                pname.InnerText = parm;

                XmlElement ptype = doc.CreateElement("Type");
                ptype.InnerText = this.Parameters[parm];

                XmlElement parmNode = doc.CreateElement("Parameter");
                parmNode.AppendChild(pname);
                parmNode.AppendChild(ptype);

                parmRoot.AppendChild(parmNode);
            }

            parent.AppendChild(loc);
            parent.AppendChild(size);
            parent.AppendChild(type);
            parent.AppendChild(parmRoot);
            parent.AppendChild(dir);
        }
    }
		 */
#endregion