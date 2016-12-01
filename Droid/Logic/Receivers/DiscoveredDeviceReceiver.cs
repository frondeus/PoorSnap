using System.Collections.Generic;
using System.Linq;
using Android.Bluetooth;
using Android.Content;

namespace BTApplication.Droid.Logic.Receivers
{
    internal class DiscoveredDeviceReceiver : BroadcastReceiver
    {
        private static List<BluetoothDevice> _foundDevices;

        public DiscoveredDeviceReceiver()
        {
            _foundDevices = new List<BluetoothDevice>();
        }

        public override void OnReceive(Context context, Intent intent)
        {
            if (!intent.Action.Equals(BluetoothDevice.ActionFound)) return;

            var dev = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
            _foundDevices.Add(dev);
        }

        public List<BluetoothDevice> GetFoundDevices()
        {
            return _foundDevices.Distinct().ToList();
        }

        public void FlushList()
        {
            _foundDevices.Clear();
        }
    }
}