using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BTApplication.Models;
using Android.Bluetooth;
using BTApplication.Droid.Logic.Receivers;
using BTApplication.Droid.Models;
using Java.Util;
using System.IO;
using Console = System.Console;

namespace BTApplication.Droid.Logic
{
    internal class BluetoothManager : IBluetoothManager
    {
        private readonly BluetoothAdapter _bluetoothAdapter;
        private readonly DiscoveredDeviceReceiver _receiver;
        public IMessageHandler MessageHandler { get; set; }
        public IConnectionHandler ConnectionHandler { get; set; }
        private Stream _outputStream;
        private Stream _inputStream;

        public BluetoothManager(DiscoveredDeviceReceiver receiver)
        {
            _bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            _receiver = receiver;

            _outputStream = null;
            _inputStream = null;

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
            var bondState = btDevice.BondState;

            if(!bondState.Equals(Bond.Bonded))
                bondState = await BondAsync(btDevice);

            if (bondState.Equals(Bond.None))
            {
                Console.WriteLine("Nie uda³o siê nawi¹zaæ po³¹czenia - urz¹dzenia nie zosta³y powi¹zane");
                return;
            }

            var socket = btDevice.CreateRfcommSocketToServiceRecord(UUID.FromString("4edd00b2-c221-11e6-a4a6-cec0c932ce01"));
            socket.Connect();

            ConnectionHandler.OnConnected(androidUser);
            _outputStream = socket.OutputStream;
            _inputStream = socket.InputStream;
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void SendMessage(Message message)
        {
            _outputStream.Flush();
            _outputStream.WriteByte(message.TextContent.Equals("1") ? (byte)1 : (byte)0);
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

                _outputStream = connectSocket.OutputStream;
                _inputStream = connectSocket.InputStream;
                ListenToMessagesTask();

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

        #region listenToMessages

        private Task ListenToMessagesTask()
        {
            return Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var msg = _inputStream.ReadByte();

                    if(msg == -1) continue;

                    Console.WriteLine("TAKA SOBIE WIADOMOSC: " + msg);
                    MessageHandler.OnMessage(new Message() {TextContent = msg.ToString()} );
                }
            });
        }

        #endregion
    }
}