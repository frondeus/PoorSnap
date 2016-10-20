using System;
namespace BTApplication
{
	public interface IBluetoothManager
	{
		IMessageHandler MessageHandler { get; set; }
		IConnectionHandler ConnectionHandler { get; set; }

		void SendMessage(Models.Message message);
		void Scan();
		void Connect(Models.User user);
	}
}
