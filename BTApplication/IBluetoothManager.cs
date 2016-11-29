using System;
namespace BTApplication
{
	public interface IBluetoothManager
	{
		// IMessageHandler MessageHandler { get; set; } TODO: Upewnić się że jest to potrzebne
		// IConnectionHandler ConnectionHandler { get; set; }

		void SendMessage(Models.Message message);
		void Scan();
		void Connect(Models.User user);
		void Disconnect();
	}
}
