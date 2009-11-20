using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Valkyrie.Engine.Maps;
using Valkyrie.Engine.Characters;
using Mono.Rocks;

namespace Valkyrie.Engine.Providers
{
	public interface ISceneProvider
		: IEngineProvider
	{
		void Update (GameTime gameTime);
		MapHeader GetPositionableLocalMap (IPositionable positionable);

		void Draw (SpriteBatch spriteBatch);
		void DrawCamera (SpriteBatch spriteBatch, string cameraname);
		void DrawCameraLayer (SpriteBatch spriteBatch, string cameraname, MapLayers layer, MapHeader header);
		void DrawAllCameras (SpriteBatch spriteBatch);
		void DrawPlayer (SpriteBatch spriteBatch, string playername, BaseCamera camera);

		ReadOnlyDictionary<string, BaseCamera> Cameras { get; }
		void AddCamera (string name, BaseCamera camera);
		bool RemoveCamera (string name);

		ReadOnlyDictionary<string, BaseCharacter> Players { get; }
		void AddPlayer (string name, BaseCharacter character);
		bool RemoveCharacter (string name);
	}
}
