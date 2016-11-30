using System;
using Android.Content;
using Android.Bluetooth;
using System.Collections.Generic;

namespace BTApplication.Droid.Logic.Activities
{
    class DiscoveredDeviceReceiver : BroadcastReceiver
    {
        private static List<BluetoothDevice> _foundDevices;

        public DiscoveredDeviceReceiver()
        {
            _foundDevices = new List<BluetoothDevice>();
        }

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action.Equals(BluetoothDevice.ActionFound))
            {
                BluetoothDevice dev = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                _foundDevices.Add(dev);
            }
        }

        public static List<BluetoothDevice> GetFoundDevices()
        {
            return _foundDevices;
        }
    }
}