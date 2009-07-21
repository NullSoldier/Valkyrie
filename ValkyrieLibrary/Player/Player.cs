using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ValkyrieLibrary.Animation;
using Microsoft.Xna.Framework.Graphics;
using ValkyrieLibrary.Core;

namespace ValkyrieLibrary.Characters
{
	public class Player : BaseCharacter
	{
        public MapPoint TileLocation
        {
            get { return new MapPoint(this.Location.X / TileEngine.CurrentMapChunk.TileSize.X, this.Location.Y / TileEngine.CurrentMapChunk.TileSize.Y); }
        }

        public MapPoint MapLocation
        {
            get { return TileEngine.GlobalTilePointToLocal(TileLocation); }
        }

		public Player()
		{
		
		}

		public Genders Gender;
		public int Level;
		public int Gold;

        public virtual void DrawOverlay(SpriteBatch spriteBatch)
        {

        }

		public override void Move(ScreenPoint Destination)
		{
			throw new NotImplementedException();
		}
	}
}
