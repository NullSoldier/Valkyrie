using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValkyrieLibrary.Characters;
using System.Threading;
using Valkyrie.Core;
using Microsoft.Xna.Framework;
using ValkyrieLibrary.Core;

namespace ValkyrieLibrary.Events
{
	public class CinematicsTestEvent
		: IMapEvent
	{

		#region IMapEvent Members

		public Rectangle Rectangle
		{
			get;
			set;
		}

		public ActivationTypes Activation
		{
			get;
			set;
		}

		public Directions Direction
		{
			get;
			set;
		}

		public Dictionary<string, string> Parameters
		{
			get;
			set;
		}

		public string GetStringType()
		{
			return "Cinematics";
		}

		public void Trigger(BaseCharacter character)
		{
			if (!(TileEngine.Camera is PokeCamera))
				return;

			TileEngine.Camera.Scale(0.9);
			//character.IgnoreMoveInput = true;

			/*Thread thread = new Thread((ParameterizedThreadStart)this.StartCinematics);
			thread.IsBackground = false;
			thread.Name = "Event Cinematics Test Thread";
			thread.Start(character);*/
		}

		public IEnumerable<string> GetParameterNames()
		{
			return new List<string>();
		}

		#endregion

		#region ICloneable Members

		public object Clone()
		{
			CinematicsTestEvent clone = new CinematicsTestEvent();
			clone.Rectangle = this.Rectangle;
			clone.Activation = this.Activation;
			clone.Direction = this.Direction;
			clone.Parameters = this.Parameters;

			return clone;
		}

		#endregion

		private void StartCinematics(object argument)
		{
			BaseCharacter character = (BaseCharacter)argument;

			PokeCamera camera = (PokeCamera)TileEngine.Camera;
			camera.ManualControl = true;

			camera.TweenFinished += this.Camera_TweenFinished;
			camera.Tween(character.Location, new ScreenPoint(character.Location.X + 15, character.Location.Y + 200), 5000);
		}

		private void Camera_TweenFinished(object sender, EventArgs e)
		{
			PokeCamera camera = (PokeCamera)sender;
			camera.ManualControl = false;
		}
	}
}
