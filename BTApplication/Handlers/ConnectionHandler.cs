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
        public ChatPage chatPage;

		public void OnAvailableConnections(IEnumerable<User> users)
		{
			Page.SetUsersList(users.ToArray());
		}

		public void OnConnected(User user)
		{
            chatPage = new ChatPage(Page.getBM(), user);
            var messageHandler = new MessageHandler();
            Page.getBM().MessageHandler = messageHandler;
            messageHandler.Page = chatPage;
            messageHandler.Nav = Nav;

            Nav.PushAsync(chatPage);

            Nav.Popped += (sender, e) =>
            {
                OnDisconnected(user);
            };
        }

		public void OnDisconnected(User user)
		{
            Nav.PopAsync();
            Nav.PushAsync(Page);
        }
	}
}
