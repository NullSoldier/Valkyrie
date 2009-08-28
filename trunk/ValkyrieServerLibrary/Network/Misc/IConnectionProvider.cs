using System;

namespace Gablarski
{
	public interface IConnectionProvider
	{
		/// <summary>
		/// A connectionless message was received.
		/// </summary>
		event EventHandler<MessageReceivedEventArgs> ConnectionlessMessageReceived;

		/// <summary>
		/// A connection was made.
		/// </summary>
		event EventHandler<ConnectionEventArgs> ConnectionMade;

		/// <summary>
		/// Starts listening for connections and connectionless messages.
		/// </summary>
		void StartListening ();

		/// <summary>
		/// Stops listening for connections and connectionless messages.
		/// </summary>
		void StopListening ();
	}

	/// <summary>
	/// Provides data for the <see cref="IConnectionProvider.ConnectionMade"/> event.
	/// </summary>
	public class ConnectionEventArgs
		: EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ConnectionEventArgs"/> class.
		/// </summary>
		/// <param name="connection">The connection that was made.</param>
		public ConnectionEventArgs (IConnection connection)
		{
			this.Connection = connection;
		}

		/// <summary>
		/// Gets the connection made.
		/// </summary>
		public IConnection Connection
		{
			get;
			private set;
		}
	}
}