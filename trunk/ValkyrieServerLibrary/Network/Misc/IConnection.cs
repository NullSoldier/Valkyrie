using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gablarski.Messages;

namespace Gablarski
{
	/// <summary>
	/// Contract for a connection in either direction.
	/// </summary>
	public interface IConnection
	{
		/// <summary>
		/// Gets whether the connection is active.
		/// </summary>
		bool IsConnected { get; }

		/// <summary>
		/// A message was received from the underlying transport.
		/// </summary>
		event EventHandler<MessageReceivedEventArgs> MessageReceived;

		/// <summary>
		/// The underlying transport has been disconnected.
		/// </summary>
		event EventHandler<ConnectionEventArgs> Disconnected;

		/// <summary>
		/// Sends <paramref name="message"/> to the other end of the connection.
		/// </summary>
		/// <param name="message">The message to send.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="message"/> is <c>null</c>.</exception>
		void Send (MessageBase message);

		/// <summary>
		/// Closes the connection.
		/// </summary>
		void Disconnect ();
	}

	public static class ConnectionExtensions
	{
		/// <summary>
		/// Sends <paramref name="message"/> to all connections in the enumeration.
		/// </summary>
		/// <param name="connections">The connections to send <paramref name="message"/> to.</param>
		/// <param name="message">The message to send.</param>
		public static void Send (this IEnumerable<IConnection> connections, MessageBase message)
		{
			#if DEBUG
			if (message == null)
				throw new ArgumentNullException ("message");
			#endif

			foreach (var connection in connections)
				connection.Send (message);
		}

		/// <summary>
		/// Sends <paramref name="message"/> to all connections in the enumeration predicated by <paramref name="predicate"/>.
		/// </summary>
		/// <param name="connections">The connections to send <paramref name="message"/> to.</param>
		/// <param name="message">The message to send.</param>
		/// <param name="predicate">The connection predicate.</param>
		public static void Send (this IEnumerable<IConnection> connections, MessageBase message, Func<IConnection, bool> predicate)
		{
			#if DEBUG
			if (message == null)
				throw new ArgumentNullException ("message");
			if (predicate == null)
				throw new ArgumentNullException ("predicate");
			#endif

			foreach (var connection in connections)
			{
				if (predicate (connection))
					connection.Send (message);
			}
		}
	}

	public class MessageReceivedEventArgs
		: ConnectionEventArgs
	{
		/// <param name="connection">The connection the message was received from.</param>
		/// <param name="message">The message received.</param>
		public MessageReceivedEventArgs (IConnection connection, MessageBase message)
			: base (connection)
		{
			#if DEBUG
			if (message == null)
				throw new ArgumentNullException ("message");
			#endif

			this.Message = message;
		}

		/// <summary>
		/// Gets the message that was received.
		/// </summary>
		public MessageBase Message
		{
			get;
			private set;
		}
	}
}