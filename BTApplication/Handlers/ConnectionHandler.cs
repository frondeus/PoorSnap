using System;
using System.Collections.Generic;
using BTApplication.Models;
using Android.Bluetooth;
using Java.IO;
using Java.Util;


namespace BTApplication.Handlers
{
    class ConnectionHandler : IConnectionHandler
    {

        private const string UuidUniverseProfile = "00001101-0000-1000-8000-00805F9B34FB";
        private BluetoothDevice result;
        private BluetoothSocket mSocket;
        private BufferedReader reader;
        private System.IO.Stream mStream;
        private InputStreamReader mReader;

        public ConnectionHandler()
        {
            reader = null;
        }

        private UUID getUUIDFromString()
        {
            return UUID.FromString(UuidUniverseProfile); 
        }

        public string getDataFromDevice()
        {
            return reader.ReadLine();
        }

        public void OnAvailableConnections(IEnumerable<Models.User> users)
        {
            foreach (User user in users)
            {
                OnConnected(user);
            }
        }
        
        public void OnConnected(User user)
        {
            try
            {
                mSocket = user.btdevice.CreateRfcommSocketToServiceRecord(getUUIDFromString());
                mSocket.Connect();
                mStream = mSocket.InputStream;
                mReader = new InputStreamReader(mStream);
                reader = new BufferedReader(mReader);
            }

            catch (IOException e)
            {
                mSocket.Close();
                mStream.Close();
                mReader.Close();
                throw e;
            }
        }

        public void OnDisconnected(IDisposable aConnectedObject)
        {
            if (aConnectedObject == null) return;
            try
            {
                aConnectedObject.Dispose();
            }
            catch (System.Exception)
            {

                throw;
            }
            aConnectedObject = null;
        }

        public void getAllPairedDevices()
        {
            BluetoothAdapter btAdapter = BluetoothAdapter.DefaultAdapter;
            var devices = btAdapter.BondedDevices;
            if (devices != null && devices.Count > 0)
            {
                List<User> users = new List<User>();

                foreach (BluetoothDevice mDevice in devices)
                {
                    users.Add(new User { btdevice = mDevice });
                }
            };

        }
    }
}

