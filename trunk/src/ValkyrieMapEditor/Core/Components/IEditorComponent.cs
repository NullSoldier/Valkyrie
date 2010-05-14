using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Valkyrie.Engine;

namespace ValkyrieMapEditor.Core
{
    public interface IEditorComponent
    {
        void OnSizeChanged(object sender, ScreenResizedEventArgs e);
        void OnScrolled(object sender, ScrollEventArgs e);
        void OnMouseDown(object sender, MouseEventArgs ev);
        void OnMouseMove(object sender, MouseEventArgs ev);
        void OnMouseUp(object sender, MouseEventArgs ev);
        void OnMouseClicked(object sender, MouseEventArgs ev);

        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);

		void LoadContent (GraphicsDevice graphicsDevice, IEngineContext context);
		
		void OnComponentActivated();
		void OnComponentDeactivated();
		void OnCut();
		void OnCopy();
		void OnPaste();
		void OnDelete();
    }

	public enum ComponentID
	{
		Draw,
		Events,
		Collsion,
		Select,
		Rectangle,
		Bucket
	};
}
