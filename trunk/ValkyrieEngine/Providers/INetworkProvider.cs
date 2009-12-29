using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie;
using Valkyrie.Messages;

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
	}
}
