using System;
using System.Collections.Generic;
using BTApplication.Models;
using Xamarin.Forms;

namespace BTApplication
{
	public partial class ChatPage : ContentPage
	{
        private List<Message> _messages = new List<Message>();
        private IBluetoothManager _bluetoothManager;

		public ChatPage(IBluetoothManager bluetoothManager)
		{
			_bluetoothManager = bluetoothManager;
			InitializeComponent();
            for(var i=0;i<10;i++)
            _messages.Add(new Message
            {
                TextContent = "Przykładowa wiadomość "+i
            });

            //TODO: Usunąć
            Output.ItemsSource = _messages;
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
