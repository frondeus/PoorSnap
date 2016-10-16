using System;
namespace BTApplication
{
	public interface IMessageHandler
	{
		void HandleMessage(Models.Message message);
	}
}
