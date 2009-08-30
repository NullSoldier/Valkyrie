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
        public String FileName;
        public String FilePath;
        public Dictionary<String, World> WorldsList;

    	public World CurrentWorld { get; private set; }

    	public WorldManager()
        {
            this.WorldsList = new Dictionary<String, World>();
            this.CurrentWorld = null;
        }

        public void SetWorld(String name, String startLoc, bool UseStartLocation)
        {
        	if (!this.WorldsList.ContainsKey(name))
        		return;
			 
        	this.CurrentWorld = this.WorldsList[name];
        	TileEngine.ClearCurrentMapChunk();
        	//TileEngine.Player.StopMoving();

			if (UseStartLocation)
			{
				if (startLoc == null || startLoc == "Default")
					TileEngine.Player.Location = this.CurrentWorld.FindDefaultStartLocation();
				else
					TileEngine.Player.Location = this.CurrentWorld.FindStartLocation(startLoc);
			}

        	TileEngine.Camera.CenterOnCharacter(TileEngine.Player);
        }

        public void Load (FileInfo WorldConfiguration)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(WorldConfiguration.FullName);

            this.FileName = WorldConfiguration.Name;
            this.FilePath = WorldConfiguration.FullName;

            XmlNodeList worldNodes = doc.GetElementsByTagName("Worlds");
            foreach (XmlNode worldNode in worldNodes[0].ChildNodes)
            {
                World w = new World(worldNode);
                this.WorldsList.Add(w.Name, w);
            }
        }

        class tempDictTwo
        {
            public Dictionary<Map, tempStorage> dict = new Dictionary<Map, tempStorage>();
        }

        class tempStorage
        {
            public String oldTexture;
            public String oldPath;
        }

        public void Export(string dir)
        {
			DirectoryInfo exportDir = new DirectoryInfo(dir);

            Dictionary<World, tempDictTwo> dict = new Dictionary<World, tempDictTwo>();

        	var maps = exportDir.CreateSubdirectory("Maps");
        	var graphics = exportDir.CreateSubdirectory("Graphics");
        	var data = exportDir.CreateSubdirectory("Data");

            foreach (var world in this.WorldsList)
            {
            	maps.CreateSubdirectory(world.Value.Name);
            	graphics.CreateSubdirectory(world.Value.Name);

                tempDictTwo td = new tempDictTwo();

                foreach (var map in world.Value.MapList)
                {
                    tempStorage t = new tempStorage();
                    t.oldTexture = map.Value.Map.TextureName;
                    t.oldPath = map.Value.MapFileLocation;

                    td.dict.Add(map.Value.Map, t);

                    var pictStrings = map.Value.Map.TextureName.Split(new char[] { '\\', '.' });
                    string imageExt = pictStrings.Last();


                    map.Value.MapFileLocation = Path.Combine (world.Value.Name, map.Value.Map.Name + ".xml");
                    String newTexturePath = Path.Combine (world.Value.Name, map.Value.Map.Name + "." + imageExt);

                    File.Copy(Path.Combine (TileEngine.TextureManager.TextureRoot, map.Value.Map.TextureName), Path.Combine (graphics.FullName, newTexturePath), true);
                    map.Value.Map.TextureName = newTexturePath;

					map.Value.Map.Save(Path.Combine(maps.FullName, map.Value.MapFileLocation));
                }

                dict.Add(world.Value, td);
            }

        	Save(Path.Combine(data.FullName, this.FileName));

            foreach (var world in this.WorldsList)
            {
                foreach (var map in world.Value.MapList)
                {
                    map.Value.Map.TextureName = dict[world.Value].dict[map.Value.Map].oldTexture;
                    map.Value.MapFileLocation = dict[world.Value].dict[map.Value.Map].oldPath;
                }
            }
        }

        public void Save()
        {
            Save(this.FilePath);
        }

        private void Save(String path)
        {
            XmlDocument doc = new XmlDocument();

            XmlElement worlds = doc.CreateElement("Worlds");

            foreach (var w in this.WorldsList)
            {
                w.Value.SaveXml(worlds, doc);
            }

            doc.AppendChild(worlds);

            doc.Save( path );
        }

        public void CleanUp()
        {
			// I think this should be named Clear cache as a standard
            this.WorldsList.Clear();
            this.CurrentWorld = null;
        }
    }
}
