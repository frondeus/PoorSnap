using System;
using System.Collections.Generic;
using BTApplication.Models;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace BTApplication
{
	public partial class ChatPage : ContentPage
	{
		private IBluetoothManager _bluetoothManager;
		private User _user;
        public ObservableCollection<Message> messages { get; set;}

        public ChatPage(IBluetoothManager bluetoothManager, User user)
		{
			_user = user;
			_bluetoothManager = bluetoothManager;
			InitializeComponent();
            messages = new ObservableCollection<Message>();
        }

		public void SetMessages(ObservableCollection<Message> messages)
		{
			Output.ItemsSource = messages;
		}

		void Handle_Clicked(object sender, EventArgs e)
		{
			string text = Input.Text;
			if (text.Length != 0)
			{
                messages.Add(new Message
                {
                    Name = "Me",
                    TextContent = text
                });
				_bluetoothManager.SendMessage(new Message
				{
					Name = "Me",
					TextContent = text
				});
			}
			Input.Text = "";
		}
	}
}
