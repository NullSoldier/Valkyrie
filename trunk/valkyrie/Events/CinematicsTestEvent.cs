using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Valkyrie.Core;
using Microsoft.Xna.Framework;
using Valkyrie.Engine.Events;
using Valkyrie.Engine.Characters;

namespace Valkyrie.Events
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

		public string GetStringType ()
		{
			return "Cinematics";
		}

		public void Trigger (BaseCharacter character)
		{
			//if(!(TileEngine.Camera is PokeCamera))
			//    return;

			////TileEngine.Camera.Scale(0.9);
			//character.IgnoreMoveInput = true;

			//Thread thread = new Thread((ParameterizedThreadStart)this.StartCinematics);
			//thread.IsBackground = false;
			//thread.Name = "Event Cinematics Test Thread";
			//thread.Start(character);
		}

		public IEnumerable<string> GetParameterNames ()
		{
			return new List<string>();
		}

		#endregion

		#region ICloneable Members

		public object Clone ()
		{
			CinematicsTestEvent clone = new CinematicsTestEvent();
			clone.Rectangle = this.Rectangle;
			clone.Activation = this.Activation;
			clone.Direction = this.Direction;
			clone.Parameters = this.Parameters;

			return clone;
		}

		#endregion

		private int stage = 0;
		private BaseCharacter character;
		//private PokeCamera camera;

		private void StartCinematics (object argument)
		{
			//this.stage = 0;
			//this.character = (BaseCharacter)argument;
			//this.camera = (PokeCamera)TileEngine.Camera;

			//camera.ManualControl = true;
			//camera.EffectFinished += this.Camera_EffectFinished;
			//camera.Tween(new ScreenPoint(((int)camera.MapOffset.X * -1) + 280, (int)(camera.MapOffset.Y * -1) - 2000), 10000);
		}

		private void SecondStage ()
		{
			//camera.Quake(50, 8000, 20);
		}

		private void FinishCinematic ()
		{
			//this.camera.ManualControl = false;
			this.character.IgnoreMoveInput = false;
		}

		//private void Camera_EffectFinished (object sender, EffectFinishedEventArgs e)
		//{
		//    //PokeCamera camera = (PokeCamera)sender;

		//    //if(this.stage == 0 && (e.Effect is TweenEffect))
		//    //{
		//    //    stage++;
		//    //    Thread.Sleep(5000);
		//    //    this.SecondStage();
		//    //}

		//    //if(this.stage == 1 && (e.Effect is QuakeEffect))
		//    //    this.FinishCinematic();
		//}
	}
}
