using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ValkyrieWorldEditor.Core;

namespace ValkyrieWorldEditor.Core
{
    public partial class XNARenderControl : UserControl
    {
        public delegate void PaintFunction(XNARenderControl ctrl);
        private PaintFunction paintMe;

        private Vector4 clearColor;
        public Vector4 ClearColor
        {
            get { return clearColor; }
        }

        private RenderTarget2D renderTarget;
        public RenderTarget2D RenderTarget
        {
            get { return renderTarget; }
        }

        private DepthStencilBuffer depthStencil;
        public DepthStencilBuffer DepthStencil
        {
            get { return depthStencil; }
        }

        private Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
	
        public Rectangle ClientArea
        {
            get { return new Rectangle(0, 0, Width, Height); }
        }

        private GraphicsDevice gfxDevice;

        public XNARenderControl()
        {
            InitializeComponent();
            this.Resize += new System.EventHandler(XNARenderControl_Resize);
        }

        void XNARenderControl_Resize(object sender, System.EventArgs e)
        {
            if (gfxDevice != null)
            {
                renderTarget = new RenderTarget2D(gfxDevice, Width, Height, 1, SurfaceFormat.Color);
                depthStencil = new DepthStencilBuffer(gfxDevice, Width, Height, DepthFormat.Depth24);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if(paintMe != null)
                paintMe(this);
        }
        
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }
        
        public void Initialize(GraphicsDevice gfxDevice, PaintFunction paintFct, Vector4 clearColor)
        {
            this.paintMe = paintFct;
            this.clearColor = clearColor;

            this.gfxDevice = gfxDevice;

            renderTarget = new RenderTarget2D(gfxDevice, Width, Height, 1, SurfaceFormat.Color);
            depthStencil = new DepthStencilBuffer(gfxDevice, Width, Height, DepthFormat.Depth24);

            Initialize(gfxDevice);
        }

        public virtual void Draw(GraphicsDevice gfxDevice, SpriteBatch spriteBatch)
        {

        }

        public virtual void Update(float delta)
        {

        }

        protected virtual void Initialize(GraphicsDevice gfxDevice)
        {

        }
    }
}
