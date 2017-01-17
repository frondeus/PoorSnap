using System;
using System.Collections.Generic;

namespace BTApplication
{
	public interface IConnectionHandler
	{
		void OnAvailableConnections(IEnumerable<Models.User> users);
		void OnConnected(Models.User user);
		void OnDisconnected();
    }
}
