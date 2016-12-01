using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Bluetooth;
using BTApplication.Droid.Logic.Receivers;

namespace BTApplication.Droid
{
    [Activity(Label = "BTApplication.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

            //Register receivers
            var receiver = new DiscoveredDeviceReceiver();
            RegisterReceiver(receiver, new IntentFilter(BluetoothDevice.ActionFound));

            //Load application
            var app = new App(new Logic.BluetoothManager(receiver));
			LoadApplication(app);
		}

    }
}
