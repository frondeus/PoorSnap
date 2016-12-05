using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BTApplication.Models;
using Android.Bluetooth;
using BTApplication.Droid.Logic.Receivers;
using BTApplication.Droid.Models;
using Java.Util;

namespace BTApplication.Droid.Logic
{
    class BluetoothManager : IBluetoothManager
    {
        private readonly BluetoothAdapter _bluetoothAdapter;
        private readonly DiscoveredDeviceReceiver _receiver;
        public IMessageHandler MessageHandler { get; set; }
        public IConnectionHandler ConnectionHandler { get; set; }

        public BluetoothManager(DiscoveredDeviceReceiver receiver)
        {
            _bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            _receiver = receiver;

            ListenAsServerTask();
        }

        public void Connect(User user)
        {

        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public async void Scan()
        {
            var foundDevices = await PerformScanAsync();
            ConnectionHandler.OnAvailableConnections(foundDevices);
        }

        public void SendMessage(Message message)
        {
            throw new NotImplementedException();
        }



        private Task<List<AndroidUser>> PerformScanAsync()
        {
            return Task.Factory.StartNew(PerformScan);
        }

        private List<AndroidUser> PerformScan()
        {
            _bluetoothAdapter.StartDiscovery();

            while (_bluetoothAdapter.IsDiscovering) { }

            var foundDevices = _receiver.GetFoundDevices();
            _receiver.FlushList();
            return foundDevices;
        }



        private Task ListenAsServerTask()
        {
            return Task.Factory.StartNew(ListenAsServer);
        }

        private void ListenAsServer()
        {
            var socket = _bluetoothAdapter.ListenUsingRfcommWithServiceRecord("serverConnection", UUID.FromString(_bluetoothAdapter.Name));
            var connectSocket = socket.Accept();

            if (connectSocket == null || !connectSocket.IsConnected) return;

            ConnectionHandler.OnConnected(new AndroidUser() {BluetoothDevice = connectSocket.RemoteDevice, Name = connectSocket.RemoteDevice.Name});
            connectSocket.Dispose();
            socket.Dispose();
        }
    }
}