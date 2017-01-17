using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BTApplication.Models;
using Android.Bluetooth;
using BTApplication.Droid.Logic.Receivers;
using BTApplication.Droid.Models;
using Java.Util;
using Android.Media;
using Console = System.Console;
using Xamarin.Forms;
using Encoding = System.Text.Encoding;
using Stream = System.IO.Stream;

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
	    private BluetoothSocket _socket;

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
			var androidUser = (AndroidUser)user;
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

			ConnectionHandler.OnConnected(androidUser);
			_outputStream = _socket.OutputStream;
			_inputStream = _socket.InputStream;
		}

		public void Disconnect()
		{
			_socket.Close();
            _socket.Dispose();
		}

		public void SendMessage(Message message)
		{
		    var encodedMessage = Encoding.UTF8.GetBytes(message.TextContent);
            _outputStream.Write(encodedMessage, 0 , encodedMessage.Length);
            _outputStream.Flush();
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

			    var dev = connectSocket.RemoteDevice;
                ConnectionHandler.OnConnected(new AndroidUser() { Name = dev.Name, BluetoothDevice = dev });
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
				    var incomingBytes = new byte[1024];
					_inputStream.Read(incomingBytes, 0, incomingBytes.Length);
				    var decodedMessage = Encoding.UTF8.GetString(incomingBytes);

					if (string.IsNullOrEmpty(decodedMessage)) continue;

					Device.BeginInvokeOnMainThread(() =>
					{
						Console.WriteLine("Incoming message: " + decodedMessage);
						MessageHandler.OnMessage(new Message() { TextContent = decodedMessage });
					});
				}
			});
		}

		#endregion
	}
}