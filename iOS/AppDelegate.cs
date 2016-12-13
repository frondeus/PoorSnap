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

			bluetoothManager = new BluetoothManager();

			LoadApplication(new App(bluetoothManager));

			return base.FinishedLaunching(app, options);
		}
	}
}
