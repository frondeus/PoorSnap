using Android.Bluetooth;
using System;

namespace BTApplication.Models
{
	public class User
	{
		public Guid Guid { get; set; }
		public string Name { get; set; }
        public BluetoothDevice btdevice { get; set; }
	}
}
