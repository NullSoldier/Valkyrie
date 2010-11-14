using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Valkyrie.Library
{
    public interface IModuleLibrary
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        void Load();
        void Unload();
        void Activate();
        void Deactivate();
    }
}
