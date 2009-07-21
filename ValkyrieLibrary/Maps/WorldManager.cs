using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using ValkyrieLibrary.Core;
using ValkyrieLibrary.Maps;


namespace ValkyrieLibrary.Maps
{
    public class WorldManager
    {
        public Dictionary<String, World> WorldsList;
        public World CurrentWorld
        {
            get
            {
                return curWorld;
            }
        }

        private World curWorld;

        public WorldManager()
        {
            this.WorldsList = new Dictionary<String, World>();
            curWorld = null;
        }

        public void SetWorld(String Name, String startLoc)
        {
            if (this.WorldsList.ContainsKey(Name))
            {
                this.curWorld = this.WorldsList[Name];
                TileEngine.ClearCurrentMapChunk();
                TileEngine.Player.ReachedMoveDestination();
                TileEngine.Player.Location = this.curWorld.FindStartLocation(startLoc);
                TileEngine.Camera.CenterOnCharacter(TileEngine.Player);
            }
        }

        public void Load(FileInfo WorldConfiguration)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(WorldConfiguration.FullName);

            XmlNodeList worldNodes = doc.GetElementsByTagName("Worlds");
            foreach (XmlNode worldNode in worldNodes[0].ChildNodes)
            {
                World w = new World();

                foreach (XmlNode node in worldNode.ChildNodes)
                {
                    string name = string.Empty;
                    string path = string.Empty;
                    int x = 0, y = 0;

                    foreach (XmlNode subnode in node.ChildNodes)
                    {
                        if (subnode.Name == "Name")
                            name = subnode.InnerText;
                        else if (subnode.Name == "FilePath")
                            path = subnode.InnerText;
                        else if (subnode.Name == "X")
                            x = Convert.ToInt32(subnode.InnerText);
                        else if (subnode.Name == "Y")
                            y = Convert.ToInt32(subnode.InnerText);
                    }

                    MapHeader header = new MapHeader(name, path, new MapPoint(x, y));
                    w.WorldList.Add(header.MapName, header);
                }

                var worldName = worldNode.Attributes.GetNamedItem("Name");
                this.WorldsList.Add(worldName.InnerText, w);
            }

            curWorld = WorldsList["Main"];
        }

    }
}
