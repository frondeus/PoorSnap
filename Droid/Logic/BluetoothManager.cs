using System;
using System.Collections.Generic;
using System.Linq;
using BTApplication.Models;
using Android.Bluetooth;
using BTApplication.Droid.Logic.Receivers;

namespace BTApplication.Droid.Logic
{
    class BluetoothManager : IBluetoothManager
    {
        private readonly BluetoothAdapter _bluetoothAdapter;
        private readonly DiscoveredDeviceReceiver _receiver;

        public BluetoothManager(DiscoveredDeviceReceiver receiver)
        {
            _bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            _receiver = receiver;
        }

        public void Connect(User user)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public List<User> Scan()
        {
            _bluetoothAdapter.StartDiscovery();
            while (_bluetoothAdapter.IsDiscovering)
            {
            }

            var result = _receiver.GetFoundDevices();
            var users = result.Select(user => new User()
            {
                Name = user.Name
            }).ToList();

            _receiver.FlushList();

            return users;
        }

        public void SendMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}