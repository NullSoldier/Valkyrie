using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ValkyrieLibrary.Core;
using ValkyrieLibrary.Maps;
using ValkyrieLibrary;
using ValkyrieWorldEditor.Forms;

namespace ValkyrieWorldEditor.Core
{
    public static class WorldEditor
    {
        public static bool IgnoreInput = false;
        public static double Scale{get { return scale; }}
     
        public static EditorXNA Game;
        public static frmMain MainForm;

        public static World CurWorld { get { return TileEngine.WorldManager.CurrentWorld; } }
        public static Map CurMap { get { return curMap; } }

        private static Map curMap = null;
        private static double scale = 1.0;

        public static void SetScale(double scale)
        {
            WorldEditor.scale = scale;
            TileEngine.Camera.Scale(scale);
        }

        public static void LoadUniverse(FileInfo UniLocation)
        {
            TileEngine.Configuration["MapRoot"] = UniLocation.Directory.Parent.FullName + "\\Maps";
            TileEngine.Configuration["GraphicsRoot"] = UniLocation.Directory.Parent.FullName + "\\Graphics";
            TileEngine.TextureManager.TextureRoot = TileEngine.Configuration["GraphicsRoot"];

            TileEngine.WorldManager.Load(UniLocation);
            MainForm.RefreshWorldList(TileEngine.WorldManager);
        }

        //this gets call via the main form thus the string
        public static void SetCurWorld(String worldName)
        {
            TileEngine.WorldManager.SetWorld(worldName, "Default");
            MainForm.RefreshWorldProp(WorldEditor.CurWorld);
        }

        //this gets called via the game engine thus the map
        public static void SetCurMap(Map map)
        {
            WorldEditor.curMap = map;
            MainForm.RefreshMapProp(WorldEditor.curMap);
        }


    }
}
