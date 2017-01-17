using BTApplication.Handlers;
using Xamarin.Forms;

namespace BTApplication
{
	public partial class App : Application
	{
		public App(IBluetoothManager bluetoothManager)
		{
			//bluetoothManager = new Fake.BluetoothManager();

			var connectionHandler = new ConnectionHandler();
			//bluetoothManager = new Fake.BluetoothManager();
			bluetoothManager.ConnectionHandler = connectionHandler;

			var connectionPage = new ConnectionPage(bluetoothManager);
			var mainPage = new NavigationPage(connectionPage);

			MainPage = mainPage;

			connectionHandler.Nav = mainPage;
			connectionHandler.Page = connectionPage;

			mainPage.Popped += (sender, e) =>
			{
				bluetoothManager.Disconnect();
			};
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
