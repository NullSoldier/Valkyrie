// Copyright (c) 2009, Eric Maupin
// All rights reserved.
//
// Redistribution and use in source and binary forms, with
// or without modification, are permitted provided that
// the following conditions are met:
//
// - Redistributions of source code must retain the above 
//   copyright notice, this list of conditions and the
//   following disclaimer.
//
// - Redistributions in binary form must reproduce the above
//   copyright notice, this list of conditions and the
//   following disclaimer in the documentation and/or other
//   materials provided with the distribution.
//
// - Neither the name of Gablarski nor the names of its
//   contributors may be used to endorse or promote products
//   or services derived from this software without specific
//   prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS
// AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
// WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
// INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH
// DAMAGE.

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