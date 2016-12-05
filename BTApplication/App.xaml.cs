using BTApplication.Handlers;
using Xamarin.Forms;

namespace BTApplication
{
	public partial class App : Application
	{
	    public App(IBluetoothManager bluetoothManager = null)
		{
            bluetoothManager.MessageHandler = new MessageHandler();
            bluetoothManager.ConnectionHandler = new ConnectionHandler();
		    MainPage = new BTApplicationPage(bluetoothManager);
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
