using System.Collections.Generic;
using BTApplication.Models;
using System.Linq;
using System;
using Xamarin.Forms;

namespace BTApplication.Handlers
{
	class ConnectionHandler : IConnectionHandler
	{
		public ConnectionPage Page { get; set; }
		public NavigationPage Nav { get; set; }

		public void OnAvailableConnections(IEnumerable<User> users)
		{
			Page.SetUsersList(users.ToArray());
		}

		public void OnConnected(User user)
		{
		}

		public void OnDisconnected(User user)
		{
		}
	}
}
