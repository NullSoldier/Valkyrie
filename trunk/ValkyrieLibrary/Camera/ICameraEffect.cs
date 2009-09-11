using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ValkyrieLibrary.Camera
{
	public interface ICameraEffect
	{
		void Update(GameTime time);
		void Draw(BaseCamera camera);

		bool Finished { get; set; }
	}

	public class EffectFinishedEventArgs
		: EventArgs
	{
		public EffectFinishedEventArgs(ICameraEffect effect)
		{
			this.Effect = effect;
		}

		public ICameraEffect Effect { get; set; }
	}
}
