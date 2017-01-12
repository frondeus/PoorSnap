using System;

using Xamarin.Forms;
using BTApplication.Models;
using System.Security.Cryptography.X509Certificates;

namespace BTApplication
{
	public partial class ConnectionPage : ContentPage
	{
		private IBluetoothManager _bluetoothManager;

        public IBluetoothManager getBM()
        {
            return _bluetoothManager;
        }

		public ConnectionPage(IBluetoothManager bluetoothManager)
		{
			_bluetoothManager = bluetoothManager;
			InitializeComponent();

			ScannedList.IsPullToRefreshEnabled = true;
			ScannedList.Refreshing += Scan_Clicked;
			ScannedList.ItemSelected += (sender, e) =>
			{
				var selectedItem = ((ListView)sender).SelectedItem;

				_bluetoothManager.Connect((User)selectedItem);

				((ListView)sender).SelectedItem = null;
			};
		}

		public void SetUsersList(User[] users)
		{
			ScannedList.ItemsSource = users;
			ScannedList.EndRefresh();
		}

		void Scan_Clicked(object sender, EventArgs e)
		{
			_bluetoothManager.Scan();
		}
	}
}
