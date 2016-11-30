using System;
using BTApplication.Models;
using Android.Bluetooth;

namespace BTApplication.Droid.Logic
{
    class BluetoothManager : IBluetoothManager
    {
        private BluetoothAdapter _bluetoothAdapter;

        public BluetoothManager()
        {
            _bluetoothAdapter = BluetoothAdapter.DefaultAdapter; 
        }

        public void Connect(User user)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void Scan()
        {
            _bluetoothAdapter.StartDiscovery();
            while (_bluetoothAdapter.IsDiscovering)
            {
                continue;
            }
        }

        public void SendMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}