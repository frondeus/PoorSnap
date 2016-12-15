using System;
using BTApplication.Models;
using Xamarin.Forms;
using static System.String;

namespace BTApplication
{
	public partial class BTApplicationPage : ContentPage
	{
		private readonly IBluetoothManager _bluetoothManager;

		public StackLayout usersLayout;
		public Label display;
		public Label choice;
		public StackLayout userChoice;

		public BTApplicationPage(IBluetoothManager bluetoothManager)
		{
			_bluetoothManager = bluetoothManager;
			InitializeComponent();

			display = this.FindByName<Label>("Display");
			usersLayout = this.FindByName<StackLayout>("UsersLayout");
			choice = this.FindByName<Label>("Choice");
			userChoice = this.FindByName<StackLayout>("UserChoice");
		}

		public void AddButton(User user)
		{
			var button = new Button { Text = user.Name };
			button.Clicked += (object sender, EventArgs e) =>
			{
				_bluetoothManager.Connect(user);
			};
			usersLayout.Children.Add(button);
		}

		public void AddMessage(string message)
		{
			var label = new Label { Text = message };
			Result.Children.Add(label);
		}

		void Scan_Clicked(object sender, EventArgs e)
		{
			display.Text = "Skanowanie w poszukiwaniu urządzeń...";
			_bluetoothManager.Scan();
		}

		public void AddChoice(byte bit)
		{
			var button = new Button { Text = bit.ToString() };
			button.Clicked += (object sender, EventArgs e) =>
			{
				_bluetoothManager.SendMessage(new Message() { TextContent = bit.ToString() });
			};
			userChoice.Children.Add(button);
		}
	}
}
