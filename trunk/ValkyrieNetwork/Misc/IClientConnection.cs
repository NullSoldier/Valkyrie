using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Gablarski
{
	/// <summary>
	/// Contract for a client -> server connection.
	/// </summary>
	public interface IClientConnection
		: IConnection
	{
		/// <summary>
		/// The client has succesfully connected to the end point.
		/// </summary>
		event EventHandler Connected;

		/// <summary>
		/// Connects to <paramref name="endpoint"/>.
		/// </summary>
		/// <param name="endpoint">The endpoint to connect to.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="endpoint"/> is <c>null</c>.</exception>
		void Connect (IPEndPoint endpoint);
	}
}