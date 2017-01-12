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

		public ChatPage(IBluetoothManager bluetoothManager, User user)
		{
			_bluetoothManager = bluetoothManager;
			InitializeComponent();
		}

		public void SetMessages(ObservableCollection<Message> messages)
		{
			Output.ItemsSource = messages;
		}

		void Handle_Clicked(object sender, EventArgs e)
		{
			string name = UserName.Text;
			string text = Input.Text;
			if (name.Length == 0)
			{
				name = "Anonimowy wysyłacz";
			}
			if (text.Length != 0)
			{
				_bluetoothManager.SendMessage(new Message
				{
					Name = name,
					TextContent = text
				});
			}
			Input.Text = "";
		}
	}
}
