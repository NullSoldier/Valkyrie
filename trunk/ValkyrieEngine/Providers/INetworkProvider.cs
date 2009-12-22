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
		event EventHandler Connected;
		event EventHandler<ConnectionEventArgs> Disconnected;
		event EventHandler<MessageReceivedEventArgs> MessageReceived;

		bool IsConnected { get; }

		bool Connect (string server, int port);
		void Disconnect();
		void Send (MessageBase message);
	}
}
