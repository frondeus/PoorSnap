using System;
namespace BTApplication
{
	public interface IMessageHandler
	{
		void OnMessage(Models.Message message);

	}
}
