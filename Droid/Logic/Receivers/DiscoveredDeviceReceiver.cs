using System.Collections.Generic;
using System.Linq;
using Android.Bluetooth;
using Android.Content;
using BTApplication.Models;
using BTApplication.Droid.Models;

namespace BTApplication.Droid.Logic.Receivers
{
    internal class DiscoveredDeviceReceiver : BroadcastReceiver
    {
        private static List<AndroidUser> _foundDevices;

        public DiscoveredDeviceReceiver()
        {
            _foundDevices = new List<AndroidUser>();
        }

        public override void OnReceive(Context context, Intent intent)
        {
            if (!intent.Action.Equals(BluetoothDevice.ActionFound)) return;
            var dev = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
            _foundDevices.Add(new AndroidUser() {Name = dev.Name, BluetoothDevice = dev});
        }

        public List<AndroidUser> GetFoundDevices()
        {
            return _foundDevices.GroupBy(x => x.Name).Select(x => x.First()).ToList();
        }

        public void FlushList()
        {
            _foundDevices.Clear();
        }
    }
}