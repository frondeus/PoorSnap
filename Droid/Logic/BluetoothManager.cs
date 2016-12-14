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
    internal class BluetoothManager : IBluetoothManager
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



        public async void Scan()
        {
            _bluetoothAdapter.CancelDiscovery();
            var foundDevices = await ScanAsync();
            ConnectionHandler.OnAvailableConnections(foundDevices);
        }

        public async void Connect(User user)
        {
            var androidUser = (AndroidUser) user;
            var btDevice = androidUser.BluetoothDevice;

            var bondState = await BondAsync(btDevice);
            if (bondState.Equals(Bond.None))
            {
                Console.WriteLine("Nie uda�o si� nawi�za� po��czenia - urz�dzenia nie zosta�y powi�zane");
                return;
            }

            var socket =
                btDevice.CreateRfcommSocketToServiceRecord(
                    UUID.FromString("4edd00b2-c221-11e6-a4a6-cec0c932ce01")
                );
            socket.Connect();
            var output = socket.OutputStream;
            var input = socket.InputStream;
            
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void SendMessage(Message message)
        {
            throw new NotImplementedException();
        }



        #region scan

        private Task<List<AndroidUser>> ScanAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                _bluetoothAdapter.StartDiscovery();
                while (_bluetoothAdapter.IsDiscovering) { }

                var foundDevices = _receiver.GetFoundDevices();
                _receiver.FlushList();
                return foundDevices;
            });
        }

        #endregion

        #region listenAsServer

        private Task ListenAsServerTask()
        {
            return Task.Factory.StartNew(() =>
            {
                var socket = _bluetoothAdapter.ListenUsingRfcommWithServiceRecord("serverConnection", UUID.FromString("4edd00b2-c221-11e6-a4a6-cec0c932ce01"));
                var connectSocket = socket.Accept();

                if (connectSocket == null || !connectSocket.IsConnected) return;

                ConnectionHandler.OnConnected(new AndroidUser() { BluetoothDevice = connectSocket.RemoteDevice, Name = connectSocket.RemoteDevice.Name });
                connectSocket.Dispose();
                socket.Dispose();
            });
        }

        #endregion

        #region bond

        private static Task<Bond> BondAsync(BluetoothDevice device)
        {
            return Task.Factory.StartNew(() =>
            {
                device.CreateBond();
                while (device.BondState.Equals(Bond.Bonding)) { }

                return device.BondState;
            });
        }

        #endregion
    }
}