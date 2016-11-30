using System;
using System.Collections.Generic;
using System.Linq;
using CoreBluetooth;
using CoreFoundation;
using Foundation;
using UIKit;

namespace BTApplication.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		BluetoothManager bluetoothManager;
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

			LoadApplication(new App());

			bluetoothManager = new BluetoothManager();
			bluetoothManager.Scan();

			return base.FinishedLaunching(app, options);
		}
	}
}
