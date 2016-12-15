using System;
using System.Collections.Generic;
using BTApplication.Models;
using Xamarin.Forms;

namespace BTApplication
{
	public partial class ChatPage : ContentPage
	{
		private IBluetoothManager _bluetoothManager;

		public ChatPage(IBluetoothManager bluetoothManager)
		{
			_bluetoothManager = bluetoothManager;
			InitializeComponent();

			//TODO: Usunąć
			Output.ItemsSource = new Message[] {
				new Message {
					TextContent = "Przykładowa wiadomość"
				}
			};
		}

		void Handle_Clicked(object sender, EventArgs e)
		{
			_bluetoothManager.SendMessage(new Message
			{
				TextContent = Input.Text
			});

			Input.Text = "";
		}
	}
}
