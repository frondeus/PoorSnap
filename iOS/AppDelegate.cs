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
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

			LoadApplication(new App());

			var myDel = new BluetoothManager();

			var myMgr = new CBCentralManager(myDel, DispatchQueue.CurrentQueue);

			return base.FinishedLaunching(app, options);
		}
	}
}
