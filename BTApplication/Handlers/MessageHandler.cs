using System.Threading.Tasks;
using BTApplication.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using System.Collections.ObjectModel;

namespace BTApplication.Handlers
{
	public class MessageHandler : IMessageHandler
	{
		private ChatPage _chatPage;

		public ChatPage ChatPage
		{
			get
			{
				return _chatPage;
			}
			set
			{
				_chatPage = value;
				_chatPage.SetMessages(_chatPage.messages);
			}
		}

		public NavigationPage Nav { get; set; }

		

		public void OnMessage(Message message)
		{
            _chatPage.messages.Add(message);
            _chatPage.ScrollToLast();
		}
	}
}