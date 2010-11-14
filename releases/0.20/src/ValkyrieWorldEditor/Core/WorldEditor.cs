using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Valkyrie.Library.Core;
using Valkyrie.Library.Maps;
using Valkyrie.Library;
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
        private static Dictionary<Map, Texture2D> imageList = new Dictionary<Map,Texture2D>();
        private static Dictionary<Map, Texture2D> imageSeclectList = new Dictionary<Map, Texture2D>();

        public static void SetScale(double scale)
        {
            WorldEditor.scale = scale;
        }

        public static void LoadUniverse(FileInfo UniLocation)
        {
            TileEngine.WorldManager.CleanUp();

            TileEngine.Configuration[TileEngineConfigurationName.MapRoot] = Path.Combine (UniLocation.Directory.Parent.FullName, "Maps");
            TileEngine.Configuration[TileEngineConfigurationName.GraphicsRoot] = Path.Combine (UniLocation.Directory.Parent.FullName, "Graphics");
            TileEngine.TextureManager.TextureRoot = TileEngine.Configuration[TileEngineConfigurationName.GraphicsRoot];

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
            TileEngine.WorldManager.SetWorld(worldName, "Default", false);
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


        public static void GenerateMapImages(GraphicsDevice gfxDevice)
        {
            if (TileEngine.WorldManager.CurrentWorld == null)
                return;

            foreach (var mapHead in TileEngine.WorldManager.CurrentWorld.MapList)
            {
                if (!WorldEditor.imageList.Keys.Contains(mapHead.Value.Map))
                {
                    Rectangle rect = (new ScreenPoint(0, 0)).ToRect(mapHead.Value.Map.MapSize.ToScreenPoint().ToPoint());

                    RenderTarget2D renderTarget = new RenderTarget2D(gfxDevice, rect.Width, rect.Height, 1, SurfaceFormat.Color);
                    DepthStencilBuffer depthStencil = new DepthStencilBuffer(gfxDevice, rect.Width, rect.Height, DepthFormat.Depth24);
                    SpriteBatch newBatch = new SpriteBatch(gfxDevice);

                    DepthStencilBuffer oldStencil = gfxDevice.DepthStencilBuffer;
                    RenderTarget2D oldTarget = (RenderTarget2D)gfxDevice.GetRenderTarget(0);

                    gfxDevice.SetRenderTarget(0, renderTarget);
                    gfxDevice.DepthStencilBuffer = depthStencil;

                    newBatch.Begin();
                    TileEngine.DrawMapLocal(newBatch, mapHead.Value);
                    newBatch.End();

                    gfxDevice.SetRenderTarget(0, null);
                    gfxDevice.SetRenderTarget(0, oldTarget);
                    gfxDevice.DepthStencilBuffer = oldStencil;

                    Texture2D newTexture = renderTarget.GetTexture();
                    WorldEditor.imageList.Add(mapHead.Value.Map, newTexture);
                }
            }
        }

        public static Texture2D GetMapImage(MapHeader mapHead)
        {
            if (WorldEditor.imageList.Keys.Contains(mapHead.Map))
                return WorldEditor.imageList[mapHead.Map];

            return null;
        }

        public static Texture2D GetMapSelectImage(GraphicsDevice gfxDevice, MapHeader mapHead)
        {
            if (!WorldEditor.imageSeclectList.Keys.Contains(mapHead.Map))
            {
                Texture2D img = RenderComponent.CreateSelectRectangle(gfxDevice, mapHead.Map.MapSize.ToScreenPoint().X, mapHead.Map.MapSize.ToScreenPoint().Y, new Color(0, 240, 255, 125));
                WorldEditor.imageSeclectList.Add(mapHead.Map, img);
            }

            return WorldEditor.imageSeclectList[mapHead.Map];;
        }

        
    }
}
