using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ValkyrieLibrary.Animation
{
	public class FrameAnimation
	{
		#region Constructors
		public FrameAnimation(Rectangle initialframe, int framecount)
			: this (initialframe, framecount, 0.2f) { }

		public FrameAnimation(int x, int y, int width, int height, int framecount)
			: this (new Rectangle(x, y, width, height), framecount) { }

		public FrameAnimation(int x, int y, int width, int height, int framecount, float framelength)
			: this (new Rectangle(x, y, width, height), framecount, framelength) { }

		public FrameAnimation(Rectangle initialframe, int framecount, float framelength)
		{
			this.InitialFrameRect = initialframe;
			this.FrameCount = framecount;
			this.FrameLength = framelength;

			this.CurrentFrame = 0;
			this.SinceLastFrame = 0;
		}
		#endregion

		public int FrameCount
		{
			get;
			set;
		}

		public int CurrentFrame
		{
			get;
			set;
		}

		public float FrameLength
		{
			get;
			set;
		}

		public float SinceLastFrame
		{
			get;
			set;
		}

		public Rectangle InitialFrameRect
		{
			get;
			set;
		}

		public Rectangle FrameRectangle
		{
			get
			{
				return new Rectangle(this.InitialFrameRect.X + (this.CurrentFrame * this.InitialFrameRect.Width),
					this.InitialFrameRect.Y, this.InitialFrameRect.Width, this.InitialFrameRect.Height);
			}
		}

		public void Update(GameTime gameTime)
		{
			this.SinceLastFrame += (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (this.SinceLastFrame > this.FrameLength)
			{
				this.SinceLastFrame = 0;
				
				this.CurrentFrame++;
				if (this.CurrentFrame >= this.FrameCount)
					this.CurrentFrame = 0;
			}
		}
	
	}
}
