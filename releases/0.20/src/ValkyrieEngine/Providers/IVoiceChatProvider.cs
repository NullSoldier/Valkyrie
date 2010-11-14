using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valkyrie.Engine.Characters;

namespace Valkyrie.Engine.Providers
{
	public interface IVoiceChatProvider
		: IEngineProvider
	{
		/// <summary>
		/// Fired when a user starts speaking including local players
		/// </summary>
		event EventHandler<TalkingChangedEventArgs> UserStartedTalking;

		/// <summary>
		/// Fired when a user stops speaking including local players
		/// </summary>
		event EventHandler<TalkingChangedEventArgs> UserStoppedTalking;

		/// <summary>
		/// Fired when the connection has been established.
		/// </summary>
		event EventHandler Connected;

		/// <summary>
		/// Fired when the connection has been disconnected
		/// </summary>
		event EventHandler Disconnected;

		/// <summary>
		/// Whether or not to use voice activation
		/// </summary>
		bool UseVoiceActivation { get; set; }

		/// <summary>
		/// True if the connection is connected.
		/// </summary>
		bool IsConnected { get; }

		/// <summary>
		/// Synchronously connect to the given host.
		/// </summary>
		/// <param name="host">The IP address or host name of the server to connect to.</param>
		/// <param name="port">The port to use when connecting.</param>
		/// <param name="username">The username to log in with.</param>
		/// <param name="password">The password to log in with.</param>
		void Connect (string host, int port, string username, string password);

		/// <summary>
		/// Asynchronously connect to the given host.
		/// </summary>
		/// <param name="host">The IP address or host name of the server to connect to.</param>
		/// <param name="port">The port to use when connecting.</param>
		/// <param name="username">The username to log in with.</param>
		/// <param name="password">The password to log in with.</param>
		void ConnectAsync (string host, int port, string username, string password);

		/// <summary>
		/// Disconnect the current connection
		/// </summary>
		void Disconnect ();

		/// <summary>
		/// Begin capturing and transmitting voice to the server
		/// </summary>
		/// <param name="character">The BaseCharacter that has started talking.</param>
		void BeginTalk (BaseCharacter character);

		/// <summary>
		/// Finish capturing and transmitting voice to the server
		/// </summary>
		/// <param name="character">The BaseCharacter that has stopped talking.</param>
		void EndTalk (BaseCharacter character);
	}

	public class TalkingChangedEventArgs
		: EventArgs
	{
		public TalkingChangedEventArgs (object id)
		{
			this.ID = id;
		}

		public object ID
		{
			get;
			private set;
		}
	}
}
