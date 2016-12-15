using System;
using System.Collections.Generic;
using System.Text;
using BTApplication.Models;
using Xamarin.Forms;
using System.Linq;

namespace BTApplication.Handlers
{
	class ConnectionHandler : IConnectionHandler
	{
		public BTApplicationPage Page { get; set; }
		public void OnAvailableConnections(IEnumerable<User> users)
		{
			Page.usersLayout.Children.Clear();
			Page.display.Text = users.Count() > 0 ? "Znaleziono: " : "Nie znaleziono urządzeń...";
			foreach (var user in users)
			{
				Page.AddButton(user);
			}
		}

		public void OnConnected(User user)
		{
            //throw new NotImplementedException();
		    Page.choice.Text = "Wybierz se bita";
            Page.AddChoice(0);
            Page.AddChoice(1);
		}

		public void OnDisconnected(User user)
		{
			//throw new NotImplementedException();
		}
	}
}
