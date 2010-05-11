using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Valkyrie.Engine.Maps;
using Valkyrie.Engine.Characters;
using Cadenza.Collections;
using System.Collections.ObjectModel;
using Valkyrie.Engine.Core.Scene;
using Valkyrie.Engine.Managers;

namespace Valkyrie.Engine.Providers
{
	public interface ISceneProvider : IEngineProvider
	{
		/// <summary>
		/// Begins the scene with the specified camera
		/// </summary>
		/// <param name="cameraid">The name of the camera to render the scene to.</param>
		/// <exception cref="ArgumentException">The camera with the name <paramref name="cameraname"/> does not exist.</exception>
		void BeginScene(string cameraid);

		/// <summary>
		/// Begins the scene with the specified camera
		/// </summary>
		/// <param name="camera">The camera to render the scene to.</param>
		/// <exception cref="ArgumentNullException">The camera is null.</exception>
		void BeginScene(BaseCamera camera);
		
		/// <summary>
		/// Ends the scene
		/// </summary>
		void EndScene();

		/// <summary>
		/// Update the current <seealso cref="ISceneProvider"/>.
		/// </summary>
		/// <param name="gameTime">The current gameTime.</param>
		void Update (GameTime gameTime);

		/// <summary>
		/// Draw all components of the current SceneProvider
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch to render to.</param>
		void Draw (RenderFlags flags);

		/// <summary>
		/// Draw a layer of the current camera
		/// </summary>
		/// <param name="layer">The layer to draw.</param>
		void DrawLayer (MapLayers layer);

		/// <summary>
		/// Draw a specific player.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch to render to.</param>
		/// <param name="playername">The name of the player to draw.</param>
		/// <param name="camera">The camera to draw to.</param>
		/// <exception cref="ArgumentException">A player with the <paramref name="playername"/> does not exist.</exception>
		void DrawPlayer (string playername);

		/// <summary>
		/// Returns the MapHeader of the players current location
		/// </summary>
		/// <param name="basecharacter">The basecharacter to find the current map for.</param>
		/// <returns>The map the <paramref name="basecharacter"/> is located on.</returns>
		MapHeader GetPositionableLocalMap(BaseCharacter basecharacter);

		IPlayerManager<BaseCharacter> Players { get; }
		ICameraManager<BaseCamera> Cameras { get; }
		IRendererManager Renderers { get; }
		IFogRenderer FogRenderer { get; }
	}
}
