using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Android.Bluetooth;
using BTApplication.Droid.Logic.Receivers;
using BTApplication.Droid.Models;
using BTApplication.Models;
using Java.Util;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace BTApplication.Droid.Logic
{
    internal class BluetoothManager : IBluetoothManager
    {
        private readonly BluetoothAdapter _bluetoothAdapter;
        private readonly DiscoveredDeviceReceiver _receiver;
        private Stream _inputStream;
        private Stream _outputStream;
        private BluetoothSocket _socket;

        public BluetoothManager(DiscoveredDeviceReceiver receiver)
        {
            _bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            _receiver = receiver;

            _outputStream = null;
            _inputStream = null;

            ListenAsServerTask();
        }

        public IMessageHandler MessageHandler { get; set; }
        public IConnectionHandler ConnectionHandler { get; set; }

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

            if (!bondState.Equals(Bond.Bonded))
                bondState = await BondAsync(btDevice);

            if (bondState.Equals(Bond.None))
            {
                Console.WriteLine("Nie uda³o siê nawi¹zaæ po³¹czenia - urz¹dzenia nie zosta³y powi¹zane");
                return;
            }

            _socket = btDevice.CreateRfcommSocketToServiceRecord(UUID.FromString("4edd00b2-c221-11e6-a4a6-cec0c932ce01"));
            _socket.Connect();

            Device.BeginInvokeOnMainThread(() => { ConnectionHandler.OnConnected(androidUser); });

            _outputStream = _socket.OutputStream;
            _inputStream = _socket.InputStream;

            ListenToMessagesTask();
        }

        public void Disconnect()
        {
            _inputStream.Close();
            _inputStream = null;
            _outputStream.Close();
            _outputStream = null;
            _socket.Close();
            _socket = null;
        }

        public void SendMessage(Message message)
        {
            var json = JsonConvert.SerializeObject(message);
            var encodedMessage = Encoding.UTF8.GetBytes(json);
            _outputStream.Write(encodedMessage, 0, encodedMessage.Length);
            _outputStream.Flush();
        }

        #region scan

        private Task<List<AndroidUser>> ScanAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                _bluetoothAdapter.StartDiscovery();
                while (_bluetoothAdapter.IsDiscovering)
                {
                }

                var foundDevices = _receiver.GetFoundDevices();
                _receiver.FlushList();
                return foundDevices;
            });
        }

        #endregion

        #region listenAsServer

        private void ListenAsServerTask()
        {
            Task.Factory.StartNew(() =>
            {
                var socket = _bluetoothAdapter.ListenUsingRfcommWithServiceRecord("serverConnection",
                    UUID.FromString("4edd00b2-c221-11e6-a4a6-cec0c932ce01"));
                _socket = socket.Accept();

                if ((_socket == null) || !_socket.IsConnected) return;

                _outputStream = _socket.OutputStream;
                _inputStream = _socket.InputStream;

                Device.BeginInvokeOnMainThread(() =>
                {
                    var dev = _socket.RemoteDevice;
                    ConnectionHandler.OnConnected(new AndroidUser
                    {
                        Name = dev.Name,
                        BluetoothDevice = dev
                    });
                });
                ListenToMessagesTask();
            });
        }

        #endregion

        #region bond

        private static Task<Bond> BondAsync(BluetoothDevice device)
        {
            return Task.Factory.StartNew(() =>
            {
                device.CreateBond();
                while (device.BondState.Equals(Bond.Bonding))
                {
                }

                return device.BondState;
            });
        }

        #endregion

        #region listenToMessages

        private void ListenToMessagesTask()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (!_socket.IsConnected)
                    {
                        ConnectionHandler.OnDisconnected();
                        break;
                    }
                }
            });

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var incomingBytes = new byte[1024];
                    try
                    {
                        _inputStream.Read(incomingBytes, 0, incomingBytes.Length);
                    }
                    catch (Java.IO.IOException)
                    {
                        break;
                    }
                    Message decodedMessage = JsonConvert.DeserializeObject<Message>(Encoding.UTF8.GetString(incomingBytes));


                    if (string.IsNullOrEmpty(decodedMessage.TextContent)) continue;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Console.WriteLine("Incoming message: " + decodedMessage.TextContent);
                        MessageHandler.OnMessage(decodedMessage);
                    });
                }
            });
        }

        #endregion
    }
}