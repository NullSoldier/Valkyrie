using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Valkyrie.Engine.Core.Characters;

namespace Valkyrie.Engine.Core.Scene
{
    public interface IRenderer
    {
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);

		IRenderable Drawable { get; }
    }
}
