using System;
namespace BTApplication
{
	public interface IBluetoothManager
	{
		void SetMessageHandler(IMessageHandler handler);
		void SendMessage(Models.Message message);
	}
}
