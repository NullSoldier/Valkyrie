using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine;
using Valkyrie.Engine.Core;
using Microsoft.Xna.Framework;

namespace Valkyrie.Library.Camera
{
	public class ValkyrieCamera
		: BaseCamera
	{
		#region Constructors

		public ValkyrieCamera (BaseCamera camera)
			: base(camera)
		{
		}

		public ValkyrieCamera (Rectangle screen)
			: base (screen)
		{
		}

		public ValkyrieCamera (int x, int y, int width, int height)
			: base (x, y, width, height)
		{
		}

		#endregion

		#region Effects

		/// <summary>
		/// Tween the camera between two ScreenPoints
		/// </summary>
		/// <param name="startPoint">The starting point of the camera.</param>
		/// <param name="endPoint">The ending point that the camera should tween to.</param>
		/// <param name="time">How long in milliseconds it should take to tween.</param>
		public void Tween (ScreenPoint startpoint, ScreenPoint endpoint, int time)
		{
			TweenEffect effect = new TweenEffect(startpoint, endpoint, time);

			lock(this.Effects)
			{
				this.Effects.Add(effect);
			}
		}

		/// <summary>
		/// Tweens the camera to a destination starting at the cameras current location
		/// </summary>
		/// <param name="endPoint">The ending point that the camera should tween to.</param>
		/// <param name="time">How long in milliseconds it should take to tween.</param>
		public void Tween (ScreenPoint endpoint, int time)
		{
			TweenEffect effect = new TweenEffect(this, endpoint, time);

			lock(this.Effects)
			{
				this.Effects.Add(effect);
			}
		}

		/// <summary>
		/// Shake the camera around it's current location
		/// </summary>
		/// <param name="Magnitude">A magnitude value of which the camera will be shaken.</param>
		/// <param name="time">How long in milliseconds it should quake for.</param>
		public void Quake (int magnitude, int time, int speeddelay)
		{
			QuakeEffect effect = new QuakeEffect(this, magnitude, time, speeddelay);

			lock(this.Effects)
			{
				this.Effects.Add(effect);
			}
		}

		//public void Scale (double scale)
		//{
		//    this.Scale(scale, scale);
		//}

		//public void Scale (double x, double y)
		//{
		//    this.Screen = new Rectangle(Screen.X, Screen.Y, (int)(Screen.Width * (1.0 / x)), (int)(Screen.Height * (1.0 / y)));
		//    //this.MapOffset = new Vector2(0, 0);

		//    this.Viewport.X = this.Screen.X;
		//    this.Viewport.Y = this.Screen.Y;
		//    this.Viewport.Width = this.Screen.Width;
		//    this.Viewport.Height = this.Screen.Height;
		//}

		#endregion
	}
}
