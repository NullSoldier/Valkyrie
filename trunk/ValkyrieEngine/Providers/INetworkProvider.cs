using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie;
using Valkyrie.Messages;
using Valkyrie.Engine.Characters;

namespace Valkyrie.Engine.Providers
{
	public interface INetworkProvider
		: IEngineProvider
	{
		/// <summary>
		/// Fired when the connection has been established.
		/// </summary>
		event EventHandler Connected;

		/// <summary>
		/// Fired when the connection has been disconnected.
		/// </summary>
		event EventHandler<ConnectionEventArgs> Disconnected;

		/// <summary>
		/// Fired when a message has been received
		/// </summary>
		event EventHandler<MessageReceivedEventArgs> MessageReceived;

		/// <summary>
		/// Returns whether the INetworkProvider is connected and messages can be sent.
		/// </summary>
		bool IsConnected { get; }

		/// <summary>
		/// Establish a connection to a server
		/// </summary>
		/// <param name="server">The server hostname or IP address to connect to.</param>
		/// <param name="port">The port on the server to use.</param>
		/// <returns>True if connection was successful.</returns>
		bool Connect (string server, int port);

		/// <summary>
		/// Establish a connection to a server asynchronously
		/// </summary>
		/// <param name="server">The server hostname or IP address to connect to.</param>
		/// <param name="port">The port on the server to use.</param>
		/// <returns>True if connection was successful.</returns>
		void ConnectAsync (string server, int port);

		/// <summary>
		/// Disconnect from the currently connected server
		/// </summary>
		void Disconnect();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		void Send (MessageBase message);

		/// <summary>
		/// Gets a network player with a specified ID
		/// </summary>
		/// <param name="networkid">The network ID of the player</param>
		/// <returns>The player with the specified network ID. Returns null if a player with that network ID cannot be found.</returns>
		BaseCharacter GetPlayer (object networkid);

		/// <summary>
		/// Returns all network players
		/// </summary>
		/// <returns>A collection of all network players</returns>
		IEnumerable<BaseCharacter> GetPlayers ();

		/// <summary>
		/// Add a player to the provider
		/// </summary>
		/// <param name="networkid">The network ID of the player</param>
		/// <param name="player">The player to add</param>
		/// <exception cref="ArgumentException">A network player with the <paramref name="networkid"/> already exists.</exception>
		void AddPlayer (object networkid, BaseCharacter player);

		/// <summary>
		/// Removes a player from the provider
		/// </summary>
		/// <param name="networkid"></param>
		/// <returns>True if the player was removed, otherwise false.</returns>
		bool RemovePlayer (object networkid);

		/// <summary>
		/// Checks to see if a player is stored in the provider.
		/// </summary>
		/// <param name="networkid">The networkid of the player to find.</param>
		/// <returns>True of the player is contained in the provider.</returns>
		bool ContainsPlayer (object networkid);
	}
}
