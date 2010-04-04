using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Animation;

namespace Valkyrie.Engine.Core.Characters
{
	public interface IAnimatable
	{
		String CurrentAnimationName { get; set; }
		FrameAnimation CurrentAnimation { get; }
		bool IsAnimating { get; set; }
		object AnimationTag { get; set; }
		
		bool ContainsAnimation (string name);
		bool ContainsAnimation (FrameAnimation animation);
		void AddAnimation (string name, FrameAnimation animation);
		bool RemoveAnimation (string name);
	}
}
