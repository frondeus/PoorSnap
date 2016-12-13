using System;
using System.Collections.Generic;
using System.Text;
using BTApplication.Models;

namespace BTApplication.Handlers
{
	class ConnectionHandler : IConnectionHandler
	{
		public void OnAvailableConnections(IEnumerable<User> users)
		{
			//throw new NotImplementedException();
			Console.WriteLine("Found connections:");
			foreach (var user in users)
			{
				Console.WriteLine(string.Format("* {0}", user.Name));
			}
		}

		public void OnConnected(User user)
		{
			//throw new NotImplementedException();
		}

		public void OnDisconnected(User user)
		{
			//throw new NotImplementedException();
		}
	}
}
