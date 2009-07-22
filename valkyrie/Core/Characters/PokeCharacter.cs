using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Characters;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ValkyrieLibrary.Core.Points;
using ValkyrieLibrary;

namespace Valkyrie.Core.Characters
{
	/// <summary>
	/// A class used for coupling the basic logic for any character in Pokemon Online
	/// </summary>
	class PokeCharacter : BaseCharacter
	{
		public Genders Gender;

		public float MoveDelay = 0.002f;
		public float LastMoveTime = 0;
		public int Speed = 2;

		public bool IsMoving = false;

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(this.Sprite, new Vector2(this.Location.X, this.Location.Y), Animations[this.CurrentAnimationName].FrameRectangle, Color.White);
		}

		public override BasePoint GetLookPoint()
		{
			BasePoint point = new BasePoint(0, 0);

			if (this.Direction == Directions.North)
				point = new BasePoint(0, -1);

			if (this.Direction == Directions.South)
				point = new BasePoint(0, 1);

			if (this.Direction == Directions.West)
				point = new BasePoint(-1, 0);

			if (this.Direction == Directions.East)
				point = new BasePoint(1, 0);

			return point;
		}

		public override void Move(ScreenPoint Destination)
		{
			if (this.IsMoving)
				return;

			TileEngine.EventManager.Movement(this);

			MapPoint point = Destination.ToMapPoint();
			
			// Convert to a map point and then back to a screen point which should result in the map points screenpoint origin
			this.MovingDestination = Destination.ToMapPoint().ToScreenPoint();

			// Clear the chunk when moving across boundries
			if (!TileEngine.CurrentMapChunk.TilePointInMapGlobal(new MapPoint(point.X, point.Y)))
				TileEngine.ClearCurrentMapChunk();

			this.IsMoving = true;
		}

		public override void DrawOverlay(SpriteBatch spriteBatch)
		{
			
		}

		public override void Action(string type)
		{
			throw new NotImplementedException();
		}

		public override void StopMoving()
		{
			this.IsMoving = false;
			this.MovingDestination = new ScreenPoint(0, 0); // eh?
			this.LastMoveTime = 0;
		}
	}
}
