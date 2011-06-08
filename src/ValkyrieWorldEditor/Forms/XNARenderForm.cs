using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ValkyrieWorldEditor.Core
{
    public class PerformanceCounter
    {
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        public static extern bool QueryPerformanceCounter(out long perfcount);

        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        public static extern bool QueryPerformanceFrequency(out long freq);

        public static long QueryPerformanceCounter()
        {
            long perfcount;
            QueryPerformanceCounter(out perfcount);
            return perfcount;
        }

        public static long QueryPerformanceFrequency()
        {
            long freq;
            QueryPerformanceFrequency(out freq);
            return freq;
        }
    }

    public class XNARenderForm : Form
    {
        class GfxService : IGraphicsDeviceService
        {
            GraphicsDevice gfxDevice;

            public GfxService(GraphicsDevice gfxDevice)
            {
                this.gfxDevice = gfxDevice;
                DeviceCreated = new EventHandler(DoNothing);
                DeviceDisposing = new EventHandler(DoNothing);
                DeviceReset = new EventHandler(DoNothing);
                DeviceResetting = new EventHandler(DoNothing);
            }

            public GraphicsDevice GraphicsDevice
            { get { return gfxDevice; } }

            public event EventHandler DeviceCreated;
            public event EventHandler DeviceDisposing;
            public event EventHandler DeviceReset;
            public event EventHandler DeviceResetting;

            void DoNothing(object o, EventArgs args)
            {
            }
        }

        public List<XNARenderControl> XNARenderControls { get; set; }

        GraphicsDevice gfxDevice;
        DepthStencilBuffer defaultDepthStencil;
        SpriteBatch copySprite;



        bool wireframe = false;
        long lastTimeCount = 0;

        public XNARenderForm()
        {
            CreateDevice();
            copySprite = new SpriteBatch(gfxDevice);
            defaultDepthStencil = gfxDevice.DepthStencilBuffer;

            //bigView.Initialize(gfxDevice, new XnaView.PaintFunction(Blit), new Vector4(0.5f, 0.5f, 1.0f, 1.0f));
            //smallView.Initialize(gfxDevice, new XnaView.PaintFunction(Blit), new Vector4(0.5f, 0.5f, 0.5f, 1.0f));

            // attach game loop
            Application.Idle += new EventHandler(Application_Idle);
            lastTimeCount = PerformanceCounter.QueryPerformanceCounter();

            XNARenderControls = new List<XNARenderControl>();
        }

        protected void InitRenderControls()
        {
            foreach (var rc in XNARenderControls)
            {
                rc.Initialize(gfxDevice, new XNARenderControl.PaintFunction(Blit), new Vector4(0.5f, 0.5f, 1.0f, 1.0f));
            }
        }

        private void CreateDevice()
        {
            PresentationParameters presentation = new PresentationParameters();
            presentation.AutoDepthStencilFormat = DepthFormat.Depth24;
            presentation.BackBufferCount = 1;
            presentation.BackBufferFormat = SurfaceFormat.Color;
            presentation.BackBufferWidth = 2000;
            presentation.BackBufferHeight = 2000;
            presentation.DeviceWindowHandle = this.Handle;
            presentation.EnableAutoDepthStencil = true;
            presentation.FullScreenRefreshRateInHz = 0;
            presentation.IsFullScreen = false;
            presentation.MultiSampleQuality = 0;
            presentation.MultiSampleType = MultiSampleType.None;
            presentation.PresentationInterval = PresentInterval.One;
            presentation.PresentOptions = PresentOptions.None;
            presentation.SwapEffect = SwapEffect.Discard;
            presentation.RenderTargetUsage = RenderTargetUsage.DiscardContents;

            gfxDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, DeviceType.Hardware, this.Handle, presentation);
        }

        private void Blit(XNARenderControl viewCtrl)
        {
            if (viewCtrl.Texture == null)
            {
                ClearOptions options = ClearOptions.Target | ClearOptions.DepthBuffer;
                Vector4 clearColor = new Vector4(1, 0, 0, 1);
                float depth = 1;
                int stencil = 128;
                gfxDevice.Clear(options, clearColor, depth, stencil);
            }

            else
            {
                gfxDevice.RenderState.FillMode = FillMode.Solid;

                copySprite.Begin();
                copySprite.Draw(viewCtrl.Texture, Vector2.Zero, Microsoft.Xna.Framework.Graphics.Color.White);
                copySprite.End();
            }

            gfxDevice.Present(viewCtrl.ClientArea, null, viewCtrl.Handle);
        }

        private delegate void DrawView(GraphicsDevice gfxDevice, SpriteBatch spriteBatch);

        private void RenderToTexture(XNARenderControl viewCtrl, DrawView drawFunction)
        {
            gfxDevice.SetRenderTarget(0, viewCtrl.RenderTarget);
            gfxDevice.DepthStencilBuffer = viewCtrl.DepthStencil;

            ClearOptions options = ClearOptions.Target | ClearOptions.DepthBuffer;
            Vector4 clearColor = viewCtrl.ClearColor;
            float depth = 1;
            int stencil = 128;
            gfxDevice.Clear(options, clearColor, depth, stencil);

            // Draw some stuff HERE
            drawFunction(gfxDevice, copySprite);

            //            gfxDevice.ResolveRenderTarget(0);
            gfxDevice.SetRenderTarget(0, null);
            gfxDevice.DepthStencilBuffer = defaultDepthStencil;

            viewCtrl.Texture = viewCtrl.RenderTarget.GetTexture();
        }


        private void Draw()
        {
            gfxDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
            gfxDevice.RenderState.DepthBufferEnable = true;
            gfxDevice.RenderState.DepthBufferFunction = CompareFunction.LessEqual;
            gfxDevice.RenderState.DepthBufferWriteEnable = true;
            gfxDevice.RenderState.FillMode = wireframe ? FillMode.WireFrame : FillMode.Solid;

            foreach (var rControl in XNARenderControls)
            {
                RenderToTexture(rControl, rControl.Draw);
                Blit(rControl);
            }
        }

        float deltaFPSTime = 0;

        private void Update(float deltaTime)
        {
            float elapsed = (float)deltaTime;

            float fps = 1 / elapsed;
            deltaFPSTime += elapsed;
            if (deltaFPSTime > 1)
            {
                this.Text = "WorldEditor  [" + fps.ToString() + " FPS]";
                deltaFPSTime -= 1;
            }

            foreach (var rControl in XNARenderControls)
            {
                rControl.Update(deltaTime);
            }
        }

        //
        // Game Loop stuff
        // from http://blogs.msdn.com/tmiller/archive/2005/05/05/415008.aspx
        //
        [StructLayout(LayoutKind.Sequential)]
        public struct Message
        {
            public IntPtr hWnd;
            public Int32 msg; // was WindowMessage
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public System.Drawing.Point p;
        }

        [System.Security.SuppressUnmanagedCodeSecurity] // We won't use this maliciously
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool PeekMessage(out Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);

        void Application_Idle(object sender, EventArgs e)
        {
            while (AppStillIdle)
            {
                long newCount = PerformanceCounter.QueryPerformanceCounter();
                long elapsedCount = newCount - lastTimeCount;
                double elapsedSeconds = (double)elapsedCount / PerformanceCounter.QueryPerformanceFrequency();
                lastTimeCount = newCount;

                Update((float)elapsedSeconds);
                Draw();
            }
        }

        protected bool AppStillIdle
        {
            get
            {
                /*NativeMethods.*/
                Message msg;
                return !/*NativeMethods.*/PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
            }
        }
        //
        // end Game Loop stuff
        //
    }
}

