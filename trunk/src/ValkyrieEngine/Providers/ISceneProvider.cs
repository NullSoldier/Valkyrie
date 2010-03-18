using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Valkyrie.Engine.Maps;
using Valkyrie.Engine.Characters;
using Cadenza.Collections;

namespace Valkyrie.Engine.Providers
{
	public interface ISceneProvider
		: IEngineProvider
	{
		/// <summary>
		/// Update the current <seealso cref="ISceneProvider"/>.
		/// </summary>
		/// <param name="gameTime">The current gameTime.</param>
		void Update (GameTime gameTime);

		/// <summary>
		/// Returns the MapHeader of the players current location
		/// </summary>
		/// <param name="basecharacter">The basecharacter to find the current map for.</param>
		/// <returns>The map the <paramref name="basecharacter"/> is located on.</returns>
		MapHeader GetPositionableLocalMap (BaseCharacter basecharacter);


		/// <summary>
		/// Draw all components of the current SceneProvider
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch to render to.</param>
		void Draw (SpriteBatch spriteBatch);

		/// <summary>
		/// Draw a specific camera
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch to render to.</param>
		/// <param name="cameraname">The name of the camera to draw.</param>
		void DrawCamera (SpriteBatch spriteBatch, string cameraname);

		/// <summary>
		/// Draw a layer of a specified camera
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch to render to.</param>
		/// <param name="cameraname">The name of the camera to draw.</param>
		/// <param name="layer">The layer to draw.</param>
		/// <param name="players">Whether players should be rendered or not.</param>
		/// <exception cref="ArgumentException">The camera with the name <paramref name="cameraname"/> does not exist.</exception>
		void DrawCameraLayer (SpriteBatch spriteBatch, string cameraname, MapLayers layer, bool players);

		/// <summary>
		/// Draw all cameras in the current ISceneProvider
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch to render to.</param>
		void DrawAllCameras (SpriteBatch spriteBatch);

		/// <summary>
		/// Draw a specific player.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch to render to.</param>
		/// <param name="playername">The name of the player to draw.</param>
		/// <param name="camera">The camera to draw to.</param>
		/// <exception cref="ArgumentException">A player with the <paramref name="playername"/> does not exist.</exception>
		void DrawPlayer (SpriteBatch spriteBatch, string playername, BaseCamera camera);

		/// <summary>
		/// Add a camera to the ISceneProvider
		/// </summary>
		/// <param name="name">The name of the camera.</param>
		/// <param name="camera">The camera to add.</param>
		/// <exception cref="ArgumentException">A camera with the <paramref name="name"/> already exists.</exception>
		void AddCamera (string name, BaseCamera camera);

		/// <summary>
		/// Remove a camera from the ISceneProvider
		/// </summary>
		/// <param name="name">The name of the camera to remove.</param>
		/// <returns>True if the camera was removed.</returns>
		bool RemoveCamera (string name);

		/// <summary>
		/// Gets a camera from the ISceneProvider
		/// </summary>
		/// <param name="name">The name of the camera to get.</param>
		/// <returns>The camera with the specified <paramref name="name"/>.</returns>
		/// <exception cref="ArgumentException">When a camera with the specified <paramref name="name"/> is not found.</exception>
		BaseCamera GetCamera (string name);

		/// <summary>
		/// Get all cameras in the ISceneProvider
		/// </summary>
		/// <returns>A non-modifiable dictionary of all cameras.</returns>
		ReadOnlyDictionary<string, BaseCamera> GetCameras ();

		/// <summary>
		/// Add a player to the ISceneProvider.
		/// </summary>
		/// <param name="name">The name of the player.</param>
		/// <param name="character">The player to add.</param>
		/// <exception cref="ArgumentException">A player with the <paramref name="name"/> already exists.</exception>
		void AddPlayer (string name, BaseCharacter character);

		/// <summary>
		/// Remove a player from the ISceneProvider
		/// </summary>
		/// <param name="name">The name of the player to remove.</param>
		/// <returns>True if the player was removed.</returns>
		bool RemoveCharacter (string name);

		/// <summary>
		/// Gets a player from the ISceneProvider.
		/// </summary>
		/// <param name="name">The name of the player to get.</param>
		/// <returns>The player with the specified <paramref name="name"/>.</returns>
		/// <exception cref="ArgumentException">When a player with the specified <paramref name="name"/> is not found.</exception>
		BaseCharacter GetPlayer (string name);

		/// <summary>
		/// Gets all players in the ISceneProvider
		/// </summary>
		/// <returns>A non-modifiable dictionary of all players.</returns>
		ReadOnlyDictionary<string, BaseCharacter> GetPlayers ();
	}
}
