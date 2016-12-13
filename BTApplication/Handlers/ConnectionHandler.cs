using System;
using System.Collections.Generic;
using System.Text;
using BTApplication.Models;
using Xamarin.Forms;

namespace BTApplication.Handlers
{
	class ConnectionHandler : IConnectionHandler
	{
        public BTApplicationPage Page { get; set; }
		public void OnAvailableConnections(IEnumerable<User> users)
		{

			//throw new NotImplementedException();
			Console.WriteLine("Found connections:");
			foreach (var user in users)
			{
                Page.userslayout.Children.Add(new Button { Text = string.Format("* {0}", user.Name) }); 
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
