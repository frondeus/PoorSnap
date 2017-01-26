using System;
using System.Collections.Generic;
using BTApplication.Models;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;

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
                    TextContent = text,
                    BgColor = "Purple"
                    
                });
				_bluetoothManager.SendMessage(new Message
				{
					Name = _user.Name,
                    TextContent = text,
                    BgColor = "Blue"
				});
			}
            Input.Text = "";
            ScrollToLast();
        }
        public void ScrollToLast()
        {
            var v = Output.ItemsSource.Cast<object>().LastOrDefault();
            Output.ScrollTo(v, ScrollToPosition.End, true);
        }
	}
}
