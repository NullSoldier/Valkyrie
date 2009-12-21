using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine.Core.Sound;
using Microsoft.Xna.Framework;
using Valkyrie.Engine;

namespace ValkyrieMapEditor.Providers
{
	public class MapEditorSoundProvider
		: ISoundProvider
	{
		#region ISoundProvider Members

		public void PlaySound (AudioSource sound, bool loop)
		{

		}

		public void PlayBGM (AudioSource sound, bool loop)
		{
			
		}

		public void StopBGM ()
		{
			
		}

		public void Update (GameTime gameTime)
		{
			
		}

		#endregion

		#region IEngineProvider Members

		public void LoadEngineContext (IEngineContext context)
		{
			
		}

		public bool IsLoaded
		{
			get { return true; }
		}

		#endregion
	}
}
