using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using BTApplication.Droid.Logic;
using static BTApplication.Droid.Logic.BluetoothManager;
using Android.Bluetooth;
using BTApplication.Droid.Logic.Activities;

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

			LoadApplication(new App());

            var receiver = new DiscoveredDeviceReceiver();
            RegisterReceiver(receiver, new IntentFilter(BluetoothDevice.ActionFound));

            var bluetoothManager = new Logic.BluetoothManager();
            bluetoothManager.Scan();
		}
	}
}
