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
			Console.WriteLine("Connected");
			chatPage = new ChatPage(Page.getBM(), user);
            var messageHandler = new MessageHandler();
			Page.getBM().MessageHandler = messageHandler;
			messageHandler.ChatPage = chatPage;
			messageHandler.Nav = Nav;

			Nav.PushAsync(chatPage);

		}

		public void OnDisconnected()
		{
			Console.WriteLine("Disconnected");
            Message mes = new Message();
            mes.Name = "System";
            mes.TextContent = "Użytkownik opuścił chat!";
            mes.BgColor = "Red";
            Page.getBM().SendMessage(mes);
        }
	}
}
