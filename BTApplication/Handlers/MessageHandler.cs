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
				_chatPage.SetMessages(messages);
			}
		}

		public NavigationPage Nav { get; set; }

		private ObservableCollection<Message> messages = new ObservableCollection<Message>();

		public void OnMessage(Message message)
		{
			messages.Add(message);
		}
	}
}