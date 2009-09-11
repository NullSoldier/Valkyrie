using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ValkyrieLibrary.Core;

namespace ValkyrieLibrary.Camera
{
	public class TweenEffect
		: ICameraEffect
	{
		public TweenEffect(BaseCamera camera, ScreenPoint endpoint, int time)
			: this(new ScreenPoint((int)camera.MapOffset.X * -1, (int)camera.MapOffset.Y * -1), endpoint, time)
		{
		}

		public TweenEffect(ScreenPoint startpoint, ScreenPoint endpoint, int time)
		{
			this.StartLocation = startpoint;
			this.EndLocation = endpoint;
			this.CurrentLocation = new ScreenPoint(this.StartLocation.X, this.StartLocation.Y);

			this.Time = time;
			this.Finished = (endpoint.X == startpoint.X && endpoint.Y == startpoint.Y);				
		}

		private ScreenPoint StartLocation { get; set; }
		private ScreenPoint EndLocation { get; set; }
		private ScreenPoint CurrentLocation { get; set; }
		private float Time { get; set; }

		#region ICameraEffect Members

		public bool Finished { get; set; }

		private float TotalTimeUpdated = 0;

		public void Update(GameTime time)
		{
			this.TotalTimeUpdated += time.ElapsedGameTime.Milliseconds;

			float newx = (this.TotalTimeUpdated / this.Time) * (float)(this.EndLocation.X - this.StartLocation.X);
			float newy = (this.TotalTimeUpdated / this.Time) * (float)(this.EndLocation.Y - this.StartLocation.Y);

			this.CurrentLocation.X = this.StartLocation.X + (int)newx;
			this.CurrentLocation.Y = this.StartLocation.Y + (int)newy;

			if (this.TotalTimeUpdated > this.Time)
			{
				this.Finished = true;
			}
		}

		public void Draw(BaseCamera camera)
		{
			camera.MapOffset = new Vector2(this.CurrentLocation.X * -1, this.CurrentLocation.Y * -1);
		}

		#endregion
	}
}
