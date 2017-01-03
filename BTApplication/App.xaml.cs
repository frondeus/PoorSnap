using BTApplication.Handlers;
using Xamarin.Forms;

namespace BTApplication
{
	public partial class App : Application
	{
		public App(IBluetoothManager bluetoothManager = null)
		{
			var connectionHandler = new ConnectionHandler();
			var messageHandler = new MessageHandler();
			bluetoothManager.MessageHandler = messageHandler;
			bluetoothManager.ConnectionHandler = connectionHandler;

			var oldPage = new BTApplicationPage(bluetoothManager); // TODO Jak już będzie widok czatu pozbyć się!
																   // TODO A dokładniej jak widok czatu już będzie w pełni obsługiwany przez messageHandler.

			var connectionPage = new ConnectionPage(bluetoothManager);
			var chatPage = new ChatPage(bluetoothManager);

			var mainPage = new NavigationPage(connectionPage);

			mainPage.Popped += (sender, e) =>
			{
				bluetoothManager.Disconnect();
			};

			//TODO: Usunac, to jest jedynie do testów za pomocą FakeHandlera
			bluetoothManager.Connect(null);
			mainPage.PushAsync(chatPage);
			//END TODO

			MainPage = mainPage;

			connectionHandler.Nav = messageHandler.Nav = mainPage;

			connectionHandler.Page = connectionPage;
			messageHandler.Page = oldPage;
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
