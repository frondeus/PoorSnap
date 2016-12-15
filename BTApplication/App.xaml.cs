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

            var connectionPage = new BTApplicationPage(bluetoothManager);
            MainPage = connectionPage;
            connectionHandler.Page = connectionPage;
		    messageHandler.Page = connectionPage;
		    // MainPage = new Page1();
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
