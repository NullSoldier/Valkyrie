using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Valkyrie.Engine.Camera;
using Valkyrie.Engine;
using Valkyrie.Engine.Core;

namespace Valkyrie.Library.Camera
{
	public class QuakeEffect
		: ICameraEffect
	{
		public QuakeEffect(BaseCamera camera, int magnitude, int time, int delay)
		{
			this.Magnitude = magnitude;
			this.Time = time;
			this.SpeedDelay = delay;
            this.OriginalOffset = camera.Location;

			this.Finished = false;
		}

		private int Magnitude { get; set; }
		private int Time { get; set; }
		private int SpeedDelay { get; set; }

		private int LastTimeQuake { get; set; }
		private int TotalQuakeTime { get; set; }

        private ScreenPoint CurrentOffset { get; set; }
        private ScreenPoint OriginalOffset
		{
			get { return this.originaloffset; }
			set
			{
				this.CurrentOffset = value;
				this.originaloffset = value;
			}
		}

        private ScreenPoint originaloffset;

		#region ICameraEffect Members

		public bool Finished { get; set; }

		public void Update(GameTime time)
		{
			this.LastTimeQuake += time.ElapsedGameTime.Milliseconds;
			this.TotalQuakeTime += time.ElapsedGameTime.Milliseconds;

			Random rand = new Random(DateTime.Now.Millisecond);

			if (this.LastTimeQuake > this.SpeedDelay)
			{
                this.CurrentOffset = new ScreenPoint(this.OriginalOffset.X + rand.Next(this.Magnitude * -1, this.Magnitude),
					this.OriginalOffset.Y + rand.Next(this.Magnitude * -1, this.Magnitude));

				this.LastTimeQuake = 0;
			}

			if(this.TotalQuakeTime > this.Time)
			{
				this.Finished = true;
			}
		}

		public void Draw(BaseCamera camera)
		{
            camera.Location = CurrentOffset;
		}

		#endregion
	}
}
