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
        public static frmMain MainForm = null;

        public static bool IgnoreInput = false;
        public static double Scale{get { return scale; }}

        public static World CurWorld { get { return TileEngine.WorldManager.CurrentWorld; } }
        public static Map CurMap { get { return curMap; } }

        private static Map curMap = null;
        private static double scale = 1.0;

        public static List<MapHeader> SelectedMaps= new List<MapHeader>();


        public static void SetScale(double scale)
        {
            WorldEditor.scale = scale;
        }

        public static void LoadUniverse(FileInfo UniLocation)
        {
            TileEngine.WorldManager.CleanUp();

            TileEngine.Configuration["MapRoot"] = UniLocation.Directory.Parent.FullName + "\\Maps";
            TileEngine.Configuration["GraphicsRoot"] = UniLocation.Directory.Parent.FullName + "\\Graphics";
            TileEngine.TextureManager.TextureRoot = TileEngine.Configuration["GraphicsRoot"];

            TileEngine.WorldManager.Load(UniLocation);
            MainForm.RefreshWorldList(TileEngine.WorldManager);
        }

        public static void NewUniverse(String firstWorldName)
        {
            AddWorld(firstWorldName);
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

        public static void AddWorld(String name)
        {
            TileEngine.WorldManager.CleanUp();

            World w = new World();
            w.Name = name;

            TileEngine.WorldManager.WorldsList.Add(name, w);
            MainForm.RefreshWorldList(TileEngine.WorldManager);
        }

        public static void AddMap(FileInfo fileInfo)
        {
            if (WorldEditor.CurWorld == null)
                return;

            MapPoint size = WorldEditor.CurWorld.WorldSize;
            MapPoint spawn = new MapPoint(0,0);

            if (size.X > size.Y)
            {
                spawn.Y = size.Y;
            }
            else
            {
                spawn.X = size.X;
            }

            MapHeader header = new MapHeader(fileInfo.Name, fileInfo.FullName, spawn);
            WorldEditor.CurWorld.MapList.Add(header.MapName, header);
            WorldEditor.CurWorld.CalcWorldSize();
            MainForm.UpdateScrollBars();
            MainForm.RefreshWorldProp(WorldEditor.CurWorld);
        }


    }
}
