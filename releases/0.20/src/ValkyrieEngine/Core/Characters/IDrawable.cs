using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Valkyrie.Engine.Core.Characters
{
    public interface IRenderable
    {
        Texture2D Texture { get; set; }
		ScreenPoint Location { get; set; }
		Rectangle SourceRect { get; set; }
		float Scale { get; set; }
		float Rotation { get; set; }
    }
}
