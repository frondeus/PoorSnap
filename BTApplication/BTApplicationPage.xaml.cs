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

		public BTApplicationPage(IBluetoothManager bluetoothManager)
		{
			_bluetoothManager = bluetoothManager;
			InitializeComponent();

			display = this.FindByName<Label>("Display");
			usersLayout = this.FindByName<StackLayout>("UsersLayout");

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

		void Scan_Clicked(object sender, EventArgs e)
		{
			display.Text = "Skanowanie w poszukiwaniu urządzeń...";
			_bluetoothManager.Scan();
		}

	}
}
