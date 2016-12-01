using System;
using System.Collections.Generic;

namespace BTApplication
{
	public interface IBluetoothManager
	{
		// IMessageHandler MessageHandler { get; set; } TODO: Upewnić się że jest to potrzebne
		// IConnectionHandler ConnectionHandler { get; set; }

		void SendMessage(Models.Message message);
		List<Models.User> Scan();
		void Connect(Models.User user);
		void Disconnect();
	}
}
